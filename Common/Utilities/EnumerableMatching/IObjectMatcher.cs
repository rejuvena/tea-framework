#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using System;
using System.Collections;
using System.Collections.Generic;

namespace TeaFramework.Common.Utilities.EnumerableMatching
{
    /// <summary>
    ///     Generic-less core interface for enumerated object matching.
    /// </summary>
    public interface IObjectMatcher : IEnumerable
    {
    }

    /// <summary>
    ///     Generic interface that provides a framework for enumerable object equality comparing.
    /// </summary>
    /// <typeparam name="T">The object type to match.</typeparam>
    public interface IObjectMatcher<T> : IObjectMatcher, IEquatable<T>, IEnumerable<T>
    {
        IEnumerable<T>? Objects { get; }

        bool Equals(IObjectMatcher<T> other);
    }
}