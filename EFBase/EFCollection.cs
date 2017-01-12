using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace EFBase
{
    /// <summary>
    /// Collection of item
    /// </summary>
    /// <typeparam name="T">Type of item</typeparam>
    public class EFCollection<T> : List<T>, IList, IEFProperty
    {
        /// <summary>
        /// Creates a new instance of collection
        /// </summary>
        /// <param name="parentProperty">Parent property</param>
        public EFCollection(IEFProperty parentProperty)
        {
            _parentProperty = parentProperty;
        }

        /// <summary>
        /// Creatse a new instance of collection
        /// </summary>
        /// <param name="component">Parent component</param>
        public EFCollection(object component)
        {
            _component = component;
        }

        /// <summary>
        /// Adds item
        /// </summary>
        /// <param name="item">Item</param>
        public new void Add(T item)
        {
            base.Add(item);
            if (item is IEFProperty)
            {
                (item as IEFProperty).ParentProperty = this;
            }
        }

        /// <summary>
        /// Adds list of items
        /// </summary>
        /// <param name="collection">List of items</param>
        public new void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            foreach (var item in collection)
            {
                if (item is IEFProperty)
                {
                    (item as IEFProperty).ParentProperty = this;
                }
            }
        }

        /// <summary>
        /// Inserts item at the specified index
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="item">Item</param>
        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            if (item is IEFProperty)
            {
                (item as IEFProperty).ParentProperty = this;
            }
        }

        /// <summary>
        /// Inserts list of items at the specified index
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="collection">Item</param>
        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            foreach (var item in collection)
            {
                if (item is IEFProperty)
                {
                    (item as IEFProperty).ParentProperty = this;
                }
            }
        }

        #region IList Members

        int IList.Add(object value) //编辑器里增加时只会触发这个
        {
            if (value is T)
            {
                base.Add((T)value);
                if (value is IEFProperty)
                {
                    (value as IEFProperty).ParentProperty = this;
                }
                return base.Count - 1;
            }
            else
            {
                throw new ArgumentException(string.Format("value is not of type:{0}.", typeof(T).Name));
            }
        }

        #endregion

        private IEFProperty _parentProperty;
        IEFProperty IEFProperty.ParentProperty
        {
            get
            {
                return _parentProperty;
            }
            set
            {
                _parentProperty = value;
            }
        }

        private object _component;
        object IEFProperty.Component
        {
            get
            {
                return _component;
            }
        }
    }

    /// <summary>
    /// Item of collection
    /// </summary>
    public abstract class EFCollectionItem : IEFProperty
    {
        private IEFProperty _parentProperty;
        IEFProperty IEFProperty.ParentProperty
        {
            get
            {
                return _parentProperty;
            }
            set
            {
                _parentProperty = value;
            }
        }

        object IEFProperty.Component
        {
            get
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Interface of Property Class
    /// </summary>
    public interface IEFProperty
    {
        /// <summary>
        /// Gets or sets parent property
        /// </summary>
        IEFProperty ParentProperty
        {
            get;
            set;
        }

        /// <summary>
        /// Gets parent component
        /// </summary>
        object Component
        {
            get;
        }
    }
}
