using System;
using System.Collections.Generic;
using System.Text;

namespace Srvtools
{
    public class TotalColumnCollection : InfoOwnerCollection
    {
        public TotalColumnCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(TotalColumn))
        {
        }

        public new TotalColumn this[int index]
        {
            get
            {
                return (TotalColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is TotalColumn)
                    {
                        //原来的Collection设置为0
                        ((TotalColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((TotalColumn)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
}
