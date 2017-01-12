using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Reflection;

using System.Data.Odbc;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.OracleClient;
#if MySql
using MySql.Data.MySqlClient;
#endif

namespace Srvtools
{
    [ToolboxItem(true)]
    [Designer(typeof(InfoTransactionEditor), typeof(IDesigner))]
    [ToolboxBitmap(typeof(UpdateComponent), "Resources.InfoTransaction.ico")]
    public class InfoTransaction : InfoBaseComp, IInfoTransaction
    {
        #region Constructor

        public InfoTransaction(System.ComponentModel.IContainer container)
        {
            ///
            /// Required for Windows.Forms Class Composition Designer support
            ///
            container.Add(this);
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            // _timing = Timing.Insert;
            // _timing = new TimingType[] {TimingType.OnInsert, TimingType.OnUpdate, TimingType.OnDelete };
            _transactions = new TransactionCollection(this, typeof(Transaction));
            _updateComp = null;
        }

        public InfoTransaction()
        {
            ///
            /// This call is required by the Windows.Forms Designer.
            ///
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            // _timing = Timing.Insert;
            // _timing = new TimingType[] { TimingType.OnInsert, TimingType.OnUpdate, TimingType.OnDelete };
            _transactions = new TransactionCollection(this, typeof(Transaction));
            _updateComp = null;
        }

        #endregion

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        #region Dispose

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Propeties

        [Category("Infolight"),
        Description("The UpdateComponent which the control is bound to")]
        public UpdateComponent UpdateComp
        {
            set { _updateComp = value; }
            get { return _updateComp; }
        }

        //[Category("Design")]
        //public TimingType[] Timing
        //{
        //    set { _timing = value; }
        //    get { return _timing; }
        //}

