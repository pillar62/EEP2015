using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;
using Microsoft.Win32;

namespace Srvtools
{
    #region InfoDataSetEditorDialog
    internal partial class InfoDataSetEditorDialog : Form
    {
        internal IDesignerHost DesignerHost = null;
        internal InfoDataSet m_designedDataSet = null;
        internal InfoDataSet DesignedDataSet
        {
            get
            {
                return m_designedDataSet;
            }
            set
            {
                m_designedDataSet = value;
            }
        }

        internal IRootDesigner RootDesigner = null;
        internal Control RootComponent = null;
        internal Control View = null;

        internal ColumnsToCreate ColumnsToCreate = null;

        private bool ReadyToCreateComponent = false;

        private const int WH_Mouse = 0x07;

        private const int WM_MouseUp = 0x0202;
        private const int WM_MouseMove = 0x0200;

        internal delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        //Declare MouseHookProcedure as HookProc type.
        HookProc MouseHookProcedure = null;

        public IntPtr HookHandle = IntPtr.Zero;

        private bool OkClicked = false;

        //Declare wrapper managed POINT class.
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int X;
            public int Y;
        }

        //Declare wrapper managed MouseHookStruct class.
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT Position;
            public int hWnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        //Import for SetWindowsHookEx function.
        //Use this function to install thread-specific hook.
        [DllImport("user32.dll")]
        protected static extern IntPtr SetWindowsHookEx(int hookType,
            HookProc func,
            IntPtr hInstance,
            int threadID);

        //Import for UnhookWindowsHookEx.
        //Call this function to uninstall the hook.
        [DllImport("user32.dll")]
        protected static extern int UnhookWindowsHookEx(IntPtr hhook);

        //Import for CallNextHookEx.
        //Use this function to pass the hook information to next hook procedure in chain.
        [DllImport("user32.dll")]
        protected static extern int CallNextHookEx(IntPtr hhook,
            int code, IntPtr wParam, IntPtr lParam);

        // Import for GetCurrentThreadId
        [DllImport("kernel32.dll")]
        protected static extern int GetCurrentThreadId();

        public InfoDataSetEditorDialog()
        {
            InitializeComponent();

            ColumnsToCreate = new ColumnsToCreate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            OkClicked = true;
            this.Close();
        }

        private void InfoDataSetEditorDialog_Load(object sender, EventArgs e)
        {
            this.AllowTransparency = true;
            if (ColumnsToCreate.Direction == CreateDirection.Horizontal)
            {
                this.cbRowsOrCols.SelectedIndex = 0;
                this.txtRowsOrCols.Text = ColumnsToCreate.VerticalRows.ToString();
            }
            else if (ColumnsToCreate.Direction == CreateDirection.Vertical)
            {
                this.cbRowsOrCols.SelectedIndex = 1;
                this.txtRowsOrCols.Text = ColumnsToCreate.HorzontallyColumns.ToString();
            }
            this.txtVerticalGaps.Text = ColumnsToCreate.VerticalGaps.ToString();
            this.txtHorizontalGaps.Text = ColumnsToCreate.HorizontalGaps.ToString();
        }

        internal void InitialTreeView()
        {
            this.ColumnsToCreate.Columns.Clear();
            if (this.DesignedDataSet.Active)
            {
                // Modify By Chenjian 2006-01-04 
                // All the icons are insert into the ImageList directly

                //// Load Images from System.Windows.Forms assembly
                //Assembly SystemWindowsFormsAssembly = typeof(Form).Assembly;
                //Assembly SystemDataAssembly = typeof(DataSet).Assembly;

                //// TextBox Icon 0
                //Bitmap bmp = Bitmap.FromStream(SystemWindowsFormsAssembly.GetManifestResourceStream("System.Windows.Forms.TextBox.bmp"), false) as Bitmap;
                //ImageCollection.Images.Add(bmp);
                ////bmp.Save("d:\\TextBox.bmp");

                //// Label Icon 1
                //bmp = Bitmap.FromStream(SystemWindowsFormsAssembly.GetManifestResourceStream("System.Windows.Forms.Label.bmp"), false) as Bitmap;
                //ImageCollection.Images.Add(bmp);
                ////bmp.Save("d:\\Label.bmp");

                //// ComboBox Icon 2
                //bmp = Bitmap.FromStream(SystemWindowsFormsAssembly.GetManifestResourceStream("System.Windows.Forms.ComboBox.bmp"), false) as Bitmap;
                //ImageCollection.Images.Add(bmp);
                ////bmp.Save("d:\\ComboBox.bmp");

                //// InfoDateTimePicker Icon 3
                //bmp = Bitmap.FromStream(SystemWindowsFormsAssembly.GetManifestResourceStream("System.Windows.Forms.InfoDateTimePicker.bmp"), false) as Bitmap;
                //ImageCollection.Images.Add(bmp);
                ////bmp.Save("d:\\InfoDateTimePicker.bmp");

                //// NumericUpDown Icon 4
                //bmp = Bitmap.FromStream(SystemWindowsFormsAssembly.GetManifestResourceStream("System.Windows.Forms.NumericUpDown.bmp"), false) as Bitmap;
                //ImageCollection.Images.Add(bmp);
                ////bmp.Save("d:\\NumericUpDown.bmp");

                //// ListBox Icon 5
                //bmp = Bitmap.FromStream(SystemWindowsFormsAssembly.GetManifestResourceStream("System.Windows.Forms.ListBox.bmp"), false) as Bitmap;
                //ImageCollection.Images.Add(bmp);
                ////bmp.Save("d:\\ListBox.bmp");

                //// LinkLabel Icon 6
                //bmp = Bitmap.FromStream(SystemWindowsFormsAssembly.GetManifestResourceStream("System.Windows.Forms.LinkLabel.bmp"), false) as Bitmap;
                //ImageCollection.Images.Add(bmp);
                ////bmp.Save("d:\\LinkLabel.bmp");

                //// image Icon 7
                //bmp = Bitmap.FromStream(SystemWindowsFormsAssembly.GetManifestResourceStream("System.Windows.Forms.image.bmp"), false) as Bitmap;
                //ImageCollection.Images.Add(bmp);
                ////bmp.Save("d:\\image.bmp");

                //// CheckBox Icon 8
                //bmp = Bitmap.FromStream(SystemWindowsFormsAssembly.GetManifestResourceStream("System.Windows.Forms.CheckBox.bmp"), false) as Bitmap;
                //ImageCollection.Images.Add(bmp);
                ////bmp.Save("d:\\CheckBox.bmp");

                //// None Icon 9
                //bmp = Bitmap.FromStream(SystemWindowsFormsAssembly.GetManifestResourceStream("System.Windows.Forms.BindingNavigator.Delete.bmp"), false) as Bitmap;
                //ImageCollection.Images.Add(bmp);
                ////bmp.Save("d:\\None.bmp");

                //// DataSet Icon 10
                //bmp = Bitmap.FromStream(SystemDataAssembly.GetManifestResourceStream("System.Data.DataSet.bmp"), false) as Bitmap;
                //ImageCollection.Images.Add(bmp);
                ////bmp.Save("d:\\DataSet.bmp");

                //// DataGrid(DataTable) Icon 11
                //bmp = Bitmap.FromStream(SystemWindowsFormsAssembly.GetManifestResourceStream("System.Windows.Forms.DataGrid.bmp"), false) as Bitmap;
                //ImageCollection.Images.Add(bmp);
                //bmp.Save("d:\\DataTable.bmp");

                //// InfoTextBox Icon 12

                //// InfoWinComboBox Icon 13

                //// RichTextBox Icon 14

                // End Modify

                // Load Data Schema into a TreeView
                this.trvDataSetSchema.Nodes.Clear();
                this.trvDataSetSchema.DesignedDataSet = this.DesignedDataSet;
                this.trvDataSetSchema.ImageCollection = this.ImageCollection;
                TreeNode DataSetNode = null;

                if (this.DesignedDataSet.Site != null)
                {
                    DataSetNode = this.trvDataSetSchema.Nodes.Add(this.DesignedDataSet.Site.Name);
                }
                else
                {
                    DataSetNode = this.trvDataSetSchema.Nodes.Add(this.DesignedDataSet.RealDataSet.DataSetName);
                }
                DataSetNode.ImageIndex = 10;

                // Tables
                //for (int i = 0; i < this.DesignedDataSet.RealDataSet.Tables.Count; ++i)
                if(this.DesignedDataSet.RealDataSet.Tables.Count > 0)
                {
                    int i = 0;
                    TreeNode TableNode = DataSetNode.Nodes.Add(this.DesignedDataSet.RealDataSet.Tables[i].TableName);
                    TableNode.ImageIndex = 11;
                    DataColumnCollection columns = this.DesignedDataSet.RealDataSet.Tables[i].Columns;
                    for (int j = 0; j < columns.Count; ++j)
                    {
                        TreeNode ColumnNode = TableNode.Nodes.Add(columns[j].ColumnName);

                        Type columnType = columns[j].DataType;
                        switch (columnType.FullName)
                        {

                            case "System.SByte":
                            case "System.Byte":
                            case "System.UInt16":
                            case "System.UInt32":
                            case "System.UInt64":
                            case "System.Int16":
                            case "System.Int32":
                            case "System.Int64":
                                ColumnNode.ImageIndex = 12;
                                break;

                            case "System.Double":
                            case "System.Single":
                            case "System.Decimal":
                                ColumnNode.ImageIndex = 12;
                                break;

                            case "System.DateTime":
                                ColumnNode.ImageIndex = 3;
                                break;

                            case "System.Char":
                            case "System.String":
                                ColumnNode.ImageIndex = 12;
                                break;

                            case "System.Boolean":
                                ColumnNode.ImageIndex = 8;
                                break;

                            case "System.Byte[]":
                                ColumnNode.ImageIndex = 9;
                                break;
                        }
                    }
                }

                // Relations
                foreach (DataRelation relation in DesignedDataSet.RealDataSet.Relations)
                {
                    TreeNode TableNode = DataSetNode.Nodes.Add(relation.RelationName);
                    TableNode.ImageIndex = 11;
                    DataColumnCollection columns = relation.ChildTable.Columns;
                    for (int j = 0; j < columns.Count; ++j)
                    {
                        TreeNode ColumnNode = TableNode.Nodes.Add(columns[j].ColumnName);

                        Type columnType = columns[j].DataType;
                        switch (columnType.FullName)
                        {

                            case "System.SByte":
                            case "System.Byte":
                            case "System.UInt16":
                            case "System.UInt32":
                            case "System.UInt64":
                            case "System.Int16":
                            case "System.Int32":
                            case "System.Int64":
                                ColumnNode.ImageIndex = 12;
                                break;

                            case "System.Double":
                            case "System.Single":
                            case "System.Decimal":
                                ColumnNode.ImageIndex = 12;
                                break;

                            case "System.DateTime":
                                ColumnNode.ImageIndex = 3;
                                break;

                            case "System.Char":
                            case "System.String":
                                ColumnNode.ImageIndex = 12;
                                break;

                            case "System.Boolean":
                                ColumnNode.ImageIndex = 8;
                                break;

                            case "System.Byte[]":
                                ColumnNode.ImageIndex = 9;
                                break;
                        }
                    }
                }

                // CreateMouseHook
                this.MouseHookProcedure = new HookProc(MouseHookProc);

                HookHandle = SetWindowsHookEx(WH_Mouse, MouseHookProcedure, IntPtr.Zero, GetCurrentThreadId());
                if (HookHandle == IntPtr.Zero)
                {
                    this.MouseHookProcedure = null;
                }
            }
        }

