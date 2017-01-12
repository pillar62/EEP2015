using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Srvtools
{
    [ToolboxItem(false)]
    public partial class InfoRefValForGrid : UserControl
    {
        public InfoRefValForGrid()
        {
            InitializeComponent();

            this.InButton.Paint += new PaintEventHandler(InButton_Paint);
            this.Load += new EventHandler(InfoRefValForGrid_Load);
        }

        private void InButton_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            g.DrawString("...", new Font("SimSun", 7), System.Drawing.Brushes.Black, new Point(11, 8), sf);
        }

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);
        //    if (e.KeyCode == Keys.F4)
        //    {
        //        this.InButton.PerformClick();
        //    }
        //}

        public int MaxLength
        {
            get { return InnerTextBox.MaxLength; }
            set { InnerTextBox.MaxLength = value; }
        }
	

        private void InfoRefValForGrid_Load(object sender, EventArgs e)
        {
            this.InTextBox.Focus();
            if (this.Parent != null && this.Parent.Parent != null)
                this.Parent.Parent.KeyDown += new KeyEventHandler(Parent_KeyDown);
        }

        public TextBox InnerTextBox
        {
            get
            {
                return this.InTextBox;
            }
        }

        public Button InnerButton
        {
            get
            {
                return this.InButton;
            }
        }

        private InfoRefVal _RefVal;
        public InfoRefVal RefVal
        {
            get
            {
                return _RefVal;
            }
            set
            {
                _RefVal = value;
            }
        }

        private void InButton_Click(object sender, EventArgs e)
        {
            frmDGVGridRefVal frmDGVGridRefVal = new frmDGVGridRefVal();
            //DataSource
            if (this.RefVal.SelectCommand != null && this.RefVal.SelectCommand != "" )
            {
                string strwhere = this.RefVal.WhereString(this.RefVal.SelectCommand);
                if (this.RefVal.AlwaysClose || strwhere.Length > 0)
                {
                    this.RefVal.InnerDs.RemoteName = "GLModule.cmdRefValUse";
                    this.RefVal.InnerDs.Execute(CliUtils.InsertWhere(this.RefVal.SelectCommand, strwhere), true);
                }
                frmDGVGridRefVal.DataSource = this.RefVal.InnerBs;
            }
            else if (this.RefVal.DataSource is InfoBindingSource)
            {
                InfoDataSet ids = ((InfoBindingSource)this.RefVal.DataSource).GetDataSource() as InfoDataSet;
                string sql = DBUtils.GetCommandText((InfoBindingSource)this.RefVal.DataSource);
                string strwhere = this.RefVal.WhereString(sql);
                if (ids.AlwaysClose || strwhere.Length > 0)
                {
                    ids.SetWhere(strwhere);
                }
                InfoBindingSource refBinding = (InfoBindingSource)this.RefVal.DataSource;
                frmDGVGridRefVal.DataSource = refBinding;
            }
            else
            {
                return;
            }
            //DataMember
            frmDGVGridRefVal.DataMember = this.RefVal.GetDataMember();
            //ValueField
            frmDGVGridRefVal.ValueField = this.RefVal.GetValueMember();
            //Ctrl
            frmDGVGridRefVal.GridCtrl = this;
            //InitValue
            frmDGVGridRefVal.InitValue = this.InnerTextBox.Text;
            //RefVal
            frmDGVGridRefVal.RefVal = this.RefVal;
            //AllowAddData
            frmDGVGridRefVal.AllowAddData = this.RefVal.AllowAddData;

            this.RefVal.Active(new OnActiveEventArgs());

            frmDGVGridRefVal.ShowDialog();

            if (this.Parent != null)
            {
                Control ctrl = this.Parent.Parent;
                if (ctrl is InfoDataGridView)
                {
                    ((InfoDataGridView)ctrl).bHasFocused = true;
                }
            }
        }

       private void InTextBox_KeyDown(object sender, KeyEventArgs e)
       {
           if (e.KeyData == (Keys.Enter | Keys.Control))
           {
               e.SuppressKeyPress = true;
               bool checkData = this.RefVal.CheckData;
               this.RefVal.CheckData = false;
               open();
               this.RefVal.CheckData = checkData;
           }
       }

        private void open()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button)
                {
                    ((Button)ctrl).PerformClick();
                }
            }
        }

        private static bool flag = false;
        void Parent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (e.Control || e.Alt)
                {
                    flag = true;
                    Select();
                }
        }

        private void InTextBox_Enter(object sender, EventArgs e)
        {
            if (flag)
                open();
            flag = false;
        }
    }
}
