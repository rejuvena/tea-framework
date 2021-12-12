#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TeaFramework.Common.Utilities.Terraria.ItemCollections
{
    /// <summary>
    ///     <see cref="IList{T}"/> implementation around <see cref="IItemCollectionProfile"/>s.
    /// </summary>
    public class ItemCollection : IList<IItemCollectionProfile>
    {
        protected readonly List<IItemCollectionProfile> UnderlyingCollection = new();

        public static ItemCollection FromIItemCollectionProfile(IItemCollectionProfile profile) => new() {profile};

        public static IItemCollectionProfile ToIItemCollectionProfile(ItemCollection collection) =>
            new WrappedItemCollectionProfile(collection.ToArray());

        #region Interface Implementation

        public virtual IEnumerator<IItemCollectionProfile> GetEnumerator() => UnderlyingCollection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public virtual void Add(IItemCollectionProfile item) => UnderlyingCollection.Add(item);

        public virtual void Clear() => UnderlyingCollection.Clear();

        public virtual bool Contains(IItemCollectionProfile item) => UnderlyingCollection.Contains(item);

        public virtual void CopyTo(IItemCollectionProfile[] array, int arrayIndex) =>
            UnderlyingCollection.CopyTo(array, arrayIndex);

        public virtual bool Remove(IItemCollectionProfile item) => UnderlyingCollection.Remove(item);

        public virtual int Count => UnderlyingCollection.Count;

        public virtual bool IsReadOnly => false;

        public virtual int IndexOf(IItemCollectionProfile item) => UnderlyingCollection.IndexOf(item);

        public void Insert(int index, IItemCollectionProfile item) => UnderlyingCollection.Insert(index, item);

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        public IItemCollectionProfile this[int index]
        {
            get => UnderlyingCollection[index];

            set => UnderlyingCollection[index] = value;
        }

        #endregion
    }
}