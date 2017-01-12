using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.ComponentModel.Design;
using System.Collections;
using System.Xml;
using EnvDTE80;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using Srvtools;


namespace OfficeTools.Design
{
    /// <summary>
    /// The class of frmDesigner, used to design the plate component
    /// </summary>
    public partial class frmDesigner : Form
    {
        IDesignerHost DesignerHost = null;
        OfficePlate plate;
        Hashtable tabledatasource = new Hashtable();

        /// <summary>
        /// Create a new instance of frmDesigner
        /// </summary>
        /// <param name="op">The plate to design</param>
        /// <param name="host">The designer host</param>
        public frmDesigner(OfficePlate op, IDesignerHost host)
        {
            InitializeComponent();
            plate = op;
            DesignerHost = host;
        }

        private void frmDesigner_Load(object sender, EventArgs e)
        {
            #region setup language
            int lang = (int)CliSysMegLag.GetClientLanguage();
            this.Text = "Plate" + OfficeTools.Properties.Resources.editor.Split(',')[lang];
            string savetext = OfficeTools.Properties.Resources.btnSave.Split(',')[lang];
            btnSaveDataSource.Text = savetext;
            btnSaveTag.Text = savetext;
            btnSaveProperty.Text = savetext;
            string undotext = OfficeTools.Properties.Resources.btnUndo.Split(',')[lang];
            btnUndoDataSource.Text = undotext;
            btnUndoTag.Text = undotext;
            btnUndoProperty.Text = undotext;
            debugToolStripMenuItem.Text = OfficeTools.Properties.Resources.btnDebug.Split(',')[lang];
            startToolStripMenuItem.Text = OfficeTools.Properties.Resources.btnStart.Split(',')[lang];
            cancelToolStripMenuItem.Text = OfficeTools.Properties.Resources.btnCancel.Split(',')[lang];
            editFileToolStripMenuItem.Text = OfficeTools.Properties.Resources.btnEdit.Split(',')[lang];
            viewToolStripMenuItem.Text = OfficeTools.Properties.Resources.btnView.Split(',')[lang];
            outputToolStripMenuItem.Text = OfficeTools.Properties.Resources.btnOutput.Split(',')[lang]; 
            #endregion
            
            for (int i = 0; i < DesignerHost.Container.Components.Count; i++)
            {
                if (DesignerHost.Container.Components[i] is DataSet)
                {
                    string datasourcename = DesignerHost.Container.Components[i].Site.Name;
                    ColumnDataSource.Items.Add(datasourcename);
                    
                    DataTableCollection tables = (DesignerHost.Container.Components[i] as DataSet).Tables;
                    ArrayList listtable = new ArrayList();
                    for (int j = 0; j < tables.Count; j++)
			        {
                        listtable.Add(tables[j].TableName);
			        }
                    tabledatasource.Add(datasourcename, listtable);
                }
                else if (DesignerHost.Container.Components[i] is Srvtools.InfoDataSet)
                {
                    string datasourcename = DesignerHost.Container.Components[i].Site.Name;
                    ColumnDataSource.Items.Add(datasourcename);

                    DataTableCollection tables = (DesignerHost.Container.Components[i] as Srvtools.InfoDataSet).RealDataSet.Tables;
                    ArrayList listtable = new ArrayList();
                    for (int j = 0; j < tables.Count; j++)
                    {
                        listtable.Add(tables[j].TableName);
                    }
                    tabledatasource.Add(datasourcename, listtable);
                }
                else if (DesignerHost.Container.Components[i] is Srvtools.InfoBindingSource)
                {
                    string datasourcename = DesignerHost.Container.Components[i].Site.Name;
                    ColumnDataSource.Items.Add(datasourcename);
                    ArrayList listtable = new ArrayList();
                    if (((DesignerHost.Container.Components[i] as Srvtools.InfoBindingSource).List as DataView != null))
                    {
                        DataTable tables = ((DesignerHost.Container.Components[i] as Srvtools.InfoBindingSource).List as DataView).Table;
                        listtable.Add(tables.TableName);
                    }
                    else
                    {
                        InfoDataSet dataset = (DesignerHost.Container.Components[i] as Srvtools.InfoBindingSource).GetDataSource();
                        if (dataset != null)
                        {
                            string tableName = (DesignerHost.Container.Components[i] as Srvtools.InfoBindingSource).GetTableName();
                            listtable.Add(tableName);
                        }
                    }
                    tabledatasource.Add(datasourcename, listtable);
                }
            }

            for (int i = 0; i < plate.DataSource.Count; i++)
            {
                dataGridViewDataSource.Rows.Add();
                dataGridViewDataSource.Rows[i].Cells["ColumnCaption"].Value = ((DataSourceItem)plate.DataSource[i]).Caption;
                dataGridViewDataSource.Rows[i].Cells["ColumnDataSource"].Value
                    = (((IComponent)((DataSourceItem)plate.DataSource[i]).DataSource).Site == null) ? string.Empty : ((IComponent)((DataSourceItem)plate.DataSource[i]).DataSource).Site.Name;
                dataGridViewDataSource.Rows[i].Cells["ColumnDataMember"].Value = ((DataSourceItem)plate.DataSource[i]).DataMember;
                dataGridViewDataSource.Rows[i].Cells["ColumnImageColumns"].Value = "(Collection)";
                dataGridViewDataSource.Rows[i].Cells["ColumnImageColumns"].Tag = ((DataSourceItem)plate.DataSource[i]).ImageColumns;
            }

            for (int i = 0; i < plate.Tags.Count; i++)
            {
                dataGridViewTag.Rows.Add();
                dataGridViewTag.Rows[i].Cells["ColumnDataField"].Value = ((TagItem)plate.Tags[i]).DataField;
                dataGridViewTag.Rows[i].Cells["ColumnExpression"].Value = ((TagItem)plate.Tags[i]).Exp;
                dataGridViewTag.Rows[i].Cells["ColumnFormat"].Value = ((TagItem)plate.Tags[i]).Format;
            }

            tbEmailAddress.Text = plate.EmailAddress;
            tbEmailTitle.Text = plate.EmailTitle;
            tbOutputFileName.Text = plate.OutputFileName;
            tbOutputFilePath.Text = plate.OutputPath;
            tbTemplateFileName.Text = plate.OfficeFile;

            cbPlateMode.Text = plate.PlateMode.ToString();
            cbOutputMode.Text = plate.OutputMode.ToString();
            cbMarkException.Checked = plate.MarkException;
            cbShowAction.Checked = plate.ShowAction;
            tabPageProperty.Text = "Other Property";
            btnSaveProperty.Enabled = false;
            btnUndoProperty.Enabled = false;
        }

