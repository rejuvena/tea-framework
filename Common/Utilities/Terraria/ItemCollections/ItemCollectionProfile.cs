#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace TeaFramework.Common.Utilities.Terraria.ItemCollections
{
    /// <summary>
    ///     Standard <see cref="IItemCollectionProfile"/> implementation.
    /// </summary>
    public class ItemCollectionProfile : IItemCollectionProfile
    {
        public virtual List<(int itemId, int itemCount)> ItemData { get; }
        
        public virtual int ExtraValue { get; set; }

        #region Constructors

        public ItemCollectionProfile()
        {
            ItemData = new List<(int, int)>();
        }

        public ItemCollectionProfile(params (int, int)[] itemData)
        {
            ItemData = itemData.ToList();
        }

        public ItemCollectionProfile(params int[] items)
        {
            ItemData = items.Select(x => (x, 1)).ToList();
        }

        public ItemCollectionProfile(IEnumerable<(int, int)> itemData)
        {
            ItemData = itemData.ToList();
        }

        public ItemCollectionProfile(IEnumerable<int> items)
        {
            ItemData = items.Select(x => (x, 1)).ToList();
        }

        #endregion
        
        public virtual IItemCollectionProfile WithExtraValue(int extraValue)
        {
            ExtraValue = extraValue;
            return this;
        }

        public virtual bool CanBeApplied() => true;

        public virtual int ToValueCount()
        {
            int total = 0;

            if (!CanBeApplied())
                return total;

            foreach ((int item, int count) in ItemData)
            {
                try
                {
                    Item i = new();
                    i.SetDefaults(item);
                    total += i.value * count;
                }
                catch
                {
                    // ignore
                }
            }

            return total + ExtraValue;
        }
    }
}