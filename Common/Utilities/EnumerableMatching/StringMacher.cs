#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using System;
using System.Linq;

namespace TeaFramework.Common.Utilities.EnumerableMatching
{
    /// <summary>
    ///     <see cref="IComparable"/> <see cref="string"/> implementation of <see cref="ObjectMacher{T}"/>.
    /// </summary>
    public class StringMacher : ObjectMacher<string>, IComparable, IComparable<string>
    {
        public StringMacher(params string[] objects) : base(objects)
        {
        }

        public virtual int CompareTo(object? obj)
        {
            if (obj is string other)
                return CompareTo(other);

            return 0;
        }

        public virtual int CompareTo(string? other)
        {
            if (other is null || Objects is null || !Objects.Contains(other))
                return 1;

            string? match = Objects.FirstOrDefault(x => x.Equals(other));

            return string.Compare(other, match, StringComparison.Ordinal);
        }
    }
}