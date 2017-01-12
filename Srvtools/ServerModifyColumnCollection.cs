using System;
using System.Collections.Generic;
using System.Text;

namespace Srvtools
{
    public class ServerModifyColumnCollection : InfoOwnerCollection
    {
        public ServerModifyColumnCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(FieldAttr))
        {
        }

        public new ServerModifyColumn this[int index]
        {
            get
            {
                return (ServerModifyColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is ServerModifyColumn)
                    {
                        //原来的Collection设置为0
                        ((ServerModifyColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((ServerModifyColumn)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
}
