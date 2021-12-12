#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;

namespace TeaFramework.Common.Utilities.Extensions
{
    /// <summary>
    ///     Numerous methods to extend array functionality.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        ///     Adds an element to the end of an array.
        /// </summary>
        /// <typeparam name="T">The array type.</typeparam>
        /// <param name="array">The instance to the array to add to.</param>
        /// <param name="item">The element to be added.</param>
        /// <returns>The array with the added element.</returns>
        /// <remarks>
        ///     Adding to array should generally be avoided. Use a list or something instead.
        /// </remarks>
        public static T[] Add<T>(this T[] array, T item)
        {
            Array.Resize(ref array, array.Length + 1);
            array[^1] = item;
            return array;
        }
    }
}