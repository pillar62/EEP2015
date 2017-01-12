using System;
using System.Collections.Generic;
using System.Text;

namespace Srvtools
{
    public class TransFieldCollection : TransFieldCollectionBase
    {
        public TransFieldCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(TransField))
        {

        }

        public new TransField this[int index]
        {
            get
            {
                return (TransField)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is TransField)
                    {
                        //原来的Collection设置为0
                        ((TransField)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((TransField)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
}
