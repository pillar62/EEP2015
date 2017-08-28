namespace sTest
{
    partial class Component
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Srvtools.Service service1 = new Srvtools.Service();
            Srvtools.KeyItem keyItem1 = new Srvtools.KeyItem();
            Srvtools.FieldAttr fieldAttr1 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr2 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr3 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr4 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr5 = new Srvtools.FieldAttr();
            Srvtools.KeyItem keyItem2 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
            Srvtools.FieldAttr fieldAttr6 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr7 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr8 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr9 = new Srvtools.FieldAttr();
            Srvtools.ColumnItem columnItem1 = new Srvtools.ColumnItem();
            Srvtools.ColumnItem columnItem2 = new Srvtools.ColumnItem();
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.TestMaster = new Srvtools.InfoCommand(this.components);
            this.ucTestMaster = new Srvtools.UpdateComponent(this.components);
            this.TestDetail = new Srvtools.InfoCommand(this.components);
            this.ucTestDetail = new Srvtools.UpdateComponent(this.components);
            this.idTestMaster_TestDetail = new Srvtools.InfoDataSource(this.components);
            this.View_TestMaster = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TestMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TestDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_TestMaster)).BeginInit();
            // 
            // serviceManager1
            // 
            service1.DelegateName = "ImportData";
            service1.NonLogin = false;
            service1.ServiceName = "ImportData";
            this.serviceManager1.ServiceCollection.Add(service1);
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "ERPS";
            // 
            // TestMaster
            // 
            this.TestMaster.CacheConnection = false;
            this.TestMaster.CommandText = "SELECT dbo.[TestMaster].* FROM dbo.[TestMaster]";
            this.TestMaster.CommandTimeout = 30;
            this.TestMaster.CommandType = System.Data.CommandType.Text;
            this.TestMaster.DynamicTableName = false;
            this.TestMaster.EEPAlias = null;
            this.TestMaster.EncodingAfter = null;
            this.TestMaster.EncodingBefore = "Windows-1252";
            this.TestMaster.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "TestID";
            this.TestMaster.KeyFields.Add(keyItem1);
            this.TestMaster.MultiSetWhere = false;
            this.TestMaster.Name = "TestMaster";
            this.TestMaster.NotificationAutoEnlist = false;
            this.TestMaster.SecExcept = null;
            this.TestMaster.SecFieldName = null;
            this.TestMaster.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.TestMaster.SelectPaging = false;
            this.TestMaster.SelectTop = 0;
            this.TestMaster.SiteControl = false;
            this.TestMaster.SiteFieldName = null;
            this.TestMaster.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucTestMaster
            // 
            this.ucTestMaster.AutoTrans = true;
            this.ucTestMaster.ExceptJoin = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "TestID";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "TestName";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "TestMark";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "FlowFlag";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "CustomerID";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            this.ucTestMaster.FieldAttrs.Add(fieldAttr1);
            this.ucTestMaster.FieldAttrs.Add(fieldAttr2);
            this.ucTestMaster.FieldAttrs.Add(fieldAttr3);
            this.ucTestMaster.FieldAttrs.Add(fieldAttr4);
            this.ucTestMaster.FieldAttrs.Add(fieldAttr5);
            this.ucTestMaster.LogInfo = null;
            this.ucTestMaster.Name = "ucTestMaster";
            this.ucTestMaster.RowAffectsCheck = true;
            this.ucTestMaster.SelectCmd = this.TestMaster;
            this.ucTestMaster.SelectCmdForUpdate = null;
            this.ucTestMaster.ServerModify = true;
            this.ucTestMaster.ServerModifyGetMax = false;
            this.ucTestMaster.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucTestMaster.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucTestMaster.UseTranscationScope = false;
            this.ucTestMaster.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // TestDetail
            // 
            this.TestDetail.CacheConnection = false;
            this.TestDetail.CommandText = "SELECT dbo.[TestDetail].* FROM dbo.[TestDetail]";
            this.TestDetail.CommandTimeout = 30;
            this.TestDetail.CommandType = System.Data.CommandType.Text;
            this.TestDetail.DynamicTableName = false;
            this.TestDetail.EEPAlias = null;
            this.TestDetail.EncodingAfter = null;
            this.TestDetail.EncodingBefore = "Windows-1252";
            this.TestDetail.InfoConnection = this.InfoConnection1;
            keyItem2.KeyName = "TestID";
            keyItem3.KeyName = "TestSeq";
            this.TestDetail.KeyFields.Add(keyItem2);
            this.TestDetail.KeyFields.Add(keyItem3);
            this.TestDetail.MultiSetWhere = false;
            this.TestDetail.Name = "TestDetail";
            this.TestDetail.NotificationAutoEnlist = false;
            this.TestDetail.SecExcept = null;
            this.TestDetail.SecFieldName = null;
            this.TestDetail.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.TestDetail.SelectPaging = false;
            this.TestDetail.SelectTop = 0;
            this.TestDetail.SiteControl = false;
            this.TestDetail.SiteFieldName = null;
            this.TestDetail.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucTestDetail
            // 
            this.ucTestDetail.AutoTrans = true;
            this.ucTestDetail.ExceptJoin = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "TestID";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "TestSeq";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "TestTry";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "CustomerID";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            this.ucTestDetail.FieldAttrs.Add(fieldAttr6);
            this.ucTestDetail.FieldAttrs.Add(fieldAttr7);
            this.ucTestDetail.FieldAttrs.Add(fieldAttr8);
            this.ucTestDetail.FieldAttrs.Add(fieldAttr9);
            this.ucTestDetail.LogInfo = null;
            this.ucTestDetail.Name = "ucTestDetail";
            this.ucTestDetail.RowAffectsCheck = true;
            this.ucTestDetail.SelectCmd = this.TestDetail;
            this.ucTestDetail.SelectCmdForUpdate = null;
            this.ucTestDetail.ServerModify = true;
            this.ucTestDetail.ServerModifyGetMax = false;
            this.ucTestDetail.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucTestDetail.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucTestDetail.UseTranscationScope = false;
            this.ucTestDetail.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // idTestMaster_TestDetail
            // 
            this.idTestMaster_TestDetail.Detail = this.TestDetail;
            columnItem1.FieldName = "TestID";
            this.idTestMaster_TestDetail.DetailColumns.Add(columnItem1);
            this.idTestMaster_TestDetail.DynamicTableName = false;
            this.idTestMaster_TestDetail.Master = this.TestMaster;
            columnItem2.FieldName = "TestID";
            this.idTestMaster_TestDetail.MasterColumns.Add(columnItem2);
            // 
            // View_TestMaster
            // 
            this.View_TestMaster.CacheConnection = false;
            this.View_TestMaster.CommandText = "SELECT * FROM dbo.[TestMaster]";
            this.View_TestMaster.CommandTimeout = 30;
            this.View_TestMaster.CommandType = System.Data.CommandType.Text;
            this.View_TestMaster.DynamicTableName = false;
            this.View_TestMaster.EEPAlias = null;
            this.View_TestMaster.EncodingAfter = null;
            this.View_TestMaster.EncodingBefore = "Windows-1252";
            this.View_TestMaster.InfoConnection = this.InfoConnection1;
            keyItem4.KeyName = "TestID";
            this.View_TestMaster.KeyFields.Add(keyItem4);
            this.View_TestMaster.MultiSetWhere = false;
            this.View_TestMaster.Name = "View_TestMaster";
            this.View_TestMaster.NotificationAutoEnlist = false;
            this.View_TestMaster.SecExcept = null;
            this.View_TestMaster.SecFieldName = null;
            this.View_TestMaster.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_TestMaster.SelectPaging = false;
            this.View_TestMaster.SelectTop = 0;
            this.View_TestMaster.SiteControl = false;
            this.View_TestMaster.SiteFieldName = null;
            this.View_TestMaster.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TestMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TestDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_TestMaster)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand TestMaster;
        private Srvtools.UpdateComponent ucTestMaster;
        private Srvtools.InfoCommand TestDetail;
        private Srvtools.UpdateComponent ucTestDetail;
        private Srvtools.InfoDataSource idTestMaster_TestDetail;
        private Srvtools.InfoCommand View_TestMaster;
    }
}
