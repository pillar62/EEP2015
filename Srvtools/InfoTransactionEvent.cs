using System;
using System.Collections.Generic;
using System.Text;

namespace Srvtools
{
    public delegate void InfoTransactionBeforeTransEventHandler(object sender, InfoTransactionBeforeTransEventArgs e);

    public delegate void InfoTransactionAfterTransEventHandler(object sender, InfoTransactionAfterTransEventArgs e);


    public sealed class InfoTransactionBeforeTransEventArgs : EventArgs
    {
        public InfoTransactionBeforeTransEventArgs(Transaction trans)
            : base()
        {
            _Transaction = trans;
        }

        private Transaction _Transaction;
        public Transaction Transaction
        {
            get { return _Transaction; }
        }
	
        
        private bool _Abort;
        public bool Abort
	    {
            get { return _Abort; }
            set { _Abort = value; }
	    }
    }

    public sealed class InfoTransactionAfterTransEventArgs : EventArgs
    {
        public InfoTransactionAfterTransEventArgs()
            : base()
        {
        }
    }
}