        [Category("Infolight"),
        Description("Specifies the settings of transcations")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TransactionCollection Transactions
        {
            set { _transactions = value; }
            get { return _transactions; }
        }

        [Browsable(false)]
        public String Name
        {
            set
            {
                if (Site != null)
                {
                    _name = Site.Name;
                }

            }
            get { return _name; }
        }

        #endregion

        #region Events

        protected void OnBeforeTrans(InfoTransactionBeforeTransEventArgs value)
        {
            InfoTransactionBeforeTransEventHandler handler = (InfoTransactionBeforeTransEventHandler)Events[EventBeforeTrans];
            if ((handler != null) && (value is InfoTransactionBeforeTransEventArgs))
            {
                handler(this, (InfoTransactionBeforeTransEventArgs)value);
            }
        }

        public event InfoTransactionBeforeTransEventHandler BeforeTrans
        {
            add { Events.AddHandler(EventBeforeTrans, value); }
            remove { Events.RemoveHandler(EventBeforeTrans, value); }
        }

        protected void OnAfterTrans(InfoTransactionAfterTransEventArgs value)
        {
            InfoTransactionAfterTransEventHandler handler = (InfoTransactionAfterTransEventHandler)Events[EventAfterTrans];
            if ((handler != null) && (value is InfoTransactionAfterTransEventArgs))
            {
                handler(this, (InfoTransactionAfterTransEventArgs)value);
            }
        }

        public event InfoTransactionAfterTransEventHandler AfterTrans
        {
            add { Events.AddHandler(EventAfterTrans, value); }
            remove { Events.RemoveHandler(EventAfterTrans, value); }
        }

        #endregion

        #region Methods

        public Boolean Update(DataRow srcRow, DataTable srcSchema, String writeBackWherePart, IDbConnection connection, List<string> sqlSentences)
        {
            return Update(srcRow, srcSchema, writeBackWherePart, connection, sqlSentences, null);
        }

        public Boolean Update(DataRow srcRow, DataTable srcSchema, String writeBackWherePart, IDbConnection connection, List<string> sqlSentences,
            IDbTransaction dbTrans)
        {
            Boolean b = true;
            
            //Boolean isTrans = false;
            //if (srcRow.RowState == DataRowState.Added)
            //{
            //    foreach (TimingType t in _timing)
            //    {
            //        if (t == TimingType.OnInsert)
            //        { isTrans = true; break; }
            //    }
            //}
            //if (srcRow.RowState == DataRowState.Deleted)
            //{
            //    foreach (TimingType t in _timing)
            //    {
            //        if (t == TimingType.OnDelete)
            //        { isTrans = true; break; }
            //    }
            //}
            //if (srcRow.RowState == DataRowState.Modified)
            //{
            //    foreach (TimingType t in _timing)
            //    {
            //        if (t == TimingType.OnUpdate)
            //        { isTrans = true; break; }
            //    }
            //}

            //if (isTrans == false)
            //{ return b; }
            
            // order.
            // Transactions.Sort(GetTransStepComparsion());

            Boolean c = false;
            Boolean d = false;
            foreach (Transaction t in _transactions)
            {
                if (srcRow.RowState == DataRowState.Added && t.WhenInsert == false)
                    continue;
                
                if (srcRow.RowState == DataRowState.Modified && t.WhenUpdate == false)
                    continue;

                if (srcRow.RowState == DataRowState.Deleted && t.WhenDelete == false)
                    continue;
                        InfoTransactionBeforeTransEventArgs args = new InfoTransactionBeforeTransEventArgs(t);

                        OnBeforeTrans(args);
                        if (args.Abort)
                        {
                            continue;
                        }
                TransSQLBuilder transSQLBuilder = new TransSQLBuilder(t, srcRow, srcSchema, writeBackWherePart);
                List<String> transSQLs = transSQLBuilder.GetTransSQL(connection, dbTrans);

                if (transSQLs != null && transSQLs.Count != 0)
                {
                    if (c == false)
                    {
                        c = true; d = true;
                    }

                    foreach (String transSQL in transSQLs)
                    {
                        IDbCommand command = connection.CreateCommand();
                        command.CommandText = transSQL;
                        if (dbTrans != null)
                        {
                            command.Transaction = dbTrans;
                            if (command.ExecuteNonQuery() == 0)
                            { throw new Exception(); /*b = false; break; */}
                        }
                        else
                        {
                            if (command.ExecuteNonQuery() == 0)
                            { b = false; break; }
                        }
                    }
                }

                String writeSQL = null;
                if (srcRow.RowState != DataRowState.Deleted)
                {
                    writeSQL = transSQLBuilder.GetWriteBackSQL(connection, dbTrans);
                }

                if (!string.IsNullOrEmpty(writeSQL))
                {
                    sqlSentences.Add(writeSQL);
                }

                if (writeSQL != null && writeSQL.Length != 0)
                {
                    if (c == false)
                    {
                        //InfoTransactionBeforeTransEventArgs args = new InfoTransactionBeforeTransEventArgs(t);
                        //OnBeforeTrans(args);
                        //if (args.Abort)
                        //{
                        //    continue;
                        //}
                        c = true; d = true;
                    }

                    IDbCommand command = connection.CreateCommand();
                    command.CommandText = writeSQL;

                    if (dbTrans != null) {
                        command.Transaction = dbTrans;
                        if (command.ExecuteNonQuery() == 0)
                        { throw new Exception(); /*b = false; break; */}
                    }
                    else {
                        if (command.ExecuteNonQuery() == 0)
                        { b = false; break; }
                    }
                }
            }

            if (d == true) {
                OnAfterTrans(new InfoTransactionAfterTransEventArgs());
            }

            return b;
        }

        // Order transaction by transstep.
        private Comparison<Transaction> GetTransStepComparsion()
        {
            Comparison<Transaction> comparison = new Comparison<Transaction>(GetTransStepRule);

            return comparison;
        }

        // Get transstep order rule.
        private Int32 GetTransStepRule(Transaction transaction1, Transaction transaction2)
        {
            if (transaction1.TransStep >= transaction2.TransStep)
            {
                return transaction1.TransStep;
            }
            else
            {
                return transaction2.TransStep;
            }
        }

        public override string ToString()
        {
            return this.Site.Name;
        }

        #endregion

        #region Vars

        private System.ComponentModel.Container components = null;
        // private Timing _timing;
        //private TimingType[] _timing;
        private UpdateComponent _updateComp;
        private TransactionCollection _transactions;
        private String _name;

        internal static readonly object EventBeforeTrans = new object();
        internal static readonly object EventAfterTrans = new object();

        #endregion
    }

    //public enum TimingType
    //{
    //    /// <summary>
    //    /// Insert flag.
    //    /// </summary>
    //    OnInsert = 0,

    //    /// <summary>
    //    /// Update flag.
    //    /// </summary>
    //    OnUpdate = 1,

    //    /// <summary>
    //    /// Delete flag.
    //    /// </summary>
    //    OnDelete = 2
    //}
}
