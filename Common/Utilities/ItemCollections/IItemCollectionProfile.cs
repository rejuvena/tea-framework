#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using System.Collections.Generic;
using JetBrains.Annotations;

namespace TeaFramework.Common.Utilities.ItemCollections
{
    /// <summary>
    ///     Buildable profile of core item collection data, supports appending a bonus value as well as dynamically modifying the underlying item collection.
    /// </summary>
    public interface IItemCollectionProfile
    {
        List<(int itemId, int itemCount)> ItemData { get; }

        int ExtraValue { get; set; }

        /// <summary>
        ///     Sets the value of <see cref="ExtraValue"/> through a builder pattern.
        /// </summary>
        /// <param name="extraValue">The extra value to set.</param>
        /// <returns>The same object instance.</returns>
        IItemCollectionProfile WithExtraValue(int extraValue);

        /// <summary>
        ///     Whether this collection should be taken into account for whatever it is being processed for.
        /// </summary>
        /// <returns>Self-explanatory.</returns>
        bool CanBeApplied();

        /// <summary>
        ///     <see cref="CanBeApplied"/>-aware total item value calculation.
        /// </summary>
        /// <returns>The value of all items in <see cref="ItemData"/> combined, plus <see cref="ExtraValue"/>.</returns>
        [MustUseReturnValue("Otherwise useless calculation.")]
        int ToValueCount();
    }
}