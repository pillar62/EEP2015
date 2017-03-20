namespace sRT401
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
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTInvTemp = new Srvtools.InfoCommand(this.components);
            this.ucRTInvTemp = new Srvtools.UpdateComponent(this.components);
            this.View_RTInvTemp = new Srvtools.InfoCommand(this.components);
            this.cmdRT401 = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTInvTemp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTInvTemp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT401)).BeginInit();
            // 
            // serviceManager1
            // 
            service1.DelegateName = "smRT401";
            service1.NonLogin = false;
            service1.ServiceName = "smRT401";
            this.serviceManager1.ServiceCollection.Add(service1);
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTInvTemp
            // 
            this.RTInvTemp.CacheConnection = false;
            this.RTInvTemp.CommandText = "SELECT dbo.[RTInvTemp].* FROM dbo.[RTInvTemp]";
            this.RTInvTemp.CommandTimeout = 30;
            this.RTInvTemp.CommandType = System.Data.CommandType.Text;
            this.RTInvTemp.DynamicTableName = false;
            this.RTInvTemp.EEPAlias = null;
            this.RTInvTemp.EncodingAfter = null;
            this.RTInvTemp.EncodingBefore = "Windows-1252";
            this.RTInvTemp.EncodingConvert = null;
            this.RTInvTemp.InfoConnection = this.InfoConnection1;
            this.RTInvTemp.MultiSetWhere = false;
            this.RTInvTemp.Name = "RTInvTemp";
            this.RTInvTemp.NotificationAutoEnlist = false;
            this.RTInvTemp.SecExcept = null;
            this.RTInvTemp.SecFieldName = null;
            this.RTInvTemp.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTInvTemp.SelectPaging = false;
            this.RTInvTemp.SelectTop = 0;
            this.RTInvTemp.SiteControl = false;
            this.RTInvTemp.SiteFieldName = null;
            this.RTInvTemp.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTInvTemp
            // 
            this.ucRTInvTemp.AutoTrans = true;
            this.ucRTInvTemp.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "GROUPNC";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "PRODNC";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "QTY";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "UNITAMT";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "RCVAMT";
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
            fieldAttr7.DataField = "SALEAMT";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "TAXAMT";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "INVDAT";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "INVNO";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "UNINO";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "INVTITLE";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "社區名稱";
            fieldAttr13.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr13.DefaultValue = null;
            fieldAttr13.TrimLength = 0;
            fieldAttr13.UpdateEnable = true;
            fieldAttr13.WhereMode = true;
            fieldAttr14.CharSetNull = false;
            fieldAttr14.CheckNull = false;
            fieldAttr14.DataField = "用戶名稱";
            fieldAttr14.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr14.DefaultValue = null;
            fieldAttr14.TrimLength = 0;
            fieldAttr14.UpdateEnable = true;
            fieldAttr14.WhereMode = true;
            fieldAttr15.CharSetNull = false;
            fieldAttr15.CheckNull = false;
            fieldAttr15.DataField = "地址";
            fieldAttr15.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr15.DefaultValue = null;
            fieldAttr15.TrimLength = 0;
            fieldAttr15.UpdateEnable = true;
            fieldAttr15.WhereMode = true;
            fieldAttr16.CharSetNull = false;
            fieldAttr16.CheckNull = false;
            fieldAttr16.DataField = "聯絡電話";
            fieldAttr16.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr16.DefaultValue = null;
            fieldAttr16.TrimLength = 0;
            fieldAttr16.UpdateEnable = true;
            fieldAttr16.WhereMode = true;
            fieldAttr17.CharSetNull = false;
            fieldAttr17.CheckNull = false;
            fieldAttr17.DataField = "業務施工收款日";
            fieldAttr17.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr17.DefaultValue = null;
            fieldAttr17.TrimLength = 0;
            fieldAttr17.UpdateEnable = true;
            fieldAttr17.WhereMode = true;
            fieldAttr18.CharSetNull = false;
            fieldAttr18.CheckNull = false;
            fieldAttr18.DataField = "施工人員";
            fieldAttr18.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr18.DefaultValue = null;
            fieldAttr18.TrimLength = 0;
            fieldAttr18.UpdateEnable = true;
            fieldAttr18.WhereMode = true;
            fieldAttr19.CharSetNull = false;
            fieldAttr19.CheckNull = false;
            fieldAttr19.DataField = "業務開發單位";
            fieldAttr19.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr19.DefaultValue = null;
            fieldAttr19.TrimLength = 0;
            fieldAttr19.UpdateEnable = true;
            fieldAttr19.WhereMode = true;
            fieldAttr20.CharSetNull = false;
            fieldAttr20.CheckNull = false;
            fieldAttr20.DataField = "業務開發人員";
            fieldAttr20.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr20.DefaultValue = null;
            fieldAttr20.TrimLength = 0;
            fieldAttr20.UpdateEnable = true;
            fieldAttr20.WhereMode = true;
            fieldAttr21.CharSetNull = false;
            fieldAttr21.CheckNull = false;
            fieldAttr21.DataField = "備註";
            fieldAttr21.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr21.DefaultValue = null;
            fieldAttr21.TrimLength = 0;
            fieldAttr21.UpdateEnable = true;
            fieldAttr21.WhereMode = true;
            fieldAttr22.CharSetNull = false;
            fieldAttr22.CheckNull = false;
            fieldAttr22.DataField = "BATCH";
            fieldAttr22.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr22.DefaultValue = null;
            fieldAttr22.TrimLength = 0;
            fieldAttr22.UpdateEnable = true;
            fieldAttr22.WhereMode = true;
            fieldAttr23.CharSetNull = false;
            fieldAttr23.CheckNull = false;
            fieldAttr23.DataField = "INVTYPE";
            fieldAttr23.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr23.DefaultValue = null;
            fieldAttr23.TrimLength = 0;
            fieldAttr23.UpdateEnable = true;
            fieldAttr23.WhereMode = true;
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr1);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr2);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr3);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr4);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr5);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr6);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr7);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr8);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr9);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr10);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr11);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr12);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr13);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr14);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr15);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr16);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr17);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr18);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr19);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr20);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr21);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr22);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr23);
            this.ucRTInvTemp.LogInfo = null;
            this.ucRTInvTemp.Name = "ucRTInvTemp";
            this.ucRTInvTemp.RowAffectsCheck = true;
            this.ucRTInvTemp.SelectCmd = this.RTInvTemp;
            this.ucRTInvTemp.SelectCmdForUpdate = null;
            this.ucRTInvTemp.SendSQLCmd = true;
            this.ucRTInvTemp.ServerModify = true;
            this.ucRTInvTemp.ServerModifyGetMax = false;
            this.ucRTInvTemp.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTInvTemp.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTInvTemp.UseTranscationScope = false;
            this.ucRTInvTemp.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_RTInvTemp
            // 
            this.View_RTInvTemp.CacheConnection = false;
            this.View_RTInvTemp.CommandText = "SELECT * FROM dbo.[RTInvTemp]";
            this.View_RTInvTemp.CommandTimeout = 30;
            this.View_RTInvTemp.CommandType = System.Data.CommandType.Text;
            this.View_RTInvTemp.DynamicTableName = false;
            this.View_RTInvTemp.EEPAlias = null;
            this.View_RTInvTemp.EncodingAfter = null;
            this.View_RTInvTemp.EncodingBefore = "Windows-1252";
            this.View_RTInvTemp.EncodingConvert = null;
            this.View_RTInvTemp.InfoConnection = this.InfoConnection1;
            this.View_RTInvTemp.MultiSetWhere = false;
            this.View_RTInvTemp.Name = "View_RTInvTemp";
            this.View_RTInvTemp.NotificationAutoEnlist = false;
            this.View_RTInvTemp.SecExcept = null;
            this.View_RTInvTemp.SecFieldName = null;
            this.View_RTInvTemp.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTInvTemp.SelectPaging = false;
            this.View_RTInvTemp.SelectTop = 0;
            this.View_RTInvTemp.SiteControl = false;
            this.View_RTInvTemp.SiteFieldName = null;
            this.View_RTInvTemp.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmdRT401
            // 
            this.cmdRT401.CacheConnection = false;
            this.cmdRT401.CommandText = null;
            this.cmdRT401.CommandTimeout = 30;
            this.cmdRT401.CommandType = System.Data.CommandType.Text;
            this.cmdRT401.DynamicTableName = false;
            this.cmdRT401.EEPAlias = null;
            this.cmdRT401.EncodingAfter = null;
            this.cmdRT401.EncodingBefore = "Windows-1252";
            this.cmdRT401.EncodingConvert = null;
            this.cmdRT401.InfoConnection = this.InfoConnection1;
            this.cmdRT401.MultiSetWhere = false;
            this.cmdRT401.Name = "cmdRT401";
            this.cmdRT401.NotificationAutoEnlist = false;
            this.cmdRT401.SecExcept = null;
            this.cmdRT401.SecFieldName = null;
            this.cmdRT401.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdRT401.SelectPaging = false;
            this.cmdRT401.SelectTop = 0;
            this.cmdRT401.SiteControl = false;
            this.cmdRT401.SiteFieldName = null;
            this.cmdRT401.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTInvTemp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTInvTemp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT401)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTInvTemp;
        private Srvtools.UpdateComponent ucRTInvTemp;
        private Srvtools.InfoCommand View_RTInvTemp;
        private Srvtools.InfoCommand cmdRT401;
    }
}
