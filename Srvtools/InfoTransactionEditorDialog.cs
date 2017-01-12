using System;
using System.Drawing;
using System.Reflection;
using System.Collections;
using System.ComponentModel;

using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;

#if MySql
using MySql.Data.MySqlClient;
#endif

using System.Data;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace Srvtools
{
    #region InfoTransactionEditorDialog
    public partial class InfoTransactionEditorDialog : Form
    {
        IDesignerHost DesignerHost = null;

        InfoTransaction transaction = null;
        InfoTransaction transPrivateCopy = null;

        SrcTablePanel srcTablePanel = null;
        ArrayList TransTablePanels = null;
        ArrayList LinkPointsList = null;

        public string SrcTableName = "";

        public InfoTransactionEditorDialog(InfoTransaction trans, IDesignerHost host)
        {
            transaction = trans;
            DesignerHost = host;

            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InfoTransactionEditorDialog_Load(object sender, EventArgs e)
        {
            // Copy the editing InfoTransaction
            //
            SetInfoTransactionPrivateCopy();

            // Set up UI
            //
            SetUpUI();
        }


        private void SetInfoTransactionPrivateCopy()
        {
            transPrivateCopy = new InfoTransaction();

            // transPrivateCopy.Timing = transaction.Timing;

            foreach (Transaction trans in transaction.Transactions)
            {
                Transaction t = new Transaction();
                t.AutoNumber = trans.AutoNumber;
                t.Name = trans.Name;

                foreach (TransField field in trans.TransFields)
                {
                    TransField tf = new TransField();
                    tf.DesField = field.DesField;
                    tf.DesValue = field.DesValue;
                    tf.FieldType = field.FieldType;
                    tf.SrcField = field.SrcField;
                    tf.SrcGetValue = field.SrcGetValue;
                    tf.SrcValue = field.SrcValue;
                    tf.UpdateMode = field.UpdateMode;

                    t.TransFields.Add(tf);
                }

                foreach (TransKeyField keyField in trans.TransKeyFields)
                {
                    TransKeyField tkf = new TransKeyField();
                    tkf.DesField = keyField.DesField;
                    tkf.FieldType = keyField.FieldType;
                    tkf.SrcField = keyField.SrcField;
                    tkf.SrcGetValue = keyField.SrcGetValue;
                    tkf.SrcValue = keyField.SrcValue;
                    tkf.WhereMode = keyField.WhereMode;

                    t.TransKeyFields.Add(tkf);
                }
                t.TransMode = trans.TransMode;
                t.TransStep = trans.TransStep;
                t.TransTableName = trans.TransTableName;
                t.WhenDelete = trans.WhenDelete;
                t.WhenInsert = trans.WhenInsert;
                t.WhenUpdate = trans.WhenUpdate;

                transPrivateCopy.Transactions.Add(t);
            }

            transPrivateCopy.UpdateComp = transaction.UpdateComp;
        }

        private void SetUpUI()
        {
            // Set up Timing Selection
            //
            // SetTimingSelection();

            // Get SrcTableName
            //
            if (transPrivateCopy.UpdateComp != null
             && transPrivateCopy.UpdateComp.SelectCmd != null
             && transPrivateCopy.UpdateComp.SelectCmd.CommandText != null
                         && transPrivateCopy.UpdateComp.SelectCmd.InfoConnection != null
                         )
            {
                string cmdText = transPrivateCopy.UpdateComp.SelectCmd.CommandText;//No need tolower
                IDbConnection conn = transPrivateCopy.UpdateComp.SelectCmd.InfoConnection;

                DataTable t = GetSchema(conn, cmdText);
                if (t != null)
                {
                    this.SrcTableName = GetTableName(t);
                }
                else
                {
                    this.SrcTableName = "";
                }
            }
            else
            {
                this.SrcTableName = "";
            }

            // Set Transactions view
            //
            this.pnlTableView.Controls.Clear();

            this.srcTablePanel = new SrcTablePanel(this.transPrivateCopy, this.SrcTableName, this);
            srcTablePanel.Location = new Point(30, 10);
            // Event to move scrTablePanel 
            this.srcTablePanel.MouseMove += new MouseEventHandler(scrTablePanel_MouseMove);
            this.srcTablePanel.MouseDown += new MouseEventHandler(scrTablePanel_MouseDown);
            this.srcTablePanel.MouseUp += new MouseEventHandler(scrTablePanel_MouseUp);
            this.pnlTableView.Controls.Add(srcTablePanel);
            this.srcTablePanel.ResizePanel();

            this.TransTablePanels = new ArrayList();

            for (int i = 0; i < this.transPrivateCopy.Transactions.Count; ++i)
            {
                TablePanel tp = new TablePanel(this.transPrivateCopy.Transactions[i], this.DesignerHost, this);

                tp.Location = new Point(this.srcTablePanel.Location.X + this.srcTablePanel.Width + 100 - i * 5,
                 this.srcTablePanel.Location.Y + i * (30 + 10 + 60));

                // Event to Select or Move TransTable
                tp.MouseMove += new MouseEventHandler(tp_MouseMove);
                tp.MouseDown += new MouseEventHandler(tp_MouseDown);
                tp.MouseUp += new MouseEventHandler(tp_MouseUp);

                this.pnlTableView.Controls.Add(tp);
                this.TransTablePanels.Add(tp);
            }
        }

        // ---------------------------------------------------------------------
        // Added by yangdong
        private string GetTableName(DataTable shema)
        {
            return shema.Rows[0]["BaseTableName"].ToString();
        }

        private DataTable _schema;
        private DataTable GetSchema(IDbConnection conn, string sSql)
        {
            if (_schema != null)
            {
                return _schema;
            }
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sSql;
            IDbDataAdapter adapter = DBUtils.CreateDbDataAdapter(cmd);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            IDataReader dr = adapter.SelectCommand.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo);

            DataTable schema = dr.GetSchemaTable();
            _schema = schema;
            if (conn.State == ConnectionState.Open)
                conn.Close();

            dr.Close();
            return schema;
        }

        // ---------------------------------------------------------------------

        int TablePanelX, TablePanelY;

        bool panelmove = false;
        void tp_MouseUp(object sender, MouseEventArgs e)
        {
            TablePanel tp = sender as TablePanel;
            if (tp == null)
            {
                return;
            }
            if (panelmove == true)
            {
                if (tp.Location.X + e.X - TablePanelX >= 0
                        && tp.Location.Y + e.Y - TablePanelY >= 0
                    &&
                    (tp.Location.X + e.X - TablePanelX > srcTablePanel.Location.X + srcTablePanel.Width
                    || tp.Location.Y + e.Y - TablePanelY > srcTablePanel.Location.Y + srcTablePanel.Height))
                {
                    tp.Location = new Point(tp.Location.X + e.X - TablePanelX,
                        tp.Location.Y + e.Y - TablePanelY);
                    this.pnlTableView.Refresh();
                }
                panelmove = false;
            }
            tp.Cursor = Cursors.Arrow;
        }

        void tp_MouseDown(object sender, MouseEventArgs e)
        {
            TablePanel tp = sender as TablePanel;

            if (tp == null)
            {
                return;
            }
            if (e.X > 12 && e.X <= tp.Size.Width - 5)
            {
                tp.Cursor = Cursors.SizeAll;
                TablePanelX = e.X;
                TablePanelY = e.Y;
                panelmove = true;
            }
            UnSelectAllTransTablePanels();
            tp.Selected = true;
        }

        private void UnSelectAllTransTablePanels()
        {
            foreach (TablePanel tp in this.TransTablePanels)
            {
                tp.Selected = false;
            }
        }

        void tp_MouseMove(object sender, MouseEventArgs e)
        {
        }

        void tp_Click(object sender, EventArgs e)
        {
            (sender as TablePanel).Selected = true;
        }

        //int scrTablePanelX = 0, scrTablePanelY = 0;

        void scrTablePanel_MouseUp(object sender, MouseEventArgs e)
        {
            //if (this.scrTablePanel.Cursor != Cursors.SizeAll)
            //{
            //    return;
            //}
            //if (this.scrTablePanel.Location.X + e.X - scrTablePanelX >= 0
            //    && this.scrTablePanel.Location.Y + e.Y - scrTablePanelY >= 0)
            //{
            //    this.scrTablePanel.Location = new Point(this.scrTablePanel.Location.X + e.X - scrTablePanelX,
            //        this.scrTablePanel.Location.Y + e.Y - scrTablePanelY);
            //    this.pnlTableView.Refresh();
            //}
            //scrTablePanel.Cursor = Cursors.Arrow;
        }

        void scrTablePanel_MouseDown(object sender, MouseEventArgs e)
        {
            UnSelectAllTransTablePanels();
            //if (e.X < this.scrTablePanel.Width - 14)
            //{
            //    scrTablePanel.Cursor = Cursors.SizeAll;
            //    scrTablePanelX = e.X;
            //    scrTablePanelY = e.Y;
            //}
        }

        void scrTablePanel_MouseMove(object sender, MouseEventArgs e)
        {
        }

        //private void SetTimingSelection()
        //{
        //    // Modify By Chenjian 2005-12-19
        //    // For the type of transPrivateCopy.Timing changes from Timing to TimingType[]

        //    //this.cbDelete.Checked = false;
        //    //this.cbInsert.Checked = false;
        //    //this.cbUpdate.Checked = false;

        //    //switch (transPrivateCopy.Timing)
        //    //{
        //    //    case Timing.Delete:
        //    //        this.cbDelete.Checked = true;
        //    //        break;
        //    //    case Timing.Insert:
        //    //        this.cbInsert.Checked = true;
        //    //        break;
        //    //    case Timing.Update:
        //    //        this.cbUpdate.Checked = true;
        //    //        break;
        //    //}

        //    foreach (TimingType tt in transPrivateCopy.Timing)
        //    {
        //        if (tt == TimingType.OnDelete)
        //        {
        //            this.cbDelete.Checked = true;
        //        }
        //        else if (tt == TimingType.OnInsert)
        //        {
        //            this.cbInsert.Checked = true;
        //        }
        //        else if (tt == TimingType.OnUpdate)
        //        {
        //            this.cbUpdate.Checked = true;
        //        }
        //    }

        //    // End Modify
        //}

        private void cbUpdate_MouseClick(object sender, MouseEventArgs e)
        {
            // Modify By Chenjian 2005-12-19
            // For the type of transPrivateCopy.Timing changes from Timing to TimingType[]

            //transPrivateCopy.Timing = Timing.Update;
            //SetTimingSelection();

            // SetUpTiming();

            // End Modify
        }

        //private void SetUpTiming()
        //{
        //    int timingCount = 0;
        //    if (cbDelete.Checked)
        //    {
        //        timingCount++;
        //    }
        //    if (cbInsert.Checked)
        //    {
        //        timingCount++;
        //    }
        //    if (cbUpdate.Checked)
        //    {
        //        timingCount++;
        //    }

        //    transPrivateCopy.Timing = new TimingType[timingCount];

        //    timingCount = 0;
        //    if (cbDelete.Checked)
        //    {
        //        transPrivateCopy.Timing[timingCount] = TimingType.OnDelete;
        //        timingCount++;
        //    }
        //    if (cbInsert.Checked)
        //    {
        //        transPrivateCopy.Timing[timingCount] = TimingType.OnInsert;
        //        timingCount++;
        //    }
        //    if (cbUpdate.Checked)
        //    {
        //        transPrivateCopy.Timing[timingCount] = TimingType.OnUpdate;
        //        timingCount++;
        //    }
        //}

        private void cbInsert_MouseClick(object sender, MouseEventArgs e)
        {
            // Modify By Chenjian 2005-12-19
            // For the type of transPrivateCopy.Timing changes from Timing to TimingType[]

            //transPrivateCopy.Timing = Timing.Insert;
            //SetTimingSelection();
            // SetUpTiming();

            // End Modify
        }

        private void cbDelete_MouseClick(object sender, MouseEventArgs e)
        {
            // Modify By Chenjian 2005-12-19
            // For the type of transPrivateCopy.Timing changes from Timing to TimingType[]

            //transPrivateCopy.Timing = Timing.Delete;
            //SetTimingSelection();
            // SetUpTiming();

            // End Modify
        }

        private void pnlTableView_Click(object sender, EventArgs e)
        {
            try
            {
                UnSelectAllTransTablePanels();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Transaction trans = new Transaction();
            int currentTransCount = this.transPrivateCopy.Transactions.Count;

            if (currentTransCount == 0)
            {
                trans.TransStep = 1;
                trans.TransTableName = "TransTableName1";
            }
            else
            {
                trans.TransStep = this.transPrivateCopy.Transactions[currentTransCount - 1].TransStep + 1;
                trans.TransTableName = "TransTableName" + trans.TransStep;
            }
            this.transPrivateCopy.Transactions.Add(trans);

            TablePanel tp = new TablePanel(trans, this.DesignerHost, this);
            tp.Location = new Point(this.srcTablePanel.Location.X + this.srcTablePanel.Width + 100 - 5 * currentTransCount,
                this.srcTablePanel.Location.Y + currentTransCount * (30 + 10 + 60));

            // Event to Select or Move TransTable
            tp.MouseMove += new MouseEventHandler(tp_MouseMove);
            tp.MouseDown += new MouseEventHandler(tp_MouseDown);
            tp.MouseUp += new MouseEventHandler(tp_MouseUp);

            this.pnlTableView.Controls.Add(tp);
            this.TransTablePanels.Add(tp);

            this.srcTablePanel.ResizePanel();
            this.pnlTableView.Refresh();

        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                // Remove the selected TransTable
                //
                for (int i = 0; i < this.TransTablePanels.Count; ++i)
                {
                    if ((this.TransTablePanels[i] as TablePanel).Selected)
                    {
                        this.pnlTableView.Controls.Remove(this.TransTablePanels[i] as Control);
                        this.TransTablePanels.RemoveAt(i);
                        this.transPrivateCopy.Transactions.RemoveAt(i);

                        this.srcTablePanel.ResizePanel();
                        this.pnlTableView.Refresh();

                        break;
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            IComponentChangeService ComponentChangeService =
                this.DesignerHost.GetService(typeof(IComponentChangeService)) as IComponentChangeService;

            // Copy from transPrivateCopy to transaction

            object oldValue = null;
            object newValue = null;

            // Timing
            //if (transaction.Timing != transPrivateCopy.Timing)
            //{
            //    PropertyDescriptor descTiming = TypeDescriptor.GetProperties(this.transaction)["Timing"];
            //    ComponentChangeService.OnComponentChanging(this.transaction, descTiming);
            //    oldValue = transaction.Timing;
            //    transaction.Timing = transPrivateCopy.Timing;
            //    newValue = transaction.Timing;
            //    ComponentChangeService.OnComponentChanged(this.transaction, descTiming, oldValue, newValue);
            //}

            // Transactions
            PropertyDescriptor descTransactions = TypeDescriptor.GetProperties(this.transaction)["Transactions"];
            ComponentChangeService.OnComponentChanging(this.transaction, descTransactions);
            oldValue = transaction.Transactions;
            transaction.Transactions = transPrivateCopy.Transactions;
            newValue = transaction.Transactions;
            ComponentChangeService.OnComponentChanged(this.transaction, descTransactions, oldValue, newValue);

            // UpdateComp
            if (transaction.UpdateComp != transPrivateCopy.UpdateComp)
            {
                PropertyDescriptor descUpdateComp = TypeDescriptor.GetProperties(this.transaction)["UpdateComp"];
                ComponentChangeService.OnComponentChanging(this.transaction, descUpdateComp);
                oldValue = transaction.UpdateComp;
                transaction.UpdateComp = transPrivateCopy.UpdateComp;
                newValue = transaction.UpdateComp;
                ComponentChangeService.OnComponentChanged(this.transaction, descUpdateComp, oldValue, newValue);
            }

            this.Close();
        }

        private void pnlTableView_Paint(object sender, PaintEventArgs e)
        {
            // Set up links
            if (this.LinkPointsList == null)
            {
                this.LinkPointsList = new ArrayList();
            }
            else
            {
                this.LinkPointsList.Clear();
            }

            int count = 2;
            for (int i = 0; i < this.TransTablePanels.Count; ++i)
            {
                TablePanel tp = this.TransTablePanels[i] as TablePanel;
                ArrayList points = new ArrayList();


                Point StartPoint = new Point(this.srcTablePanel.Location.X + this.srcTablePanel.Width,
                    this.srcTablePanel.Location.Y + 36 + i * 52 + 13);
                Point EndPoint = new Point(tp.Location.X, tp.Location.Y + 15);

                // Start Point of the link
                points.Add(StartPoint);

                // Points Along the link

                if (EndPoint.X >= srcTablePanel.Location.X
                    && EndPoint.X <= srcTablePanel.Location.X + srcTablePanel.Width
                    && EndPoint.Y >= srcTablePanel.Location.Y
                    && EndPoint.Y <= srcTablePanel.Location.Y + srcTablePanel.Height)
                {
                }
                else if (EndPoint.X > StartPoint.X)
                {
                    points.Add(new Point((StartPoint.X + EndPoint.X) / 2, StartPoint.Y));
                    points.Add(new Point((StartPoint.X + EndPoint.X) / 2, EndPoint.Y));
                }
                else if (EndPoint.X < StartPoint.X
                    && tp.Location.Y > srcTablePanel.Location.Y + srcTablePanel.Height)
                {
                    points.Add(new Point(StartPoint.X + count * 5, StartPoint.Y));
                    points.Add(new Point(StartPoint.X + count * 5,
                        (srcTablePanel.Location.Y + srcTablePanel.Height + tp.Location.Y) / 2));
                    points.Add(new Point(EndPoint.X - 10,
                        (srcTablePanel.Location.Y + srcTablePanel.Height + tp.Location.Y) / 2));
                    points.Add(new Point(EndPoint.X - 10, EndPoint.Y));

                    count++;
                }

                // End Point of the link
                points.Add(EndPoint);
                this.LinkPointsList.Add(points);
            }

            Pen pen = new Pen(Brushes.Gray, 1.0f);
            foreach (ArrayList points in LinkPointsList)
            {
                for (int i = 1; i < points.Count; ++i)
                {
                    e.Graphics.DrawLine(pen, (Point)points[i - 1], (Point)points[i]);
                }
            }
        }

        private void pnlTableView_Scroll(object sender, ScrollEventArgs e)
        {
            pnlTableView.Refresh();
        }

        private void cbdetail_CheckedChanged(object sender, EventArgs e)
        {
            pnlTableView.Refresh();
        }
    }
    #endregion InfoTransactionEditorDialog

    #region SrcTablePanel
    [ToolboxItem(false)]
    internal class SrcTablePanel : Panel
    {
        public InfoTransaction transaction = null;
        public string SrcTableName = "";
        private Form frminfotransaction = null;
        private int intPannelWidth = 0;


        public SrcTablePanel(InfoTransaction trans, string scrTableName, Form frm)
        {
            this.transaction = trans;
            this.SrcTableName = scrTableName;
            this.BackColor = Color.Wheat;
            frminfotransaction = frm;
            intPannelWidth = ResizePanel();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //ResizePanel();

            Font font = new Font("Arial", 12.0f);
            Pen pen = new Pen(Brushes.Gray, 1.0f);


            e.Graphics.DrawString(this.SrcTableName, font, Brushes.Black, 10 + (this.Size.Width - intPannelWidth) / 2, 10);

            e.Graphics.DrawLine(pen, 0, 0, 0, this.Height - 1);
            e.Graphics.DrawLine(pen, 0, this.Height - 1, this.Width - 1 - 7, this.Height - 1);
            e.Graphics.DrawLine(pen, this.Width - 1 - 7, this.Height - 1, this.Width - 1 - 7, 0);
            e.Graphics.DrawLine(pen, this.Width - 1 - 7, 0, 0, 0);

            e.Graphics.FillRectangle(Brushes.Lavender, this.Width - 7, 0, this.Width - 1, this.Height);
            for (int i = 0; i < this.transaction.Transactions.Count; ++i)
            {
                e.Graphics.DrawLine(pen, 0, 36 + i * 52, this.Width - 1 - 7, 36 + i * 52);
                e.Graphics.DrawString(this.transaction.Transactions[i].TransStep.ToString("00"), font, Brushes.Black, this.Width / 2 - 20, 36 + 52 * i + 5);

                if (((CheckBox)frminfotransaction.Controls["cbdetail"]).Checked == true)
                {
                    if (((Transaction)this.transaction.Transactions[i]).TransKeyFields.Count > 0)
                    {
                        string strTranskf = "";
                        strTranskf = ((TransKeyField)((Transaction)this.transaction.Transactions[i]).TransKeyFields[0]).DesField
                        + " = " + ((TransKeyField)((Transaction)this.transaction.Transactions[i]).TransKeyFields[0]).SrcField;

                        strTranskf += " ";
                        Size TextSize = new Size();
                        do
                        {
                            strTranskf = strTranskf.Substring(0, strTranskf.Length - 1);
                            TextSize = TextRenderer.MeasureText(strTranskf, new Font("Arial", 6.5f));

                        } while (TextSize.Width + 2 > this.Size.Width);


                        e.Graphics.DrawString(strTranskf, new Font("Arial", 6.5f), Brushes.Black, 2, 36 + 52 * i + 5 + 26);
                    }
                }
                e.Graphics.FillPie(Brushes.Gray, new Rectangle(this.Width - 1 - 14, 36 + 6 + i * 52, 14, 14), 0.0f, 360);
            }
        }

        public int ResizePanel()
        {
            Size TextSize = TextRenderer.MeasureText(this.SrcTableName, new Font("Arial", 12.0f));

            if (this.SrcTableName == "")
            {
                this.Size = new Size(TextSize.Width + 20 + 7 + (40), 36 + 52 * this.transaction.Transactions.Count);
            }
            else
            {
                this.Size = new Size(TextSize.Width + 20 + 7, 36 + 52 * this.transaction.Transactions.Count);
            }
            return this.Size.Width;
        }


        private int X_down = 0;
        private int Y_down = 0;
        private bool mousedown = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                if (e.X > this.Width - 14 && e.Y > 36)
                {
                    // Find the position the mouse
                    int i = 0;
                    for (i = 0; i < this.transaction.Transactions.Count; ++i)
                    {
                        if (e.Y - 36 - i * 52 >= 5 && e.Y - 36 - i * 52 <= 14 + 5)
                        {
                            break;
                        }
                    }
                    if (i < this.transaction.Transactions.Count)
                    {
                        InfoTransactionEditorKeyFieldsDialog itekfd =
                            new InfoTransactionEditorKeyFieldsDialog(this.transaction.Transactions[i], this.transaction.UpdateComp, this.transaction.Transactions[i].TransTableName
                            , ((InfoTransactionEditorDialog)frminfotransaction).SrcTableName);
                        itekfd.ShowDialog();
                        this.Refresh();
                    }
                }
                else if (e.X > this.Width - 14 && e.Y < 36)
                {
                    this.Cursor = Cursors.SizeAll;
                    X_down = e.X;
                    Y_down = e.Y;
                    mousedown = true;
                }



            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (mousedown == true)
            {
                this.Cursor = Cursors.Arrow;
                if (this.Size.Width + e.X - X_down >= intPannelWidth && this.Size.Width + e.X - X_down <= intPannelWidth + 50)
                {
                    this.Size = new Size(this.Size.Width + e.X - X_down, this.Size.Height);
                }
                else if (this.Size.Width + e.X - X_down < intPannelWidth)
                {
                    this.Size = new Size(intPannelWidth, this.Size.Height);
                }
                else
                {
                    this.Size = new Size(intPannelWidth + 50, this.Size.Height);

                }

                mousedown = false;




            }
            this.Refresh();
            this.Parent.Refresh();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (e.X > this.Width - 14 && e.Y < 36)
            {
                this.Size = new Size(intPannelWidth, this.Size.Height);
                mousedown = false;
            }
            this.Refresh();
            this.Parent.Refresh();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button != MouseButtons.Left)
            {
                if (e.X > this.Width - 14 && e.Y < 36)
                {
                    this.Cursor = Cursors.VSplit;
                }

                else if (e.X > this.Width - 14 && e.Y > 36)
                {
                    int i = 0;
                    for (i = 0; i < this.transaction.Transactions.Count; ++i)
                    {
                        if (e.Y - 36 - i * 52 >= 5 && e.Y - 36 - i * 52 <= 14 + 5)
                        {
                            break;
                        }
                    }
                    if (i < this.transaction.Transactions.Count)
                    {
                        this.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        this.Cursor = Cursors.Arrow;
                    }
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                }
            }
        }
    }
    #endregion SrcTablePanel

    #region TablePanel
    [ToolboxItem(false)]
    internal class TablePanel : Panel
    {
        public Transaction transaction;
        public IDesignerHost DesignerHost = null;
        private Form frminfotransaction = null;
        private RichTextBox rtbfield = new RichTextBox();
        private int intPannelWidth = 0;



        bool selected = false;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (this.selected == value)
                {
                    return;
                }
                this.selected = value;
                this.Refresh();
            }
        }

        public TablePanel(Transaction trans, IDesignerHost host, Form frm)
        {
            this.transaction = trans;
            DesignerHost = host;
            this.BackColor = Color.Pink;
            frminfotransaction = frm;
            this.Controls.Add(rtbfield);
            this.rtbfield.WordWrap = false;
            this.rtbfield.Text = "";
            this.rtbfield.ReadOnly = true;
            intPannelWidth = InitialPanelWidth();


            foreach (TransField field in this.transaction.TransFields)
            {
                this.rtbfield.Text += field.DesField + UpdateModeToString(field.UpdateMode) + field.SrcField + "\n";
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ResizePanel();

            Font font = new Font("Arial", 12.0f);
            Pen pen = new Pen(Brushes.Gray, 1.0f);
            Pen selPen = new Pen(Brushes.Blue, 1.0f);
            Pen unSelPen = new Pen(Brushes.Lavender, 1.0f);

            e.Graphics.DrawString(transaction.TransTableName,
                font, Brushes.Black, 12 + 5 + (this.Width - intPannelWidth) / 2, 1 + 5);

            e.Graphics.DrawLine(pen, 12, 1, this.Width - 1 - 1, 1);
            e.Graphics.DrawLine(pen, this.Width - 1 - 1, 1, this.Width - 1 - 1, this.Height - 1 - 1);
            e.Graphics.DrawLine(pen, this.Width - 1 - 1, this.Height - 1 - 1, 12, this.Height - 1 - 1);
            e.Graphics.DrawLine(pen, 12, this.Height - 1 - 1, 12, 1);

            if (this.Selected)
            {
                e.Graphics.DrawLine(selPen, 11, 0, this.Width - 1, 0);
                e.Graphics.DrawLine(selPen, this.Width - 1, 0, this.Width - 1, this.Height - 1);
                e.Graphics.DrawLine(selPen, this.Width - 1, this.Height - 1, 11, this.Height - 1);
                e.Graphics.DrawLine(selPen, 11, this.Height - 1, 11, 0);
            }
            else
            {
                e.Graphics.DrawLine(unSelPen, 11, 0, this.Width - 1, 0);
                e.Graphics.DrawLine(unSelPen, this.Width - 1, 0, this.Width - 1, this.Height - 1);
                e.Graphics.DrawLine(unSelPen, this.Width - 1, this.Height - 1, 11, this.Height - 1);
                e.Graphics.DrawLine(unSelPen, 11, this.Height - 1, 11, 0);
            }

            e.Graphics.FillRectangle(Brushes.Lavender, 0, 0, 11, this.Height);

            e.Graphics.FillPolygon(Brushes.Gray, new Point[] { new Point(0, 8), new Point(0, 22), new Point(12, 15) });
        }

        private int ResizePanel()
        {
            Size TextSize = TextRenderer.MeasureText(transaction.TransTableName + " ", new Font("Arial", 12.0f));
            if (((CheckBox)frminfotransaction.Controls["cbdetail"]).Checked == false)
            {
                this.Size = new Size(this.Size.Width, TextSize.Height + 10 + 2);
                if (this.rtbfield.Visible == true)
                    this.rtbfield.Visible = false;
            }
            else
            {
                this.Size = new Size(this.Size.Width, TextSize.Height + 10 + 2 + 60);
                this.rtbfield.Location = new Point(13, TextSize.Height + 10 + 2);
                this.rtbfield.Size = new Size(this.Width - 15, 58);
                if (this.rtbfield.Visible == false)
                {
                    this.rtbfield.Visible = true;
                }
            }

            return this.Width;
        }

        private int InitialPanelWidth()
        {
            Size TextSize = TextRenderer.MeasureText(transaction.TransTableName + " ", new Font("Arial", 12.0f));
            this.Size = new Size(12 + TextSize.Width + 10 + 1, TextSize.Height + 10 + 2);
            return this.Width;
        }


        private int X_down = 0;
        private int Y_down = 0;
        private bool mousedown = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                if (e.X < 12 && e.Y > 8 && e.Y < 22)
                {
                    InfoTransactionEditorTransFieldsDialog itetfd =
                        new InfoTransactionEditorTransFieldsDialog(this.transaction, ((InfoTransaction)this.transaction.Owner).UpdateComp, this.transaction.TransTableName
                        , ((InfoTransactionEditorDialog)frminfotransaction).SrcTableName);
                    itetfd.ShowDialog();

                    this.rtbfield.Text = "";
                    foreach (TransField field in this.transaction.TransFields)
                    {
                        this.rtbfield.Text += field.DesField + UpdateModeToString(field.UpdateMode) + field.SrcField + "\n";
                    }
                }
                else if (e.X > this.Width - 5)
                {
                    this.Cursor = Cursors.SizeAll;
                    X_down = e.X;
                    Y_down = e.Y;
                    mousedown = true;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (mousedown == true)
            {
                this.Cursor = Cursors.Arrow;
                if (this.Size.Width + e.X - X_down >= intPannelWidth && this.Size.Width + e.X - X_down <= intPannelWidth + 130)
                {
                    this.Size = new Size(this.Size.Width + e.X - X_down, this.Size.Height);
                }
                else if (this.Size.Width + e.X - X_down < intPannelWidth)
                {
                    this.Size = new Size(intPannelWidth, this.Size.Height);
                }
                else
                {
                    this.Size = new Size(intPannelWidth + 130, this.Size.Height);

                }

                mousedown = false;
            }
            this.Refresh();
            this.Parent.Refresh();




        }

        private string UpdateModeToString(UpdateMode um)
        {
            string strvalue = "";
            switch (um)
            {
                case UpdateMode.Dec: strvalue = " - "; break;
                case UpdateMode.Disable: strvalue = " X "; break;
                case UpdateMode.Inc: strvalue = " + "; break;
                case UpdateMode.Replace: strvalue = " = "; break;
                case UpdateMode.WriteBack: strvalue = " <- "; break;
                default: break;
            }
            return strvalue;

        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (e.X > this.Width - 5)
            {
                this.Size = new Size(intPannelWidth, this.Size.Height);
                mousedown = false;
            }
            else
            {
                InfoTransactionEditorTTransactionDialog itettd =
                  new InfoTransactionEditorTTransactionDialog(this.transaction, this.DesignerHost
                  , ((InfoTransactionEditorDialog)frminfotransaction).SrcTableName);
                itettd.ShowDialog();
                this.rtbfield.Text = "";
                foreach (TransField field in this.transaction.TransFields)
                {
                    this.rtbfield.Text += field.DesField + UpdateModeToString(field.UpdateMode) + field.SrcField + "\n";
                }
                intPannelWidth = InitialPanelWidth();
                this.Refresh();
            }
        }

        //protected override void OnDoubleClick(MouseEventArgs e)
        //{
        //    base.OnDoubleClick(e);



        //    InfoTransactionEditorTTransactionDialog itettd = 
        //        new InfoTransactionEditorTTransactionDialog(this.transaction, this.DesignerHost);
        //    itettd.ShowDialog();
        //    this.rtbfield.Text = "";
        //    foreach (TransField field in this.transaction.TransFields)
        //    {
        //        this.rtbfield.Text += field.DesField + UpdateModeToString(field.UpdateMode) + field.SrcField + "\n";
        //    }

        //}

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button != MouseButtons.Left && e.Button != MouseButtons.Right)
            {
                if (e.X > this.Width - 5)
                {
                    this.Cursor = Cursors.VSplit;
                }
                else if (e.X < 12 && e.Y > 8 && e.Y < 22)
                {
                    this.Cursor = Cursors.Hand;
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                }
            }
        }
    }
    #endregion TablePanel
}
