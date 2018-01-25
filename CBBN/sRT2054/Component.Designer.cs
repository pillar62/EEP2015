namespace sRT2054
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
            Srvtools.Service service13 = new Srvtools.Service();
            Srvtools.Service service14 = new Srvtools.Service();
            Srvtools.Service service15 = new Srvtools.Service();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Component));
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
            Srvtools.FieldAttr fieldAttr28 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr29 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr30 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr31 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr32 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr33 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr34 = new Srvtools.FieldAttr();
            Srvtools.KeyItem keyItem1 = new Srvtools.KeyItem();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTSndWork = new Srvtools.InfoCommand(this.components);
            this.ucRTSndWork = new Srvtools.UpdateComponent(this.components);
            this.View_RTSndWork = new Srvtools.InfoCommand(this.components);
            this.cmd = new Srvtools.InfoCommand(this.components);
            this.autoNumber1 = new Srvtools.AutoNumber(this.components);
            this.cmdRT2054R = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTSndWork)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTSndWork)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT2054R)).BeginInit();
            // 
            // serviceManager1
            // 
            service13.DelegateName = "smRT20541";
            service13.NonLogin = false;
            service13.ServiceName = "smRT20541";
            service14.DelegateName = "smRT20543";
            service14.NonLogin = false;
            service14.ServiceName = "smRT20543";
            service15.DelegateName = "smRT20544";
            service15.NonLogin = false;
            service15.ServiceName = "smRT20544";
            this.serviceManager1.ServiceCollection.Add(service13);
            this.serviceManager1.ServiceCollection.Add(service14);
            this.serviceManager1.ServiceCollection.Add(service15);
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTSndWork
            // 
            this.RTSndWork.CacheConnection = false;
            this.RTSndWork.CommandText = resources.GetString("RTSndWork.CommandText");
            this.RTSndWork.CommandTimeout = 30;
            this.RTSndWork.CommandType = System.Data.CommandType.Text;
            this.RTSndWork.DynamicTableName = false;
            this.RTSndWork.EEPAlias = null;
            this.RTSndWork.EncodingAfter = null;
            this.RTSndWork.EncodingBefore = "Windows-1252";
            this.RTSndWork.EncodingConvert = null;
            this.RTSndWork.InfoConnection = this.InfoConnection1;
            keyItem3.KeyName = "WORKNO";
            this.RTSndWork.KeyFields.Add(keyItem3);
            this.RTSndWork.MultiSetWhere = false;
            this.RTSndWork.Name = "RTSndWork";
            this.RTSndWork.NotificationAutoEnlist = false;
            this.RTSndWork.SecExcept = null;
            this.RTSndWork.SecFieldName = null;
            this.RTSndWork.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTSndWork.SelectPaging = false;
            this.RTSndWork.SelectTop = 0;
            this.RTSndWork.SiteControl = false;
            this.RTSndWork.SiteFieldName = null;
            this.RTSndWork.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTSndWork
            // 
            this.ucRTSndWork.AutoTrans = true;
            this.ucRTSndWork.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "WORKNO";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "LINKNO";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "WORKTYPE";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "ASSIGNENG";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "ASSIGNCONS";
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
            fieldAttr7.DataField = "SNDWRKUSR";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "SNDWRKDAT";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "FINISHENG";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "FINISHCONS";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "FINISHUSR";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "FINISHDAT";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "UUSR";
            fieldAttr13.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr13.DefaultValue = null;
            fieldAttr13.TrimLength = 0;
            fieldAttr13.UpdateEnable = true;
            fieldAttr13.WhereMode = true;
            fieldAttr14.CharSetNull = false;
            fieldAttr14.CheckNull = false;
            fieldAttr14.DataField = "UDAT";
            fieldAttr14.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr14.DefaultValue = null;
            fieldAttr14.TrimLength = 0;
            fieldAttr14.UpdateEnable = true;
            fieldAttr14.WhereMode = true;
            fieldAttr15.CharSetNull = false;
            fieldAttr15.CheckNull = false;
            fieldAttr15.DataField = "CANCELUSR";
            fieldAttr15.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr15.DefaultValue = null;
            fieldAttr15.TrimLength = 0;
            fieldAttr15.UpdateEnable = true;
            fieldAttr15.WhereMode = true;
            fieldAttr16.CharSetNull = false;
            fieldAttr16.CheckNull = false;
            fieldAttr16.DataField = "CANCELDAT";
            fieldAttr16.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr16.DefaultValue = null;
            fieldAttr16.TrimLength = 0;
            fieldAttr16.UpdateEnable = true;
            fieldAttr16.WhereMode = true;
            fieldAttr17.CharSetNull = false;
            fieldAttr17.CheckNull = false;
            fieldAttr17.DataField = "FINISHTYP";
            fieldAttr17.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr17.DefaultValue = null;
            fieldAttr17.TrimLength = 0;
            fieldAttr17.UpdateEnable = true;
            fieldAttr17.WhereMode = true;
            fieldAttr18.CharSetNull = false;
            fieldAttr18.CheckNull = false;
            fieldAttr18.DataField = "ASSIGNDAT";
            fieldAttr18.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr18.DefaultValue = null;
            fieldAttr18.TrimLength = 0;
            fieldAttr18.UpdateEnable = true;
            fieldAttr18.WhereMode = true;
            fieldAttr19.CharSetNull = false;
            fieldAttr19.CheckNull = false;
            fieldAttr19.DataField = "ASSIGNTIME";
            fieldAttr19.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr19.DefaultValue = null;
            fieldAttr19.TrimLength = 0;
            fieldAttr19.UpdateEnable = true;
            fieldAttr19.WhereMode = true;
            fieldAttr20.CharSetNull = false;
            fieldAttr20.CheckNull = false;
            fieldAttr20.DataField = "SCORE01";
            fieldAttr20.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr20.DefaultValue = null;
            fieldAttr20.TrimLength = 0;
            fieldAttr20.UpdateEnable = true;
            fieldAttr20.WhereMode = true;
            fieldAttr21.CharSetNull = false;
            fieldAttr21.CheckNull = false;
            fieldAttr21.DataField = "SCORE02";
            fieldAttr21.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr21.DefaultValue = null;
            fieldAttr21.TrimLength = 0;
            fieldAttr21.UpdateEnable = true;
            fieldAttr21.WhereMode = true;
            fieldAttr22.CharSetNull = false;
            fieldAttr22.CheckNull = false;
            fieldAttr22.DataField = "SCORE03";
            fieldAttr22.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr22.DefaultValue = null;
            fieldAttr22.TrimLength = 0;
            fieldAttr22.UpdateEnable = true;
            fieldAttr22.WhereMode = true;
            fieldAttr23.CharSetNull = false;
            fieldAttr23.CheckNull = false;
            fieldAttr23.DataField = "SCORE04";
            fieldAttr23.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr23.DefaultValue = null;
            fieldAttr23.TrimLength = 0;
            fieldAttr23.UpdateEnable = true;
            fieldAttr23.WhereMode = true;
            fieldAttr24.CharSetNull = false;
            fieldAttr24.CheckNull = false;
            fieldAttr24.DataField = "SCORE05";
            fieldAttr24.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr24.DefaultValue = null;
            fieldAttr24.TrimLength = 0;
            fieldAttr24.UpdateEnable = true;
            fieldAttr24.WhereMode = true;
            fieldAttr25.CharSetNull = false;
            fieldAttr25.CheckNull = false;
            fieldAttr25.DataField = "SCORE06";
            fieldAttr25.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr25.DefaultValue = null;
            fieldAttr25.TrimLength = 0;
            fieldAttr25.UpdateEnable = true;
            fieldAttr25.WhereMode = true;
            fieldAttr26.CharSetNull = false;
            fieldAttr26.CheckNull = false;
            fieldAttr26.DataField = "SCORE07";
            fieldAttr26.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr26.DefaultValue = null;
            fieldAttr26.TrimLength = 0;
            fieldAttr26.UpdateEnable = true;
            fieldAttr26.WhereMode = true;
            fieldAttr27.CharSetNull = false;
            fieldAttr27.CheckNull = false;
            fieldAttr27.DataField = "SCORE08";
            fieldAttr27.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr27.DefaultValue = null;
            fieldAttr27.TrimLength = 0;
            fieldAttr27.UpdateEnable = true;
            fieldAttr27.WhereMode = true;
            fieldAttr28.CharSetNull = false;
            fieldAttr28.CheckNull = false;
            fieldAttr28.DataField = "SCORE09";
            fieldAttr28.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr28.DefaultValue = null;
            fieldAttr28.TrimLength = 0;
            fieldAttr28.UpdateEnable = true;
            fieldAttr28.WhereMode = true;
            fieldAttr29.CharSetNull = false;
            fieldAttr29.CheckNull = false;
            fieldAttr29.DataField = "SCORE10";
            fieldAttr29.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr29.DefaultValue = null;
            fieldAttr29.TrimLength = 0;
            fieldAttr29.UpdateEnable = true;
            fieldAttr29.WhereMode = true;
            fieldAttr30.CharSetNull = false;
            fieldAttr30.CheckNull = false;
            fieldAttr30.DataField = "SCORE11";
            fieldAttr30.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr30.DefaultValue = null;
            fieldAttr30.TrimLength = 0;
            fieldAttr30.UpdateEnable = true;
            fieldAttr30.WhereMode = true;
            fieldAttr31.CharSetNull = false;
            fieldAttr31.CheckNull = false;
            fieldAttr31.DataField = "SCORE12";
            fieldAttr31.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr31.DefaultValue = null;
            fieldAttr31.TrimLength = 0;
            fieldAttr31.UpdateEnable = true;
            fieldAttr31.WhereMode = true;
            fieldAttr32.CharSetNull = false;
            fieldAttr32.CheckNull = false;
            fieldAttr32.DataField = "SCORE13";
            fieldAttr32.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr32.DefaultValue = null;
            fieldAttr32.TrimLength = 0;
            fieldAttr32.UpdateEnable = true;
            fieldAttr32.WhereMode = true;
            fieldAttr33.CharSetNull = false;
            fieldAttr33.CheckNull = false;
            fieldAttr33.DataField = "SCORE14";
            fieldAttr33.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr33.DefaultValue = null;
            fieldAttr33.TrimLength = 0;
            fieldAttr33.UpdateEnable = true;
            fieldAttr33.WhereMode = true;
            fieldAttr34.CharSetNull = false;
            fieldAttr34.CheckNull = false;
            fieldAttr34.DataField = "SCORE15";
            fieldAttr34.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr34.DefaultValue = null;
            fieldAttr34.TrimLength = 0;
            fieldAttr34.UpdateEnable = true;
            fieldAttr34.WhereMode = true;
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr1);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr2);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr3);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr4);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr5);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr6);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr7);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr8);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr9);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr10);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr11);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr12);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr13);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr14);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr15);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr16);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr17);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr18);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr19);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr20);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr21);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr22);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr23);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr24);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr25);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr26);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr27);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr28);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr29);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr30);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr31);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr32);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr33);
            this.ucRTSndWork.FieldAttrs.Add(fieldAttr34);
            this.ucRTSndWork.LogInfo = null;
            this.ucRTSndWork.Name = "ucRTSndWork";
            this.ucRTSndWork.RowAffectsCheck = true;
            this.ucRTSndWork.SelectCmd = this.RTSndWork;
            this.ucRTSndWork.SelectCmdForUpdate = null;
            this.ucRTSndWork.SendSQLCmd = true;
            this.ucRTSndWork.ServerModify = true;
            this.ucRTSndWork.ServerModifyGetMax = false;
            this.ucRTSndWork.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTSndWork.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTSndWork.UseTranscationScope = false;
            this.ucRTSndWork.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_RTSndWork
            // 
            this.View_RTSndWork.CacheConnection = false;
            this.View_RTSndWork.CommandText = "SELECT * FROM dbo.[RTSndWork]";
            this.View_RTSndWork.CommandTimeout = 30;
            this.View_RTSndWork.CommandType = System.Data.CommandType.Text;
            this.View_RTSndWork.DynamicTableName = false;
            this.View_RTSndWork.EEPAlias = null;
            this.View_RTSndWork.EncodingAfter = null;
            this.View_RTSndWork.EncodingBefore = "Windows-1252";
            this.View_RTSndWork.EncodingConvert = null;
            this.View_RTSndWork.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "WORKNO";
            this.View_RTSndWork.KeyFields.Add(keyItem1);
            this.View_RTSndWork.MultiSetWhere = false;
            this.View_RTSndWork.Name = "View_RTSndWork";
            this.View_RTSndWork.NotificationAutoEnlist = false;
            this.View_RTSndWork.SecExcept = null;
            this.View_RTSndWork.SecFieldName = null;
            this.View_RTSndWork.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTSndWork.SelectPaging = false;
            this.View_RTSndWork.SelectTop = 0;
            this.View_RTSndWork.SiteControl = false;
            this.View_RTSndWork.SiteFieldName = null;
            this.View_RTSndWork.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmd
            // 
            this.cmd.CacheConnection = false;
            this.cmd.CommandText = "";
            this.cmd.CommandTimeout = 30;
            this.cmd.CommandType = System.Data.CommandType.Text;
            this.cmd.DynamicTableName = false;
            this.cmd.EEPAlias = null;
            this.cmd.EncodingAfter = null;
            this.cmd.EncodingBefore = "Windows-1252";
            this.cmd.EncodingConvert = null;
            this.cmd.InfoConnection = this.InfoConnection1;
            this.cmd.MultiSetWhere = false;
            this.cmd.Name = "cmd";
            this.cmd.NotificationAutoEnlist = false;
            this.cmd.SecExcept = null;
            this.cmd.SecFieldName = null;
            this.cmd.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmd.SelectPaging = false;
            this.cmd.SelectTop = 0;
            this.cmd.SiteControl = false;
            this.cmd.SiteFieldName = null;
            this.cmd.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // autoNumber1
            // 
            this.autoNumber1.Active = true;
            this.autoNumber1.AutoNoID = "RT2054";
            this.autoNumber1.Description = null;
            this.autoNumber1.GetFixed = "getFix()";
            this.autoNumber1.isNumFill = false;
            this.autoNumber1.Name = "autoNumber1";
            this.autoNumber1.Number = null;
            this.autoNumber1.NumDig = 3;
            this.autoNumber1.OldVersion = false;
            this.autoNumber1.OverFlow = true;
            this.autoNumber1.StartValue = 1;
            this.autoNumber1.Step = 1;
            this.autoNumber1.TargetColumn = "WORKNO";
            this.autoNumber1.UpdateComp = this.ucRTSndWork;
            // 
            // cmdRT2054R
            // 
            this.cmdRT2054R.CacheConnection = false;
            this.cmdRT2054R.CommandText = "SELECT A.* FROM V_RT205R A";
            this.cmdRT2054R.CommandTimeout = 30;
            this.cmdRT2054R.CommandType = System.Data.CommandType.Text;
            this.cmdRT2054R.DynamicTableName = false;
            this.cmdRT2054R.EEPAlias = null;
            this.cmdRT2054R.EncodingAfter = null;
            this.cmdRT2054R.EncodingBefore = "Windows-1252";
            this.cmdRT2054R.EncodingConvert = null;
            this.cmdRT2054R.InfoConnection = this.InfoConnection1;
            this.cmdRT2054R.MultiSetWhere = false;
            this.cmdRT2054R.Name = "cmdRT2054R";
            this.cmdRT2054R.NotificationAutoEnlist = false;
            this.cmdRT2054R.SecExcept = null;
            this.cmdRT2054R.SecFieldName = null;
            this.cmdRT2054R.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdRT2054R.SelectPaging = false;
            this.cmdRT2054R.SelectTop = 0;
            this.cmdRT2054R.SiteControl = false;
            this.cmdRT2054R.SiteFieldName = null;
            this.cmdRT2054R.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTSndWork)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTSndWork)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT2054R)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTSndWork;
        private Srvtools.UpdateComponent ucRTSndWork;
        private Srvtools.InfoCommand View_RTSndWork;
        private Srvtools.InfoCommand cmd;
        private Srvtools.AutoNumber autoNumber1;
        private Srvtools.InfoCommand cmdRT2054R;
    }
}