        private void frmDesigner_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (td != null && td.ThreadState != ThreadState.Stopped)
            {
                td.Abort();
                cancelToolStripMenuItem.Enabled = false;
                startToolStripMenuItem.Enabled = true;
            }
            if (tabPageDataSource.Text.EndsWith("*") || tabPageTag.Text.EndsWith("*") || tabPageProperty.Text.EndsWith("*"))
            {
                DialogResult dr = MessageBox.Show("Some items have changed, save it?","Plate Designer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else if (dr == DialogResult.Yes)
                {
                    if(tabPageDataSource.Text.EndsWith("*") && !SaveDataSource())
                    {
                        tabControl.SelectedTab = tabPageDataSource;
                        e.Cancel = true;
                        return;
                    }
                    if(tabPageTag.Text.EndsWith("*") && !SaveTag())
                    {
                        tabControl.SelectedTab = tabPageTag;
                        e.Cancel = true;
                        return;
                    }
                    if(tabPageProperty.Text.EndsWith("*") && !SaveProperty())
                    {
                        tabControl.SelectedTab = tabPageProperty;
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        #region Item Changed event
        private void dataGridViewDataSource_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dataGridViewDataSource.Columns[e.ColumnIndex].Name == "ColumnDataMember")
            {
                object strdatasource = dataGridViewDataSource.Rows[e.RowIndex].Cells["ColumnDataSource"].Value;
                if (strdatasource == null)
                {
                    strdatasource = string.Empty;
                }
                object strdatamember = dataGridViewDataSource.Rows[e.RowIndex].Cells["ColumnDataMember"].Value;
                if (strdatamember == null)
                {
                    strdatamember = string.Empty;
                }
                if (tabledatasource[strdatasource] != null)
                {
                    ArrayList objdatamember = (ArrayList)tabledatasource[strdatasource];
                    frmDataMember fdm = new frmDataMember(objdatamember, strdatamember.ToString());
                    if (fdm.ShowDialog() == DialogResult.OK)
                    {
                        if (fdm.lbDataMember.SelectedItem != null)
                        {
                            dataGridViewDataSource.Rows[e.RowIndex].Cells["ColumnDataMember"].Value = fdm.lbDataMember.SelectedItem;
                        }
                        else
                        {
                            dataGridViewDataSource.Rows[e.RowIndex].Cells["ColumnDataMember"].Value = string.Empty;
                        }
                        tabPageDataSource.Text = "DataSource Defination*";
                        btnSaveDataSource.Enabled = true;
                        btnUndoDataSource.Enabled = true;
                    }
                }
                e.Cancel = true;
            }
            else if (dataGridViewDataSource.Columns[e.ColumnIndex].Name == "ColumnImageColumns")
            {
                DataSourceImageColumnCollections dc = dataGridViewDataSource.Rows[e.RowIndex].Cells["ColumnImageColumns"].Tag as DataSourceImageColumnCollections;
                //create a new item
                DataSourceItem di = new DataSourceItem();
                object objdatasource = dataGridViewDataSource.Rows[e.RowIndex].Cells["ColumnDataSource"].Value;
                object objdatamember = dataGridViewDataSource.Rows[e.RowIndex].Cells["ColumnDataMember"].Value;
                if (objdatasource == null || objdatasource.ToString().Trim().Length == 0 || objdatamember == null || objdatamember.ToString().Trim().Length == 0)
                {
                    MessageBox.Show("Select datasource first!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    di.DataSource = DesignerHost.Container.Components[objdatasource.ToString()];
                    di.DataMember = objdatamember.ToString();
                    frmImageColumns fic = new frmImageColumns(dc, di);
                    if (fic.ShowDialog() == DialogResult.OK)
                    {
                        dataGridViewDataSource.Rows[e.RowIndex].Cells["ColumnImageColumns"].Tag = fic.imagecollectionedit;
                        tabPageDataSource.Text = "DataSource Defination*";
                        btnSaveDataSource.Enabled = true;
                        btnUndoDataSource.Enabled = true;
                    }
                }
                e.Cancel = true;
            }
            else
            {
                tabPageDataSource.Text = "DataSource Defination*";
                btnSaveDataSource.Enabled = true;
                btnUndoDataSource.Enabled = true;
            }
        }

        private void dataGridViewDataSource_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            dataGridViewDataSource.Rows[e.Row.Index - 1].Cells["ColumnImageColumns"].Value = "(Collection)";
            dataGridViewDataSource.Rows[e.Row.Index - 1].Cells["ColumnImageColumns"].Tag = new DataSourceImageColumnCollections(null, typeof(DataSourceImageColumnItem));
        }

        private void dataGridViewDataSource_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            tabPageDataSource.Text = "DataSource Defination*";
            btnSaveDataSource.Enabled = true;
            btnUndoDataSource.Enabled = true;
        }

        private void dataGridViewTag_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dataGridViewTag.Columns[e.ColumnIndex].Name == "ColumnExpression")
            {
                object strexpression = dataGridViewTag.Rows[e.RowIndex].Cells["ColumnExpression"].Value;
                if (strexpression == null)
                {
                    strexpression = string.Empty;
                }
                List<IComponent> listcomponent = new List<IComponent>();
                for (int i = 0; i < DesignerHost.Container.Components.Count; i++)
                {
                    if (DesignerHost.Container.Components[i] is Control)
                    {
                        listcomponent.Add(DesignerHost.Container.Components[i]);
                    }
                }
                frmValue fv = new frmValue(strexpression.ToString(), plate, listcomponent);
                if (fv.ShowDialog() == DialogResult.OK)
                {
                    dataGridViewTag.Rows[e.RowIndex].Cells["ColumnExpression"].Value = fv.tbScript.Text.Replace("\r", string.Empty).Replace("\n", string.Empty);
                    tabPageTag.Text = "Tag Defination*";
                    btnSaveTag.Enabled = true;
                    btnUndoTag.Enabled = true;
                }
                e.Cancel = true;
            }
            else
            {
                tabPageTag.Text = "Tag Defination*";
                btnSaveTag.Enabled = true;
                btnUndoTag.Enabled = true;
            }
        }

