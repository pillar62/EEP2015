using System;
using System.Collections.Generic;
using System.Text;

namespace Srvtools
{
    public class ServiceCollection : InfoOwnerCollection
    {
        public ServiceCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(Service))
        {

        }

        public new Service this[int index]
        {
            get
            {
                return (Service)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is Service)
                    {
                        //原来的Collection设置为0
                        ((Service)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((Service)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
}
