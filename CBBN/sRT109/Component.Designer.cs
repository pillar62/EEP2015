namespace sRT109
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
            Srvtools.KeyItem keyItem1 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem2 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
            Srvtools.FieldAttr fieldAttr1 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr2 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr3 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr4 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr5 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr6 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr7 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr8 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr9 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr10 = new Srvtools.FieldAttr();
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem5 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem6 = new Srvtools.KeyItem();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTBillCharge = new Srvtools.InfoCommand(this.components);
            this.ucRTBillCharge = new Srvtools.UpdateComponent(this.components);
            this.View_RTBillCharge = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTBillCharge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTBillCharge)).BeginInit();
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTBillCharge
            // 
            this.RTBillCharge.CacheConnection = false;
            this.RTBillCharge.CommandText = "SELECT dbo.[RTBillCharge].* FROM dbo.[RTBillCharge]";
            this.RTBillCharge.CommandTimeout = 30;
            this.RTBillCharge.CommandType = System.Data.CommandType.Text;
            this.RTBillCharge.DynamicTableName = false;
            this.RTBillCharge.EEPAlias = null;
            this.RTBillCharge.EncodingAfter = null;
            this.RTBillCharge.EncodingBefore = "Windows-1252";
            this.RTBillCharge.EncodingConvert = null;
            this.RTBillCharge.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "CASETYPE";
            keyItem2.KeyName = "CASEKIND";
            keyItem3.KeyName = "PAYCYCLE";
            this.RTBillCharge.KeyFields.Add(keyItem1);
            this.RTBillCharge.KeyFields.Add(keyItem2);
            this.RTBillCharge.KeyFields.Add(keyItem3);
            this.RTBillCharge.MultiSetWhere = false;
            this.RTBillCharge.Name = "RTBillCharge";
            this.RTBillCharge.NotificationAutoEnlist = false;
            this.RTBillCharge.SecExcept = null;
            this.RTBillCharge.SecFieldName = null;
            this.RTBillCharge.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTBillCharge.SelectPaging = false;
            this.RTBillCharge.SelectTop = 0;
            this.RTBillCharge.SiteControl = false;
            this.RTBillCharge.SiteFieldName = null;
            this.RTBillCharge.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTBillCharge
            // 
            this.ucRTBillCharge.AutoTrans = true;
            this.ucRTBillCharge.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "CASETYPE";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "CASEKIND";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "PAYCYCLE";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "PERIOD";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "AMT";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "AMT2";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "DROPAMT";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "DROPAMT2";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "MEMO";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "BILLCOD";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            this.ucRTBillCharge.FieldAttrs.Add(fieldAttr1);
            this.ucRTBillCharge.FieldAttrs.Add(fieldAttr2);
            this.ucRTBillCharge.FieldAttrs.Add(fieldAttr3);
            this.ucRTBillCharge.FieldAttrs.Add(fieldAttr4);
            this.ucRTBillCharge.FieldAttrs.Add(fieldAttr5);
            this.ucRTBillCharge.FieldAttrs.Add(fieldAttr6);
            this.ucRTBillCharge.FieldAttrs.Add(fieldAttr7);
            this.ucRTBillCharge.FieldAttrs.Add(fieldAttr8);
            this.ucRTBillCharge.FieldAttrs.Add(fieldAttr9);
            this.ucRTBillCharge.FieldAttrs.Add(fieldAttr10);
            this.ucRTBillCharge.LogInfo = null;
            this.ucRTBillCharge.Name = null;
            this.ucRTBillCharge.RowAffectsCheck = true;
            this.ucRTBillCharge.SelectCmd = this.RTBillCharge;
            this.ucRTBillCharge.SelectCmdForUpdate = null;
            this.ucRTBillCharge.SendSQLCmd = true;
            this.ucRTBillCharge.ServerModify = true;
            this.ucRTBillCharge.ServerModifyGetMax = false;
            this.ucRTBillCharge.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTBillCharge.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTBillCharge.UseTranscationScope = false;
            this.ucRTBillCharge.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_RTBillCharge
            // 
            this.View_RTBillCharge.CacheConnection = false;
            this.View_RTBillCharge.CommandText = "SELECT * FROM dbo.[RTBillCharge]";
            this.View_RTBillCharge.CommandTimeout = 30;
            this.View_RTBillCharge.CommandType = System.Data.CommandType.Text;
            this.View_RTBillCharge.DynamicTableName = false;
            this.View_RTBillCharge.EEPAlias = null;
            this.View_RTBillCharge.EncodingAfter = null;
            this.View_RTBillCharge.EncodingBefore = "Windows-1252";
            this.View_RTBillCharge.EncodingConvert = null;
            this.View_RTBillCharge.InfoConnection = this.InfoConnection1;
            keyItem4.KeyName = "CASETYPE";
            keyItem5.KeyName = "CASEKIND";
            keyItem6.KeyName = "PAYCYCLE";
            this.View_RTBillCharge.KeyFields.Add(keyItem4);
            this.View_RTBillCharge.KeyFields.Add(keyItem5);
            this.View_RTBillCharge.KeyFields.Add(keyItem6);
            this.View_RTBillCharge.MultiSetWhere = false;
            this.View_RTBillCharge.Name = "View_RTBillCharge";
            this.View_RTBillCharge.NotificationAutoEnlist = false;
            this.View_RTBillCharge.SecExcept = null;
            this.View_RTBillCharge.SecFieldName = null;
            this.View_RTBillCharge.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTBillCharge.SelectPaging = false;
            this.View_RTBillCharge.SelectTop = 0;
            this.View_RTBillCharge.SiteControl = false;
            this.View_RTBillCharge.SiteFieldName = null;
            this.View_RTBillCharge.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTBillCharge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTBillCharge)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTBillCharge;
        private Srvtools.UpdateComponent ucRTBillCharge;
        private Srvtools.InfoCommand View_RTBillCharge;
    }
}
