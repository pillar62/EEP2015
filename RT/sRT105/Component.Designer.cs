namespace sRT105
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
            Srvtools.KeyItem keyItem2 = new Srvtools.KeyItem();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTLessorAVSCase = new Srvtools.InfoCommand(this.components);
            this.ucRTLessorAVSCase = new Srvtools.UpdateComponent(this.components);
            this.View_RTLessorAVSCase = new Srvtools.InfoCommand(this.components);
            this.autoNumber1 = new Srvtools.AutoNumber(this.components);
            this.infoTransaction1 = new Srvtools.InfoTransaction(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCase)).BeginInit();
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTLessorAVSCase
            // 
            this.RTLessorAVSCase.CacheConnection = false;
            this.RTLessorAVSCase.CommandText = "SELECT dbo.[RTLessorAVSCase].* FROM dbo.[RTLessorAVSCase]";
            this.RTLessorAVSCase.CommandTimeout = 30;
            this.RTLessorAVSCase.CommandType = System.Data.CommandType.Text;
            this.RTLessorAVSCase.DynamicTableName = false;
            this.RTLessorAVSCase.EEPAlias = null;
            this.RTLessorAVSCase.EncodingAfter = null;
            this.RTLessorAVSCase.EncodingBefore = "Windows-1252";
            this.RTLessorAVSCase.EncodingConvert = null;
            this.RTLessorAVSCase.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "CASEID";
            this.RTLessorAVSCase.KeyFields.Add(keyItem1);
            this.RTLessorAVSCase.MultiSetWhere = false;
            this.RTLessorAVSCase.Name = "RTLessorAVSCase";
            this.RTLessorAVSCase.NotificationAutoEnlist = false;
            this.RTLessorAVSCase.SecExcept = null;
            this.RTLessorAVSCase.SecFieldName = null;
            this.RTLessorAVSCase.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTLessorAVSCase.SelectPaging = false;
            this.RTLessorAVSCase.SelectTop = 0;
            this.RTLessorAVSCase.SiteControl = false;
            this.RTLessorAVSCase.SiteFieldName = null;
            this.RTLessorAVSCase.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTLessorAVSCase
            // 
            this.ucRTLessorAVSCase.AutoTrans = true;
            this.ucRTLessorAVSCase.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "CASEID";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "CASENAME";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "STARTDAT";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "ENDDAT";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "CRTUSR";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "MEMO";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "EUSR";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "EDAT";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "UUSR";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "UDAT";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr1);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr2);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr3);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr4);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr5);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr6);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr7);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr8);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr9);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr10);
            this.ucRTLessorAVSCase.LogInfo = null;
            this.ucRTLessorAVSCase.Name = "ucRTLessorAVSCase";
            this.ucRTLessorAVSCase.RowAffectsCheck = true;
            this.ucRTLessorAVSCase.SelectCmd = this.RTLessorAVSCase;
            this.ucRTLessorAVSCase.SelectCmdForUpdate = null;
            this.ucRTLessorAVSCase.SendSQLCmd = true;
            this.ucRTLessorAVSCase.ServerModify = true;
            this.ucRTLessorAVSCase.ServerModifyGetMax = false;
            this.ucRTLessorAVSCase.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTLessorAVSCase.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTLessorAVSCase.UseTranscationScope = false;
            this.ucRTLessorAVSCase.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_RTLessorAVSCase
            // 
            this.View_RTLessorAVSCase.CacheConnection = false;
            this.View_RTLessorAVSCase.CommandText = "SELECT * FROM dbo.[RTLessorAVSCase]";
            this.View_RTLessorAVSCase.CommandTimeout = 30;
            this.View_RTLessorAVSCase.CommandType = System.Data.CommandType.Text;
            this.View_RTLessorAVSCase.DynamicTableName = false;
            this.View_RTLessorAVSCase.EEPAlias = null;
            this.View_RTLessorAVSCase.EncodingAfter = null;
            this.View_RTLessorAVSCase.EncodingBefore = "Windows-1252";
            this.View_RTLessorAVSCase.EncodingConvert = null;
            this.View_RTLessorAVSCase.InfoConnection = this.InfoConnection1;
            keyItem2.KeyName = "CASEID";
            this.View_RTLessorAVSCase.KeyFields.Add(keyItem2);
            this.View_RTLessorAVSCase.MultiSetWhere = false;
            this.View_RTLessorAVSCase.Name = "View_RTLessorAVSCase";
            this.View_RTLessorAVSCase.NotificationAutoEnlist = false;
            this.View_RTLessorAVSCase.SecExcept = null;
            this.View_RTLessorAVSCase.SecFieldName = null;
            this.View_RTLessorAVSCase.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTLessorAVSCase.SelectPaging = false;
            this.View_RTLessorAVSCase.SelectTop = 0;
            this.View_RTLessorAVSCase.SiteControl = false;
            this.View_RTLessorAVSCase.SiteFieldName = null;
            this.View_RTLessorAVSCase.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // autoNumber1
            // 
            this.autoNumber1.Active = true;
            this.autoNumber1.AutoNoID = "";
            this.autoNumber1.Description = null;
            this.autoNumber1.GetFixed = "SS()";
            this.autoNumber1.isNumFill = false;
            this.autoNumber1.Name = null;
            this.autoNumber1.Number = null;
            this.autoNumber1.NumDig = 3;
            this.autoNumber1.OldVersion = false;
            this.autoNumber1.OverFlow = true;
            this.autoNumber1.StartValue = 1;
            this.autoNumber1.Step = 1;
            this.autoNumber1.TargetColumn = "CASEID";
            this.autoNumber1.UpdateComp = this.ucRTLessorAVSCase;
            // 
            // infoTransaction1
            // 
            this.infoTransaction1.Name = null;
            this.infoTransaction1.UpdateComp = this.ucRTLessorAVSCase;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCase)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTLessorAVSCase;
        private Srvtools.UpdateComponent ucRTLessorAVSCase;
        private Srvtools.InfoCommand View_RTLessorAVSCase;
        private Srvtools.AutoNumber autoNumber1;
        private Srvtools.InfoTransaction infoTransaction1;
    }
}
