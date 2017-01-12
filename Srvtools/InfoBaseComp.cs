using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Srvtools
{
    [ToolboxItem(false)]
    public class InfoBaseComp : Component, IFindContainer
    {
        virtual protected void DoBeforeSetOwner(IDataModule value)
        {
        }
        virtual protected void DoAfterSetOwner(IDataModule value)
        {
        }

        private IDataModule fOwnerComp;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDataModule OwnerComp
        {
            get
            {
                return fOwnerComp;
            }
            set
            {
                DoBeforeSetOwner(fOwnerComp);
                fOwnerComp = value;
                DoAfterSetOwner(fOwnerComp);
            }
        }
    }
}
