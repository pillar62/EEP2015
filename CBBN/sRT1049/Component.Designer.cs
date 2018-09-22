namespace sRT1049
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
            Srvtools.Service service2 = new Srvtools.Service();
            Srvtools.Service service3 = new Srvtools.Service();
            Srvtools.Service service4 = new Srvtools.Service();
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
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            Srvtools.InfoParameter infoParameter1 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoParameter2 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoParameter3 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoParameter4 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoParameter5 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoParameter6 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoParameter7 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoParameter8 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoParameter9 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoParameter10 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoParameter11 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoParameter12 = new Srvtools.InfoParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Component));
            Srvtools.KeyItem keyItem5 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem6 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem7 = new Srvtools.KeyItem();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTLessorAVSCustAdjDay = new Srvtools.InfoCommand(this.components);
            this.ucRTLessorAVSCustAdjDay = new Srvtools.UpdateComponent(this.components);
            this.View_RTLessorAVSCustAdjDay = new Srvtools.InfoCommand(this.components);
            this.cmdRT10491 = new Srvtools.InfoCommand(this.components);
            this.cmdRT10492 = new Srvtools.InfoCommand(this.components);
            this.cmdRT10493 = new Srvtools.InfoCommand(this.components);
            this.cmdRT10494 = new Srvtools.InfoCommand(this.components);
            this.cmd = new Srvtools.InfoCommand(this.components);
            this.RT10491 = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustAdjDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCustAdjDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT10491)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT10492)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT10493)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT10494)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT10491)).BeginInit();
            // 
            // serviceManager1
            // 
            service1.DelegateName = "smRT10491";
            service1.NonLogin = false;
            service1.ServiceName = "smRT10491";
            service2.DelegateName = "smRT10492";
            service2.NonLogin = false;
            service2.ServiceName = "smRT10492";
            service3.DelegateName = "smRT10493";
            service3.NonLogin = false;
            service3.ServiceName = "smRT10493";
            service4.DelegateName = "smRT10494";
            service4.NonLogin = false;
            service4.ServiceName = "smRT10494";
            this.serviceManager1.ServiceCollection.Add(service1);
            this.serviceManager1.ServiceCollection.Add(service2);
            this.serviceManager1.ServiceCollection.Add(service3);
            this.serviceManager1.ServiceCollection.Add(service4);
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTLessorAVSCustAdjDay
            // 
            this.RTLessorAVSCustAdjDay.CacheConnection = false;
            this.RTLessorAVSCustAdjDay.CommandText = "SELECT dbo.[RTLessorAVSCustAdjDay].* FROM dbo.[RTLessorAVSCustAdjDay]\r\nORDER BY A" +
    "DJCLOSEDAT DESC";
            this.RTLessorAVSCustAdjDay.CommandTimeout = 30;
            this.RTLessorAVSCustAdjDay.CommandType = System.Data.CommandType.Text;
            this.RTLessorAVSCustAdjDay.DynamicTableName = false;
            this.RTLessorAVSCustAdjDay.EEPAlias = null;
            this.RTLessorAVSCustAdjDay.EncodingAfter = null;
            this.RTLessorAVSCustAdjDay.EncodingBefore = "Windows-1252";
            this.RTLessorAVSCustAdjDay.EncodingConvert = null;
            this.RTLessorAVSCustAdjDay.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "CUSID";
            keyItem2.KeyName = "ENTRYNO";
            this.RTLessorAVSCustAdjDay.KeyFields.Add(keyItem1);
            this.RTLessorAVSCustAdjDay.KeyFields.Add(keyItem2);
            this.RTLessorAVSCustAdjDay.MultiSetWhere = false;
            this.RTLessorAVSCustAdjDay.Name = "RTLessorAVSCustAdjDay";
            this.RTLessorAVSCustAdjDay.NotificationAutoEnlist = false;
            this.RTLessorAVSCustAdjDay.SecExcept = null;
            this.RTLessorAVSCustAdjDay.SecFieldName = null;
            this.RTLessorAVSCustAdjDay.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTLessorAVSCustAdjDay.SelectPaging = false;
            this.RTLessorAVSCustAdjDay.SelectTop = 0;
            this.RTLessorAVSCustAdjDay.SiteControl = false;
            this.RTLessorAVSCustAdjDay.SiteFieldName = null;
            this.RTLessorAVSCustAdjDay.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTLessorAVSCustAdjDay
            // 
            this.ucRTLessorAVSCustAdjDay.AutoTrans = true;
            this.ucRTLessorAVSCustAdjDay.ExceptJoin = false;
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
            fieldAttr3.DataField = "ADJPERIOD";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "ADJDAY";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "ADJCLOSEDAT";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "ADJCLOSEUSR";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "CANCELDAT";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "CANCELUSR";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "EUSR";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "EDAT";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "UUSR";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "UDAT";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "memo";
            fieldAttr13.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr13.DefaultValue = null;
            fieldAttr13.TrimLength = 0;
            fieldAttr13.UpdateEnable = true;
            fieldAttr13.WhereMode = true;
            this.ucRTLessorAVSCustAdjDay.FieldAttrs.Add(fieldAttr1);
            this.ucRTLessorAVSCustAdjDay.FieldAttrs.Add(fieldAttr2);
            this.ucRTLessorAVSCustAdjDay.FieldAttrs.Add(fieldAttr3);
            this.ucRTLessorAVSCustAdjDay.FieldAttrs.Add(fieldAttr4);
            this.ucRTLessorAVSCustAdjDay.FieldAttrs.Add(fieldAttr5);
            this.ucRTLessorAVSCustAdjDay.FieldAttrs.Add(fieldAttr6);
            this.ucRTLessorAVSCustAdjDay.FieldAttrs.Add(fieldAttr7);
            this.ucRTLessorAVSCustAdjDay.FieldAttrs.Add(fieldAttr8);
            this.ucRTLessorAVSCustAdjDay.FieldAttrs.Add(fieldAttr9);
            this.ucRTLessorAVSCustAdjDay.FieldAttrs.Add(fieldAttr10);
            this.ucRTLessorAVSCustAdjDay.FieldAttrs.Add(fieldAttr11);
            this.ucRTLessorAVSCustAdjDay.FieldAttrs.Add(fieldAttr12);
            this.ucRTLessorAVSCustAdjDay.FieldAttrs.Add(fieldAttr13);
            this.ucRTLessorAVSCustAdjDay.LogInfo = null;
            this.ucRTLessorAVSCustAdjDay.Name = "ucRTLessorAVSCustAdjDay";
            this.ucRTLessorAVSCustAdjDay.RowAffectsCheck = true;
            this.ucRTLessorAVSCustAdjDay.SelectCmd = this.RTLessorAVSCustAdjDay;
            this.ucRTLessorAVSCustAdjDay.SelectCmdForUpdate = null;
            this.ucRTLessorAVSCustAdjDay.SendSQLCmd = true;
            this.ucRTLessorAVSCustAdjDay.ServerModify = true;
            this.ucRTLessorAVSCustAdjDay.ServerModifyGetMax = false;
            this.ucRTLessorAVSCustAdjDay.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTLessorAVSCustAdjDay.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTLessorAVSCustAdjDay.UseTranscationScope = false;
            this.ucRTLessorAVSCustAdjDay.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_RTLessorAVSCustAdjDay
            // 
            this.View_RTLessorAVSCustAdjDay.CacheConnection = false;
            this.View_RTLessorAVSCustAdjDay.CommandText = "SELECT * FROM dbo.[RTLessorAVSCustAdjDay]";
            this.View_RTLessorAVSCustAdjDay.CommandTimeout = 30;
            this.View_RTLessorAVSCustAdjDay.CommandType = System.Data.CommandType.Text;
            this.View_RTLessorAVSCustAdjDay.DynamicTableName = false;
            this.View_RTLessorAVSCustAdjDay.EEPAlias = null;
            this.View_RTLessorAVSCustAdjDay.EncodingAfter = null;
            this.View_RTLessorAVSCustAdjDay.EncodingBefore = "Windows-1252";
            this.View_RTLessorAVSCustAdjDay.EncodingConvert = null;
            this.View_RTLessorAVSCustAdjDay.InfoConnection = this.InfoConnection1;
            keyItem3.KeyName = "CUSID";
            keyItem4.KeyName = "ENTRYNO";
            this.View_RTLessorAVSCustAdjDay.KeyFields.Add(keyItem3);
            this.View_RTLessorAVSCustAdjDay.KeyFields.Add(keyItem4);
            this.View_RTLessorAVSCustAdjDay.MultiSetWhere = false;
            this.View_RTLessorAVSCustAdjDay.Name = "View_RTLessorAVSCustAdjDay";
            this.View_RTLessorAVSCustAdjDay.NotificationAutoEnlist = false;
            this.View_RTLessorAVSCustAdjDay.SecExcept = null;
            this.View_RTLessorAVSCustAdjDay.SecFieldName = null;
            this.View_RTLessorAVSCustAdjDay.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTLessorAVSCustAdjDay.SelectPaging = false;
            this.View_RTLessorAVSCustAdjDay.SelectTop = 0;
            this.View_RTLessorAVSCustAdjDay.SiteControl = false;
            this.View_RTLessorAVSCustAdjDay.SiteFieldName = null;
            this.View_RTLessorAVSCustAdjDay.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmdRT10491
            // 
            this.cmdRT10491.CacheConnection = false;
            this.cmdRT10491.CommandText = "usp_RTLessorAVSCustAdjDayF";
            this.cmdRT10491.CommandTimeout = 30;
            this.cmdRT10491.CommandType = System.Data.CommandType.StoredProcedure;
            this.cmdRT10491.DynamicTableName = false;
            this.cmdRT10491.EEPAlias = null;
            this.cmdRT10491.EncodingAfter = null;
            this.cmdRT10491.EncodingBefore = "Windows-1252";
            this.cmdRT10491.EncodingConvert = null;
            this.cmdRT10491.InfoConnection = this.InfoConnection1;
            infoParameter1.InfoDbType = Srvtools.InfoDbType.VarChar;
            infoParameter1.ParameterName = "cusid";
            infoParameter1.Precision = ((byte)(0));
            infoParameter1.Scale = ((byte)(0));
            infoParameter1.Size = 15;
            infoParameter1.SourceColumn = null;
            infoParameter1.XmlSchemaCollectionDatabase = null;
            infoParameter1.XmlSchemaCollectionName = null;
            infoParameter1.XmlSchemaCollectionOwningSchema = null;
            infoParameter2.InfoDbType = Srvtools.InfoDbType.Int;
            infoParameter2.ParameterName = "entryno";
            infoParameter2.Precision = ((byte)(0));
            infoParameter2.Scale = ((byte)(0));
            infoParameter2.Size = 0;
            infoParameter2.SourceColumn = null;
            infoParameter2.XmlSchemaCollectionDatabase = null;
            infoParameter2.XmlSchemaCollectionName = null;
            infoParameter2.XmlSchemaCollectionOwningSchema = null;
            infoParameter3.InfoDbType = Srvtools.InfoDbType.VarChar;
            infoParameter3.ParameterName = "usr";
            infoParameter3.Precision = ((byte)(0));
            infoParameter3.Scale = ((byte)(0));
            infoParameter3.Size = 6;
            infoParameter3.SourceColumn = null;
            infoParameter3.XmlSchemaCollectionDatabase = null;
            infoParameter3.XmlSchemaCollectionName = null;
            infoParameter3.XmlSchemaCollectionOwningSchema = null;
            this.cmdRT10491.InfoParameters.Add(infoParameter1);
            this.cmdRT10491.InfoParameters.Add(infoParameter2);
            this.cmdRT10491.InfoParameters.Add(infoParameter3);
            this.cmdRT10491.MultiSetWhere = false;
            this.cmdRT10491.Name = "cmdRT10491";
            this.cmdRT10491.NotificationAutoEnlist = false;
            this.cmdRT10491.SecExcept = null;
            this.cmdRT10491.SecFieldName = null;
            this.cmdRT10491.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdRT10491.SelectPaging = false;
            this.cmdRT10491.SelectTop = 0;
            this.cmdRT10491.SiteControl = false;
            this.cmdRT10491.SiteFieldName = null;
            this.cmdRT10491.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmdRT10492
            // 
            this.cmdRT10492.CacheConnection = false;
            this.cmdRT10492.CommandText = "usp_RTLessorAVSCustAdjDayFR";
            this.cmdRT10492.CommandTimeout = 30;
            this.cmdRT10492.CommandType = System.Data.CommandType.StoredProcedure;
            this.cmdRT10492.DynamicTableName = false;
            this.cmdRT10492.EEPAlias = null;
            this.cmdRT10492.EncodingAfter = null;
            this.cmdRT10492.EncodingBefore = "Windows-1252";
            this.cmdRT10492.EncodingConvert = null;
            this.cmdRT10492.InfoConnection = this.InfoConnection1;
            infoParameter4.InfoDbType = Srvtools.InfoDbType.VarChar;
            infoParameter4.ParameterName = "cusid";
            infoParameter4.Precision = ((byte)(0));
            infoParameter4.Scale = ((byte)(0));
            infoParameter4.Size = 15;
            infoParameter4.SourceColumn = null;
            infoParameter4.XmlSchemaCollectionDatabase = null;
            infoParameter4.XmlSchemaCollectionName = null;
            infoParameter4.XmlSchemaCollectionOwningSchema = null;
            infoParameter5.InfoDbType = Srvtools.InfoDbType.Int;
            infoParameter5.ParameterName = "entryno";
            infoParameter5.Precision = ((byte)(0));
            infoParameter5.Scale = ((byte)(0));
            infoParameter5.Size = 0;
            infoParameter5.SourceColumn = null;
            infoParameter5.XmlSchemaCollectionDatabase = null;
            infoParameter5.XmlSchemaCollectionName = null;
            infoParameter5.XmlSchemaCollectionOwningSchema = null;
            infoParameter6.InfoDbType = Srvtools.InfoDbType.VarChar;
            infoParameter6.ParameterName = "usr";
            infoParameter6.Precision = ((byte)(0));
            infoParameter6.Scale = ((byte)(0));
            infoParameter6.Size = 6;
            infoParameter6.SourceColumn = null;
            infoParameter6.XmlSchemaCollectionDatabase = null;
            infoParameter6.XmlSchemaCollectionName = null;
            infoParameter6.XmlSchemaCollectionOwningSchema = null;
            this.cmdRT10492.InfoParameters.Add(infoParameter4);
            this.cmdRT10492.InfoParameters.Add(infoParameter5);
            this.cmdRT10492.InfoParameters.Add(infoParameter6);
            this.cmdRT10492.MultiSetWhere = false;
            this.cmdRT10492.Name = "cmdRT10492";
            this.cmdRT10492.NotificationAutoEnlist = false;
            this.cmdRT10492.SecExcept = null;
            this.cmdRT10492.SecFieldName = null;
            this.cmdRT10492.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdRT10492.SelectPaging = false;
            this.cmdRT10492.SelectTop = 0;
            this.cmdRT10492.SiteControl = false;
            this.cmdRT10492.SiteFieldName = null;
            this.cmdRT10492.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmdRT10493
            // 
            this.cmdRT10493.CacheConnection = false;
            this.cmdRT10493.CommandText = "usp_RTLessorAVSCustAdjDayCancel";
            this.cmdRT10493.CommandTimeout = 30;
            this.cmdRT10493.CommandType = System.Data.CommandType.StoredProcedure;
            this.cmdRT10493.DynamicTableName = false;
            this.cmdRT10493.EEPAlias = null;
            this.cmdRT10493.EncodingAfter = null;
            this.cmdRT10493.EncodingBefore = "Windows-1252";
            this.cmdRT10493.EncodingConvert = null;
            this.cmdRT10493.InfoConnection = this.InfoConnection1;
            infoParameter7.InfoDbType = Srvtools.InfoDbType.VarChar;
            infoParameter7.ParameterName = "cusid";
            infoParameter7.Precision = ((byte)(0));
            infoParameter7.Scale = ((byte)(0));
            infoParameter7.Size = 15;
            infoParameter7.SourceColumn = null;
            infoParameter7.XmlSchemaCollectionDatabase = null;
            infoParameter7.XmlSchemaCollectionName = null;
            infoParameter7.XmlSchemaCollectionOwningSchema = null;
            infoParameter8.InfoDbType = Srvtools.InfoDbType.Int;
            infoParameter8.ParameterName = "entryno";
            infoParameter8.Precision = ((byte)(0));
            infoParameter8.Scale = ((byte)(0));
            infoParameter8.Size = 0;
            infoParameter8.SourceColumn = null;
            infoParameter8.XmlSchemaCollectionDatabase = null;
            infoParameter8.XmlSchemaCollectionName = null;
            infoParameter8.XmlSchemaCollectionOwningSchema = null;
            infoParameter9.InfoDbType = Srvtools.InfoDbType.VarChar;
            infoParameter9.ParameterName = "usr";
            infoParameter9.Precision = ((byte)(0));
            infoParameter9.Scale = ((byte)(0));
            infoParameter9.Size = 6;
            infoParameter9.SourceColumn = null;
            infoParameter9.XmlSchemaCollectionDatabase = null;
            infoParameter9.XmlSchemaCollectionName = null;
            infoParameter9.XmlSchemaCollectionOwningSchema = null;
            this.cmdRT10493.InfoParameters.Add(infoParameter7);
            this.cmdRT10493.InfoParameters.Add(infoParameter8);
            this.cmdRT10493.InfoParameters.Add(infoParameter9);
            this.cmdRT10493.MultiSetWhere = false;
            this.cmdRT10493.Name = "cmdRT10493";
            this.cmdRT10493.NotificationAutoEnlist = false;
            this.cmdRT10493.SecExcept = null;
            this.cmdRT10493.SecFieldName = null;
            this.cmdRT10493.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdRT10493.SelectPaging = false;
            this.cmdRT10493.SelectTop = 0;
            this.cmdRT10493.SiteControl = false;
            this.cmdRT10493.SiteFieldName = null;
            this.cmdRT10493.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmdRT10494
            // 
            this.cmdRT10494.CacheConnection = false;
            this.cmdRT10494.CommandText = "usp_RTLessorAVSCustadjdaycancelRTN";
            this.cmdRT10494.CommandTimeout = 30;
            this.cmdRT10494.CommandType = System.Data.CommandType.StoredProcedure;
            this.cmdRT10494.DynamicTableName = false;
            this.cmdRT10494.EEPAlias = null;
            this.cmdRT10494.EncodingAfter = null;
            this.cmdRT10494.EncodingBefore = "Windows-1252";
            this.cmdRT10494.EncodingConvert = null;
            this.cmdRT10494.InfoConnection = this.InfoConnection1;
            infoParameter10.InfoDbType = Srvtools.InfoDbType.VarChar;
            infoParameter10.ParameterName = "cusid";
            infoParameter10.Precision = ((byte)(0));
            infoParameter10.Scale = ((byte)(0));
            infoParameter10.Size = 15;
            infoParameter10.SourceColumn = null;
            infoParameter10.XmlSchemaCollectionDatabase = null;
            infoParameter10.XmlSchemaCollectionName = null;
            infoParameter10.XmlSchemaCollectionOwningSchema = null;
            infoParameter11.InfoDbType = Srvtools.InfoDbType.Int;
            infoParameter11.ParameterName = "entryno";
            infoParameter11.Precision = ((byte)(0));
            infoParameter11.Scale = ((byte)(0));
            infoParameter11.Size = 0;
            infoParameter11.SourceColumn = null;
            infoParameter11.XmlSchemaCollectionDatabase = null;
            infoParameter11.XmlSchemaCollectionName = null;
            infoParameter11.XmlSchemaCollectionOwningSchema = null;
            infoParameter12.InfoDbType = Srvtools.InfoDbType.VarChar;
            infoParameter12.ParameterName = "usr";
            infoParameter12.Precision = ((byte)(0));
            infoParameter12.Scale = ((byte)(0));
            infoParameter12.Size = 6;
            infoParameter12.SourceColumn = null;
            infoParameter12.XmlSchemaCollectionDatabase = null;
            infoParameter12.XmlSchemaCollectionName = null;
            infoParameter12.XmlSchemaCollectionOwningSchema = null;
            this.cmdRT10494.InfoParameters.Add(infoParameter10);
            this.cmdRT10494.InfoParameters.Add(infoParameter11);
            this.cmdRT10494.InfoParameters.Add(infoParameter12);
            this.cmdRT10494.MultiSetWhere = false;
            this.cmdRT10494.Name = "cmdRT10494";
            this.cmdRT10494.NotificationAutoEnlist = false;
            this.cmdRT10494.SecExcept = null;
            this.cmdRT10494.SecFieldName = null;
            this.cmdRT10494.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdRT10494.SelectPaging = false;
            this.cmdRT10494.SelectTop = 0;
            this.cmdRT10494.SiteControl = false;
            this.cmdRT10494.SiteFieldName = null;
            this.cmdRT10494.UpdatedRowSource = System.Data.UpdateRowSource.None;
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
            // RT10491
            // 
            this.RT10491.CacheConnection = false;
            this.RT10491.CommandText = resources.GetString("RT10491.CommandText");
            this.RT10491.CommandTimeout = 30;
            this.RT10491.CommandType = System.Data.CommandType.Text;
            this.RT10491.DynamicTableName = false;
            this.RT10491.EEPAlias = null;
            this.RT10491.EncodingAfter = null;
            this.RT10491.EncodingBefore = "Windows-1252";
            this.RT10491.EncodingConvert = null;
            this.RT10491.InfoConnection = this.InfoConnection1;
            keyItem5.KeyName = "CUSID";
            keyItem6.KeyName = "ENTRYNO";
            keyItem7.KeyName = "seq";
            this.RT10491.KeyFields.Add(keyItem5);
            this.RT10491.KeyFields.Add(keyItem6);
            this.RT10491.KeyFields.Add(keyItem7);
            this.RT10491.MultiSetWhere = false;
            this.RT10491.Name = "RT10491";
            this.RT10491.NotificationAutoEnlist = false;
            this.RT10491.SecExcept = null;
            this.RT10491.SecFieldName = null;
            this.RT10491.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RT10491.SelectPaging = false;
            this.RT10491.SelectTop = 0;
            this.RT10491.SiteControl = false;
            this.RT10491.SiteFieldName = null;
            this.RT10491.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustAdjDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCustAdjDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT10491)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT10492)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT10493)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT10494)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT10491)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTLessorAVSCustAdjDay;
        private Srvtools.UpdateComponent ucRTLessorAVSCustAdjDay;
        private Srvtools.InfoCommand View_RTLessorAVSCustAdjDay;
        private Srvtools.InfoCommand cmdRT10491;
        private Srvtools.InfoCommand cmdRT10492;
        private Srvtools.InfoCommand cmdRT10493;
        private Srvtools.InfoCommand cmdRT10494;
        private Srvtools.InfoCommand cmd;
        private Srvtools.InfoCommand RT10491;
    }
}