        bool bDDHasGot = false;
        DataSet ds = new DataSet();
        private string GetCaption(string strTableName, string strFieldName)
        {
            if (!bDDHasGot)
            {
                ds = DBUtils.GetDataDictionary(this.DesignedDataSet, null, true);
                bDDHasGot = true;
            }
            string strCaption = "";
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    string str = ds.Tables[0].Rows[j]["CAPTION"].ToString();
                    if (string.Compare(ds.Tables[0].Rows[j]["FIELD_NAME"].ToString(), strFieldName, true) == 0 && str != "")//IgnoreCase
                    {
                        strCaption = str;
                    }
                }
            }
            if (strCaption == "")
            {
                strCaption = strFieldName;
            }
            return strCaption;
        }

        private int GetMaxLength(string strTableName, string strFieldName)
        {
            if (!bDDHasGot)
            {

                ds = DBUtils.GetDataDictionary(this.DesignedDataSet, null, true);
                bDDHasGot = true;
            }

            int length = 0;
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    object o = ds.Tables[0].Rows[j]["FIELD_LENGTH"];
                    if (string.Compare(ds.Tables[0].Rows[j]["FIELD_NAME"].ToString(), strFieldName, true) == 0
                        && o != null && o != DBNull.Value)//IgnoreCase
                    {
                        length = Convert.ToInt32(o);
                    }
                }
            }
            return length;
        }

        private InfoDataSet GetInfoDataSet(InfoBindingSource bindingSource)
        {
            if (bindingSource != null)
            {
                object datasource = bindingSource.DataSource;
                if (datasource is InfoDataSet)
                {
                    return datasource as InfoDataSet;
                }
                else if (datasource is InfoBindingSource)
                {
                    return GetInfoDataSet(datasource as InfoBindingSource);
                }
            }
            return null;
        }
        // end add

        // Create Component in a Mouse Hook Procedure
        public int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(HookHandle, nCode, wParam, lParam);
            }
            else
            {
                if (ReadyToCreateComponent)
                {
                    // Marshall the data from callback.
                    MouseHookStruct mouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                    Point ptOnScreen = new Point(mouseHookStruct.Position.X, mouseHookStruct.Position.Y);
                    Point ptOnEditorDialog = this.PointToClient(ptOnScreen);
                    Point ptOnRootComponent = this.RootComponent.PointToClient(ptOnScreen);

                    if (wParam.ToInt32() == WM_MouseMove)
                    {
                        if ((ptOnEditorDialog.X < 0 || ptOnEditorDialog.X >= this.Width
                            || ptOnEditorDialog.Y < 0 || ptOnEditorDialog.Y >= this.Height)
                            && ptOnRootComponent.X >= 0 && ptOnRootComponent.X < RootComponent.Width
                            && ptOnRootComponent.Y >= 0 && ptOnRootComponent.Y < RootComponent.Height)
                        {
                            Cursor.Current = Cursors.Cross;
                        }
                        else
                        {
                            Cursor.Current = Cursors.No;
                        }
                        return -1;
                    }
                    // Mouse Up Message
                    else if (wParam.ToInt32() == WM_MouseUp)
                    {
                        // Point locates on Editor Dialog, do nothing
                        if (ptOnEditorDialog.X >= 0 && ptOnEditorDialog.Y >= 0
                            && ptOnEditorDialog.X < this.Width && ptOnEditorDialog.Y < this.Height)
                        {
                        }
                        // Point locates on Root Component, Create Component
                        else if (ptOnRootComponent.X >= 0 && ptOnRootComponent.Y >= 0
                            && ptOnRootComponent.X < this.RootComponent.Width && ptOnRootComponent.Y < this.RootComponent.Height)
                        {
                            // Get the Columns to be created
                            ColumnsToCreate.LabelWidth = 0;
                            //ColumnsToCreate.ColumnControlWidth = 0;
                            ColumnsToCreate.Columns.Clear();

                            Font labelFont = new Font("PMingLiU", 9.0f);
                            int labelWidth = 0;

                            foreach (TreeNode TableNode in this.trvDataSetSchema.Nodes[0].Nodes)
                            {
                                foreach (TreeNode ColumnNode in TableNode.Nodes)
                                {
                                    if (ColumnNode.Checked && ColumnNode.ImageIndex != 9)
                                    {
                                        ColumnsToCreate.TableName = ColumnNode.Parent.Text;

                                        Column col = new Column();
                                        // modified by Rax
                                        string caption = GetCaption(ColumnNode.Parent.Text, ColumnNode.Text);
                                        int length = GetMaxLength(ColumnNode.Parent.Text, ColumnNode.Text);

                                        if (length != 0)
                                            col.MaxLength = length;
                                        col.Caption = caption;
                                        col.ColumnName = ColumnNode.Text;
                                        // end modified
                                        switch (ColumnNode.ImageIndex)
                                        {
                                            case 0:
                                                col.ColumnType = typeof(TextBox);
                                                break;
                                            case 1:
                                                col.ColumnType = typeof(Label);
                                                break;
                                            case 2:
                                                col.ColumnType = typeof(ComboBox);
                                                break;
                                            case 3:
                                                col.ColumnType = typeof(InfoDateTimePicker);
                                                break;
                                            case 4:
                                                col.ColumnType = typeof(NumericUpDown);
                                                break;
                                            case 5:
                                                col.ColumnType = typeof(ListBox);
                                                break;
                                            case 6:
                                                col.ColumnType = typeof(LinkLabel);
                                                break;
                                            case 7:
                                                col.ColumnType = typeof(InfoPictureBox);
                                                break;
                                            case 8:
                                                col.ColumnType = typeof(InfoCheckBox);
                                                break;
                                            case 12:
                                                col.ColumnType = typeof(InfoTextBox);
                                                break;
                                            case 13:
                                                col.ColumnType = typeof(InfoComboBox);
                                                break;
                                            case 14:
                                                col.ColumnType = typeof(RichTextBox);
                                                break;
                                            case 15:
                                                col.ColumnType = typeof(InfoRefvalBox);
                                                break;
                                            case 16:
                                                col.ColumnType = typeof(InfoMaskedTextBox);
                                                break;
                                            case 17:
                                                col.ColumnType = typeof(InfoDateTimeBox);
                                                break;
                                        }
                                        ColumnsToCreate.Columns.Add(col);
                                        labelWidth = TextRenderer.MeasureText(ColumnNode.Text, labelFont).Width;
                                        if (labelWidth > ColumnsToCreate.LabelWidth)
                                        {
                                            ColumnsToCreate.LabelWidth = labelWidth;
                                        }
                                    }
                                }
                                if (ColumnsToCreate.Columns.Count > 0)
                                {
                                    break;
                                }
                            }

                            if (ColumnsToCreate.Columns.Count > 0)
                            {
                                // First Create a InfoBindingSource to be associated with TextBox

                                // Check whether a InfoBindingSource we need exists
                                InfoBindingSource bindingSource = null;
                                int loop = 0;


                                if (DesignedDataSet.Site != null)
                                {
                                    foreach (IComponent component in DesignedDataSet.Site.Container.Components)
                                    {
                                        if (component is InfoBindingSource)
                                        {
                                            //InfoDataSet datasource = GetInfoDataSet(component as InfoBindingSource);
                                            //if ((datasource == DesignedDataSet) && ((component as InfoBindingSource).DataMember == ColumnsToCreate.TableName))
                                            if ((((component as InfoBindingSource).DataMember == ColumnsToCreate.TableName)))
                                            {
                                                bindingSource = component as InfoBindingSource;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (bindingSource == null)
                                {
                                    MessageBox.Show(this, string.Format("Can not locate InfoBindingSource with datamember:{0}", ColumnsToCreate.TableName), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return CallNextHookEx(HookHandle, nCode, wParam, lParam);
                                }

                                // Find the Control(Form, Panel, GroupBox, SplitterPanel, TabPage(on TabControl), TableLayoutPanel) to create control in
                                Control CreateOnControl = RootComponent;
                                Control prevCreateOnControl = RootComponent;
                                Point ptOnControl = new Point();
                                do
                                {
                                    prevCreateOnControl = CreateOnControl;
                                    foreach (Control ctrl in CreateOnControl.Controls)
                                    {
                                        if (ctrl is Panel
                                            || ctrl is GroupBox)
                                        {
                                            ptOnControl = ctrl.PointToClient(ptOnScreen);
                                            if (ptOnControl.X >= 0 && ptOnControl.X < ctrl.Width
                                                && ptOnControl.Y >= 0 && ptOnControl.Y < ctrl.Height)
                                            {
                                                CreateOnControl = ctrl;
                                                break;
                                            }
                                        }
                                        else if (ctrl is TabControl)
                                        {
                                            CreateOnControl = (ctrl as TabControl).SelectedTab;
                                        }
                                        else if (ctrl is SplitContainer)
                                        {
                                            SplitContainer container = ctrl as SplitContainer;

                                            ptOnControl = container.Panel1.PointToClient(ptOnScreen);
                                            if (ptOnControl.X >= 0 && ptOnControl.X < container.Panel1.Width
                                                && ptOnControl.Y >= 0 && ptOnControl.Y < container.Panel1.Height)
                                            {
                                                CreateOnControl = container.Panel1;
                                                break;
                                            }

                                            ptOnControl = container.Panel2.PointToClient(ptOnScreen);
                                            if (ptOnControl.X >= 0 && ptOnControl.X < container.Panel2.Width
                                                && ptOnControl.Y >= 0 && ptOnControl.Y < container.Panel2.Height)
                                            {
                                                CreateOnControl = container.Panel2;
                                                break;
                                            }
                                        }
                                    }
                                } while (CreateOnControl != prevCreateOnControl);

                                Point StartPosition = CreateOnControl.PointToClient(ptOnScreen);
                                Point CurPosition = StartPosition;
                                Label label = null;
                                string controlName = string.Empty;

                                // controls is used to select all the controls after creating them
                                ArrayList controls = new ArrayList();

                                // Then Create TextBoxes and Labels
                                for (int i = 0; i < ColumnsToCreate.Columns.Count; ++i)
                                {
                                    label = null;

                                    // Determine the Current Location for the Control to be create on
                                    if (ColumnsToCreate.Direction == CreateDirection.Vertical)
                                    {
                                        CurPosition.X = StartPosition.X + (i % ColumnsToCreate.HorzontallyColumns) *
                                            (ColumnsToCreate.ColumnControlWidth + ColumnsToCreate.LabelWidth
                                            + ColumnsToCreate.SpaceBetweenLabelAndColumnControl + ColumnsToCreate.HorizontalGaps);

                                        CurPosition.Y = StartPosition.Y + (i / ColumnsToCreate.HorzontallyColumns) *
                                            (22 /* The height of a TextBox*/ + ColumnsToCreate.VerticalGaps);
                                    }
                                    else if (ColumnsToCreate.Direction == CreateDirection.Horizontal)
                                    {
                                        CurPosition.X = StartPosition.X + (i / ColumnsToCreate.VerticalRows) *
                                            (ColumnsToCreate.ColumnControlWidth + ColumnsToCreate.LabelWidth
                                            + ColumnsToCreate.SpaceBetweenLabelAndColumnControl + ColumnsToCreate.HorizontalGaps);

                                        CurPosition.Y = StartPosition.Y + (i % ColumnsToCreate.VerticalRows) *
                                            (22 /* The height of a TextBox*/ + ColumnsToCreate.VerticalGaps);
                                    }

                                    // Create Label
                                    //controlName = ColumnsToCreate.Columns[i].ColumnName.Replace(' ', '_') + "Label";
                                    //controlName = "Label" + i.ToString();
                                    controlName = ColumnsToCreate.Columns[i].ColumnName + "Label";
                                    loop = 0;
                                    while (label == null)
                                    {
                                        try
                                        {
                                            if (loop == 0)
                                            {
                                                label = DesignerHost.CreateComponent(typeof(Label), controlName)
                                                    as Label;
                                            }
                                            else
                                            {
                                                label = DesignerHost.CreateComponent(typeof(Label), controlName + loop.ToString())
                                                    as Label;
                                            }
                                            if (label != null)
                                            {
                                                label.AutoSize = true;
                                                label.Location = new Point(CurPosition.X, CurPosition.Y + 3);
                                                label.Text = ColumnsToCreate.Columns[i].Caption;
                                                CreateOnControl.Controls.Add(label);
                                                controls.Add(label);
                                            }
                                        }
                                        catch
                                        {
                                            label = null;
                                            loop++;
                                        }
                                    }

                                    // Create Control
                                    switch (ColumnsToCreate.Columns[i].ColumnType.FullName)
                                    {
                                        case "System.Windows.Forms.TextBox":
                                            TextBox textBox = null;
                                            //controlName = ColumnsToCreate.Columns[i].ColumnName.Replace(' ', '_') + "TextBox";
                                            //controlName = "TextBox" + i.ToString();
                                            controlName = ColumnsToCreate.Columns[i].ColumnName + "TextBox";
                                            loop = 0;
                                            while (textBox == null)
                                            {
                                                try
                                                {
                                                    if (loop == 0)
                                                    {
                                                        textBox = DesignerHost.CreateComponent(typeof(TextBox), controlName)
                                                            as TextBox;
                                                    }
                                                    else
                                                    {
                                                        textBox = DesignerHost.CreateComponent(typeof(TextBox), controlName + loop.ToString())
                                                            as TextBox;
                                                    }
                                                    if (textBox != null)
                                                    {


                                                        textBox.Width = ColumnsToCreate.ColumnControlWidth;
                                                        textBox.Location = new Point(CurPosition.X + ColumnsToCreate.LabelWidth + ColumnsToCreate.SpaceBetweenLabelAndColumnControl,
                                                            CurPosition.Y);
                                                        textBox.DataBindings.Add(new Binding("Text", bindingSource, ColumnsToCreate.Columns[i].ColumnName, true));

                                                        CreateOnControl.Controls.Add(textBox);
                                                        controls.Add(textBox);
                                                    }
                                                }
                                                catch
                                                {
                                                    textBox = null;
                                                    loop++;
                                                }
                                            }
                                            break;

                                        case "System.Windows.Forms.Label":
                                            Label lbl = null;
                                            //controlName = ColumnsToCreate.Columns[i].ColumnName.Replace(' ', '_') + "Label";
                                            //controlName = "DataLabel" + i.ToString();
                                            controlName = ColumnsToCreate.Columns[i].ColumnName + "DataLabel";
                                            loop = 0;
                                            while (lbl == null)
                                            {
                                                try
                                                {
                                                    if (loop == 0)
                                                    {
                                                        lbl = DesignerHost.CreateComponent(typeof(Label), controlName)
                                                            as Label;
                                                    }
                                                    else
                                                    {
                                                        lbl = DesignerHost.CreateComponent(typeof(Label), controlName + loop.ToString())
                                                            as Label;
                                                    }
                                                    if (lbl != null)
                                                    {
                                                        lbl.Height = 22;
                                                        lbl.Width = ColumnsToCreate.ColumnControlWidth;
                                                        lbl.Location = new Point(CurPosition.X + ColumnsToCreate.LabelWidth + ColumnsToCreate.SpaceBetweenLabelAndColumnControl,
                                                            CurPosition.Y);
                                                        lbl.DataBindings.Add(new Binding("Text", bindingSource, ColumnsToCreate.Columns[i].ColumnName, true));

                                                        CreateOnControl.Controls.Add(lbl);
                                                        controls.Add(lbl);
                                                    }
                                                }
                                                catch
                                                {
                                                    lbl = null;
                                                    loop++;
                                                }
                                            }
                                            break;

                                        case "System.Windows.Forms.ComboBox":
                                            ComboBox comboBox = null;
                                            //controlName = ColumnsToCreate.Columns[i].ColumnName.Replace(' ', '_') + "ComboBox";
                                            //controlName = "ComboBox" + i.ToString();
                                            controlName = ColumnsToCreate.Columns[i].ColumnName + "ComboBox";
                                            loop = 0;
                                            while (comboBox == null)
                                            {
                                                try
                                                {
                                                    if (loop == 0)
                                                    {
                                                        comboBox = DesignerHost.CreateComponent(typeof(ComboBox), controlName)
                                                            as ComboBox;
                                                    }
                                                    else
                                                    {
                                                        comboBox = DesignerHost.CreateComponent(typeof(ComboBox), controlName + loop.ToString())
                                                            as ComboBox;
                                                    }
                                                    if (comboBox != null)
                                                    {
                                                        comboBox.Height = 22;
                                                        comboBox.FormattingEnabled = true;
                                                        comboBox.Width = ColumnsToCreate.ColumnControlWidth;
                                                        comboBox.Location = new Point(CurPosition.X + ColumnsToCreate.LabelWidth + ColumnsToCreate.SpaceBetweenLabelAndColumnControl,
                                                            CurPosition.Y);
                                                        comboBox.DataBindings.Add(new Binding("Text", bindingSource, ColumnsToCreate.Columns[i].ColumnName, true));

                                                        CreateOnControl.Controls.Add(comboBox);
                                                        controls.Add(comboBox);
                                                    }
                                                }
                                                catch
                                                {
                                                    comboBox = null;
                                                    loop++;
                                                }
                                            }
                                            break;

                                        case "Srvtools.InfoDateTimePicker":
                                            InfoDateTimePicker dateTimePicker = null;
                                            //controlName = ColumnsToCreate.Columns[i].ColumnName.Replace(' ', '_') + "DateTimePicker";
                                            //controlName = "InfoDateTimePicker" + i.ToString();
                                            DataTable dtTemp;
                                            controlName = ColumnsToCreate.Columns[i].ColumnName + "InfoDateTimePicker";
                                            loop = 0;
                                            while (dateTimePicker == null)
                                            {
                                                try
                                                {
                                                    if (loop == 0)
                                                    {
                                                        dateTimePicker = DesignerHost.CreateComponent(typeof(InfoDateTimePicker), controlName)
                                                            as InfoDateTimePicker;
                                                    }
                                                    else
                                                    {
                                                        dateTimePicker = DesignerHost.CreateComponent(typeof(InfoDateTimePicker), controlName + loop.ToString())
                                                            as InfoDateTimePicker;
                                                    }
                                                    if (dateTimePicker != null)
                                                    {
                                                        dateTimePicker.Height = 22;
                                                        dateTimePicker.Width = ColumnsToCreate.ColumnControlWidth;
                                                        dateTimePicker.Location = new Point(CurPosition.X + ColumnsToCreate.LabelWidth + ColumnsToCreate.SpaceBetweenLabelAndColumnControl,
                                                            CurPosition.Y);
                                                        //modified by lily 2007/10/24 for datetime from relation error
                                                        // modified by rax
                                                        dtTemp = this.DesignedDataSet.RealDataSet.Tables[this.ColumnsToCreate.TableName];
                                                        if (dtTemp == null)
                                                        {
                                                            dtTemp = this.DesignedDataSet.RealDataSet.Relations[this.ColumnsToCreate.TableName].ChildTable;
                                                        }
                                                        if (dtTemp == null)
                                                        {
                                                            break;
                                                        }
                                                        //modified by lily 2007/10/24 for datetime from relation error
                                                        if (dtTemp.Columns[ColumnsToCreate.Columns[i].ColumnName].DataType == typeof(String))
                                                        {
                                                            dateTimePicker.DataBindings.Add(new Binding("DateTimeString", bindingSource, ColumnsToCreate.Columns[i].ColumnName, true));
                                                        }
                                                        else
                                                        {
                                                            dateTimePicker.DataBindings.Add(new Binding("Text", bindingSource, ColumnsToCreate.Columns[i].ColumnName, true));
                                                        }
                                                        // end modify
                                                        CreateOnControl.Controls.Add(dateTimePicker);
                                                        controls.Add(dateTimePicker);
                                                    }
                                                }
                                                catch
                                                {
                                                    dateTimePicker = null;
                                                    loop++;
                                                }
                                            }
                                            break;

                                        case "System.Windows.Forms.NumericUpDown":
                                            NumericUpDown numericUpDown = null;
                                            //controlName = ColumnsToCreate.Columns[i].ColumnName.Replace(' ', '_') + "NumericUpDown";
                                            //controlName = "NumericUpDown" + i.ToString();
                                            controlName = ColumnsToCreate.Columns[i].ColumnName + "NumericUpDown";
                                            loop = 0;
                                            while (numericUpDown == null)
                                            {
                                                try
                                                {
                                                    if (loop == 0)
                                                    {
                                                        numericUpDown = DesignerHost.CreateComponent(typeof(NumericUpDown), controlName)
                                                            as NumericUpDown;
                                                    }
                                                    else
                                                    {
                                                        numericUpDown = DesignerHost.CreateComponent(typeof(NumericUpDown), controlName + loop.ToString())
                                                            as NumericUpDown;
                                                    }
                                                    if (numericUpDown != null)
                                                    {
                                                        numericUpDown.Height = 22;
                                                        numericUpDown.Width = ColumnsToCreate.ColumnControlWidth;
                                                        numericUpDown.Location = new Point(CurPosition.X + ColumnsToCreate.LabelWidth + ColumnsToCreate.SpaceBetweenLabelAndColumnControl,
                                                            CurPosition.Y);
                                                        numericUpDown.DataBindings.Add(new Binding("Value", bindingSource, ColumnsToCreate.Columns[i].ColumnName, true));

                                                        CreateOnControl.Controls.Add(numericUpDown);
                                                        controls.Add(numericUpDown);
                                                    }
                                                }
                                                catch
                                                {
                                                    numericUpDown = null;
                                                    loop++;
                                                }
                                            }
                                            break;

                                        case "System.Windows.Forms.ListBox":
                                            break;

                                        case "System.Windows.Forms.LinkLabel":
                                            LinkLabel linkLabel = null;
                                            //controlName = ColumnsToCreate.Columns[i].ColumnName.Replace(' ', '_') + "LinkLabel";
                                            //controlName = "LinkLabel" + i.ToString();
                                            controlName = ColumnsToCreate.Columns[i].ColumnName + "LinkLabel";
                                            loop = 0;
                                            while (linkLabel == null)
                                            {
                                                try
                                                {
                                                    if (loop == 0)
                                                    {
                                                        linkLabel = DesignerHost.CreateComponent(typeof(LinkLabel), controlName)
                                                            as LinkLabel;
                                                    }
                                                    else
                                                    {
                                                        linkLabel = DesignerHost.CreateComponent(typeof(LinkLabel), controlName + loop.ToString())
                                                            as LinkLabel;
                                                    }
                                                    if (linkLabel != null)
                                                    {
                                                        linkLabel.Height = 22;
                                                        linkLabel.Width = ColumnsToCreate.ColumnControlWidth;
                                                        linkLabel.Location = new Point(CurPosition.X + ColumnsToCreate.LabelWidth + ColumnsToCreate.SpaceBetweenLabelAndColumnControl,
                                                            CurPosition.Y);
                                                        linkLabel.DataBindings.Add(new Binding("Text", bindingSource, ColumnsToCreate.Columns[i].ColumnName, true));

                                                        CreateOnControl.Controls.Add(linkLabel);
                                                        controls.Add(linkLabel);
                                                    }
                                                }
                                                catch
                                                {
                                                    linkLabel = null;
                                                    loop++;
                                                }
                                            }
                                            break;

                                        case "Srvtools.InfoPictureBox":
                                            InfoPictureBox pictureBox = null;
                                            //controlName = ColumnsToCreate.Columns[i].ColumnName.Replace(' ', '_') + "PictureBox";
                                            //controlName = "PictureBox" + i.ToString();
                                            controlName = ColumnsToCreate.Columns[i].ColumnName + "PictureBox";
                                            loop = 0;
                                            while (pictureBox == null)
                                            {
                                                try
                                                {
                                                    if (loop == 0)
                                                    {
                                                        pictureBox = DesignerHost.CreateComponent(typeof(InfoPictureBox), controlName)
                                                            as InfoPictureBox;
                                                    }
                                                    else
                                                    {
                                                        pictureBox = DesignerHost.CreateComponent(typeof(InfoPictureBox), controlName + loop.ToString())
                                                            as InfoPictureBox;
                                                    }
                                                    if (pictureBox != null)
                                                    {
                                                        pictureBox.Height = 22;
                                                        pictureBox.Width = ColumnsToCreate.ColumnControlWidth;
                                                        pictureBox.Location = new Point(CurPosition.X + ColumnsToCreate.LabelWidth + ColumnsToCreate.SpaceBetweenLabelAndColumnControl,
                                                            CurPosition.Y);
                                                        pictureBox.DataBindings.Add(new Binding("Image", bindingSource, ColumnsToCreate.Columns[i].ColumnName, true));

                                                        CreateOnControl.Controls.Add(pictureBox);
                                                        controls.Add(pictureBox);
                                                    }
                                                }
                                                catch
                                                {
                                                    pictureBox = null;
                                                    loop++;
                                                }
                                            }
                                            break;

                                        case "Srvtools.InfoCheckBox":
                                            InfoCheckBox checkBox = null;
                                            //controlName = ColumnsToCreate.Columns[i].ColumnName.Replace(' ', '_') + "CheckBox";
                                            //controlName = "CheckBox" + i.ToString();
                                            controlName = ColumnsToCreate.Columns[i].ColumnName + "CheckBox";
                                            loop = 0;
                                            while (checkBox == null)
                                            {
                                                try
                                                {
                                                    if (loop == 0)
                                                    {
                                                        checkBox = DesignerHost.CreateComponent(typeof(InfoCheckBox), controlName)
                                                            as InfoCheckBox;
                                                    }
                                                    else
                                                    {
                                                        checkBox = DesignerHost.CreateComponent(typeof(InfoCheckBox), controlName + loop.ToString())
                                                            as InfoCheckBox;
                                                    }
                                                    if (checkBox != null)
                                                    {
                                                        checkBox.Height = 22;
                                                        checkBox.Width = ColumnsToCreate.ColumnControlWidth;
                                                        checkBox.Location = new Point(CurPosition.X + ColumnsToCreate.LabelWidth + ColumnsToCreate.SpaceBetweenLabelAndColumnControl,
                                                            CurPosition.Y);
                                                        checkBox.DataBindings.Add(new Binding("CheckBinding", bindingSource, ColumnsToCreate.Columns[i].ColumnName, true));
                                                        CreateOnControl.Controls.Add(checkBox);
                                                        controls.Add(checkBox);
                                                    }
                                                }
                                                catch
                                                {
                                                    checkBox = null;
                                                    loop++;
                                                }
                                            }
                                            break;


                                        case "System.Windows.Forms.RichTextBox":
                                            RichTextBox richTextBox = null;
                                            //controlName = ColumnsToCreate.Columns[i].ColumnName.Replace(' ', '_') + "RichTextBox";
                                            //controlName = "RichTextBox" + i.ToString();
                                            controlName = ColumnsToCreate.Columns[i].ColumnName + "RichTextBox";
                                            loop = 0;
                                            while (richTextBox == null)
                                            {
                                                try
                                                {
                                                    if (loop == 0)
                                                    {
                                                        richTextBox = DesignerHost.CreateComponent(typeof(RichTextBox), controlName)
                                                            as RichTextBox;
                                                    }
                                                    else
                                                    {
                                                        richTextBox = DesignerHost.CreateComponent(typeof(RichTextBox), controlName + loop.ToString())
                                                            as RichTextBox;
                                                    }
                                                    if (richTextBox != null)
                                                    {
                                                        richTextBox.Height = 22;
                                                        richTextBox.Width = ColumnsToCreate.ColumnControlWidth;
                                                        richTextBox.Location = new Point(CurPosition.X + ColumnsToCreate.LabelWidth + ColumnsToCreate.SpaceBetweenLabelAndColumnControl,
                                                            CurPosition.Y);
                                                        richTextBox.DataBindings.Add(new Binding("Text", bindingSource, ColumnsToCreate.Columns[i].ColumnName, true));

                                                        CreateOnControl.Controls.Add(richTextBox);
                                                        controls.Add(richTextBox);
                                                    }
                                                }
                                                catch
                                                {
                                                    richTextBox = null;
                                                    loop++;
                                                }
                                            }
                                            break;

                                        case "Srvtools.InfoTextBox":
                                            InfoTextBox infoTextBox = null;
                                            //controlName = ColumnsToCreate.Columns[i].ColumnName.Replace(' ', '_') + "InfoTextBox";
                                            //controlName = "InfoTextBox" + i.ToString();
                                            controlName = ColumnsToCreate.Columns[i].ColumnName + "InfoTextBox";
                                            loop = 0;
                                            while (infoTextBox == null)
                                            {
                                                try
                                                {
                                                    if (loop == 0)
                                                    {
                                                        infoTextBox = DesignerHost.CreateComponent(typeof(InfoTextBox), controlName)
                                                            as InfoTextBox;
                                                    }
                                                    else
                                                    {
                                                        infoTextBox = DesignerHost.CreateComponent(typeof(InfoTextBox), controlName + loop.ToString())
                                                            as InfoTextBox;
                                                    }
                                                    if (infoTextBox != null)
                                                    {
                                                        //infoTextBox.Caption = ColumnsToCreate.Columns[i].ColumnName;
                                                        //if (this.DesignedDataSet.RealDataSet.Tables[this.ColumnsToCreate.TableName].Columns[ColumnsToCreate.Columns[i].ColumnName].DataType == typeof(string))
                                                        //{
                                                        //    infoTextBox.MaxLength = 50;
                                                        //}
                                                        if (this.DesignedDataSet.RealDataSet.Tables.Contains(this.ColumnsToCreate.TableName))
                                                        {
                                                            if (this.DesignedDataSet.RealDataSet.Tables[this.ColumnsToCreate.TableName].Columns[ColumnsToCreate.Columns[i].ColumnName].DataType == typeof(string))
                                                                infoTextBox.MaxLength = ColumnsToCreate.Columns[i].MaxLength;
                                                        }
                                                        //else
                                                        //{
                                                        //    if (this.DesignedDataSet.RealDataSet.Relations[this.ColumnsToCreate.TableName].ChildTable.Columns[ColumnsToCreate.Columns[i].ColumnName].DataType == typeof(string))
                                                        //        infoTextBox.MaxLength = ColumnsToCreate.Columns[i].MaxLength;
                                                        //}
                                                    }

                                                    infoTextBox.Height = 22;
                                                    infoTextBox.Width = ColumnsToCreate.ColumnControlWidth;
                                                    infoTextBox.Location = new Point(CurPosition.X + ColumnsToCreate.LabelWidth + ColumnsToCreate.SpaceBetweenLabelAndColumnControl,
                                                        CurPosition.Y);
                                                    infoTextBox.DataBindings.Add(new Binding("Text", bindingSource, ColumnsToCreate.Columns[i].ColumnName, true));

                                                    CreateOnControl.Controls.Add(infoTextBox);
                                                    controls.Add(infoTextBox);
                                                }

                                                catch
                                                {
                                                    infoTextBox = null;
                                                    loop++;
                                                }
                                            }
                                            break;

                                        case "Srvtools.InfoRefvalBox":
                                            InfoRefvalBox infoRefValBox = null;
                                            //controlName = "InfoRefValBox" + i.ToString();
                                            controlName = ColumnsToCreate.Columns[i].ColumnName + "InfoRefValBox";
                                            loop = 0;
                                            while (infoRefValBox == null)
                                            {
                                                try
                                                {
                                                    if (loop == 0)
                                                    {
                                                        infoRefValBox = DesignerHost.CreateComponent(typeof(InfoRefvalBox), controlName)
                                                            as InfoRefvalBox;
                                                    }
                                                    else
                                                    {
                                                        infoRefValBox = DesignerHost.CreateComponent(typeof(InfoRefvalBox), controlName + loop.ToString())
                                                            as InfoRefvalBox;
                                                    }
                                                    if (infoRefValBox != null)
                                                    {
                                                        infoRefValBox.Height = 22;
                                                        infoRefValBox.Width = ColumnsToCreate.ColumnControlWidth;
                                                        infoRefValBox.Location = new Point(CurPosition.X + ColumnsToCreate.LabelWidth + ColumnsToCreate.SpaceBetweenLabelAndColumnControl,
                                                            CurPosition.Y);
                                                        infoRefValBox.TextBoxBindingSource = bindingSource;
                                                        infoRefValBox.TextBoxBindingMember = ColumnsToCreate.Columns[i].ColumnName;
                                                        CreateOnControl.Controls.Add(infoRefValBox);
                                                        controls.Add(infoRefValBox);
                                                    }
                                                }
                                                catch
                                                {
                                                    infoRefValBox = null;
                                                    loop++;
                                                }
                                            }
                                            break;

                                        case "Srvtools.InfoComboBox":
                                            InfoComboBox infoWinComboBox = null;
                                            //controlName = ColumnsToCreate.Columns[i].ColumnName.Replace(' ', '_') + "InfoWinComboBox";
                                            //controlName = "InfoComboBox" + i.ToString();
                                            controlName = ColumnsToCreate.Columns[i].ColumnName + "InfoComboBox";
                                            loop = 0;
                                            while (infoWinComboBox == null)
                                            {
                                                try
                                                {
                                                    if (loop == 0)
                                                    {
                                                        infoWinComboBox = DesignerHost.CreateComponent(typeof(InfoComboBox), controlName)
                                                            as InfoComboBox;
                                                    }
                                                    else
                                                    {
                                                        infoWinComboBox = DesignerHost.CreateComponent(typeof(InfoComboBox), controlName + loop.ToString())
                                                            as InfoComboBox;
                                                    }
                                                    if (infoWinComboBox != null)
                                                    {
                                                        infoWinComboBox.Height = 22;
                                                        infoWinComboBox.Width = ColumnsToCreate.ColumnControlWidth;
                                                        infoWinComboBox.Location = new Point(CurPosition.X + ColumnsToCreate.LabelWidth + ColumnsToCreate.SpaceBetweenLabelAndColumnControl,
                                                            CurPosition.Y);
                                                        infoWinComboBox.DataBindings.Add(new Binding("SelectedValue", bindingSource, ColumnsToCreate.Columns[i].ColumnName, true));

                                                        CreateOnControl.Controls.Add(infoWinComboBox);
                                                        controls.Add(infoWinComboBox);
                                                    }
                                                }
                                                catch
                                                {
                                                    infoWinComboBox = null;
                                                    loop++;
                                                }
                                            }
                                            break;

                                        case "Srvtools.InfoMaskedTextBox":
                                            InfoMaskedTextBox infoMaskedTextBox = null;
                                            controlName = ColumnsToCreate.Columns[i].ColumnName + "InfoMaskedTextBox";
                                            loop = 0;
                                            while (infoMaskedTextBox == null)
                                            {
                                                try
                                                {
                                                    if (loop == 0)
                                                    {
                                                        infoMaskedTextBox = DesignerHost.CreateComponent(typeof(InfoMaskedTextBox), controlName)
                                                            as InfoMaskedTextBox;
                                                    }
                                                    else
                                                    {
                                                        infoMaskedTextBox = DesignerHost.CreateComponent(typeof(InfoMaskedTextBox), controlName + loop.ToString())
                                                            as InfoMaskedTextBox;
                                                    }
                                                    if (infoMaskedTextBox != null)
                                                    {
                                                        if (this.DesignedDataSet.RealDataSet.Tables[this.ColumnsToCreate.TableName].Columns[ColumnsToCreate.Columns[i].ColumnName].DataType == typeof(string))
                                                            infoMaskedTextBox.MaxLength = ColumnsToCreate.Columns[i].MaxLength;
                                                    }

                                                    infoMaskedTextBox.Height = 22;
                                                    infoMaskedTextBox.Width = ColumnsToCreate.ColumnControlWidth;
                                                    infoMaskedTextBox.Location = new Point(CurPosition.X + ColumnsToCreate.LabelWidth + ColumnsToCreate.SpaceBetweenLabelAndColumnControl,
                                                        CurPosition.Y);
                                                    infoMaskedTextBox.DataBindings.Add(new Binding("Text", bindingSource, ColumnsToCreate.Columns[i].ColumnName, true));

                                                    CreateOnControl.Controls.Add(infoMaskedTextBox);
                                                    controls.Add(infoMaskedTextBox);
                                                }

                                                catch
                                                {
                                                    infoMaskedTextBox = null;
                                                    loop++;
                                                }
                                            }
                                            break;
                                        case "Srvtools.InfoDateTimeBox":
                                            InfoDateTimeBox infoDateTimeBox = null;
                                            controlName = ColumnsToCreate.Columns[i].ColumnName + "InfoDateTimeBox";
                                            loop = 0;
                                            while (infoDateTimeBox == null)
                                            {
                                                try
                                                {
                                                    if (loop == 0)
                                                    {
                                                        infoDateTimeBox = DesignerHost.CreateComponent(typeof(InfoDateTimeBox), controlName)
                                                            as InfoDateTimeBox;
                                                    }
                                                    else
                                                    {
                                                        infoDateTimeBox = DesignerHost.CreateComponent(typeof(InfoDateTimeBox), controlName + loop.ToString())
                                                            as InfoDateTimeBox;
                                                    }
 
                                                    //infoDateTimeBox.Height = 22;
                                                    infoDateTimeBox.Width = ColumnsToCreate.ColumnControlWidth;
                                                    infoDateTimeBox.Location = new Point(CurPosition.X + ColumnsToCreate.LabelWidth + ColumnsToCreate.SpaceBetweenLabelAndColumnControl,
                                                        CurPosition.Y);
                                                    infoDateTimeBox.DataBindings.Add(new Binding("BindingValue", bindingSource, ColumnsToCreate.Columns[i].ColumnName, true));

                                                    CreateOnControl.Controls.Add(infoDateTimeBox);
                                                    controls.Add(infoDateTimeBox);
                                                }

                                                catch
                                                {
                                                    infoDateTimeBox = null;
                                                    loop++;
                                                }
                                            }
                                            break;

                                    }
                                }

                                // Select all the controls just created 
                                ISelectionService selectionService = DesignerHost.GetService(typeof(ISelectionService)) as ISelectionService;
                                if (selectionService != null)
                                {
                                    selectionService.SetSelectedComponents(controls);
                                }
                            }
                        }
                        ReadyToCreateComponent = false;
                    }
                }

                return CallNextHookEx(HookHandle, nCode, wParam, lParam);
            }
        }

        private void trvDataSetSchema_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReadyToCreateComponent = true;
            }
        }

        private void InfoDataSetEditorDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (OkClicked)
            {
                if (ValidateOptions() == false)
                {
                    e.Cancel = true;
                    OkClicked = false;
                }
            }

            if (this.MouseHookProcedure != null)
            {
                UnhookWindowsHookEx(HookHandle).ToString();
                HookHandle = IntPtr.Zero;
            }
        }

        private bool InTableOperation = false; // Used as a lock to not allow "Checked On Column" during "Checked On Table"
        private void trvDataSetSchema_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node == trvDataSetSchema.Nodes[0])
            {
                if (e.Node.Checked == true)
                {
                    e.Node.Checked = false;
                }
            }
            else
            {
                // Checked On Table
                if (e.Node.Parent == trvDataSetSchema.Nodes[0])
                {
                    if (e.Node.Checked)
                    {
                        UnCheckChildNodesExcept(e.Node.Parent, e.Node);

                        InTableOperation = true;
                        CheckChildNodes(e.Node);
                        InTableOperation = false;
                    }
                    else
                    {
                        InTableOperation = true;
                        UnCheckChildNodes(e.Node);
                        InTableOperation = false;
                    }
                }
                // Checked On Column
                else if (e.Node.Parent != trvDataSetSchema.Nodes[0] && InTableOperation == false)
                {
                    if (e.Node.Checked)
                    {
                        UnCheckChildNodesExcept(e.Node.Parent.Parent, e.Node.Parent);
                    }
                }
            }
        }

        private void UnCheckChildNodesExcept(TreeNode parentNode, TreeNode exceptNode)
        {
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                if (childNode != exceptNode)
                {
                    childNode.Checked = false;
                }
            }
        }

        private void UnCheckChildNodes(TreeNode parentNode)
        {
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                childNode.Checked = false;
            }
        }

        private void CheckChildNodes(TreeNode parentNode)
        {
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                childNode.Checked = true;
            }
        }

        private void cbRowsOrCols_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbRowsOrCols.SelectedIndex == 0)
            {
                this.txtRowsOrCols.Text = ColumnsToCreate.VerticalRows.ToString();
            }
            else if (this.cbRowsOrCols.SelectedIndex == 1)
            {
                this.txtRowsOrCols.Text = ColumnsToCreate.HorzontallyColumns.ToString();
            }
        }

        private void tabDataSetColumnSelector_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == 0)
            {
                if (ValidateOptions() == false)
                {
                    e.Cancel = true;
                }
            }
        }

        private bool ValidateOptions()
        {
            // Validate Data
            int num = 0;
            string ErrMsg = string.Empty;

            try
            {
                num = int.Parse(txtRowsOrCols.Text);
                if (num <= 0)
                {
                    ErrMsg += this.cbRowsOrCols.Text + " must be a positive integer.\n\r";
                }
            }
            catch
            {
                ErrMsg += this.cbRowsOrCols.Text + " must be a positive integer.\n\r";
            }

            try
            {
                num = int.Parse(txtHorizontalGaps.Text);
                if (num <= 0)
                {
                    ErrMsg += "Horizontal Gaps must be a positive integer.\n\r";
                }
            }
            catch
            {
                ErrMsg += "Horizontal Gaps must be a positive integer.\n\r";
            }

            try
            {
                num = int.Parse(txtVerticalGaps.Text);
                if (num <= 0)
                {
                    ErrMsg += "Vertical Gaps must be a positive integer.\n\r";
                }
            }
            catch
            {
                ErrMsg += "Vertical Gaps must be a positive integer.\n\r";
            }

            // Save Data
            if (ErrMsg == string.Empty)
            {
                if (this.cbRowsOrCols.SelectedIndex == 0)
                {
                    ColumnsToCreate.VerticalRows = int.Parse(txtRowsOrCols.Text);
                    ColumnsToCreate.Direction = CreateDirection.Horizontal;
                }
                else if (this.cbRowsOrCols.SelectedIndex == 1)
                {
                    ColumnsToCreate.HorzontallyColumns = int.Parse(txtRowsOrCols.Text);
                    ColumnsToCreate.Direction = CreateDirection.Vertical;
                }

                ColumnsToCreate.HorizontalGaps = int.Parse(txtHorizontalGaps.Text);
                ColumnsToCreate.VerticalGaps = int.Parse(txtVerticalGaps.Text);

                return true;
            }
            else
            {
                MessageBox.Show(ErrMsg);
                return false;
            }
        }
    }

    internal class ColumnsToCreate
    {
        public string TableName = string.Empty;
        public int LabelWidth = 0;
        public int ColumnControlWidth = 200;
        public int SpaceBetweenLabelAndColumnControl = 10;

        public CreateDirection Direction = CreateDirection.Vertical;
        public int VerticalRows = 1;
        public int HorzontallyColumns = 1;
        public int VerticalGaps = 5;
        public int HorizontalGaps = 5;

        public List<Column> Columns = new List<Column>();
    }

    internal class Column
    {
        public string ColumnName = string.Empty;
        public string Caption = String.Empty;
        public int MaxLength = 255;
        public Type ColumnType = typeof(TextBox);
    }

    internal enum CreateDirection
    {
        Vertical,
        Horizontal
    }
    #endregion InfoDataSetEditorDialog

    #region InfoDataSetEditor
    internal class InfoDataSetEditor : ComponentDesigner
    {
        private IDesignerHost DesignerHost = null;
        private InfoDataSet DesignedDataSet = null;

        private Control RootComponent = null;
        private IRootDesigner RootDesigner = null;
        private Control View = null;
        private ColumnsToCreate ColumnsToCreate = null;

        private InfoDataSetEditorDialog editorDialog = null;

        public InfoDataSetEditor()
            : base()
        {
            DesignerVerb createVerb = new DesignerVerb("Save To Report", new EventHandler(OnCreate));
            this.Verbs.Add(createVerb);
            DesignerVerb createXSDVerb = new DesignerVerb("Create XSD File", new EventHandler(OnCreateXSD));
            this.Verbs.Add(createXSDVerb);
        }

        public void OnCreate(object sender, EventArgs e)
        {
            if (this.Component != null)
            {
                InfoDataSet infoDataSet = (InfoDataSet)this.Component;
                if (infoDataSet.RealDataSet == null)
                {
                    infoDataSet.Active = true;
                }
                string s = EEPRegistry.Server;
                string xmlfile = string.Format("{0}\\EEPNetReport\\", Directory.GetParent(s));
                string str = infoDataSet.RemoteName;
                xmlfile = xmlfile + str.Substring(0, str.IndexOf('.')) + "_" + str.Substring(str.IndexOf('.') + 1) + ".xml";
                infoDataSet.RealDataSet.WriteXmlSchema(xmlfile);
            }
        }

        public virtual void OnCreateXSD(object sender, EventArgs e)
        {
            if (this.Component != null)
            {
                InfoDataSet infoDataSet = (InfoDataSet)this.Component;
                if (infoDataSet.RealDataSet == null)
                {
                    infoDataSet.Active = true;
                }

                string filePath = EditionDifference.ActiveDocumentPath();
                bool CreateFileSucess = true;
                string fileName = "";
                try
                {
                    fileName = filePath + infoDataSet.Site.Name + ".xsd";
                    infoDataSet.RealDataSet.WriteXmlSchema(fileName);
                }
                catch
                {
                    CreateFileSucess = false;
                    MessageBox.Show("Failed to create xsd file!");
                }
                finally
                {
                    if (CreateFileSucess && File.Exists(fileName))
                    {
                        EditionDifference.AddProjectItem(fileName);
                    }
                }
            }
        }

        public override void DoDefaultAction()
        {
            try
            {
                if (DesignerHost == null)
                {
                    DesignerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
                }
                if (DesignedDataSet == null)
                {
                    DesignedDataSet = this.Component as InfoDataSet;
                }
                if (RootComponent == null)
                {
                    RootComponent = DesignerHost.RootComponent as Control;
                }
                if (RootDesigner == null)
                {
                    RootDesigner = DesignerHost.GetDesigner(RootComponent) as IRootDesigner;
                }
                if (View == null)
                {
                    View = RootDesigner.GetView(ViewTechnology.Default) as Control;
                }
                if (ColumnsToCreate == null)
                {
                    ColumnsToCreate = new ColumnsToCreate();
                }

                // if the RootComponent is not a Form
                if (RootComponent == null)
                {
                    return;
                }
                editorDialog = new InfoDataSetEditorDialog();
                editorDialog.ColumnsToCreate = ColumnsToCreate;
                editorDialog.DesignedDataSet = DesignedDataSet;
                editorDialog.DesignerHost = DesignerHost;
                editorDialog.RootDesigner = RootDesigner;
                editorDialog.RootComponent = RootComponent;
                editorDialog.View = View;

                editorDialog.InitialTreeView();
                editorDialog.Show();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
    #endregion InfoDataSetEditor

    #region ComboBoxTreeView
    internal class TreeViewForInfoDataSetColumnSelector : TreeView
    {
        internal InfoDataSet DesignedDataSet = null;
        internal ImageList ImageCollection = null;

        private Button ChangeColumnTypeButton = new Button();

        private ContextMenuStrip ChangeColumnTypeContextMenuStrip = new ContextMenuStrip();
        // TextBox
        private ToolStripMenuItem menuTextBox = new ToolStripMenuItem();
        // Label
        private ToolStripMenuItem menuLabel = new ToolStripMenuItem();
        // ComboBox
        private ToolStripMenuItem menuComboBox = new ToolStripMenuItem();
        // InfoDateTimePicker
        private ToolStripMenuItem menuDateTimePicker = new ToolStripMenuItem();
        // NumericUpDown
        private ToolStripMenuItem menuNumericUpDown = new ToolStripMenuItem();
        // ListBox
        private ToolStripMenuItem menuListBox = new ToolStripMenuItem();
        // LinkLabel
        private ToolStripMenuItem menuLinkLabel = new ToolStripMenuItem();
        // Image
        private ToolStripMenuItem menuImage = new ToolStripMenuItem();
        // CheckBox
        private ToolStripMenuItem menuCheckBox = new ToolStripMenuItem();
        // None
        private ToolStripMenuItem menuNone = new ToolStripMenuItem();
        // InfoTextBox
        private ToolStripMenuItem menuInfoTextBox = new ToolStripMenuItem();
        // InfoRefValBox
        private ToolStripMenuItem menuInfoRefValBox = new ToolStripMenuItem();
        // InfoComboBox
        private ToolStripMenuItem menuInfoWinComboBox = new ToolStripMenuItem();
        // RichTextBox
        private ToolStripMenuItem menuRichTextBox = new ToolStripMenuItem();
        //InfoMaskedTextBox
        private ToolStripMenuItem menuInfoMaskedTextBox = new ToolStripMenuItem();
        //InfoDateTimeBox
        private ToolStripMenuItem menuInfoDateTimeBox = new ToolStripMenuItem();

        bool ChangeColumnTypeContextMenuStripInitialized = false;

        public TreeViewForInfoDataSetColumnSelector()
        {
            // ChangeColumnTypeButton
            ChangeColumnTypeButton.BackColor = SystemColors.Control;
            ChangeColumnTypeButton.FlatStyle = FlatStyle.Popup;
            ChangeColumnTypeButton.Size = new Size(16, 16);

            ChangeColumnTypeButton.Paint += delegate(object sender, PaintEventArgs e)
            {
                e.Graphics.FillPolygon(Brushes.Black, new Point[] { new Point(4, 6), new Point(12, 6), new Point(8, 10) });
            };

            ChangeColumnTypeButton.MouseDown += delegate(object sender, MouseEventArgs e)
            {
                this.Focus();
                if (this.SelectedNode != null && this.SelectedNode.Parent != null)
                {
                    //ChangeColumnTypeContextMenuStrip
                    if (ChangeColumnTypeContextMenuStripInitialized == false)
                    {
                        ChangeColumnTypeContextMenuStrip.ImageList = ImageCollection;
                        ChangeColumnTypeContextMenuStrip.AllowTransparency = true;

                        // TextBox 0
                        menuTextBox.Text = "TextBox";
                        menuTextBox.ImageIndex = 0;
                        menuTextBox.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuTextBox.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };

                        // Label 1
                        menuLabel.Text = "Label";
                        menuLabel.ImageIndex = 1;
                        menuLabel.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuLabel.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };

                        // ComboBox 2
                        menuComboBox.Text = "ComboBox";
                        menuComboBox.ImageIndex = 2;
                        menuComboBox.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuComboBox.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };

                        // InfoDateTimePicker 3
                        menuDateTimePicker.Text = "InfoDateTimePicker";
                        menuDateTimePicker.ImageIndex = 3;
                        menuDateTimePicker.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuDateTimePicker.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };

                        // NumericUpDown 4
                        menuNumericUpDown.Text = "NumericUpDown";
                        menuNumericUpDown.ImageIndex = 4;
                        menuNumericUpDown.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuNumericUpDown.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };

                        // ListBox 5
                        menuListBox.Text = "ListBox";
                        menuListBox.ImageIndex = 5;
                        menuListBox.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuListBox.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };

                        // LinkLabel 6
                        menuLinkLabel.Text = "LinkLabel";
                        menuLinkLabel.ImageIndex = 6;
                        menuLinkLabel.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuLinkLabel.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };

                        // Image 7
                        menuImage.Text = "Image";
                        menuImage.ImageIndex = 7;
                        menuImage.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuImage.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };

                        // CheckBox  8
                        menuCheckBox.Text = "InfoCheckBox";
                        menuCheckBox.ImageIndex = 8;
                        menuCheckBox.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuCheckBox.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };

                        // None 9
                        menuNone.Text = "None";
                        menuNone.ImageIndex = 9;
                        menuNone.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuNone.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };

                        // InfoTextBox 12
                        menuInfoTextBox.Text = "InfoTextBox";
                        menuInfoTextBox.ImageIndex = 12;
                        menuInfoTextBox.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuInfoTextBox.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };
                        // InfoWinComboBox 13
                        menuInfoWinComboBox.Text = "InfoComboBox";
                        menuInfoWinComboBox.ImageIndex = 13;
                        menuInfoWinComboBox.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuInfoWinComboBox.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };

                        // RichTextBox 14
                        menuRichTextBox.Text = "RichTextBox";
                        menuRichTextBox.ImageIndex = 14;
                        menuRichTextBox.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuRichTextBox.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };
                        //InfoRefValBox 15
                        menuInfoRefValBox.Text = "InfoRefValBox";
                        menuInfoRefValBox.ImageIndex = 15;
                        menuInfoRefValBox.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuInfoRefValBox.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };
                        //InfoMaskedTextBox 16
                        menuInfoMaskedTextBox.Text = "InfoMaskedTextBox";
                        menuInfoMaskedTextBox.ImageIndex = 16;
                        menuInfoMaskedTextBox.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuInfoMaskedTextBox.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };
                        //InfoDateTimeBox 17
                        menuInfoDateTimeBox.Text = "InfoDateTimeBox";
                        menuInfoDateTimeBox.ImageIndex = 17;
                        menuInfoDateTimeBox.Click += delegate(object s, EventArgs ea)
                        {
                            this.SelectedNode.ImageIndex = menuInfoDateTimeBox.ImageIndex;
                            this.SelectedNode.SelectedImageIndex = SelectedNode.ImageIndex;
                        };

                        ChangeColumnTypeContextMenuStrip.Items.Add(menuNone);
                        ChangeColumnTypeContextMenuStrip.Items.Add(menuInfoTextBox);
                        ChangeColumnTypeContextMenuStrip.Items.Add(menuInfoRefValBox);
                        ChangeColumnTypeContextMenuStrip.Items.Add(menuInfoWinComboBox);
                        ChangeColumnTypeContextMenuStrip.Items.Add(menuTextBox);
                        ChangeColumnTypeContextMenuStrip.Items.Add(menuRichTextBox);
                        ChangeColumnTypeContextMenuStrip.Items.Add(menuLabel);
                        ChangeColumnTypeContextMenuStrip.Items.Add(menuComboBox);
                        ChangeColumnTypeContextMenuStrip.Items.Add(menuDateTimePicker);
                        ChangeColumnTypeContextMenuStrip.Items.Add(menuNumericUpDown);
                        ChangeColumnTypeContextMenuStrip.Items.Add(menuListBox);
                        ChangeColumnTypeContextMenuStrip.Items.Add(menuLinkLabel);
                        ChangeColumnTypeContextMenuStrip.Items.Add(menuImage);
                        ChangeColumnTypeContextMenuStrip.Items.Add(menuCheckBox);
                        ChangeColumnTypeContextMenuStrip.Items.Add(menuInfoMaskedTextBox);
                        ChangeColumnTypeContextMenuStrip.Items.Add(menuInfoDateTimeBox);

                        ChangeColumnTypeContextMenuStripInitialized = true;
                    }

                    string tableName = this.SelectedNode.Parent.Text;
                    DataTable table = this.DesignedDataSet.RealDataSet.Tables[tableName];
                    if (table == null)
                    {
                        DataRelation relation = this.DesignedDataSet.RealDataSet.Relations[tableName];
                        if (relation == null)
                        {
                            return;
                        }
                        else
                        {
                            table = relation.ChildTable;
                        }
                    }

                    string columnName = this.SelectedNode.Text;
                    DataColumn column = table.Columns[columnName];
                    if (column == null)
                    {
                        return;
                    }

                    Type columnType = column.DataType;
                    switch (columnType.FullName)
                    {
                        case "System.SByte":
                        case "System.Byte":
                        case "System.UInt16":
                        case "System.UInt32":
                        case "System.UInt64":
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                            menuTextBox.Visible = false;
                            menuLabel.Visible = true;
                            menuComboBox.Visible = true;
                            menuDateTimePicker.Visible = false;
                            menuNumericUpDown.Visible = true;
                            menuListBox.Visible = false;
                            menuLinkLabel.Visible = true;
                            menuImage.Visible = false;
                            menuCheckBox.Visible = true;
                            menuNone.Visible = true;
                            menuInfoTextBox.Visible = true;
                            menuInfoRefValBox.Visible = true;
                            menuInfoWinComboBox.Visible = true;
                            menuRichTextBox.Visible = true;
                            menuInfoMaskedTextBox.Visible = true;
                            menuInfoDateTimeBox.Visible = false;
                            break;

                        case "System.Double":
                        case "System.Single":
                        case "System.Decimal":
                            menuTextBox.Visible = false;
                            menuLabel.Visible = true;
                            menuComboBox.Visible = true;
                            menuDateTimePicker.Visible = false;
                            menuNumericUpDown.Visible = false;
                            menuListBox.Visible = false;
                            menuLinkLabel.Visible = true;
                            menuImage.Visible = false;
                            menuCheckBox.Visible = false;
                            menuNone.Visible = true;
                            menuInfoTextBox.Visible = true;
                            menuInfoRefValBox.Visible = true;
                            menuInfoWinComboBox.Visible = true;
                            menuRichTextBox.Visible = true;
                            menuInfoMaskedTextBox.Visible = true;
                            menuInfoDateTimeBox.Visible = false;
                            break;

                        case "System.DateTime":
                            menuTextBox.Visible = false;
                            menuLabel.Visible = true;
                            menuComboBox.Visible = true;
                            menuDateTimePicker.Visible = true;
                            menuNumericUpDown.Visible = false;
                            menuListBox.Visible = false;
                            menuLinkLabel.Visible = true;
                            menuImage.Visible = false;
                            menuCheckBox.Visible = false;
                            menuNone.Visible = true;
                            menuInfoTextBox.Visible = true;
                            menuInfoRefValBox.Visible = true;
                            menuInfoWinComboBox.Visible = true;
                            menuRichTextBox.Visible = true;
                            menuInfoMaskedTextBox.Visible = true;
                            menuInfoDateTimeBox.Visible = true;
                            break;

                        case "System.Char":
                        case "System.String":
                            menuTextBox.Visible = false;
                            menuLabel.Visible = true;
                            menuComboBox.Visible = true;
                            menuDateTimePicker.Visible = true;
                            menuNumericUpDown.Visible = false;
                            menuListBox.Visible = false;
                            menuLinkLabel.Visible = true;
                            menuImage.Visible = false;
                            menuCheckBox.Visible = true;
                            menuNone.Visible = true;
                            menuInfoTextBox.Visible = true;
                            menuInfoRefValBox.Visible = true;
                            menuInfoWinComboBox.Visible = true;
                            menuRichTextBox.Visible = true;
                            menuInfoMaskedTextBox.Visible = true;
                            menuInfoDateTimeBox.Visible = true;
                            break;

                        case "System.Boolean":
                            menuTextBox.Visible = false;
                            menuLabel.Visible = true;
                            menuComboBox.Visible = true;
                            menuDateTimePicker.Visible = false;
                            menuNumericUpDown.Visible = false;
                            menuListBox.Visible = false;
                            menuLinkLabel.Visible = true;
                            menuImage.Visible = false;
                            menuCheckBox.Visible = true;
                            menuNone.Visible = true;
                            menuInfoTextBox.Visible = true;
                            menuInfoRefValBox.Visible = true;
                            menuInfoWinComboBox.Visible = true;
                            menuRichTextBox.Visible = true;
                            menuInfoMaskedTextBox.Visible = true;
                            menuInfoDateTimeBox.Visible = false;
                            break;

                        case "System.Byte[]":
                            menuTextBox.Visible = false;
                            menuLabel.Visible = true;
                            menuComboBox.Visible = true;
                            menuDateTimePicker.Visible = false;
                            menuNumericUpDown.Visible = false;
                            menuListBox.Visible = false;
                            menuLinkLabel.Visible = true;
                            menuImage.Visible = true;
                            menuCheckBox.Visible = false;
                            menuNone.Visible = true;
                            menuInfoTextBox.Visible = true;
                            menuInfoRefValBox.Visible = true;
                            menuInfoWinComboBox.Visible = true;
                            menuRichTextBox.Visible = true;
                            menuInfoMaskedTextBox.Visible = true;
                            menuInfoDateTimeBox.Visible = false;
                            break;
                    }

                    menuTextBox.Checked = false;
                    menuLabel.Checked = false;
                    menuComboBox.Checked = false;
                    menuDateTimePicker.Checked = false;
                    menuNumericUpDown.Checked = false;
                    menuListBox.Checked = false;
                    menuLinkLabel.Checked = false;
                    menuImage.Checked = false;
                    menuCheckBox.Checked = false;
                    menuNone.Checked = false;
                    menuInfoTextBox.Checked = false;
                    menuInfoRefValBox.Visible = true;
                    menuInfoWinComboBox.Checked = false;
                    menuRichTextBox.Checked = false;
                    menuInfoMaskedTextBox.Checked = false;
                    menuInfoDateTimeBox.Checked = false;

                    switch (this.SelectedNode.ImageIndex)
                    {
                        case 0:
                            menuTextBox.Checked = true;
                            break;
                        case 1:
                            menuLabel.Checked = true;
                            break;
                        case 2:
                            menuComboBox.Checked = true;
                            break;
                        case 3:
                            menuDateTimePicker.Checked = true;
                            break;
                        case 4:
                            menuNumericUpDown.Checked = true;
                            break;
                        case 5:
                            menuListBox.Checked = true;
                            break;
                        case 6:
                            menuLinkLabel.Checked = true;
                            break;
                        case 7:
                            menuImage.Checked = true;
                            break;
                        case 8:
                            menuCheckBox.Checked = true;
                            break;
                        case 9:
                            menuNone.Checked = true;
                            break;
                        case 12:
                            menuInfoTextBox.Checked = true;
                            break;
                        case 13:
                            menuInfoWinComboBox.Checked = true;
                            break;
                        case 14:
                            menuRichTextBox.Checked = true;
                            break;
                        case 15:
                            menuInfoRefValBox.Checked = true;
                            break;
                        case 16:
                            menuInfoMaskedTextBox.Checked = true;
                            break;
                        case 17:
                            menuInfoDateTimeBox.Checked = true;
                            break;

                    }

                    Point displayPosition = this.PointToScreen(new Point(this.SelectedNode.Bounds.Left, this.SelectedNode.Bounds.Top + this.SelectedNode.Bounds.Height));
                    this.ChangeColumnTypeContextMenuStrip.Show(displayPosition);
                }
            };

            ChangeColumnTypeButton.Hide();
            this.Controls.Add(ChangeColumnTypeButton);
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            if (e.Node.SelectedImageIndex != e.Node.ImageIndex)
            {
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
            // Select on Column
            if (this.Nodes.Count > 0 && e.Node != null
                && e.Node != this.Nodes[0] && e.Node.Parent != this.Nodes[0])
            {
                ChangeColumnTypeButton.Left = this.SelectedNode.Bounds.Location.X + this.SelectedNode.Bounds.Width;
                ChangeColumnTypeButton.Top = this.SelectedNode.Bounds.Location.Y;
                ChangeColumnTypeButton.Show();
            }
            else
            {
                ChangeColumnTypeButton.Hide();
            }

            base.OnAfterSelect(e);
        }
    }
    #endregion ComboBoxTreeView
}
