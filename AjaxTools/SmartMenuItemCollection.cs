using System;
using System.Drawing.Design;
using System.ComponentModel;

namespace AjaxTools
{
    [Editor(typeof(SmartMenuCollectionEditor), typeof(UITypeEditor))]
    public class SmartMenuItemCollection : InfoOwnerCollection
    {
        public SmartMenuItemCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(SmartMenuItem))
        {
        }

        public new SmartMenuItem this[int index]
        {
            get
            {
                return (SmartMenuItem)this[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is SmartMenuItem)
                    {
                        //原来的Collection设置为0
                        ((SmartMenuItem)this[index]).Collection = null;
                        this[index] = value;
                        //Collection设置为this
                        ((SmartMenuItem)this[index]).Collection = this;
                    }
                }
            }
        }
    }

}