        private void dataGridViewTag_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            tabPageTag.Text = "Tag Defination*";
            btnSaveTag.Enabled = true;
            btnUndoTag.Enabled = true;
        }

        private void Item_Changed(object sender, EventArgs e)
        {
            tabPageProperty.Text = "Other Property*";
            btnSaveProperty.Enabled = true;
            btnUndoProperty.Enabled = true;
        } 
        #endregion

        #region Undo Button Click event
        private void btnUndoDataSource_Click(object sender, EventArgs e)
        {
            dataGridViewDataSource.Rows.Clear();
            for (int i = 0; i < plate.DataSource.Count; i++)
            {
                dataGridViewDataSource.Rows.Add();
                dataGridViewDataSource.Rows[i].Cells["ColumnCaption"].Value = ((DataSourceItem)plate.DataSource[i]).Caption;
                dataGridViewDataSource.Rows[i].Cells["ColumnDataSource"].Value
                    = (((IComponent)((DataSourceItem)plate.DataSource[i]).DataSource).Site == null) ? string.Empty : ((IComponent)((DataSourceItem)plate.DataSource[i]).DataSource).Site.Name;
                dataGridViewDataSource.Rows[i].Cells["ColumnDataMember"].Value = ((DataSourceItem)plate.DataSource[i]).DataMember;
                dataGridViewDataSource.Rows[i].Cells["ColumnImageColumns"].Value = "(Collection)";
                dataGridViewDataSource.Rows[i].Cells["ColumnImageColumns"].Tag = ((DataSourceItem)plate.DataSource[i]).ImageColumns;
            }

            tabPageDataSource.Text = "DataSource Defination";
            btnSaveDataSource.Enabled = false;
            btnUndoDataSource.Enabled = false;
        }

        private void btnUndoTag_Click(object sender, EventArgs e)
        {
            dataGridViewTag.Rows.Clear();
            for (int i = 0; i < plate.Tags.Count; i++)
            {
                dataGridViewTag.Rows.Add();
                dataGridViewTag.Rows[i].Cells["ColumnDataField"].Value = ((TagItem)plate.Tags[i]).DataField;
                dataGridViewTag.Rows[i].Cells["ColumnExpression"].Value = ((TagItem)plate.Tags[i]).Exp;
                dataGridViewTag.Rows[i].Cells["ColumnFormat"].Value = ((TagItem)plate.Tags[i]).Format;
            }

            tabPageTag.Text = "Tag Defination";
            btnSaveTag.Enabled = false;
            btnUndoTag.Enabled = false;
        }

        private void btnUndoProperty_Click(object sender, EventArgs e)
        {
            tbEmailAddress.Text = plate.EmailAddress;
            tbEmailTitle.Text = plate.EmailTitle;
            tbOutputFileName.Text = plate.OutputFileName;
            tbOutputFilePath.Text = plate.OutputPath;
            tbTemplateFileName.Text = plate.OfficeFile;

            cbOutputMode.Text = plate.OutputMode.ToString();
            cbPlateMode.Text = plate.PlateMode.ToString();
            cbMarkException.Checked = plate.MarkException;
            cbShowAction.Checked = plate.ShowAction;

            tabPageProperty.Text = "Other Property";
            btnSaveProperty.Enabled = false;
            btnUndoProperty.Enabled = false;
        }
        #endregion

        #region Save Button Click event
        private void btnSaveDataSource_Click(object sender, EventArgs e)
        {
            SaveDataSource();
        }

        private bool SaveDataSource()
        {
            int rowcount = dataGridViewDataSource.Rows.Count;
            OfficePlate platenew = new OfficePlate();
            for (int i = 0; i < rowcount - 1; i++)
            {
                DataSourceItem di = new DataSourceItem();
                object objcaption = dataGridViewDataSource.Rows[i].Cells["ColumnCaption"].Value;
                if (objcaption == null || objcaption.ToString().Trim().Length == 0)
                {
                    MessageBox.Show("Caption can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                di.Caption = objcaption.ToString();
                object objdatasource = dataGridViewDataSource.Rows[i].Cells["ColumnDataSource"].Value;
                if (objdatasource == null || objdatasource.ToString().Trim().Length == 0)
                {
                    MessageBox.Show("DataSource can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                di.DataSource = DesignerHost.Container.Components[objdatasource.ToString()];
                object objdatamember = dataGridViewDataSource.Rows[i].Cells["ColumnDataMember"].Value;
                if (objdatamember == null || objdatamember.ToString().Trim().Length == 0)
                {
                    MessageBox.Show("DataMember can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                di.DataMember = objdatamember.ToString();
                DataSourceImageColumnCollections dc = dataGridViewDataSource.Rows[i].Cells["ColumnImageColumns"].Tag as DataSourceImageColumnCollections;
                for (int j = 0; j < dc.Count; j++)
                {
                    di.ImageColumns.Add(dc[j]);
                }
                platenew.DataSource.Add(di);
            }

            IComponentChangeService ComponentChangeService =
               this.DesignerHost.GetService(typeof(IComponentChangeService)) as IComponentChangeService;

            object oldValue = null;
            object newValue = null;

            PropertyDescriptor desDataSource = TypeDescriptor.GetProperties(plate)["DataSource"];
            ComponentChangeService.OnComponentChanging(this.plate, desDataSource);
            oldValue = plate.DataSource;
            plate.DataSource = platenew.DataSource;
            newValue = plate.DataSource;
            ComponentChangeService.OnComponentChanged(plate, desDataSource, oldValue, newValue);

            tabPageDataSource.Text = "DataSource Defination";
            btnSaveDataSource.Enabled = false;
            btnUndoDataSource.Enabled = false;
            return true;
        }

        private void btnSaveTag_Click(object sender, EventArgs e)
        {
            SaveTag();
        }

        private bool SaveTag()
        {
            int rowcount = dataGridViewTag.Rows.Count;
            OfficePlate platenew = new OfficePlate();
            for (int i = 0; i < rowcount - 1; i++)
            {
                TagItem ti = new TagItem();
                object objdatafield = dataGridViewTag.Rows[i].Cells["ColumnDataField"].Value;
                if (objdatafield == null || objdatafield.ToString().Trim().Length == 0)
                {
                    MessageBox.Show("DataField can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                ti.DataField = objdatafield.ToString();
                object objexpression = dataGridViewTag.Rows[i].Cells["ColumnExpression"].Value;
                if (objexpression == null || objexpression.ToString().Trim().Length == 0)
                {
                    MessageBox.Show("Expression can not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                ti.Exp = objexpression.ToString();
                object objformat = dataGridViewTag.Rows[i].Cells["ColumnFormat"].Value;
                if (objformat == null)
                {
                    objformat = string.Empty;
                }
                ti.Format = objformat.ToString();
                platenew.Tags.Add(ti);
            }

            IComponentChangeService ComponentChangeService =
               this.DesignerHost.GetService(typeof(IComponentChangeService)) as IComponentChangeService;

            object oldValue = null;
            object newValue = null;

            PropertyDescriptor desTags = TypeDescriptor.GetProperties(plate)["Tags"];
            ComponentChangeService.OnComponentChanging(this.plate, desTags);
            oldValue = plate.Tags;
            plate.Tags = platenew.Tags;
            newValue = plate.Tags;
            ComponentChangeService.OnComponentChanged(plate, desTags, oldValue, newValue);

            tabPageTag.Text = "Tag Defination";
            btnSaveTag.Enabled = false;
            btnUndoTag.Enabled = false;
            return true;
        }

        private void btnSaveProperty_Click(object sender, EventArgs e)
        {
            SaveProperty();
        }

        private bool SaveProperty()
        {
            IComponentChangeService ComponentChangeService =
              this.DesignerHost.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            object oldValue = null;
            object newValue = null;

            PropertyDescriptor desEmailAddress = TypeDescriptor.GetProperties(plate)["EmailAddress"];
            ComponentChangeService.OnComponentChanging(this.plate, desEmailAddress);
            oldValue = plate.EmailAddress;
            plate.EmailAddress = tbEmailAddress.Text;
            newValue = plate.EmailAddress;
            ComponentChangeService.OnComponentChanged(plate, desEmailAddress, oldValue, newValue);

            PropertyDescriptor desEmailTitle = TypeDescriptor.GetProperties(plate)["EmailTitle"];
            ComponentChangeService.OnComponentChanging(this.plate, desEmailTitle);
            oldValue = plate.EmailTitle;
            plate.EmailTitle = tbEmailTitle.Text;
            newValue = plate.EmailTitle;
            ComponentChangeService.OnComponentChanged(plate, desEmailTitle, oldValue, newValue);

            PropertyDescriptor desOutputFileName = TypeDescriptor.GetProperties(plate)["OutputFileName"];
            ComponentChangeService.OnComponentChanging(this.plate, desOutputFileName);
            oldValue = plate.OutputFileName;
            plate.OutputFileName = tbOutputFileName.Text;
            newValue = plate.OutputFileName;
            ComponentChangeService.OnComponentChanged(plate, desOutputFileName, oldValue, newValue);

            PropertyDescriptor desOutputPath = TypeDescriptor.GetProperties(plate)["OutputPath"];
            ComponentChangeService.OnComponentChanging(this.plate, desOutputPath);
            oldValue = plate.OutputPath;
            plate.OutputPath = tbOutputFilePath.Text;
            newValue = plate.OutputPath;
            ComponentChangeService.OnComponentChanged(plate, desOutputPath, oldValue, newValue);

            PropertyDescriptor desOfficeFile = TypeDescriptor.GetProperties(plate)["OfficeFile"];
            ComponentChangeService.OnComponentChanging(this.plate, desOfficeFile);
            oldValue = plate.OfficeFile;
            plate.OfficeFile = tbTemplateFileName.Text;
            newValue = plate.OfficeFile;
            ComponentChangeService.OnComponentChanged(plate, desOfficeFile, oldValue, newValue);

            PropertyDescriptor desOutputMode = TypeDescriptor.GetProperties(plate)["OutputMode"];
            ComponentChangeService.OnComponentChanging(this.plate, desOutputMode);
            oldValue = plate.OutputMode;
            switch (cbOutputMode.Text)
            {
                case "None": plate.OutputMode = OfficePlate.OutputModeType.None; break;
                case "Email": plate.OutputMode = OfficePlate.OutputModeType.Email; break;
                case "Launch": plate.OutputMode = OfficePlate.OutputModeType.Launch; break;
                default: plate.OutputMode = OfficePlate.OutputModeType.None; break;
            }
            newValue = plate.OutputMode;
            ComponentChangeService.OnComponentChanged(plate, desOutputMode, oldValue, newValue);

            PropertyDescriptor desPlateMode = TypeDescriptor.GetProperties(plate)["PlateMode"];
            ComponentChangeService.OnComponentChanging(this.plate, desPlateMode);
            oldValue = plate.PlateMode;
            switch (cbPlateMode.Text)
            {
                case "Xml": plate.PlateMode = PlateModeType.Xml; break;
                case "Com": plate.PlateMode = PlateModeType.Com; break;
                default: plate.PlateMode = PlateModeType.Xml; break;
            }
            newValue = plate.PlateMode;
            ComponentChangeService.OnComponentChanged(plate, desPlateMode, oldValue, newValue);

            PropertyDescriptor desMarkException = TypeDescriptor.GetProperties(plate)["MarkException"];
            ComponentChangeService.OnComponentChanging(this.plate, desMarkException);
            oldValue = plate.MarkException;
            plate.MarkException = cbMarkException.Checked;
            newValue = plate.MarkException;
            ComponentChangeService.OnComponentChanged(plate, desMarkException, oldValue, newValue);

            PropertyDescriptor desShowAction = TypeDescriptor.GetProperties(plate)["ShowAction"];
            ComponentChangeService.OnComponentChanging(this.plate, desShowAction);
            oldValue = plate.ShowAction;
            plate.ShowAction = cbShowAction.Checked;
            newValue = plate.ShowAction;
            ComponentChangeService.OnComponentChanged(plate, desShowAction, oldValue, newValue);

            tabPageProperty.Text = "Other Property";
            btnSaveProperty.Enabled = false;
            btnUndoProperty.Enabled = false;
            return true;
        }
        #endregion

        private void btnDetail_Click(object sender, EventArgs e)
        {
            if (tbEmailAddressDetail.Visible)
            {
                tbEmailAddress.Text = tbEmailAddressDetail.Text.Replace("\r\n", ";");
            }
            else
            {
                tbEmailAddressDetail.Text = tbEmailAddress.Text.Replace(";", "\r\n");
            }
            tbEmailAddressDetail.Visible = !tbEmailAddressDetail.Visible;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(Directory.Exists(tbOutputFilePath.Text))
            {
                fbd.SelectedPath = tbOutputFilePath.Text;
            }
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbOutputFilePath.Text = fbd.SelectedPath;
            }
        }

        Thread td = null;
        #region MenuItem Click event
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridViewDebug.Rows.Clear();
            toolStripProgressBar.Value = 0;
            toolStripStatusLabel.Text = "Debugging...";
            cancelToolStripMenuItem.Enabled = true;
            startToolStripMenuItem.Enabled = false;

            td = new Thread(new ThreadStart(Debug));
            td.Start();
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (td != null && td.ThreadState != ThreadState.Stopped)
            {
                td.Abort();
                cancelToolStripMenuItem.Enabled = false;
                startToolStripMenuItem.Enabled = true;
            }
        }

        private void editFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DTE2 aDTE = (DTE2)Marshal.GetActiveObject("VisualStudio.DTE.10.0");
            Solution2 sol = (Solution2)aDTE.Solution;
            string sCurProject = Path.ChangeExtension(Path.GetFileName(sol.FileName), string.Empty);
            string officefile = string.Format("{0}\\{1}\\{2}", EEPRegistry.Client, sCurProject, plate.OfficeFile);

            if (!File.Exists(officefile))
            {
                DialogResult dr =  MessageBox.Show(string.Format("Can not find Template file: {0},Create it?", plate.OfficeFile), "Plate Designer", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    CreateFile(officefile);
                }
                else
                {
                    return;
                }
            }

            if (plate is ExcelPlate)
            {
                System.Diagnostics.Process.Start("excel.exe", "\"" + officefile + "\"");
               
            }
            else if (plate is WordPlate)
            {
                System.Diagnostics.Process.Start("winword.exe", "\"" + officefile + "\"");
            }
        }

        private void outputToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            panelMessage.Visible = outputToolStripMenuItem.Checked;
        } 
        #endregion

        private void CreateFile(string filename)
        {
            StreamWriter sw = new StreamWriter(filename, false);
            
            if (plate is ExcelPlate)
            {
                sw.WriteLine("<?xml version=\"1.0\"?>");
                sw.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
                sw.WriteLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
                sw.WriteLine("xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
                sw.WriteLine("xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
                sw.WriteLine("xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");
                sw.WriteLine("<Worksheet ss:Name=\"Sheet1\">");
                sw.WriteLine("</Worksheet>");
                sw.WriteLine("</Workbook>");
            }
            else if (plate is WordPlate)
            {
                sw.WriteLine("<?xml version=\"1.0\"?>");
                sw.WriteLine("<?mso-application progid=\"Word.Document\"?>");
                sw.WriteLine("<w:wordDocument xmlns:w=\"http://schemas.microsoft.com/office/word/2003/wordml\"");
                sw.WriteLine("xmlns:wx=\"http://schemas.microsoft.com/office/word/2003/auxHint\">");
                sw.WriteLine("<w:body><wx:sect><w:p/></wx:sect></w:body>");
                sw.WriteLine("</w:wordDocument>");
            }
            sw.Close();
        }

        /// <summary>
        /// The function used to debug
        /// </summary>
        public void Debug()
        {
            DTE2 aDTE = (DTE2)Marshal.GetActiveObject("VisualStudio.DTE.10.0");
            Solution2 sol = (Solution2)aDTE.Solution;
            string sCurProject = Path.ChangeExtension(Path.GetFileName(sol.FileName), string.Empty);
            string officefile = string.Format("{0}\\{1}\\{2}", EEPRegistry.Client, sCurProject, plate.OfficeFile);;

            if (!File.Exists(officefile))
            {
                SetText(toolStripStatusLabel, "Check failed");
                SetProgress(toolStripProgressBar, 100);
                AddRow(dataGridViewDebug, string.Format("Can not find Template file: {0}", plate.OfficeFile), string.Empty, "error");
                EnableControl(cancelToolStripMenuItem, false);
                EnableControl(startToolStripMenuItem, true);
                return;
            }
            bool blsuccess = true;
            if (plate.PlateMode == PlateModeType.Xml)
            {
                blsuccess = DebugXml(officefile);
            }
            else
            {
                //blsuccess = DebugCom(officefile);
            }

            SetProgress(toolStripProgressBar, 100);
            if (blsuccess)
            {
                SetText(toolStripStatusLabel, "Check complete with success");
            }
            else
            {
                SetText(toolStripStatusLabel, "Check complete with errors");
            }
            EnableControl(cancelToolStripMenuItem, false);
            EnableControl(startToolStripMenuItem, true);
        }

        /// <summary>
        /// The function used to debug in Xml mode
        /// </summary>
        /// <param name="filename">Name of file</param>
        /// <returns>The result of debug</returns>
        public bool DebugXml(string filename)
        {
            bool blsuccess = true;
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(filename);
            }
            catch
            {
                AddRow(dataGridViewDebug, string.Format("Template file: {0} is not stored in xml format", plate.OfficeFile), string.Empty, "error");
                return false;
            }

            XmlNamespaceManager xmlmgr = new XmlNamespaceManager(xml.NameTable);
            xmlmgr.AddNamespace("sheet", "urn:schemas-microsoft-com:office:spreadsheet");
            xmlmgr.AddNamespace("w", "http://schemas.microsoft.com/office/word/2003/wordml");
            xmlmgr.AddNamespace("wx", "http://schemas.microsoft.com/office/word/2003/auxHint");

            string cellname = string.Empty;
            if (plate is ExcelPlate)
            {
                cellname = "/sheet:Workbook/sheet:Worksheet/sheet:Table/sheet:Row";
            }
            else if (plate is WordPlate)
            {
                cellname = "/w:wordDocument/w:body/wx:sect/w:p/w:r/w:t";
            }

            XmlNodeList nodelist = xml.DocumentElement.SelectNodes(cellname, xmlmgr);
            for (int i = 0; i < nodelist.Count; i++)
            {
                if (plate is WordPlate)
                {
                    string[] arrinnertext = nodelist[i].InnerText.Split(' ');
                    foreach (string str in arrinnertext)
                    {
                        if (str.StartsWith("$") && str.Length > 1)
                        {
                            object[] objret = DebugDetail(plate, str.Substring(1));
                            if ((int)objret[0] == 1)
                            {
                                AddRow(dataGridViewDebug, objret[1].ToString(), str, objret[2].ToString());
                                blsuccess = false;
                            }
                        }
                    }
                }
                else if (plate is ExcelPlate)
                {
                    foreach (XmlNode xn in nodelist[i].ChildNodes)
                    {
                        string strvalue = xn.InnerText.Trim();
                        if (strvalue.StartsWith("$") && strvalue.Length > 1)
                        {
                            object[] objret = DebugDetail(plate, strvalue.Substring(1));
                            if ((int)objret[0] == 1)
                            {
                                AddRow(dataGridViewDebug, objret[1].ToString(), strvalue, objret[2].ToString());
                                blsuccess = false;
                            }
                        }
                    }
                }
                SetProgress(toolStripProgressBar, (i * 100 / nodelist.Count));
            }
            return blsuccess;
        }

        /// <summary>
        /// The function used to debug in Com mode
        /// </summary>
        /// <param name="filename">Name of file</param>
        /// <returns>The result of debug</returns>
        public bool DebugCom(string filename)
        {
            bool blsuccess = true;
            if (plate is ExcelPlate)
            {
                Excel.Application objExcel = new Excel.Application();
                if (objExcel == null)
                {
                    AddRow(dataGridViewDebug, "EXCEL could not be started. Check that your office installation and project references are correct"
                        , string.Empty, "error");
                    return false;
                }
                objExcel.Visible = false;
                objExcel.DisplayAlerts = false;
                object objMiss = System.Reflection.Missing.Value;
                Excel.Workbook objWorkBook = objExcel.Workbooks.Open(filename, objMiss, objMiss, objMiss
                    , objMiss, objMiss, objMiss, objMiss, objMiss, objMiss, objMiss, objMiss, objMiss, objMiss, objMiss);
                Excel.Sheets objWorkSheets = objWorkBook.Worksheets;
                Excel.Worksheet objWorkSheet = null;
                Excel.Range objRange = null;
                try
                {
                    for (int i = 1; i <= objWorkSheets.Count; i++)
                    {
                        objWorkSheet = (Excel.Worksheet)objWorkSheets[i];
                        int rowcount = objWorkSheet.UsedRange.Rows.Count;
                        int columncount = objWorkSheet.UsedRange.Columns.Count;
                        objRange = objWorkSheet.UsedRange;
                        if (objRange.Cells.Count == 1)
                        {
                            objRange = objRange.get_Resize(1, 2);
                        }
                        object[,] objValue = (object[,])objRange.get_Value(objMiss);
                        for (int j = 1; j <= rowcount; j++)
                        {
                            for (int k = 1; k <= columncount; k++)
                            {
                                if (objValue[j, k] is string)
                                {
                                    string strtext = objValue[j, k].ToString();
                                    if (strtext.StartsWith("$") && strtext.Length > 1)
                                    {
                                        object[] objret = DebugDetail(plate, strtext.Substring(1));
                                        if ((int)objret[0] == 1)
                                        {
                                            AddRow(dataGridViewDebug, objret[1].ToString(), strtext, objret[2].ToString());
                                            blsuccess = false;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("Debug encounter unexpected error:\n{0}{1}", e.Message, (e.InnerException != null ? "\n" + e.InnerException.Message : string.Empty))
                        , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    objExcel.Quit();
                    Marshal.ReleaseComObject(objRange);
                    Marshal.ReleaseComObject(objWorkSheet);
                    Marshal.ReleaseComObject(objWorkSheets);
                    Marshal.ReleaseComObject(objWorkBook);
                    Marshal.ReleaseComObject(objExcel);

                    objRange = null;
                    objWorkSheet = null;
                    objWorkSheets = null;
                    objWorkBook = null;
                    objExcel = null;
                    objMiss = null;

                    GC.Collect();
                }
            }
            else if (plate is WordPlate)
            {
                Word.Application objWord = new Word.Application();
                if (objWord == null)
                {
                    AddRow(dataGridViewDebug, "Word could not be started. Check that your office installation and project references are correct"
                        , string.Empty, "error");
                    return false;
                }
                objWord.Visible = false;
                objWord.DisplayAlerts = Word.WdAlertLevel.wdAlertsNone;
                object objMiss = System.Reflection.Missing.Value;
                object strfile = filename;
                Word.Document objDocument = objWord.Documents.Open(ref strfile, ref objMiss, ref objMiss, ref objMiss
                    , ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss
                    , ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss);
                try
                {
                    for (int i = 1; i <= objDocument.Sentences.Count; i++)
                    {
                        string[] arrtext = objDocument.Sentences[i].Text.Split(' ');
                        foreach (string str in arrtext)
                        {
                            if (str.StartsWith("$") && str.Length > 1)
                            {
                                object[] objret = DebugDetail(plate, str.Substring(1));
                                if ((int)objret[0] == 1)
                                {
                                    AddRow(dataGridViewDebug, objret[1].ToString(), str, objret[2].ToString());
                                    blsuccess = false;
                                }
                            }
                        }
                        SetProgress(toolStripProgressBar, ((i - 1) * 100 / objDocument.Sentences.Count));
                    }
                    for (int i = 1; i <= objDocument.Tables.Count; i++)
                    {
                        for (int j = 1; j <= objDocument.Tables[i].Rows.Count; j++)
                        {
                            for (int k = 1; k <= objDocument.Tables[i].Columns.Count; k++)
                            {
                                string strtext = objDocument.Tables[i].Cell(j, k).Range.Text.Trim();
                                if (strtext.StartsWith("$") && strtext.Length > 1)
                                {
                                    object[] objret = DebugDetail(plate, strtext.Substring(1));
                                    if ((int)objret[0] == 1)
                                    {
                                        AddRow(dataGridViewDebug, objret[1].ToString(), strtext, objret[2].ToString());
                                        blsuccess = false;
                                    }
                                }
                            }
                            SetProgress(toolStripProgressBar, ((j - 1) * 100 / objDocument.Tables[i].Rows.Count));
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("Debug encounter unexpected error:\n{0}{1}", e.Message, (e.InnerException != null ? "\n" + e.InnerException.Message : string.Empty))
                        , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    ((Word._Application)objWord).Quit(ref objMiss, ref objMiss, ref objMiss);
                    Marshal.ReleaseComObject(objDocument);
                    Marshal.ReleaseComObject(objWord);

                    objDocument = null;
                    objWord = null;
                    objMiss = null;

                    GC.Collect();
                }
            }
            return blsuccess;
        }

        /// <summary>
        /// The function used to analyze tag
        /// </summary>
        /// <param name="op">The plate to design</param>
        /// <param name="stag">The name of tag</param>
        /// <returns></returns>
        public object[] DebugDetail(OfficePlate op, string stag)
        {
            object[] objvalue =  Automation.AnalyzeTag(op, stag);
            switch ((Automation.TagType)objvalue[0])
            {
                case Automation.TagType.Constant:
                    {
                        foreach (TagItem ti in op.Tags)
                        {
                            if (ti.DataField == ((object[])objvalue[1])[0].ToString())
                            {
                                return new object[]{0};
                            }
                        }
                        return new object[]{1, "Constant Tag not defined in Plate","error"};
                    }
                case Automation.TagType.DataSource:
                    {
                        return new object[]{0};
                    }
                case Automation.TagType.Function:
                    {
                        string name = ((object[])objvalue[1])[0].ToString();
                        string param = ((object[])objvalue[1])[1].ToString();
                        foreach (TagItem ti in op.Tags)
                        {
                            if (ti.DataField.StartsWith(name) && ti.DataField.Length > name.Length)
                            {
                                string defparam = ti.DataField.Substring(name.Length).Trim();
                                if (defparam[0] == '(' && defparam[defparam.Length - 1] == ')')
                                {
                                    string[] arrparam = param.Split(',');
                                    string[] arrdefparm = defparam.Split(',');
                                    if (arrparam.Length == arrdefparm.Length)
                                    {
                                        return new object[] { 0 };
                                    }
                                }
                            }
                        }
                        return new object[] { 1, "Function Tag not defined in Plate, if you have defined it in the form, then ignore this message", "warning" };
                    }
                default:
                    {
                        string message = ((object[])objvalue[1])[0].ToString();
                        return new object[] {1, message, "error" };
                    }
            }
        }

        #region Delegate
        private delegate void AddRowMethod(DataGridView dgv, string description, string tag, string errortype);

        private delegate void EnableControlMethod(ToolStripItem ct, bool enable);

        private delegate void SetProgressMethod(ToolStripProgressBar bar, int value);

        private delegate void SetTextMethod(ToolStripItem ct, string text);

        public void AddRow(DataGridView dgv, string description, string tag, string errortype)
        {
            if (dgv.InvokeRequired)
            {
                AddRowMethod call = delegate(DataGridView dgvd, string descriptiond, string tagd, string errortyped)
                {
                    dgvd.Rows.Add();
                    DataGridViewRow dr = dgvd.Rows[dgvd.Rows.Count - 1];
                    dr.Cells["ColumnNo"].Value = dgvd.Rows.Count;
                    dr.Cells["ColumnImage"].Value = imageList.Images[errortyped];
                    dr.Cells["ColumnDescription"].Value = descriptiond;
                    dr.Cells["ColumnTag"].Value = tagd;
                };
                this.Invoke(call, new object[] { dgv, description, tag, errortype });
            }
            else
            {
                dgv.Rows.Add();
                DataGridViewRow dr = dgv.Rows[dgv.Rows.Count - 1];
                dr.Cells["ColumnNo"].Value = dgv.Rows.Count;
                dr.Cells["ColumnImage"].Value = imageList.Images[errortype];
                dr.Cells["ColumnDescription"].Value = description;
                dr.Cells["ColumnTag"].Value = tag;
            }
        }

        public void EnableControl(ToolStripItem ct, bool enable)
        {
            EnableControlMethod call = delegate(ToolStripItem ctd, bool enabled)
            {
                ctd.Enabled = enabled;
            };
            this.Invoke(call, new object[] { ct, enable });
        }

        public void SetProgress(ToolStripProgressBar bar, int value)
        {
            SetProgressMethod call = delegate(ToolStripProgressBar bard, int valued)
            {
                bard.Value = valued;
            };
            this.Invoke(call, new object[] { bar, value });
        }

        public void SetText(ToolStripItem ct, string text)
        {
            SetTextMethod call = delegate(ToolStripItem ctd, string textd)
            {
                ctd.Text = textd;
            };
            this.Invoke(call, new object[] { ct, text });
        }
        
        #endregion




    }
}