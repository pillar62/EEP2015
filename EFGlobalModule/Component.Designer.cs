namespace EFGlobalModule
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
            this.cmdMenus = new EFServerTools.EFCommand(this.components);
            this.cmdToDoHis = new EFServerTools.EFCommand(this.components);
            this.cmdSYS_REPORT = new EFServerTools.EFCommand(this.components);
            this.ucSYS_REPORT = new EFServerTools.EFUpdateComponent(this.components);
            this.cmdSDUsers = new EFServerTools.EFCommand(this.components);
            this.ucSDUsers = new EFServerTools.EFUpdateComponent(this.components);
            this.cmdSDGroups = new EFServerTools.EFCommand(this.components);
            this.ucSDGroups = new EFServerTools.EFUpdateComponent(this.components);
            this.cmdUsers = new EFServerTools.EFCommand(this.components);
            this.ucUsers = new EFServerTools.EFUpdateComponent(this.components);
            this.cmdGroups = new EFServerTools.EFCommand(this.components);
            this.ucGroups = new EFServerTools.EFUpdateComponent(this.components);
            // 
            // cmdMenus
            // 
            this.cmdMenus.CommandText = "select value m from Entities.MENUTABLE as m where m.MODULETYPE in {\'W\',\'O\',\'L\'}";
            this.cmdMenus.CommandTimeout = 30;
            this.cmdMenus.CommandType = System.Data.CommandType.Text;
            this.cmdMenus.DataBase = null;
            this.cmdMenus.MetadataFile = "EFModel";
            this.cmdMenus.MultiSetWhere = false;
            this.cmdMenus.SecExcept = null;
            this.cmdMenus.SecFieldName = null;
            this.cmdMenus.SecStyle = EFServerTools.SecurityStyle.None;
            this.cmdMenus.SelectTop = 0;
            this.cmdMenus.ServerModify = true;
            this.cmdMenus.StoreProcedureEntitySet = null;
            this.cmdMenus.UseSystemDB = false;
            // 
            // cmdToDoHis
            // 
            this.cmdToDoHis.CommandText = "Select value s from Entities.SYS_TODOHIS as s";
            this.cmdToDoHis.CommandTimeout = 30;
            this.cmdToDoHis.CommandType = System.Data.CommandType.Text;
            this.cmdToDoHis.DataBase = null;
            this.cmdToDoHis.MetadataFile = "EFModel";
            this.cmdToDoHis.MultiSetWhere = false;
            this.cmdToDoHis.SecExcept = null;
            this.cmdToDoHis.SecFieldName = null;
            this.cmdToDoHis.SecStyle = EFServerTools.SecurityStyle.None;
            this.cmdToDoHis.SelectTop = 0;
            this.cmdToDoHis.ServerModify = true;
            this.cmdToDoHis.StoreProcedureEntitySet = null;
            this.cmdToDoHis.UseSystemDB = false;
            // 
            // cmdSYS_REPORT
            // 
            this.cmdSYS_REPORT.CommandText = "Select value s from Entities.SYS_REPORT as s";
            this.cmdSYS_REPORT.CommandTimeout = 30;
            this.cmdSYS_REPORT.CommandType = System.Data.CommandType.Text;
            this.cmdSYS_REPORT.DataBase = null;
            this.cmdSYS_REPORT.MetadataFile = "EFModel";
            this.cmdSYS_REPORT.MultiSetWhere = false;
            this.cmdSYS_REPORT.SecExcept = null;
            this.cmdSYS_REPORT.SecFieldName = null;
            this.cmdSYS_REPORT.SecStyle = EFServerTools.SecurityStyle.None;
            this.cmdSYS_REPORT.SelectTop = 0;
            this.cmdSYS_REPORT.ServerModify = true;
            this.cmdSYS_REPORT.StoreProcedureEntitySet = null;
            this.cmdSYS_REPORT.UseSystemDB = false;
            // 
            // ucSYS_REPORT
            // 
            this.ucSYS_REPORT.Command = this.cmdSYS_REPORT;
            // 
            // cmdSDUsers
            // 
            this.cmdSDUsers.CommandText = "Select value s from Entities.SYS_SDUSERS as s";
            this.cmdSDUsers.CommandTimeout = 30;
            this.cmdSDUsers.CommandType = System.Data.CommandType.Text;
            this.cmdSDUsers.DataBase = null;
            this.cmdSDUsers.MetadataFile = "EFModel";
            this.cmdSDUsers.MultiSetWhere = false;
            this.cmdSDUsers.SecExcept = null;
            this.cmdSDUsers.SecFieldName = null;
            this.cmdSDUsers.SecStyle = EFServerTools.SecurityStyle.None;
            this.cmdSDUsers.SelectTop = 0;
            this.cmdSDUsers.ServerModify = true;
            this.cmdSDUsers.StoreProcedureEntitySet = null;
            this.cmdSDUsers.UseSystemDB = false;
            // 
            // ucSDUsers
            // 
            this.ucSDUsers.Command = this.cmdSDUsers;
            // 
            // cmdSDGroups
            // 
            this.cmdSDGroups.CommandText = "Select value s from Entities.SYS_SDGROUPS as s";
            this.cmdSDGroups.CommandTimeout = 30;
            this.cmdSDGroups.CommandType = System.Data.CommandType.Text;
            this.cmdSDGroups.DataBase = null;
            this.cmdSDGroups.MetadataFile = "EFModel";
            this.cmdSDGroups.MultiSetWhere = false;
            this.cmdSDGroups.SecExcept = null;
            this.cmdSDGroups.SecFieldName = null;
            this.cmdSDGroups.SecStyle = EFServerTools.SecurityStyle.None;
            this.cmdSDGroups.SelectTop = 0;
            this.cmdSDGroups.ServerModify = true;
            this.cmdSDGroups.StoreProcedureEntitySet = null;
            this.cmdSDGroups.UseSystemDB = false;
            // 
            // ucSDGroups
            // 
            this.ucSDGroups.Command = this.cmdSDGroups;
            // 
            // cmdUsers
            // 
            this.cmdUsers.CommandText = "Select value u from Entities.USERS as u";
            this.cmdUsers.CommandTimeout = 30;
            this.cmdUsers.CommandType = System.Data.CommandType.Text;
            this.cmdUsers.DataBase = null;
            this.cmdUsers.MetadataFile = "EFModel";
            this.cmdUsers.MultiSetWhere = false;
            this.cmdUsers.SecExcept = null;
            this.cmdUsers.SecFieldName = null;
            this.cmdUsers.SecStyle = EFServerTools.SecurityStyle.None;
            this.cmdUsers.SelectTop = 0;
            this.cmdUsers.ServerModify = true;
            this.cmdUsers.StoreProcedureEntitySet = null;
            this.cmdUsers.UseSystemDB = false;
            // 
            // ucUsers
            // 
            this.ucUsers.Command = this.cmdUsers;
            this.ucUsers.BeforeInsert += new EFServerTools.EFUpdateComponentUpdateEventHandler(this.ucUsers_BeforeInsert);
            // 
            // cmdGroups
            // 
            this.cmdGroups.CommandText = "Select value g from Entities.GROUPS as g";
            this.cmdGroups.CommandTimeout = 30;
            this.cmdGroups.CommandType = System.Data.CommandType.Text;
            this.cmdGroups.DataBase = null;
            this.cmdGroups.MetadataFile = "EFModel";
            this.cmdGroups.MultiSetWhere = false;
            this.cmdGroups.SecExcept = null;
            this.cmdGroups.SecFieldName = null;
            this.cmdGroups.SecStyle = EFServerTools.SecurityStyle.None;
            this.cmdGroups.SelectTop = 0;
            this.cmdGroups.ServerModify = true;
            this.cmdGroups.StoreProcedureEntitySet = null;
            this.cmdGroups.UseSystemDB = false;
            // 
            // ucGroups
            // 
            this.ucGroups.Command = this.cmdGroups;

        }

        #endregion

        private EFServerTools.EFCommand cmdMenus;
        private EFServerTools.EFCommand cmdToDoHis;
        private EFServerTools.EFCommand cmdSYS_REPORT;
        private EFServerTools.EFUpdateComponent ucSYS_REPORT;
        private EFServerTools.EFCommand cmdSDUsers;
        private EFServerTools.EFUpdateComponent ucSDUsers;
        private EFServerTools.EFCommand cmdSDGroups;
        private EFServerTools.EFUpdateComponent ucSDGroups;
        private EFServerTools.EFCommand cmdUsers;
        private EFServerTools.EFUpdateComponent ucUsers;
        private EFServerTools.EFCommand cmdGroups;
        private EFServerTools.EFUpdateComponent ucGroups;
    }
}
