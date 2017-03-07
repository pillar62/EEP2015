namespace RT1041
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
            Srvtools.FieldAttr fieldAttr11 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr12 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr13 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr14 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr15 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr16 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr17 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr18 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr19 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr20 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr21 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr22 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr23 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr24 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr25 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr26 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr27 = new Srvtools.FieldAttr();
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTLessorAVSCustRepair = new Srvtools.InfoCommand(this.components);
            this.ucRTLessorAVSCustRepair = new Srvtools.UpdateComponent(this.components);
            this.View_RTLessorAVSCustRepair = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustRepair)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCustRepair)).BeginInit();
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTLessorAVSCustRepair
            // 
            this.RTLessorAVSCustRepair.CacheConnection = false;
            this.RTLessorAVSCustRepair.CommandText = "SELECT dbo.[RTLessorAVSCustRepair].* FROM dbo.[RTLessorAVSCustRepair]";
            this.RTLessorAVSCustRepair.CommandTimeout = 30;
            this.RTLessorAVSCustRepair.CommandType = System.Data.CommandType.Text;
            this.RTLessorAVSCustRepair.DynamicTableName = false;
            this.RTLessorAVSCustRepair.EEPAlias = null;
            this.RTLessorAVSCustRepair.EncodingAfter = null;
            this.RTLessorAVSCustRepair.EncodingBefore = "Windows-1252";
            this.RTLessorAVSCustRepair.EncodingConvert = null;
            this.RTLessorAVSCustRepair.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "CUSID";
            keyItem2.KeyName = "ENTRYNO";
            this.RTLessorAVSCustRepair.KeyFields.Add(keyItem1);
            this.RTLessorAVSCustRepair.KeyFields.Add(keyItem2);
            this.RTLessorAVSCustRepair.MultiSetWhere = false;
            this.RTLessorAVSCustRepair.Name = "RTLessorAVSCustRepair";
            this.RTLessorAVSCustRepair.NotificationAutoEnlist = false;
            this.RTLessorAVSCustRepair.SecExcept = null;
            this.RTLessorAVSCustRepair.SecFieldName = null;
            this.RTLessorAVSCustRepair.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTLessorAVSCustRepair.SelectPaging = false;
            this.RTLessorAVSCustRepair.SelectTop = 0;
            this.RTLessorAVSCustRepair.SiteControl = false;
            this.RTLessorAVSCustRepair.SiteFieldName = null;
            this.RTLessorAVSCustRepair.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTLessorAVSCustRepair
            // 
            this.ucRTLessorAVSCustRepair.AutoTrans = true;
            this.ucRTLessorAVSCustRepair.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "CUSID";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "ENTRYNO";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "APPLYDAT";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "EQUIP";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "EQUIPAMT";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "SETAMT";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "MOVEAMT";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "PAYTYPE";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "CREDITCARDTYPE";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "CREDITBANK";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "CREDITCARDNO";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "CREDITNAME";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "CREDITDUEM";
            fieldAttr13.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr13.DefaultValue = null;
            fieldAttr13.TrimLength = 0;
            fieldAttr13.UpdateEnable = true;
            fieldAttr13.WhereMode = true;
            fieldAttr14.CharSetNull = false;
            fieldAttr14.CheckNull = false;
            fieldAttr14.DataField = "CREDITDUEY";
            fieldAttr14.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr14.DefaultValue = null;
            fieldAttr14.TrimLength = 0;
            fieldAttr14.UpdateEnable = true;
            fieldAttr14.WhereMode = true;
            fieldAttr15.CharSetNull = false;
            fieldAttr15.CheckNull = false;
            fieldAttr15.DataField = "MAXENTRYNO";
            fieldAttr15.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr15.DefaultValue = null;
            fieldAttr15.TrimLength = 0;
            fieldAttr15.UpdateEnable = true;
            fieldAttr15.WhereMode = true;
            fieldAttr16.CharSetNull = false;
            fieldAttr16.CheckNull = false;
            fieldAttr16.DataField = "TARDAT";
            fieldAttr16.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr16.DefaultValue = null;
            fieldAttr16.TrimLength = 0;
            fieldAttr16.UpdateEnable = true;
            fieldAttr16.WhereMode = true;
            fieldAttr17.CharSetNull = false;
            fieldAttr17.CheckNull = false;
            fieldAttr17.DataField = "BATCHNO";
            fieldAttr17.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr17.DefaultValue = null;
            fieldAttr17.TrimLength = 0;
            fieldAttr17.UpdateEnable = true;
            fieldAttr17.WhereMode = true;
            fieldAttr18.CharSetNull = false;
            fieldAttr18.CheckNull = false;
            fieldAttr18.DataField = "TUSR";
            fieldAttr18.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr18.DefaultValue = null;
            fieldAttr18.TrimLength = 0;
            fieldAttr18.UpdateEnable = true;
            fieldAttr18.WhereMode = true;
            fieldAttr19.CharSetNull = false;
            fieldAttr19.CheckNull = false;
            fieldAttr19.DataField = "FINISHDAT";
            fieldAttr19.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr19.DefaultValue = null;
            fieldAttr19.TrimLength = 0;
            fieldAttr19.UpdateEnable = true;
            fieldAttr19.WhereMode = true;
            fieldAttr20.CharSetNull = false;
            fieldAttr20.CheckNull = false;
            fieldAttr20.DataField = "CANCELDAT";
            fieldAttr20.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr20.DefaultValue = null;
            fieldAttr20.TrimLength = 0;
            fieldAttr20.UpdateEnable = true;
            fieldAttr20.WhereMode = true;
            fieldAttr21.CharSetNull = false;
            fieldAttr21.CheckNull = false;
            fieldAttr21.DataField = "CANCELUSR";
            fieldAttr21.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr21.DefaultValue = null;
            fieldAttr21.TrimLength = 0;
            fieldAttr21.UpdateEnable = true;
            fieldAttr21.WhereMode = true;
            fieldAttr22.CharSetNull = false;
            fieldAttr22.CheckNull = false;
            fieldAttr22.DataField = "MEMO";
            fieldAttr22.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr22.DefaultValue = null;
            fieldAttr22.TrimLength = 0;
            fieldAttr22.UpdateEnable = true;
            fieldAttr22.WhereMode = true;
            fieldAttr23.CharSetNull = false;
            fieldAttr23.CheckNull = false;
            fieldAttr23.DataField = "REALENGINEER";
            fieldAttr23.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr23.DefaultValue = null;
            fieldAttr23.TrimLength = 0;
            fieldAttr23.UpdateEnable = true;
            fieldAttr23.WhereMode = true;
            fieldAttr24.CharSetNull = false;
            fieldAttr24.CheckNull = false;
            fieldAttr24.DataField = "REALCONSIGNEE";
            fieldAttr24.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr24.DefaultValue = null;
            fieldAttr24.TrimLength = 0;
            fieldAttr24.UpdateEnable = true;
            fieldAttr24.WhereMode = true;
            fieldAttr25.CharSetNull = false;
            fieldAttr25.CheckNull = false;
            fieldAttr25.DataField = "UDAT";
            fieldAttr25.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr25.DefaultValue = null;
            fieldAttr25.TrimLength = 0;
            fieldAttr25.UpdateEnable = true;
            fieldAttr25.WhereMode = true;
            fieldAttr26.CharSetNull = false;
            fieldAttr26.CheckNull = false;
            fieldAttr26.DataField = "UUSR";
            fieldAttr26.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr26.DefaultValue = null;
            fieldAttr26.TrimLength = 0;
            fieldAttr26.UpdateEnable = true;
            fieldAttr26.WhereMode = true;
            fieldAttr27.CharSetNull = false;
            fieldAttr27.CheckNull = false;
            fieldAttr27.DataField = "RCVMONEYDAT";
            fieldAttr27.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr27.DefaultValue = null;
            fieldAttr27.TrimLength = 0;
            fieldAttr27.UpdateEnable = true;
            fieldAttr27.WhereMode = true;
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr1);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr2);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr3);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr4);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr5);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr6);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr7);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr8);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr9);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr10);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr11);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr12);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr13);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr14);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr15);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr16);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr17);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr18);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr19);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr20);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr21);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr22);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr23);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr24);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr25);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr26);
            this.ucRTLessorAVSCustRepair.FieldAttrs.Add(fieldAttr27);
            this.ucRTLessorAVSCustRepair.LogInfo = null;
            this.ucRTLessorAVSCustRepair.Name = null;
            this.ucRTLessorAVSCustRepair.RowAffectsCheck = true;
            this.ucRTLessorAVSCustRepair.SelectCmd = this.RTLessorAVSCustRepair;
            this.ucRTLessorAVSCustRepair.SelectCmdForUpdate = null;
            this.ucRTLessorAVSCustRepair.SendSQLCmd = true;
            this.ucRTLessorAVSCustRepair.ServerModify = true;
            this.ucRTLessorAVSCustRepair.ServerModifyGetMax = false;
            this.ucRTLessorAVSCustRepair.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTLessorAVSCustRepair.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTLessorAVSCustRepair.UseTranscationScope = false;
            this.ucRTLessorAVSCustRepair.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_RTLessorAVSCustRepair
            // 
            this.View_RTLessorAVSCustRepair.CacheConnection = false;
            this.View_RTLessorAVSCustRepair.CommandText = "SELECT * FROM dbo.[RTLessorAVSCustRepair]";
            this.View_RTLessorAVSCustRepair.CommandTimeout = 30;
            this.View_RTLessorAVSCustRepair.CommandType = System.Data.CommandType.Text;
            this.View_RTLessorAVSCustRepair.DynamicTableName = false;
            this.View_RTLessorAVSCustRepair.EEPAlias = null;
            this.View_RTLessorAVSCustRepair.EncodingAfter = null;
            this.View_RTLessorAVSCustRepair.EncodingBefore = "Windows-1252";
            this.View_RTLessorAVSCustRepair.EncodingConvert = null;
            this.View_RTLessorAVSCustRepair.InfoConnection = this.InfoConnection1;
            keyItem3.KeyName = "CUSID";
            keyItem4.KeyName = "ENTRYNO";
            this.View_RTLessorAVSCustRepair.KeyFields.Add(keyItem3);
            this.View_RTLessorAVSCustRepair.KeyFields.Add(keyItem4);
            this.View_RTLessorAVSCustRepair.MultiSetWhere = false;
            this.View_RTLessorAVSCustRepair.Name = "View_RTLessorAVSCustRepair";
            this.View_RTLessorAVSCustRepair.NotificationAutoEnlist = false;
            this.View_RTLessorAVSCustRepair.SecExcept = null;
            this.View_RTLessorAVSCustRepair.SecFieldName = null;
            this.View_RTLessorAVSCustRepair.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTLessorAVSCustRepair.SelectPaging = false;
            this.View_RTLessorAVSCustRepair.SelectTop = 0;
            this.View_RTLessorAVSCustRepair.SiteControl = false;
            this.View_RTLessorAVSCustRepair.SiteFieldName = null;
            this.View_RTLessorAVSCustRepair.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustRepair)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCustRepair)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTLessorAVSCustRepair;
        private Srvtools.UpdateComponent ucRTLessorAVSCustRepair;
        private Srvtools.InfoCommand View_RTLessorAVSCustRepair;
    }
}
