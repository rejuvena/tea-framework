using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using TeaFramework.API.Exceptions;

namespace TeaFramework.Utilities
{
    partial class Reflection
    {
        public readonly record struct DynamicIdentifier(Type Type, string MemberName)
        {
            public override string ToString() {
                return Type.FullName + "." + MemberName;
            }
        }

        #region Fields

        private static readonly Dictionary<DynamicIdentifier, Func<object, object?>> FieldGetters = new();
        private static readonly Dictionary<DynamicIdentifier, Func<object?>> StaticFieldGetters = new();
        private static readonly Dictionary<DynamicIdentifier, Action<object, object?>> FieldSetters = new();
        private static readonly Dictionary<DynamicIdentifier, Action<object?>> StaticFieldSetters = new();

        private static bool TryGetFieldGetter(
            DynamicIdentifier id,
            out Func<object, object?>? getter,
            out Func<object?>? staticGetter
        ) {
            getter = null;
            staticGetter = null;
            return FieldGetters.TryGetValue(id, out getter) || StaticFieldGetters.TryGetValue(id, out staticGetter);
        }

        private static bool TryGetFieldSetter(
            DynamicIdentifier id,
            out Action<object, object?>? setter,
            out Action<object?>? staticSetter
        ) {
            setter = null;
            staticSetter = null;
            return FieldSetters.TryGetValue(id, out setter) || StaticFieldSetters.TryGetValue(id, out staticSetter);
        }

        public static object? InvokeFieldGetter(Type type, string fieldName, object? instance) {
            DynamicIdentifier id = new(type, fieldName);

            if (!TryGetFieldGetter(id, out Func<object, object?>? getter, out Func<object?>? staticGetter)) {
                GenerateFieldGetter(id);
                TryGetFieldGetter(id, out getter, out staticGetter);
            }

            if (getter is not null) {
                if (instance is null) throw new ReflectionInvocationException("Attempted to get value of instanced field w/o instance.");

                return getter(instance);
            }

            if (staticGetter is not null) {
                if (instance is not null) throw new ReflectionInvocationException("Attempted to get value of static field w/ instance.");

                return staticGetter();
            }

            throw new ReflectionInvocationException(
                $"Unable to create or retrieve a getter for field \"{id.MemberName}\" of type \"{id.Type.FullName}\"."
            );
        }

        public static void InvokeFieldSetter(Type type, string fieldName, object? instance, object? value) {
            DynamicIdentifier id = new(type, fieldName);

            if (!TryGetFieldSetter(id, out Action<object, object?>? setter, out Action<object?>? staticSetter)) {
                GenerateFieldSetter(id);
                TryGetFieldSetter(id, out setter, out staticSetter);
            }

            if (setter is not null) {
                if (instance is null) throw new ReflectionInvocationException("Attempted to set value of instanced field w/o instance.");

                setter(instance, value);
                return;
            }

            if (staticSetter is not null) {
                if (instance is not null) throw new ReflectionInvocationException("Attempted to set value of static field w/ instance.");

                staticSetter(value);
                return;
            }

            throw new ReflectionInvocationException(
                $"Unable to create or retrieve a setter for field \"{id.MemberName}\" of type \"{id.Type.FullName}\"."
            );
        }

        public static void GenerateFieldGetter(DynamicIdentifier id) {
            if (FieldGetters.ContainsKey(id) || StaticFieldGetters.ContainsKey(id)) return;

            FieldInfo? info = id.Type.GetCachedFieldNullable(id.MemberName);

            if (info is null)
                throw new ReflectionGenerationException(
                    $"Could not resolve field \"{id.MemberName}\" of type \"{id.Type.FullName}\""
                );

            DynamicMethod dynaMethod = info.IsStatic
                ? new DynamicMethod(id.MemberName, typeof(object), null, id.Type.Module, true)
                : new DynamicMethod(id.MemberName, typeof(object), new[] {typeof(object)}, id.Type.Module, true);

            ILGenerator il = dynaMethod.GetILGenerator();

            if (info.IsStatic) {
                // Load static field.
                il.Emit(OpCodes.Ldsfld, info);
            }
            else {
                // Load instanced field with first argument.
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, info);
            }

            // Box value types.
            if (info.FieldType.IsValueType) il.Emit(OpCodes.Box, info.FieldType);

            // Return the pushed value.
            il.Emit(OpCodes.Ret);

