using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;

namespace Srvtools
{
    public class RefColumnsCollection : InfoOwnerCollection
    {
        public RefColumnsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(RefColumns))
        {
        }

        public DataSet DsForDD = new DataSet();
        public new RefColumns this[int index]
        {
            get
            {
                return (RefColumns)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is RefColumns)
                    {
                        //原来的Collection设置为0
                        ((RefColumns)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((RefColumns)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
}
