namespace sRT10423
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
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem5 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem6 = new Srvtools.KeyItem();
            Srvtools.InfoParameter infoParameter1 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoParameter2 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoParameter3 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoParameter4 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoParameter5 = new Srvtools.InfoParameter();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTLessorAVSCustHardware = new Srvtools.InfoCommand(this.components);
            this.ucRTLessorAVSCustHardware = new Srvtools.UpdateComponent(this.components);
            this.View_RTLessorAVSCustHardware = new Srvtools.InfoCommand(this.components);
            this.cmdRT104231 = new Srvtools.InfoCommand(this.components);
            this.cmdRT104232 = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustHardware)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCustHardware)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT104231)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT104232)).BeginInit();
            // 
            // serviceManager1
            // 
            service1.DelegateName = "smRT104231";
            service1.NonLogin = false;
            service1.ServiceName = "smRT104231";
            service2.DelegateName = "smRT104232";
            service2.NonLogin = false;
            service2.ServiceName = "smRT104232";
            this.serviceManager1.ServiceCollection.Add(service1);
            this.serviceManager1.ServiceCollection.Add(service2);
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTLessorAVSCustHardware
            // 
            this.RTLessorAVSCustHardware.CacheConnection = false;
            this.RTLessorAVSCustHardware.CommandText = "SELECT dbo.[RTLessorAVSCustHardware].* FROM dbo.[RTLessorAVSCustHardware]";
            this.RTLessorAVSCustHardware.CommandTimeout = 30;
            this.RTLessorAVSCustHardware.CommandType = System.Data.CommandType.Text;
            this.RTLessorAVSCustHardware.DynamicTableName = false;
            this.RTLessorAVSCustHardware.EEPAlias = null;
            this.RTLessorAVSCustHardware.EncodingAfter = null;
            this.RTLessorAVSCustHardware.EncodingBefore = "Windows-1252";
            this.RTLessorAVSCustHardware.EncodingConvert = null;
            this.RTLessorAVSCustHardware.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "CUSID";
            keyItem2.KeyName = "PRTNO";
            keyItem3.KeyName = "ENTRYNO";
            this.RTLessorAVSCustHardware.KeyFields.Add(keyItem1);
            this.RTLessorAVSCustHardware.KeyFields.Add(keyItem2);
            this.RTLessorAVSCustHardware.KeyFields.Add(keyItem3);
            this.RTLessorAVSCustHardware.MultiSetWhere = false;
            this.RTLessorAVSCustHardware.Name = "RTLessorAVSCustHardware";
            this.RTLessorAVSCustHardware.NotificationAutoEnlist = false;
            this.RTLessorAVSCustHardware.SecExcept = null;
            this.RTLessorAVSCustHardware.SecFieldName = null;
            this.RTLessorAVSCustHardware.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTLessorAVSCustHardware.SelectPaging = false;
            this.RTLessorAVSCustHardware.SelectTop = 0;
            this.RTLessorAVSCustHardware.SiteControl = false;
            this.RTLessorAVSCustHardware.SiteFieldName = null;
            this.RTLessorAVSCustHardware.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTLessorAVSCustHardware
            // 
            this.ucRTLessorAVSCustHardware.AutoTrans = true;
            this.ucRTLessorAVSCustHardware.ExceptJoin = false;
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
            fieldAttr2.DataField = "PRTNO";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "ENTRYNO";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "PRODNO";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "ITEMNO";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "QTY";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "DROPDAT";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "DROPREASON";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "WAREHOUSE";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "ASSETNO";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "DROPUSR";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "UNIT";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "AMT";
            fieldAttr13.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr13.DefaultValue = null;
            fieldAttr13.TrimLength = 0;
            fieldAttr13.UpdateEnable = true;
            fieldAttr13.WhereMode = true;
            fieldAttr14.CharSetNull = false;
            fieldAttr14.CheckNull = false;
            fieldAttr14.DataField = "EUSR";
            fieldAttr14.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr14.DefaultValue = null;
            fieldAttr14.TrimLength = 0;
            fieldAttr14.UpdateEnable = true;
            fieldAttr14.WhereMode = true;
            fieldAttr15.CharSetNull = false;
            fieldAttr15.CheckNull = false;
            fieldAttr15.DataField = "EDAT";
            fieldAttr15.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr15.DefaultValue = null;
            fieldAttr15.TrimLength = 0;
            fieldAttr15.UpdateEnable = true;
            fieldAttr15.WhereMode = true;
            fieldAttr16.CharSetNull = false;
            fieldAttr16.CheckNull = false;
            fieldAttr16.DataField = "UUSR";
            fieldAttr16.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr16.DefaultValue = null;
            fieldAttr16.TrimLength = 0;
            fieldAttr16.UpdateEnable = true;
            fieldAttr16.WhereMode = true;
            fieldAttr17.CharSetNull = false;
            fieldAttr17.CheckNull = false;
            fieldAttr17.DataField = "UDAT";
            fieldAttr17.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr17.DefaultValue = null;
            fieldAttr17.TrimLength = 0;
            fieldAttr17.UpdateEnable = true;
            fieldAttr17.WhereMode = true;
            fieldAttr18.CharSetNull = false;
            fieldAttr18.CheckNull = false;
            fieldAttr18.DataField = "MEMO";
            fieldAttr18.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr18.DefaultValue = null;
            fieldAttr18.TrimLength = 0;
            fieldAttr18.UpdateEnable = true;
            fieldAttr18.WhereMode = true;
            fieldAttr19.CharSetNull = false;
            fieldAttr19.CheckNull = false;
            fieldAttr19.DataField = "BATCHNO";
            fieldAttr19.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr19.DefaultValue = null;
            fieldAttr19.TrimLength = 0;
            fieldAttr19.UpdateEnable = true;
            fieldAttr19.WhereMode = true;
            fieldAttr20.CharSetNull = false;
            fieldAttr20.CheckNull = false;
            fieldAttr20.DataField = "TARDAT";
            fieldAttr20.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr20.DefaultValue = null;
            fieldAttr20.TrimLength = 0;
            fieldAttr20.UpdateEnable = true;
            fieldAttr20.WhereMode = true;
            fieldAttr21.CharSetNull = false;
            fieldAttr21.CheckNull = false;
            fieldAttr21.DataField = "TUSR";
            fieldAttr21.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr21.DefaultValue = null;
            fieldAttr21.TrimLength = 0;
            fieldAttr21.UpdateEnable = true;
            fieldAttr21.WhereMode = true;
            fieldAttr22.CharSetNull = false;
            fieldAttr22.CheckNull = false;
            fieldAttr22.DataField = "RCVPRTNO";
            fieldAttr22.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr22.DefaultValue = null;
            fieldAttr22.TrimLength = 0;
            fieldAttr22.UpdateEnable = true;
            fieldAttr22.WhereMode = true;
            fieldAttr23.CharSetNull = false;
            fieldAttr23.CheckNull = false;
            fieldAttr23.DataField = "RCVDAT";
            fieldAttr23.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr23.DefaultValue = null;
            fieldAttr23.TrimLength = 0;
            fieldAttr23.UpdateEnable = true;
            fieldAttr23.WhereMode = true;
            fieldAttr24.CharSetNull = false;
            fieldAttr24.CheckNull = false;
            fieldAttr24.DataField = "RCVFINISHDAT";
            fieldAttr24.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr24.DefaultValue = null;
            fieldAttr24.TrimLength = 0;
            fieldAttr24.UpdateEnable = true;
            fieldAttr24.WhereMode = true;
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr1);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr2);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr3);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr4);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr5);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr6);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr7);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr8);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr9);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr10);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr11);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr12);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr13);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr14);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr15);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr16);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr17);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr18);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr19);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr20);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr21);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr22);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr23);
            this.ucRTLessorAVSCustHardware.FieldAttrs.Add(fieldAttr24);
            this.ucRTLessorAVSCustHardware.LogInfo = null;
            this.ucRTLessorAVSCustHardware.Name = "ucRTLessorAVSCustHardware";
            this.ucRTLessorAVSCustHardware.RowAffectsCheck = true;
            this.ucRTLessorAVSCustHardware.SelectCmd = this.RTLessorAVSCustHardware;
            this.ucRTLessorAVSCustHardware.SelectCmdForUpdate = null;
            this.ucRTLessorAVSCustHardware.SendSQLCmd = true;
            this.ucRTLessorAVSCustHardware.ServerModify = true;
            this.ucRTLessorAVSCustHardware.ServerModifyGetMax = false;
            this.ucRTLessorAVSCustHardware.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTLessorAVSCustHardware.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTLessorAVSCustHardware.UseTranscationScope = false;
            this.ucRTLessorAVSCustHardware.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_RTLessorAVSCustHardware
            // 
            this.View_RTLessorAVSCustHardware.CacheConnection = false;
            this.View_RTLessorAVSCustHardware.CommandText = "SELECT * FROM dbo.[RTLessorAVSCustHardware]";
            this.View_RTLessorAVSCustHardware.CommandTimeout = 30;
            this.View_RTLessorAVSCustHardware.CommandType = System.Data.CommandType.Text;
            this.View_RTLessorAVSCustHardware.DynamicTableName = false;
            this.View_RTLessorAVSCustHardware.EEPAlias = null;
            this.View_RTLessorAVSCustHardware.EncodingAfter = null;
            this.View_RTLessorAVSCustHardware.EncodingBefore = "Windows-1252";
            this.View_RTLessorAVSCustHardware.EncodingConvert = null;
            this.View_RTLessorAVSCustHardware.InfoConnection = this.InfoConnection1;
            keyItem4.KeyName = "CUSID";
            keyItem5.KeyName = "PRTNO";
            keyItem6.KeyName = "ENTRYNO";
            this.View_RTLessorAVSCustHardware.KeyFields.Add(keyItem4);
            this.View_RTLessorAVSCustHardware.KeyFields.Add(keyItem5);
            this.View_RTLessorAVSCustHardware.KeyFields.Add(keyItem6);
            this.View_RTLessorAVSCustHardware.MultiSetWhere = false;
            this.View_RTLessorAVSCustHardware.Name = "View_RTLessorAVSCustHardware";
            this.View_RTLessorAVSCustHardware.NotificationAutoEnlist = false;
            this.View_RTLessorAVSCustHardware.SecExcept = null;
            this.View_RTLessorAVSCustHardware.SecFieldName = null;
            this.View_RTLessorAVSCustHardware.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTLessorAVSCustHardware.SelectPaging = false;
            this.View_RTLessorAVSCustHardware.SelectTop = 0;
            this.View_RTLessorAVSCustHardware.SiteControl = false;
            this.View_RTLessorAVSCustHardware.SiteFieldName = null;
            this.View_RTLessorAVSCustHardware.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmdRT104231
            // 
            this.cmdRT104231.CacheConnection = false;
            this.cmdRT104231.CommandText = "usp_RTLessorAVSCustHardwareTRNRCVExe";
            this.cmdRT104231.CommandTimeout = 30;
            this.cmdRT104231.CommandType = System.Data.CommandType.StoredProcedure;
            this.cmdRT104231.DynamicTableName = false;
            this.cmdRT104231.EEPAlias = null;
            this.cmdRT104231.EncodingAfter = null;
            this.cmdRT104231.EncodingBefore = "Windows-1252";
            this.cmdRT104231.EncodingConvert = null;
            this.cmdRT104231.InfoConnection = this.InfoConnection1;
            infoParameter1.InfoDbType = Srvtools.InfoDbType.VarChar;
            infoParameter1.ParameterName = "cusid";
            infoParameter1.Precision = ((byte)(0));
            infoParameter1.Scale = ((byte)(0));
            infoParameter1.Size = 15;
            infoParameter1.SourceColumn = null;
            infoParameter1.XmlSchemaCollectionDatabase = null;
            infoParameter1.XmlSchemaCollectionName = null;
            infoParameter1.XmlSchemaCollectionOwningSchema = null;
            infoParameter2.InfoDbType = Srvtools.InfoDbType.VarChar;
            infoParameter2.ParameterName = "prtno";
            infoParameter2.Precision = ((byte)(0));
            infoParameter2.Scale = ((byte)(0));
            infoParameter2.Size = 12;
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
            this.cmdRT104231.InfoParameters.Add(infoParameter1);
            this.cmdRT104231.InfoParameters.Add(infoParameter2);
            this.cmdRT104231.InfoParameters.Add(infoParameter3);
            this.cmdRT104231.MultiSetWhere = false;
            this.cmdRT104231.Name = "cmdRT104231";
            this.cmdRT104231.NotificationAutoEnlist = false;
            this.cmdRT104231.SecExcept = null;
            this.cmdRT104231.SecFieldName = null;
            this.cmdRT104231.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdRT104231.SelectPaging = false;
            this.cmdRT104231.SelectTop = 0;
            this.cmdRT104231.SiteControl = false;
            this.cmdRT104231.SiteFieldName = null;
            this.cmdRT104231.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmdRT104232
            // 
            this.cmdRT104232.CacheConnection = false;
            this.cmdRT104232.CommandText = "usp_RTLessorAVSCustHardwareTRNRCVRTNExe";
            this.cmdRT104232.CommandTimeout = 30;
            this.cmdRT104232.CommandType = System.Data.CommandType.StoredProcedure;
            this.cmdRT104232.DynamicTableName = false;
            this.cmdRT104232.EEPAlias = null;
            this.cmdRT104232.EncodingAfter = null;
            this.cmdRT104232.EncodingBefore = "Windows-1252";
            this.cmdRT104232.EncodingConvert = null;
            this.cmdRT104232.InfoConnection = this.InfoConnection1;
            infoParameter4.InfoDbType = Srvtools.InfoDbType.VarChar;
            infoParameter4.ParameterName = "rcvprtno";
            infoParameter4.Precision = ((byte)(0));
            infoParameter4.Scale = ((byte)(0));
            infoParameter4.Size = 13;
            infoParameter4.SourceColumn = null;
            infoParameter4.XmlSchemaCollectionDatabase = null;
            infoParameter4.XmlSchemaCollectionName = null;
            infoParameter4.XmlSchemaCollectionOwningSchema = null;
            infoParameter5.InfoDbType = Srvtools.InfoDbType.VarChar;
            infoParameter5.ParameterName = "usr";
            infoParameter5.Precision = ((byte)(0));
            infoParameter5.Scale = ((byte)(0));
            infoParameter5.Size = 6;
            infoParameter5.SourceColumn = null;
            infoParameter5.XmlSchemaCollectionDatabase = null;
            infoParameter5.XmlSchemaCollectionName = null;
            infoParameter5.XmlSchemaCollectionOwningSchema = null;
            this.cmdRT104232.InfoParameters.Add(infoParameter4);
            this.cmdRT104232.InfoParameters.Add(infoParameter5);
            this.cmdRT104232.MultiSetWhere = false;
            this.cmdRT104232.Name = "cmdRT104232";
            this.cmdRT104232.NotificationAutoEnlist = false;
            this.cmdRT104232.SecExcept = null;
            this.cmdRT104232.SecFieldName = null;
            this.cmdRT104232.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdRT104232.SelectPaging = false;
            this.cmdRT104232.SelectTop = 0;
            this.cmdRT104232.SiteControl = false;
            this.cmdRT104232.SiteFieldName = null;
            this.cmdRT104232.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustHardware)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCustHardware)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT104231)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT104232)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTLessorAVSCustHardware;
        private Srvtools.UpdateComponent ucRTLessorAVSCustHardware;
        private Srvtools.InfoCommand View_RTLessorAVSCustHardware;
        private Srvtools.InfoCommand cmdRT104231;
        private Srvtools.InfoCommand cmdRT104232;
    }
}
