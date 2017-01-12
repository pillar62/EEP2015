using System;
using System.Collections.Generic;
using System.Text;

namespace Srvtools
{
    public delegate void UpdateComponentBeforeApplyEventHandler(object sender, UpdateComponentBeforeApplyEventArgs e);

    public delegate void UpdateComponentAfterApplyEventHandler(object sender, UpdateComponentAfterApplyEventArgs e);

    public delegate void UpdateComponentBeforeInsertEventHandler(object sender, UpdateComponentBeforeInsertEventArgs e);

    public delegate void UpdateComponentAfterInsertEventHandler(object sender, UpdateComponentAfterInsertEventArgs e);

    public delegate void UpdateComponentBeforeDeleteEventHandler(object sender, UpdateComponentBeforeDeleteEventArgs e);

    public delegate void UpdateComponentAfterDeleteEventHandler(object sender, UpdateComponentAfterDeleteEventArgs e);

    public delegate void UpdateComponentBeforeModifyEventHandler(object sender, UpdateComponentBeforeModifyEventArgs e);

    public delegate void UpdateComponentAfterModifyEventHandler(object sender, UpdateComponentAfterModifyEventArgs e);

    #region UpdateComponentBeforeApplyEventArgs

    public sealed class UpdateComponentBeforeApplyEventArgs : EventArgs
    {
        public UpdateComponentBeforeApplyEventArgs()
            : base()
        {
        }
    }
    #endregion

    #region UpdateComponentAfterApplyEventArgs

    public sealed class UpdateComponentAfterApplyEventArgs : EventArgs
    {
        
        public UpdateComponentAfterApplyEventArgs(): base() 
        {
        }
    }

    #endregion

    #region UpdateComponentBeforeInsertEventArgs

    public sealed class UpdateComponentBeforeInsertEventArgs : EventArgs
    {
        public UpdateComponentBeforeInsertEventArgs() : base() 
        {
        }
    }

    #endregion

    #region UpdateComponentAfterInsertEventArgs

    public sealed class UpdateComponentAfterInsertEventArgs : EventArgs
    {
        public UpdateComponentAfterInsertEventArgs() : base()
        {
        }
    }

    #endregion

    #region UpdateComponentBeforeDeleteEventArgs

    public sealed class UpdateComponentBeforeDeleteEventArgs : EventArgs
    {
        public UpdateComponentBeforeDeleteEventArgs() : base()
        {
        }
    }

    #endregion

    #region UpdateComponentAfterDeleteEventArgs

    public sealed class UpdateComponentAfterDeleteEventArgs : EventArgs
    {
        public UpdateComponentAfterDeleteEventArgs() : base()
        {
        }
    }

    #endregion

    #region UpdateComponentBeforeModifyEventArgs

    public sealed class UpdateComponentBeforeModifyEventArgs : EventArgs
    {
        public UpdateComponentBeforeModifyEventArgs() : base()
        {
        }
    }

    #endregion

    #region UpdateComponentAfterModifyEventArgs

    public sealed class UpdateComponentAfterModifyEventArgs : EventArgs
    {
        public UpdateComponentAfterModifyEventArgs() : base()
        {
        }
    }

    #endregion
}
