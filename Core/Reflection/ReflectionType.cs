#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System.Reflection;

namespace TeaFramework.Core.Reflection
{
    /// <summary>
    ///     Types of expected cache-able reflection-associated types.
    /// </summary>
    public enum ReflectionType
    {
        /// <summary>
        ///     Corresponds to <see cref="FieldInfo"/>.
        /// </summary>
        Field,

        /// <summary>
        ///     Corresponds to <see cref="PropertyInfo"/>.
        /// </summary>
        Property,

        /// <summary>
        ///     Corresponds to <see cref="MethodInfo"/>.
        /// </summary>
        Method,

        /// <summary>
        ///     Corresponds to <see cref="ConstructorInfo"/>.
        /// </summary>
        Constructor,

        /// <summary>
        ///     Corresponds to <see cref="System.Type"/>.
        /// </summary>
        Type,

        /// <summary>
        ///     Corresponds specifically to nested <see cref="System.Type"/><c>s</c>, as they're treated differently..
        /// </summary>
        NestedType
    }
}