            if (info.IsStatic)
                StaticFieldGetters[id] = dynaMethod.CreateDelegate<Func<object?>>();
            else
                FieldGetters[id] = dynaMethod.CreateDelegate<Func<object, object?>>();
        }

        public static void GenerateFieldSetter(DynamicIdentifier id) {
            if (FieldSetters.ContainsKey(id) || StaticFieldSetters.ContainsKey(id)) return;

            FieldInfo? info = id.Type.GetCachedFieldNullable(id.MemberName);

            if (info is null)
                throw new ReflectionGenerationException(
                    $"Could not resolve field \"{id.MemberName}\" of type \"{id.Type.FullName}\""
                );

            DynamicMethod dynaMethod = info.IsStatic
                ? new DynamicMethod(id.MemberName, null, new[] {typeof(object)}, id.Type.Module, true)
                : new DynamicMethod(id.MemberName, null, new[] {typeof(object), typeof(object)}, id.Type.Module, true);

            ILGenerator il = dynaMethod.GetILGenerator();

            // Static: load value, Instanced: load field value.
            il.Emit(OpCodes.Ldarg_0);

            // Load value to set.
            if (!info.IsStatic) il.Emit(OpCodes.Ldarg_1);

            // Unbox/cast as needed.
            il.Emit(info.FieldType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, info.FieldType);

            // Static: set static field, Instanced: set field
            il.Emit(info.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, info);

            il.Emit(OpCodes.Ret);

            if (info.IsStatic)
                StaticFieldSetters[id] = dynaMethod.CreateDelegate<Action<object?>>();
            else
                FieldSetters[id] = dynaMethod.CreateDelegate<Action<object, object?>>();
        }

        #endregion

        #region Properties

        private static readonly Dictionary<DynamicIdentifier, Func<object, object?>> PropertyGetters = new();
        private static readonly Dictionary<DynamicIdentifier, Func<object?>> StaticPropertyGetters = new();
        private static readonly Dictionary<DynamicIdentifier, Action<object, object?>> PropertySetters = new();
        private static readonly Dictionary<DynamicIdentifier, Action<object?>> StaticPropertySetters = new();

        private static bool TryGetPropertyGetter(
            DynamicIdentifier id,
            out Func<object, object?>? getter,
            out Func<object?>? staticGetter
        ) {
            getter = null;
            staticGetter = null;
            return PropertyGetters.TryGetValue(id, out getter) || StaticPropertyGetters.TryGetValue(id, out staticGetter);
        }

        private static bool TryGetPropertySetter(
            DynamicIdentifier id,
            out Action<object, object?>? setter,
            out Action<object?>? staticSetter
        ) {
            setter = null;
            staticSetter = null;
            return PropertySetters.TryGetValue(id, out setter) || StaticPropertySetters.TryGetValue(id, out staticSetter);
        }

        public static object? InvokePropertyGetter(Type type, string propertyName, object? instance) {
            DynamicIdentifier id = new(type, propertyName);

            if (!TryGetPropertyGetter(id, out Func<object, object?>? getter, out Func<object?>? staticGetter)) {
                GeneratePropertyGetter(id);
                TryGetPropertyGetter(id, out getter, out staticGetter);
            }

            if (getter is not null) {
                if (instance is null) throw new ReflectionInvocationException("Attempted to get value of instanced property w/o instance.");

                return getter(instance);
            }

            if (staticGetter is not null) {
                if (instance is not null) throw new ReflectionInvocationException("Attempted to get value of static property w/ instance.");

                return staticGetter();
            }

            throw new ReflectionInvocationException(
                $"Unable to create or retrieve a getter for property \"{id.MemberName}\" of type \"{id.Type.FullName}\"."
            );
        }

        public static void InvokePropertySetter(Type type, string propertyName, object? instance, object? value) {
            DynamicIdentifier id = new(type, propertyName);

            if (!TryGetPropertySetter(id, out Action<object, object?>? setter, out Action<object?>? staticSetter)) {
                GeneratePropertySetter(id);
                TryGetPropertySetter(id, out setter, out staticSetter);
            }

            if (setter is not null) {
                if (instance is null) throw new ReflectionInvocationException("Attempted to set value of instanced property w/o instance.");

                setter(instance, value);
                return;
            }

            if (staticSetter is not null) {
                if (instance is not null) throw new ReflectionInvocationException("Attempted to set value of static property w/ instance.");

                staticSetter(value);
                return;
            }

            throw new ReflectionInvocationException(
                $"Unable to create or retrieve a setter for property \"{id.MemberName}\" of type \"{id.Type.FullName}\"."
            );
        }

