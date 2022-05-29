using System;
using System.Reflection;

namespace TeaFramework.Features.Utility
{
    /// <summary>
    ///     Utilities for handling reflection.
    /// </summary>
    public static partial class Reflection
    {
        public static bool IsStatic(this PropertyInfo property) {
            return (property.GetMethod?.IsStatic ?? false) || (property.SetMethod?.IsStatic ?? false);
        }

        #region Binding Flags

        /// <summary>
        ///     Encompasses <see cref="BindingFlags.Public" /> and <see cref="BindingFlags.NonPublic" />.
        /// </summary>
        public const BindingFlags PublicityFlags = BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        ///     Encompasses <see cref="BindingFlags.Static" /> and <see cref="BindingFlags.Instance" />.
        /// </summary>
        public const BindingFlags InstanceFlags = BindingFlags.Static | BindingFlags.Instance;

        /// <summary>
        ///     Grants access to members regardless of visibility and static status.
        /// </summary>
        public const BindingFlags UniversalFlags = PublicityFlags | InstanceFlags;

        #endregion

        #region InvokeUnderlyingMethod

        public static object? InvokeUnderlyingMethod(
            this FieldInfo field,
            string method,
            Type[]? signature,
            int genericCount,
            object? instance,
            object?[]? args
        ) {
            return field.FieldType.GetCachedMethod(method, signature, genericCount).Invoke(instance, args);
        }

        public static T? InvokeUnderlyingMethod<T>(
            this FieldInfo field,
            string method,
            Type[]? signature,
            int genericCount,
            object? instance,
            object?[]? args
        ) {
            return (T?) field.InvokeUnderlyingMethod(method, signature, genericCount, instance, args);
        }

        public static object? InvokeUnderlyingMethod(
            this PropertyInfo property,
            string method,
            Type[]? signature,
            int genericCount,
            object? instance,
            object?[]? args
        ) {
            return property.PropertyType.GetCachedMethod(method, signature, genericCount).Invoke(instance, args);
        }

        public static T? InvokeUnderlyingMethod<T>(
            this PropertyInfo property,
            string method,
            Type[]? signature,
            int genericCount,
            object? instance,
            object?[]? args
        ) {
            return (T?) property.InvokeUnderlyingMethod(method, signature, genericCount, instance, args);
        }

        #endregion

        #region GetFieldValue

        public static object? GetFieldValue(this object obj, string name) {
            return obj.GetType().GetCachedField(name).GetValue(obj);
        }

        public static TF? GetFieldValue<T, TF>(this T obj, string name) {
            return (TF?) typeof(T).GetCachedField(name).GetValue(obj);
        }

        #endregion

        #region GetPropertyValue

        public static object? GetPropertyValue(this object obj, string name) {
            return obj.GetType().GetCachedProperty(name).GetValue(obj);
        }

        public static TP? GetPropertyValue<T, TP>(this T obj, string name) {
            return (TP?) typeof(T).GetCachedProperty(name).GetValue(obj);
        }

        #endregion

        #region SetFieldValue

        public static void SetFieldValue(this object obj, string name, object? value) {
            obj.GetType().GetCachedField(name).SetValue(obj, value);
        }

        public static void SetFieldValue<TF>(this object obj, string name, TF? value) {
            SetFieldValue(obj, name, (object?) value);
        }

        #endregion

        #region SetPropertyValue

        public static void SetPropertyValue(this object obj, string name, object? value) {
            obj.GetType().GetCachedProperty(name).SetValue(obj, value);
        }

        public static void SetPropertyValue<TF>(this object obj, string name, TF? value) {
            SetPropertyValue(obj, name, (object?) value);
        }

        #endregion

        #region SetNewInstance

        public static void SetNewInstance(this FieldInfo field, object? instance = null, object? value = null) {
            field.SetValue(instance, value ?? Activator.CreateInstance(field.FieldType));
        }

        public static void SetNewInstance(this PropertyInfo property, object? instance = null, object? value = null) {
            property.SetValue(instance, value ?? Activator.CreateInstance(property.PropertyType));
        }

        #endregion
    }
}