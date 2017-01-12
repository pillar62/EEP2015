using System;
using System.Collections.Generic;
using System.Text;

namespace Srvtools
{
    public class WhereItemCollection : InfoOwnerCollection
    {
        public WhereItemCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(WhereItem))
        {
        }

        public new WhereItem this[int index]
        {
            get
            {
                return (WhereItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is WhereItem)
                    {
                        //原来的Collection设置为0
                        ((WhereItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WhereItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
}
