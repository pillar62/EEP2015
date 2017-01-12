using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Srvtools
{
    public class ColumnMatchCollection : InfoOwnerCollection
    {
        public ColumnMatchCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(ColumnMatch))
        {

        }

        public new ColumnMatch this[int index]
        {
            get
            {
                return (ColumnMatch)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is ColumnMatch)
                    {
                        //原来的Collection设置为0
                        ((ColumnMatch)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((ColumnMatch)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
}
