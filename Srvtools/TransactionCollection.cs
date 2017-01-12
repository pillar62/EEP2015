using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Srvtools
{
    public class TransactionCollection : InfoOwnerCollection
    {
        public TransactionCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(Transaction))
        {

        }

        public new Transaction this[int index]
        {
            get
            {
                return (Transaction)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is Transaction)
                    {
                        //原来的Collection设置为0
                        ((Transaction)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((Transaction)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
}
