#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TeaFramework.Common.Utilities.EnumerableMatching
{
    /// <summary>
    ///     Default <see cref="IObjectMatcher{T}"/> implementation.
    /// </summary>
    /// <typeparam name="T">The object type to match.</typeparam>
    public abstract class ObjectMacher<T> : IObjectMatcher<T>
    {
        public virtual IEnumerable<T>? Objects { get; }

        protected ObjectMacher(params T[] objects)
        {
            Objects = objects;
        }

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                null => false,
                IObjectMatcher<T> self => Equals(self),
                T inner => Equals(inner),
                _ => obj.GetHashCode() == GetHashCode()
            };
        }

        public virtual bool Equals(T? other) => other is not null &&
                                                Objects != null &&
                                                Objects.Any(x => x != null && x.Equals(other));

        public virtual bool Equals(IObjectMatcher<T>? other) => other is not null &&
                                                                Objects != null &&
                                                                other.Objects != null &&
                                                                Objects.Any(x => other.Objects.Contains(x));

        public override int GetHashCode() => Objects != null ? Objects.GetHashCode() : 0;

        public virtual IEnumerator<T> GetEnumerator() => Objects?.GetEnumerator() ?? new List<T>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}