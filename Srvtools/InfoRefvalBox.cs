using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Reflection;

namespace Srvtools
{
    [ToolboxBitmap(typeof(InfoRefvalBox), "Resources.InfoRefValBox.ico")]
    public partial class InfoRefvalBox : UserControl, ISupportInitialize, IGetValues, IWriteValue
    {
        public InfoRefvalBox()
        {
            InitializeComponent();
            this.InButton.Paint += new PaintEventHandler(InButton_Paint);
            this.Enter += new EventHandler(InfoRefvalBox_Enter);
            this.Leave += new EventHandler(InfoRefvalBox_Leave);
            this.Load += new EventHandler(InfoRefvalBox_Load);
        }

        void InfoRefvalBox_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                if (!string.IsNullOrEmpty(ExternalRefVal))
                {
                    int index = ExternalRefVal.LastIndexOf('.');
                    if (index > 0 && index < ExternalRefVal.Length - 1)
                    {
                        string formname = ExternalRefVal.Substring(0, index);
                        string refvalname = ExternalRefVal.Substring(index + 1);
                        Form form = this.FindForm();
                        Form mdi = null;
                        if (form.MdiParent != null)
                        {
                            mdi = form.MdiParent;
                        }
                        else
                        {
                            foreach (Form frm in Application.OpenForms)
                            {
                                if (frm.IsMdiContainer)
                                {
                                    mdi = frm;
                                    break;
                                }
                            }
                        }
                        if (mdi != null)
                        {
                            foreach (Form frm in mdi.MdiChildren)
                            {
                                if (string.Compare(frm.GetType().FullName, formname) == 0)
                                {
                                    FieldInfo field = frm.GetType().GetField(refvalname, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                    if (field != null)
                                    {
                                        object obj = field.GetValue(frm);
                                        if (obj is InfoRefVal)
                                        {
                                            this.RefVal = obj as InfoRefVal;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        void InfoRefvalBox_Enter(object sender, EventArgs e)
        {
            if (this.RefVal != null)
            {
                this.RefVal.ActiveBox = this;
                this.RefVal.ActiveColumn = null;
            }
            GetRefValEnter();
        }

        public event EventHandler SelectedValueChanged
        {
            add { Events.AddHandler(EventSelectedChanged, value); }
            remove { Events.RemoveHandler(EventSelectedChanged, value); }
        }

        private static readonly object EventSelectedChanged = new object();

        protected void OnSelectedValueChanged(EventArgs e)
        {
            EventHandler SelectedChangedHandler = (EventHandler)Events[EventSelectedChanged];
            if (SelectedChangedHandler != null)
            {
                SelectedChangedHandler(this, e);
            }
        }
        public void GetRefValEnter()
        {
            if (this.InTextBox.RefVal != null)
            {
                if (!this.DisableValueMember)
                {
                    this.InTextBox.fDisplayReal = true;
                    this.InTextBox.Text = this.InTextBox.SelectedValue;
                }
                else
                {
                    this.InTextBox.fDisplayReal = false;
                }
            }
        }

        void InfoRefvalBox_Leave(object sender, EventArgs e)
        {
            if (this.FindForm() != null && !this.FindForm().Disposing)
                GetRefValLeave();
        }

        public void GetRefValLeave()
        {
            if (this.InTextBox.RefVal != null)
            {
                string strText = this.InTextBox.Text;
                if (this.DisableValueMember)
                {
                    strText = this.InTextBox.SelectedValue;
                }
                object[] obj = this.InTextBox.RefVal.CheckValid_And_ReturnDisplayValue(ref strText, this.InTextBox.RefVal.CheckData, true);
                if (!(bool)obj[0] && this.RefVal.CheckData)
                {
                    this.InTextBox.Text = "";
                    this.InTextBox.Focus();
                }
                if (this.InTextBox.RefVal != null)
                {
                    this.InTextBox.fDisplayReal = false;
                    if (!this.DisableValueMember)
                    {
                        this.TextBoxSelectedValue = this.InTextBox.Text;
                    }
                    else
                    {
                        this.TextBoxSelectedValue = this.InTextBox.SelectedValue;
                    }
                    if (this.InTextBox.DataBindings["SelectedValue"] != null)
                    {
                        try
                        {
                            this.InTextBox.DataBindings["SelectedValue"].WriteValue();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    if (this.TextBoxBindingSource == null || (this.TextBoxBindingSource is InfoBindingSource
                        && (this.TextBoxBindingSource as InfoBindingSource).isEdited))
                    {
                        if ((this.InTextBox.RefVal.DataSource is InfoBindingSource) && (this.InTextBox.RefVal.columnMatch.Count > 0))
                        {
                            //for (int i = 0; i < ((InfoBindingSource)this.InTextBox.RefVal.DataSource).Count; i++)
                            //{
                            //    DataRowView drv = (DataRowView)((InfoBindingSource)this.InTextBox.RefVal.DataSource).List[i];
                            //    if (this.InTextBox.LeaveText == drv.Row[this.InTextBox.RefVal.ValueMember].ToString())
                            //    {
                            this.InTextBox.RefVal.DoColumnMatch(this.InTextBox.LeaveText, (DataRow)obj[2]);
                            //    }
                            //}
                        }
                    }
                    if ((this.RefVal.AlwaysClose) && (((InfoBindingSource)this.InTextBox.RefVal.DataSource).Count == 0))
                    {
                        this.RefVal.InnerDs.RemoteName = "GLModule.cmdRefValUse";
                        this.RefVal.InnerDs.Execute(this.RefVal.SelectCommand, true);
                    }
                }
                object[] obj2 = this.InTextBox.RefVal.CheckValid_And_ReturnLinkValue(ref strText);
                if ((bool)obj2[0])
                {
                    this.InTextBox.RefVal.DoLinkMatch(obj2[1].ToString());
                }
                this.InTextBox.LeaveText = null;
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                InButton.ForeColor = value;
            }
        }

        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                InButton.Font = value;
            }
        }

        [Category("Infolight"), DefaultValue(true)]
        public bool TextBoxEnabled
        {
            get { return this.InTextBox.Enabled; }
            set
            {
                this.InTextBox.Enabled = value;
            }
        }

        private void InButton_Paint(object sender, PaintEventArgs e)
        {
            if (this.ButtonText == "..." && this.ButtonImage == null)
            {
                this.InButton.AutoSize = false;
                Graphics g = e.Graphics;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString(this.ButtonText, new Font("Simsun", 9.0f), new SolidBrush(this.ForeColor), new Point(11, 8), sf);
            }
            else
            {
                this.InButton.AutoSize = true;
            }
        }

        public void BeginInit()
        {
        }

        public void EndInit()
        {
            if (this.TextBoxBindingSource != null && this.TextBoxBindingMember != null && this.TextBoxBindingMember != "")
            {
                this.InTextBox.DataBindings.Add("SelectedValue", this.TextBoxBindingSource, this.TextBoxBindingMember, false, DataSourceUpdateMode.OnValidation);
                this.InTextBox.DataBindings["SelectedValue"].FormattingEnabled = this.FormattingEnabled;
            }

            this.InTextBox.Enabled = this.TextBoxEnabled;
        }

        public void WriteDataBinding()
        {
            this.InTextBox.DataBindings["SelectedValue"].WriteValue();
        }

        public void ReadDataBinding()
        {
            this.InTextBox.DataBindings["SelectedValue"].ReadValue();
        }

        private object _TextBoxBindingSource;
        [Category("Data"),
        AttributeProvider(typeof(IListSource)),
        RefreshProperties(RefreshProperties.All)]
        public object TextBoxBindingSource
        {
            get
            {
                return _TextBoxBindingSource;
            }
            set
            {
                _TextBoxBindingSource = value;
                if (_TextBoxBindingSource == null)
                {
                    this.InTextBox.Text = "";
                    this.InTextBox.SelectedValue = "";
                }
            }
        }

        private string _TextBoxBindingMember;
        [Category("Data"),
        Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string TextBoxBindingMember
        {
            get
            {
                return _TextBoxBindingMember;
            }
            set
            {
                _TextBoxBindingMember = value;
                if (_TextBoxBindingMember == null || _TextBoxBindingMember == "")
                {
                    this.InTextBox.Text = "";
                    this.InTextBox.SelectedValue = "";
                }
            }
        }

        private string externalRefVal;
        [Editor(typeof(ExternalRefvalEditor), typeof(UITypeEditor))]
        public string ExternalRefVal
        {
            get { return externalRefVal; }
            set { externalRefVal = value; }
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
                if (_RefVal != null)
                {
                    this.InTextBox.RefVal = _RefVal;
                }
            }
        }

        [Category("InnerTextBox")]
        public Color TextBoxBackColor
        {
            get
            {
                return this.InTextBox.BackColor;
            }
            set
            {
                this.InTextBox.BackColor = value;
            }
        }

        [Category("InnerTextBox")]
        public Font TextBoxFont
        {
            get
            {
                return this.InTextBox.Font;
            }
            set
            {
                this.InTextBox.Font = value;
            }
        }

        [Category("InnerTextBox")]
        public Color TextBoxForeColor
        {
            get
            {
                return this.InTextBox.ForeColor;
            }
            set
            {
                this.InTextBox.ForeColor = value;
                this.InButton.ForeColor = value;
            }
        }

        [Category("InnerTextBox")]
        public string TextBoxText
        {
            get
            {
                return this.InTextBox.Text;
            }
            set
            {
                this.InTextBox.Text = value;
            }
        }

        [Category("InnerTextBox")]
        public HorizontalAlignment TextBoxTextAlign
        {
            get
            {
                return this.InTextBox.TextAlign;
            }
            set
            {
                this.InTextBox.TextAlign = value;
            }
        }

        [Browsable(false)]
        public string TextBoxLeaveText
        {
            get
            {
                return this.InTextBox.LeaveText;
            }
            set
            {
                this.InTextBox.LeaveText = value;
            }
        }

        [Category("InnerTextBox")]
        public string TextBoxSelectedValue
        {
            get
            {
                return InTextBox.SelectedValue;
            }
            set
            {
                string oldvalue = InTextBox.SelectedValue;
                InTextBox.SelectedValue = value;
                if (oldvalue != value)
                {
                    OnSelectedValueChanged(EventArgs.Empty);
                }
            }
        }

        [Category("InnerTextBox")]
        public int MaxLength
        {
            get
            {
                return InTextBox.MaxLength;
            }
            set
            {
                InTextBox.MaxLength = value;
            }
        }

        [Category("InnerTextBox")]
        public Color BackColorDisabled
        {
            get
            {
                return InTextBox.BackColorDisabled;
            }
            set
            {
                InTextBox.BackColorDisabled = value;
            }
        }

        [Category("InnerTextBox")]
        public Color ForeColorDisabled
        {
            get
            {
                return InTextBox.ForeColorDisabled;
            }
            set
            {
                InTextBox.ForeColorDisabled = value;
            }
        }

        public CharacterCasing CharacterCasing
        {
            get { return InTextBox.CharacterCasing; }
            set { InTextBox.CharacterCasing = value; }
        }

        [Category("InnerButton")]
        public string ButtonText
        {
            get
            {
                if (InButton.Text != null && InButton.Text != "")
                {
                    return InButton.Text;
                }
                return "...";
            }
            set
            {
                if (value != "..." && InButton.Image == null)
                {
                    InButton.Text = value;
                }
                else
                {
                    InButton.Text = "";
                }
            }
        }

        [Category("InnerButton")]
        public Image ButtonImage
        {
            get
            {
                return InButton.Image;
            }
            set
            {
                InButton.Image = value;
                if (value != null)
                {
                    InButton.Text = "";
                }
            }
        }

        private bool _EnterEnable;
        [Category("Infolight")]
        public bool EnterEnable
        {
            get
            {
                return _EnterEnable;
            }
            set
            {
                _EnterEnable = value;
                if (_EnterEnable)
                {
                    this.InTextBox.EnterEnable = true;
                }
                else
                {
                    this.InTextBox.EnterEnable = false;
                }
            }
        }

        private bool _DisableValueMember = false;
        [Category("Infolight"), DefaultValue(false)]
        public bool DisableValueMember
        {
            get
            {
                return _DisableValueMember;
            }
            set
            {
                _DisableValueMember = value;
            }
        }

        private bool _FormattingEnabled = false;
        [Category("Infolight"), DefaultValue(false)]
        public bool FormattingEnabled
        {
            get
            {
                return _FormattingEnabled;
            }
            set
            {
                _FormattingEnabled = value;
            }
        }

        private void InButton_Click(object sender, EventArgs e)
        {
            if (this.TextBoxBindingSource != null && this.TextBoxBindingSource is InfoBindingSource)
            {
                if (((InfoBindingSource)this.TextBoxBindingSource).BeginEdit() == false)
                {
                    return;
                }
            }
            frmDGVGridRefVal frmDGVGridRefVal = new frmDGVGridRefVal();
            //DataSource
            if (this.RefVal.SelectCommand != null && this.RefVal.SelectCommand != "")
            {
                string strwhere = this.RefVal.WhereString(this.RefVal.SelectCommand);
                if (this.RefVal.AlwaysClose || strwhere.Length > 0)
                {
                    this.RefVal.InnerDs.RemoteName = "GLModule.cmdRefValUse";
                    this.RefVal.InnerDs.Execute(CliUtils.InsertWhere(this.RefVal.SelectCommand, strwhere), true);
                }
                frmDGVGridRefVal.DataSource = this.RefVal.InnerBs;
            }
            else if (this.InTextBox.RefVal.DataSource is InfoBindingSource)
            {
                InfoDataSet ids = (this.RefVal.DataSource as InfoBindingSource).GetDataSource() as InfoDataSet;
                string sql = DBUtils.GetCommandText((this.RefVal.DataSource as InfoBindingSource));
                string strwhere = this.RefVal.WhereString(sql);
                if (ids.AlwaysClose || strwhere.Length > 0)
                {
                    ids.SetWhere(strwhere);
                }
                InfoBindingSource refBinding = (InfoBindingSource)this.InTextBox.RefVal.DataSource;
                frmDGVGridRefVal.DataSource = refBinding;
            }
            else
            {
                return;
            }
            //DataMember
            frmDGVGridRefVal.DataMember = this.InTextBox.RefVal.GetDataMember();
            //ValueField
            frmDGVGridRefVal.ValueField = this.InTextBox.RefVal.GetValueMember();
            //Ctrl
            frmDGVGridRefVal.BoxCtrl = this;
            //InitValue
            frmDGVGridRefVal.InitValue = this.InTextBox.Text;
            //RefVal
            frmDGVGridRefVal.RefVal = this.RefVal;
            //AllowAddData
            frmDGVGridRefVal.AllowAddData = this.RefVal.AllowAddData;

            this.RefVal.Active(new OnActiveEventArgs());

            frmDGVGridRefVal.ShowDialog(this.RefVal.OwnerComp as InfoForm);

            if (this.DisableValueMember)
            {
                this.InTextBox.SelectedValue = this.TextBoxLeaveText;
                String sSelectedValue = this.InTextBox.SelectedValue;
                object[] obj = this.InTextBox.RefVal.CheckValid_And_ReturnDisplayValue(ref sSelectedValue, this.InTextBox.RefVal.CheckData, true);
                if (!(bool)obj[0] && this.RefVal.CheckData)
                {
                    this.InTextBox.Text = "";
                    this.InTextBox.Focus();
                }
                else if ((bool)obj[0])
                {
                    this.InTextBox.Text = obj[1].ToString();
                }
            }
            String sSelectedValue2 = this.InTextBox.SelectedValue;
            object[] obj2 = this.InTextBox.RefVal.CheckValid_And_ReturnLinkValue(ref sSelectedValue2);
            if ((bool)obj2[0])
            {
                this.InTextBox.RefVal.DoLinkMatch(obj2[1].ToString());
            }
        }

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            if (string.Compare(sKind, "textboxbindingmember", true) == 0)//IgnoreCase
            {
                if (this.TextBoxBindingSource != null && this.TextBoxBindingSource is InfoBindingSource)
                {
                    //DataColumnCollection dcc = (((InfoBindingSource)this.TextBoxBindingSource).List as DataView).Table.Columns;
                    DataColumnCollection dcc = null;
                    if ((this.TextBoxBindingSource as InfoBindingSource).DataSource is InfoDataSet)
                    {
                        dcc = ((InfoDataSet)((InfoBindingSource)this.TextBoxBindingSource).DataSource).RealDataSet.Tables[0].Columns;
                    }
                    else
                    {
                        dcc = ((InfoDataSet)((InfoBindingSource)this.TextBoxBindingSource).GetDataSource()).RealDataSet.Relations[((InfoBindingSource)this.TextBoxBindingSource).DataMember].ChildTable.Columns;
                    }
                    // = ((InfoDataSet)((InfoBindingSource)this.TextBoxBindingSource).DataSource).RealDataSet.Tables[0].Columns;
                    int i = dcc.Count;
                    string[] arrItems = new string[i];
                    for (int j = 0; j < i; j++)
                    {
                        arrItems[j] = dcc[j].ColumnName;
                    }
                    retList = arrItems;
                }
            }
            return retList;
        }

        //#region IReadOnly Members 取消使用IReadOnly接口,使AutoDisableControl时设置Enabled属性

        public bool ReadOnly
        {
            get
            {
                return this.InTextBox.ReadOnly;
            }
            set
            {
                this.InTextBox.ReadOnly = value;
                this.InButton.Enabled = !value;
            }
        }

        //#endregion

        #region IWriteValue Members

        public void WriteValue()
        {
            if (this.InTextBox.DataBindings["SelectedValue"] != null)
            {
                this.InTextBox.DataBindings["SelectedValue"].WriteValue();
            }
        }

        public void WriteValue(object value)
        {
            this.TextBoxSelectedValue = value.ToString();
            WriteValue();
        }

        #endregion
    }

    public class ExternalRefvalEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                IWindowsFormsEditorService editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                if (editorService != null)
                {
                    frmExternalRefvalEditor editor = new frmExternalRefvalEditor();
                    if (editorService.ShowDialog(editor) == DialogResult.OK)
                    {
                        return editor.SelectedValue;
                    }
                }
            }
            return value;
        }
    }


}
