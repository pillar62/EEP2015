namespace sSYS001
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
            Srvtools.KeyItem keyItem2 = new Srvtools.KeyItem();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.MENUFUNCTION = new Srvtools.InfoCommand(this.components);
            this.ucMENUFUNCTION = new Srvtools.UpdateComponent(this.components);
            this.View_MENUFUNCTION = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MENUFUNCTION)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_MENUFUNCTION)).BeginInit();
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // MENUFUNCTION
            // 
            this.MENUFUNCTION.CacheConnection = false;
            this.MENUFUNCTION.CommandText = "SELECT dbo.[MENUFUNCTION].* FROM dbo.[MENUFUNCTION]\r\nORDER BY NO_SORT";
            this.MENUFUNCTION.CommandTimeout = 30;
            this.MENUFUNCTION.CommandType = System.Data.CommandType.Text;
            this.MENUFUNCTION.DynamicTableName = false;
            this.MENUFUNCTION.EEPAlias = null;
            this.MENUFUNCTION.EncodingAfter = null;
            this.MENUFUNCTION.EncodingBefore = "Windows-1252";
            this.MENUFUNCTION.EncodingConvert = null;
            this.MENUFUNCTION.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "MENUID";
            this.MENUFUNCTION.KeyFields.Add(keyItem1);
            this.MENUFUNCTION.MultiSetWhere = false;
            this.MENUFUNCTION.Name = "MENUFUNCTION";
            this.MENUFUNCTION.NotificationAutoEnlist = false;
            this.MENUFUNCTION.SecExcept = null;
            this.MENUFUNCTION.SecFieldName = null;
            this.MENUFUNCTION.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.MENUFUNCTION.SelectPaging = false;
            this.MENUFUNCTION.SelectTop = 0;
            this.MENUFUNCTION.SiteControl = false;
            this.MENUFUNCTION.SiteFieldName = null;
            this.MENUFUNCTION.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucMENUFUNCTION
            // 
            this.ucMENUFUNCTION.AutoTrans = true;
            this.ucMENUFUNCTION.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "MENUID";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "CAPTION";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "PARENT";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "PACKAGE";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "PACKAGECLA";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "ITEMPARAM";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "FORM";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "ISSHOWMODAL";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "FUNCTIONS";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "ITEMTYPE";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "MODULETYPE";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "SEQ_NO";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "NO_SORT";
            fieldAttr13.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr13.DefaultValue = null;
            fieldAttr13.TrimLength = 0;
            fieldAttr13.UpdateEnable = true;
            fieldAttr13.WhereMode = true;
            fieldAttr14.CharSetNull = false;
            fieldAttr14.CheckNull = false;
            fieldAttr14.DataField = "NM_SHOW";
            fieldAttr14.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr14.DefaultValue = null;
            fieldAttr14.TrimLength = 0;
            fieldAttr14.UpdateEnable = true;
            fieldAttr14.WhereMode = true;
            fieldAttr15.CharSetNull = false;
            fieldAttr15.CheckNull = false;
            fieldAttr15.DataField = "DT_START";
            fieldAttr15.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr15.DefaultValue = null;
            fieldAttr15.TrimLength = 0;
            fieldAttr15.UpdateEnable = true;
            fieldAttr15.WhereMode = true;
            fieldAttr16.CharSetNull = false;
            fieldAttr16.CheckNull = false;
            fieldAttr16.DataField = "DT_PREND";
            fieldAttr16.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr16.DefaultValue = null;
            fieldAttr16.TrimLength = 0;
            fieldAttr16.UpdateEnable = true;
            fieldAttr16.WhereMode = true;
            fieldAttr17.CharSetNull = false;
            fieldAttr17.CheckNull = false;
            fieldAttr17.DataField = "DT_END";
            fieldAttr17.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr17.DefaultValue = null;
            fieldAttr17.TrimLength = 0;
            fieldAttr17.UpdateEnable = true;
            fieldAttr17.WhereMode = true;
            fieldAttr18.CharSetNull = false;
            fieldAttr18.CheckNull = false;
            fieldAttr18.DataField = "NM_DESC";
            fieldAttr18.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr18.DefaultValue = null;
            fieldAttr18.TrimLength = 0;
            fieldAttr18.UpdateEnable = true;
            fieldAttr18.WhereMode = true;
            fieldAttr19.CharSetNull = false;
            fieldAttr19.CheckNull = false;
            fieldAttr19.DataField = "YN_DO";
            fieldAttr19.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr19.DefaultValue = null;
            fieldAttr19.TrimLength = 0;
            fieldAttr19.UpdateEnable = true;
            fieldAttr19.WhereMode = true;
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr1);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr2);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr3);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr4);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr5);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr6);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr7);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr8);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr9);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr10);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr11);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr12);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr13);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr14);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr15);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr16);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr17);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr18);
            this.ucMENUFUNCTION.FieldAttrs.Add(fieldAttr19);
            this.ucMENUFUNCTION.LogInfo = null;
            this.ucMENUFUNCTION.Name = "ucMENUFUNCTION";
            this.ucMENUFUNCTION.RowAffectsCheck = true;
            this.ucMENUFUNCTION.SelectCmd = this.MENUFUNCTION;
            this.ucMENUFUNCTION.SelectCmdForUpdate = null;
            this.ucMENUFUNCTION.SendSQLCmd = true;
            this.ucMENUFUNCTION.ServerModify = true;
            this.ucMENUFUNCTION.ServerModifyGetMax = false;
            this.ucMENUFUNCTION.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucMENUFUNCTION.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucMENUFUNCTION.UseTranscationScope = false;
            this.ucMENUFUNCTION.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_MENUFUNCTION
            // 
            this.View_MENUFUNCTION.CacheConnection = false;
            this.View_MENUFUNCTION.CommandText = "SELECT * FROM dbo.[MENUFUNCTION]";
            this.View_MENUFUNCTION.CommandTimeout = 30;
            this.View_MENUFUNCTION.CommandType = System.Data.CommandType.Text;
            this.View_MENUFUNCTION.DynamicTableName = false;
            this.View_MENUFUNCTION.EEPAlias = null;
            this.View_MENUFUNCTION.EncodingAfter = null;
            this.View_MENUFUNCTION.EncodingBefore = "Windows-1252";
            this.View_MENUFUNCTION.EncodingConvert = null;
            this.View_MENUFUNCTION.InfoConnection = this.InfoConnection1;
            keyItem2.KeyName = "MENUID";
            this.View_MENUFUNCTION.KeyFields.Add(keyItem2);
            this.View_MENUFUNCTION.MultiSetWhere = false;
            this.View_MENUFUNCTION.Name = "View_MENUFUNCTION";
            this.View_MENUFUNCTION.NotificationAutoEnlist = false;
            this.View_MENUFUNCTION.SecExcept = null;
            this.View_MENUFUNCTION.SecFieldName = null;
            this.View_MENUFUNCTION.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_MENUFUNCTION.SelectPaging = false;
            this.View_MENUFUNCTION.SelectTop = 0;
            this.View_MENUFUNCTION.SiteControl = false;
            this.View_MENUFUNCTION.SiteFieldName = null;
            this.View_MENUFUNCTION.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MENUFUNCTION)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_MENUFUNCTION)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand MENUFUNCTION;
        private Srvtools.UpdateComponent ucMENUFUNCTION;
        private Srvtools.InfoCommand View_MENUFUNCTION;
    }
}
