namespace sRT207
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
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTInvMonth = new Srvtools.InfoCommand(this.components);
            this.ucRTInvMonth = new Srvtools.UpdateComponent(this.components);
            this.View_RTInvMonth = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTInvMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTInvMonth)).BeginInit();
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTInvMonth
            // 
            this.RTInvMonth.CacheConnection = false;
            this.RTInvMonth.CommandText = "SELECT dbo.[RTInvMonth].* FROM dbo.[RTInvMonth]";
            this.RTInvMonth.CommandTimeout = 30;
            this.RTInvMonth.CommandType = System.Data.CommandType.Text;
            this.RTInvMonth.DynamicTableName = false;
            this.RTInvMonth.EEPAlias = null;
            this.RTInvMonth.EncodingAfter = null;
            this.RTInvMonth.EncodingBefore = "Windows-1252";
            this.RTInvMonth.EncodingConvert = null;
            this.RTInvMonth.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "INVYEAR";
            keyItem2.KeyName = "INVMONTH";
            this.RTInvMonth.KeyFields.Add(keyItem1);
            this.RTInvMonth.KeyFields.Add(keyItem2);
            this.RTInvMonth.MultiSetWhere = false;
            this.RTInvMonth.Name = "RTInvMonth";
            this.RTInvMonth.NotificationAutoEnlist = false;
            this.RTInvMonth.SecExcept = null;
            this.RTInvMonth.SecFieldName = null;
            this.RTInvMonth.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTInvMonth.SelectPaging = false;
            this.RTInvMonth.SelectTop = 0;
            this.RTInvMonth.SiteControl = false;
            this.RTInvMonth.SiteFieldName = null;
            this.RTInvMonth.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTInvMonth
            // 
            this.ucRTInvMonth.AutoTrans = true;
            this.ucRTInvMonth.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "INVYEAR";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "INVMONTH";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "INVTRACK";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "INVNOS";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "INVNOE";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "INVNOS3";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "INVNOE3";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            this.ucRTInvMonth.FieldAttrs.Add(fieldAttr1);
            this.ucRTInvMonth.FieldAttrs.Add(fieldAttr2);
            this.ucRTInvMonth.FieldAttrs.Add(fieldAttr3);
            this.ucRTInvMonth.FieldAttrs.Add(fieldAttr4);
            this.ucRTInvMonth.FieldAttrs.Add(fieldAttr5);
            this.ucRTInvMonth.FieldAttrs.Add(fieldAttr6);
            this.ucRTInvMonth.FieldAttrs.Add(fieldAttr7);
            this.ucRTInvMonth.LogInfo = null;
            this.ucRTInvMonth.Name = null;
            this.ucRTInvMonth.RowAffectsCheck = true;
            this.ucRTInvMonth.SelectCmd = this.RTInvMonth;
            this.ucRTInvMonth.SelectCmdForUpdate = null;
            this.ucRTInvMonth.SendSQLCmd = true;
            this.ucRTInvMonth.ServerModify = true;
            this.ucRTInvMonth.ServerModifyGetMax = false;
            this.ucRTInvMonth.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTInvMonth.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTInvMonth.UseTranscationScope = false;
            this.ucRTInvMonth.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_RTInvMonth
            // 
            this.View_RTInvMonth.CacheConnection = false;
            this.View_RTInvMonth.CommandText = "SELECT * FROM dbo.[RTInvMonth]";
            this.View_RTInvMonth.CommandTimeout = 30;
            this.View_RTInvMonth.CommandType = System.Data.CommandType.Text;
            this.View_RTInvMonth.DynamicTableName = false;
            this.View_RTInvMonth.EEPAlias = null;
            this.View_RTInvMonth.EncodingAfter = null;
            this.View_RTInvMonth.EncodingBefore = "Windows-1252";
            this.View_RTInvMonth.EncodingConvert = null;
            this.View_RTInvMonth.InfoConnection = this.InfoConnection1;
            keyItem3.KeyName = "INVYEAR";
            keyItem4.KeyName = "INVMONTH";
            this.View_RTInvMonth.KeyFields.Add(keyItem3);
            this.View_RTInvMonth.KeyFields.Add(keyItem4);
            this.View_RTInvMonth.MultiSetWhere = false;
            this.View_RTInvMonth.Name = "View_RTInvMonth";
            this.View_RTInvMonth.NotificationAutoEnlist = false;
            this.View_RTInvMonth.SecExcept = null;
            this.View_RTInvMonth.SecFieldName = null;
            this.View_RTInvMonth.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTInvMonth.SelectPaging = false;
            this.View_RTInvMonth.SelectTop = 0;
            this.View_RTInvMonth.SiteControl = false;
            this.View_RTInvMonth.SiteFieldName = null;
            this.View_RTInvMonth.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTInvMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTInvMonth)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTInvMonth;
        private Srvtools.UpdateComponent ucRTInvMonth;
        private Srvtools.InfoCommand View_RTInvMonth;
    }
}
