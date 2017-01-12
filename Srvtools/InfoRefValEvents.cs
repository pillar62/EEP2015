using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Srvtools
{
    public delegate void OnActiveEventHandler(object sender, OnActiveEventArgs e);

    public delegate void OnCloseEventHandler(object sender, OnCloseEventArgs e);

    public delegate void OnReturnEventHandler(object sender, OnReturnEventArgs e);


    #region OnActiveEventArgs
    public sealed class OnActiveEventArgs : EventArgs
    {
        public OnActiveEventArgs()
            : base()
        {
        }
    }
    #endregion

    #region OnCloseEventArgs
    public sealed class OnCloseEventArgs : EventArgs
    {
        public OnCloseEventArgs()
            : base()
        {
        }
    }
    #endregion

    #region OnReturnEventArgs
    public sealed class OnReturnEventArgs : EventArgs
    {
        public OnReturnEventArgs(DataRowView row)
        {
            _returnRow = row;
        }

        private DataRowView _returnRow;
        public DataRowView ReturnRow
        {
            get { return _returnRow; }
        }
    }
    #endregion
}