        public static void GeneratePropertyGetter(DynamicIdentifier id) {
            if (PropertyGetters.ContainsKey(id) || StaticPropertyGetters.ContainsKey(id)) return;

            PropertyInfo? info = id.Type.GetCachedPropertyNullable(id.MemberName);

            if (info is null)
                throw new ReflectionGenerationException(
                    $"Could not resolve property \"{id.MemberName}\" of type \"{id.Type.FullName}\""
                );

            if (info.GetMethod is null)
                throw new ReflectionGenerationException(
                    $"Property \"{id.MemberName}\" of type \"{id.Type.FullName}\" does not have a getter!"
                );

            DynamicMethod dynaMethod = info.IsStatic()
                ? new DynamicMethod(id.MemberName, typeof(object), null, id.Type.Module, true)
                : new DynamicMethod(id.MemberName, typeof(object), new[] {typeof(object)}, id.Type.Module, true);

            ILGenerator il = dynaMethod.GetILGenerator();

            if (info.IsStatic()) {
                // Call property getter.
                il.Emit(OpCodes.Call, info.GetMethod);
            }
            else {
                // Load instance value and then call property getter.
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Callvirt, info.GetMethod);
            }

            // Box value types.
            if (info.PropertyType.IsValueType) il.Emit(OpCodes.Box, info.PropertyType);

            // Return the pushed value.
            il.Emit(OpCodes.Ret);

            if (info.IsStatic())
                StaticPropertyGetters[id] = dynaMethod.CreateDelegate<Func<object?>>();
            else
                PropertyGetters[id] = dynaMethod.CreateDelegate<Func<object, object?>>();
        }

        public static void GeneratePropertySetter(DynamicIdentifier id) {
            if (PropertySetters.ContainsKey(id) || StaticPropertySetters.ContainsKey(id)) return;

            PropertyInfo? info = id.Type.GetCachedPropertyNullable(id.MemberName);

            if (info is null)
                throw new ReflectionGenerationException(
                    $"Could not resolve property \"{id.MemberName}\" of type \"{id.Type.FullName}\""
                );

            if (info.SetMethod is null)
                throw new ReflectionGenerationException(
                    $"Property \"{id.MemberName}\" of type \"{id.Type.FullName}\" does not have a setter!"
                );

            DynamicMethod dynaMethod = info.IsStatic()
                ? new DynamicMethod(id.MemberName, null, new[] {typeof(object)}, id.Type.Module, true)
                : new DynamicMethod(id.MemberName, null, new[] {typeof(object), typeof(object)}, id.Type.Module, true);

            ILGenerator il = dynaMethod.GetILGenerator();

            // Static: load value, Instanced: load property value.
            il.Emit(OpCodes.Ldarg_0);

            // Load value to set.
            if (!info.IsStatic()) il.Emit(OpCodes.Ldarg_1);

            // Unbox/cast as needed.
            il.Emit(info.PropertyType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, info.PropertyType);

            // Static: set static property, Instanced: set property
            il.Emit(info.IsStatic() ? OpCodes.Call : OpCodes.Callvirt, info.SetMethod);

            il.Emit(OpCodes.Ret);

            if (info.IsStatic())
                StaticPropertySetters[id] = dynaMethod.CreateDelegate<Action<object?>>();
            else
                PropertySetters[id] = dynaMethod.CreateDelegate<Action<object, object?>>();
        }

        #endregion
    }

    public static class Reflection<T>
    {
        public static object? InvokeFieldGetter(string fieldName, object? instance) {
            return Reflection.InvokeFieldGetter(typeof(T), fieldName, instance);
        }

        public static void InvokeFieldSetter(string fieldName, object? instance, object? value) {
            Reflection.InvokeFieldSetter(typeof(T), fieldName, instance, value);
        }

        public static object? InvokePropertyGetter(string fieldName, object? instance) {
            return Reflection.InvokePropertyGetter(typeof(T), fieldName, instance);
        }

        public static void InvokePropertySetter(string fieldName, object? instance, object? value) {
            Reflection.InvokePropertySetter(typeof(T), fieldName, instance, value);
        }
    }
}