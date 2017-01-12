using System;
using System.Collections.Generic;
using System.Text;

namespace Srvtools
{
    public class TransKeyFieldCollection : TransFieldCollectionBase
    {
        public TransKeyFieldCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(TransKeyField))
        {

        }

        public new TransKeyField this[int index]
        {
            get
            {
                return (TransKeyField)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is TransKeyField)
                    {
                        //原来的Collection设置为0
                        ((TransKeyField)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((TransKeyField)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
}
