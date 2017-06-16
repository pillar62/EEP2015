namespace sRT306
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
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Component));
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTInvoice = new Srvtools.InfoCommand(this.components);
            this.ucRTInvoice = new Srvtools.UpdateComponent(this.components);
            this.View_RTInvoice = new Srvtools.InfoCommand(this.components);
            this.cmdRT306 = new Srvtools.InfoCommand(this.components);
            this.cmdRT3061 = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTInvoice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTInvoice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT306)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT3061)).BeginInit();
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTInvoice
            // 
            this.RTInvoice.CacheConnection = false;
            this.RTInvoice.CommandText = "SELECT dbo.[RTInvoice].* FROM dbo.[RTInvoice]";
            this.RTInvoice.CommandTimeout = 30;
            this.RTInvoice.CommandType = System.Data.CommandType.Text;
            this.RTInvoice.DynamicTableName = false;
            this.RTInvoice.EEPAlias = null;
            this.RTInvoice.EncodingAfter = null;
            this.RTInvoice.EncodingBefore = "Windows-1252";
            this.RTInvoice.EncodingConvert = null;
            this.RTInvoice.InfoConnection = this.InfoConnection1;
            keyItem2.KeyName = "INVNO";
            this.RTInvoice.KeyFields.Add(keyItem2);
            this.RTInvoice.MultiSetWhere = false;
            this.RTInvoice.Name = "RTInvoice";
            this.RTInvoice.NotificationAutoEnlist = false;
            this.RTInvoice.SecExcept = null;
            this.RTInvoice.SecFieldName = null;
            this.RTInvoice.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTInvoice.SelectPaging = false;
            this.RTInvoice.SelectTop = 0;
            this.RTInvoice.SiteControl = false;
            this.RTInvoice.SiteFieldName = null;
            this.RTInvoice.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTInvoice
            // 
            this.ucRTInvoice.AutoTrans = true;
            this.ucRTInvoice.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "INVNO";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "INVDAT";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "INVTITLE";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "UNINO";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "CHKNO";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "TAXTYPE";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "SALESUM";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "TAXSUM";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "TOTALSUM";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "INVTYPE";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "AMTC";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "INVDATC";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "CANCELDAT";
            fieldAttr13.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr13.DefaultValue = null;
            fieldAttr13.TrimLength = 0;
            fieldAttr13.UpdateEnable = true;
            fieldAttr13.WhereMode = true;
            fieldAttr14.CharSetNull = false;
            fieldAttr14.CheckNull = false;
            fieldAttr14.DataField = "MEMO";
            fieldAttr14.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr14.DefaultValue = null;
            fieldAttr14.TrimLength = 0;
            fieldAttr14.UpdateEnable = true;
            fieldAttr14.WhereMode = true;
            fieldAttr15.CharSetNull = false;
            fieldAttr15.CheckNull = false;
            fieldAttr15.DataField = "UUSR";
            fieldAttr15.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr15.DefaultValue = null;
            fieldAttr15.TrimLength = 0;
            fieldAttr15.UpdateEnable = true;
            fieldAttr15.WhereMode = true;
            fieldAttr16.CharSetNull = false;
            fieldAttr16.CheckNull = false;
            fieldAttr16.DataField = "UDAT";
            fieldAttr16.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr16.DefaultValue = null;
            fieldAttr16.TrimLength = 0;
            fieldAttr16.UpdateEnable = true;
            fieldAttr16.WhereMode = true;
            fieldAttr17.CharSetNull = false;
            fieldAttr17.CheckNull = false;
            fieldAttr17.DataField = "BATCH";
            fieldAttr17.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr17.DefaultValue = null;
            fieldAttr17.TrimLength = 0;
            fieldAttr17.UpdateEnable = true;
            fieldAttr17.WhereMode = true;
            fieldAttr18.CharSetNull = false;
            fieldAttr18.CheckNull = false;
            fieldAttr18.DataField = "CASETYPE";
            fieldAttr18.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr18.DefaultValue = null;
            fieldAttr18.TrimLength = 0;
            fieldAttr18.UpdateEnable = true;
            fieldAttr18.WhereMode = true;
            fieldAttr19.CharSetNull = false;
            fieldAttr19.CheckNull = false;
            fieldAttr19.DataField = "ARSRC";
            fieldAttr19.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr19.DefaultValue = null;
            fieldAttr19.TrimLength = 0;
            fieldAttr19.UpdateEnable = true;
            fieldAttr19.WhereMode = true;
            fieldAttr20.CharSetNull = false;
            fieldAttr20.CheckNull = false;
            fieldAttr20.DataField = "BATCHNO";
            fieldAttr20.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr20.DefaultValue = null;
            fieldAttr20.TrimLength = 0;
            fieldAttr20.UpdateEnable = true;
            fieldAttr20.WhereMode = true;
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr1);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr2);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr3);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr4);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr5);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr6);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr7);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr8);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr9);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr10);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr11);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr12);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr13);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr14);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr15);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr16);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr17);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr18);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr19);
            this.ucRTInvoice.FieldAttrs.Add(fieldAttr20);
            this.ucRTInvoice.LogInfo = null;
            this.ucRTInvoice.Name = "ucRTInvoice";
            this.ucRTInvoice.RowAffectsCheck = true;
            this.ucRTInvoice.SelectCmd = this.RTInvoice;
            this.ucRTInvoice.SelectCmdForUpdate = null;
            this.ucRTInvoice.SendSQLCmd = true;
            this.ucRTInvoice.ServerModify = true;
            this.ucRTInvoice.ServerModifyGetMax = false;
            this.ucRTInvoice.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTInvoice.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTInvoice.UseTranscationScope = false;
            this.ucRTInvoice.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_RTInvoice
            // 
            this.View_RTInvoice.CacheConnection = false;
            this.View_RTInvoice.CommandText = "SELECT * FROM dbo.[RTInvoice]";
            this.View_RTInvoice.CommandTimeout = 30;
            this.View_RTInvoice.CommandType = System.Data.CommandType.Text;
            this.View_RTInvoice.DynamicTableName = false;
            this.View_RTInvoice.EEPAlias = null;
            this.View_RTInvoice.EncodingAfter = null;
            this.View_RTInvoice.EncodingBefore = "Windows-1252";
            this.View_RTInvoice.EncodingConvert = null;
            this.View_RTInvoice.InfoConnection = this.InfoConnection1;
            keyItem3.KeyName = "INVNO";
            this.View_RTInvoice.KeyFields.Add(keyItem3);
            this.View_RTInvoice.MultiSetWhere = false;
            this.View_RTInvoice.Name = "View_RTInvoice";
            this.View_RTInvoice.NotificationAutoEnlist = false;
            this.View_RTInvoice.SecExcept = null;
            this.View_RTInvoice.SecFieldName = null;
            this.View_RTInvoice.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTInvoice.SelectPaging = false;
            this.View_RTInvoice.SelectTop = 0;
            this.View_RTInvoice.SiteControl = false;
            this.View_RTInvoice.SiteFieldName = null;
            this.View_RTInvoice.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmdRT306
            // 
            this.cmdRT306.CacheConnection = false;
            this.cmdRT306.CommandText = "SELECT Max(BATCH) as maxbatch, CAST(\'1900/01/01\' AS datetime)  as dt_s, GETDATE()" +
    " as dt_e \r\nFROM RTInvoice";
            this.cmdRT306.CommandTimeout = 30;
            this.cmdRT306.CommandType = System.Data.CommandType.Text;
            this.cmdRT306.DynamicTableName = false;
            this.cmdRT306.EEPAlias = null;
            this.cmdRT306.EncodingAfter = null;
            this.cmdRT306.EncodingBefore = "Windows-1252";
            this.cmdRT306.EncodingConvert = null;
            this.cmdRT306.InfoConnection = this.InfoConnection1;
            this.cmdRT306.MultiSetWhere = false;
            this.cmdRT306.Name = "cmdRT306";
            this.cmdRT306.NotificationAutoEnlist = false;
            this.cmdRT306.SecExcept = null;
            this.cmdRT306.SecFieldName = null;
            this.cmdRT306.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdRT306.SelectPaging = false;
            this.cmdRT306.SelectTop = 0;
            this.cmdRT306.SiteControl = false;
            this.cmdRT306.SiteFieldName = null;
            this.cmdRT306.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmdRT3061
            // 
            this.cmdRT3061.CacheConnection = false;
            this.cmdRT3061.CommandText = resources.GetString("cmdRT3061.CommandText");
            this.cmdRT3061.CommandTimeout = 30;
            this.cmdRT3061.CommandType = System.Data.CommandType.Text;
            this.cmdRT3061.DynamicTableName = false;
            this.cmdRT3061.EEPAlias = null;
            this.cmdRT3061.EncodingAfter = null;
            this.cmdRT3061.EncodingBefore = "Windows-1252";
            this.cmdRT3061.EncodingConvert = null;
            this.cmdRT3061.InfoConnection = this.InfoConnection1;
            this.cmdRT3061.MultiSetWhere = false;
            this.cmdRT3061.Name = "cmdRT3061";
            this.cmdRT3061.NotificationAutoEnlist = false;
            this.cmdRT3061.SecExcept = null;
            this.cmdRT3061.SecFieldName = null;
            this.cmdRT3061.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdRT3061.SelectPaging = false;
            this.cmdRT3061.SelectTop = 0;
            this.cmdRT3061.SiteControl = false;
            this.cmdRT3061.SiteFieldName = null;
            this.cmdRT3061.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTInvoice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTInvoice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT306)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT3061)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTInvoice;
        private Srvtools.UpdateComponent ucRTInvoice;
        private Srvtools.InfoCommand View_RTInvoice;
        private Srvtools.InfoCommand cmdRT306;
        private Srvtools.InfoCommand cmdRT3061;
    }
}
