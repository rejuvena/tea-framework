#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TeaFramework.Common.Utilities;
#pragma warning disable 8603

namespace TeaFramework.Core.Reflection
{
    /// <summary>
    ///     Exhaustive suite of reflection utilities.
    /// </summary>
    public static class ReflectionCache
    {
        private static readonly Dictionary<ReflectionType, Dictionary<string, object>> Cache;

        static ReflectionCache()
        {
            Cache = new Dictionary<ReflectionType, Dictionary<string, object>>();
            GenerateEmptyCache(ref Cache, ReflectionType.Field);
            GenerateEmptyCache(ref Cache, ReflectionType.Property);
            GenerateEmptyCache(ref Cache, ReflectionType.Method);
            GenerateEmptyCache(ref Cache, ReflectionType.Constructor);
            GenerateEmptyCache(ref Cache, ReflectionType.Type);
            GenerateEmptyCache(ref Cache, ReflectionType.NestedType);
        }

        private static void GenerateEmptyCache(ref Dictionary<ReflectionType, Dictionary<string, object>> dictionary,
            ReflectionType reflectionType) => dictionary.Add(reflectionType, new Dictionary<string, object>());

        public static object? InvokeUnderlyingMethod(FieldInfo info, string methodName, object instance,
            params object[] parameters) => GetCachedMethod(info.FieldType, methodName)?.Invoke(instance, parameters);

        public static object? InvokeUnderlyingMethod(PropertyInfo info, string methodName, object instance,
            params object[] parameters) =>
            GetCachedMethod(info.PropertyType, methodName)?.Invoke(instance, parameters);

        public static FieldInfo GetCachedField(Type type, string key) => RetrieveFromCache(ReflectionType.Field,
            GetUniqueFieldKey(type, key), () => type.GetField(key, ReflectionHelper.UniversalFlags))!;

        public static PropertyInfo GetCachedProperty(Type type, string key) => RetrieveFromCache(
            ReflectionType.Property,
            GetUniquePropertyKey(type, key), () => type.GetProperty(key, ReflectionHelper.UniversalFlags))!;

        public static MethodInfo GetCachedMethod(Type type, string key) => RetrieveFromCache(ReflectionType.Method,
            GetUniqueMethodKey(type, key), () => type.GetMethod(key, ReflectionHelper.UniversalFlags))!;

        public static ConstructorInfo GetCachedConstructor(Type type, params Type[] identity) => RetrieveFromCache(
            ReflectionType.Constructor, GetUniqueConstructorKey(type, identity),
            () => type.GetConstructor(ReflectionHelper.UniversalFlags, null, identity, null))!;

        public static Type GetCachedType(Assembly assembly, string key) => RetrieveFromCache(ReflectionType.Type,
            GetUniqueTypeKey(assembly, key), () => assembly.GetType(key));

        public static Type GetCachedNestedType(Type type, string key) => RetrieveFromCache(ReflectionType.Type,
            GetUniqueNestedTypeKey(type, key), () => type.GetNestedType(key, ReflectionHelper.UniversalFlags));

        public static string GetUniqueFieldKey(Type type, string key) => $"{type.FullName}->{key}";

        public static string GetUniquePropertyKey(Type type, string key) => $"{type.FullName}->{key}";

        public static string GetUniqueMethodKey(Type type, string key) => $"{type.FullName}::{key}";

        public static string GetUniqueConstructorKey(Type type, params Type[] identity)
        {
            string sewnTypes = string.Join(",", identity.Select(x => x.FullName));

            return $"{type.FullName}.ctor:{{{sewnTypes}}}";
        }

        public static string GetUniqueTypeKey(Assembly assembly, string key) => $"{assembly.FullName}.{key}";

        public static string GetUniqueNestedTypeKey(Type type, string key) => $"{type.FullName}.{key}";

        public static TReturn RetrieveFromCache<TReturn>(ReflectionType refType, string key, Func<TReturn> fallback)
        {
            if (Cache[refType].TryGetValue(key, out object? found))
                return (TReturn) found;

            return (TReturn) (Cache[refType][key] = fallback() ?? throw new InvalidOperationException());
        }

        public static void ReplaceInfoInstance(FieldInfo info, object? instance = null,
            object? replacementInstance = null) =>
            info.SetValue(instance, replacementInstance ?? Activator.CreateInstance(info.FieldType));

        public static void ReplaceInfoInstance(PropertyInfo info, object? instance = null,
            object? replacementInstance = null) =>
            info.SetValue(instance, replacementInstance ?? Activator.CreateInstance(info.PropertyType));

        public static TReturn? GetValue<TReturn>(FieldInfo info, object? instance = null) =>
            CastSafely<TReturn>(info.GetValue(instance));

        public static TReturn? GetValue<TReturn>(PropertyInfo info, object? instance = null) =>
            CastSafely<TReturn>(info.GetValue(instance));

        public static void SetValue(FieldInfo info, object? fieldInstance = null, object? fieldValue = null) =>
            info.SetValue(fieldInstance, fieldValue);

        public static void SetValue(PropertyInfo info, object? fieldInstance = null, object? fieldValue = null) =>
            info.SetValue(fieldInstance, fieldValue);

        public static TReturn? GetFieldValue<TType, TReturn>(TType instance, string field) =>
            CastSafely<TReturn>(GetCachedField(typeof(TType), field)?.GetValue(instance));

        public static TReturn? GetPropertyValue<TType, TReturn>(TType instance, string property) =>
            CastSafely<TReturn>(GetCachedProperty(typeof(TType), property)?.GetValue(instance));

        public static void SetFieldValue<TType>(TType instance, string field, object? fieldValue = null) =>
            GetCachedField(typeof(TType), field)?.SetValue(instance, fieldValue);

        public static void SetPropertyValue<TType>(TType instance, string property, object? fieldValue = null) =>
            GetCachedProperty(typeof(TType), property)?.SetValue(instance, fieldValue);

        private static T? CastSafely<T>(object? value)
        {
            if (value is null)
                return default;

            return (T) value;
        }
    }
}