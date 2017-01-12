using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Srvtools
{
    public class FieldAttrCollection : InfoOwnerCollection
    {
        public FieldAttrCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(FieldAttr))
        {

        }

        public new FieldAttr this[int index]
        {
            get
            {
                return (FieldAttr)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is FieldAttr)
                    {
                        //原来的Collection设置为0
                        ((FieldAttr)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((FieldAttr)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
}
