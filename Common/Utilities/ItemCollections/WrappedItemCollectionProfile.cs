#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace TeaFramework.Common.Utilities.ItemCollections
{
    /// <summary>
    ///     <see cref="IItemCollectionProfile"/> implementation capable of combining a collection of other <see cref="IItemCollectionProfile"/> instances.
    /// </summary>
    public class WrappedItemCollectionProfile : IItemCollectionProfile
    {
        public virtual List<(int itemId, int itemCount)> ItemData =>
            Profiles.Where(x => x.CanBeApplied()).SelectMany(x => x.ItemData).ToList();

        public int ExtraValue
        {
            get => Profiles.Where(x => x.CanBeApplied()).Sum(x => x.ExtraValue);

            set => throw new InvalidOperationException(
                "Attempted to perform single-profile operation on multiple profiles."
            );
        }
        
        public IItemCollectionProfile[] Profiles { get; }

        public WrappedItemCollectionProfile(params IItemCollectionProfile[] profiles)
        {
            Profiles = profiles;
        }

        public virtual IItemCollectionProfile WithExtraValue(int extraValue) => throw new InvalidOperationException(
            "Attempted to perform single-profile operation on multiple profiles."
        );

        public virtual bool CanBeApplied() => true;

        public virtual int ToValueCount() => Profiles.Sum(x => x.ToValueCount());
    }
}