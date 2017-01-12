using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Drawing;

namespace Srvtools
{
    //重写
    [ToolboxBitmap(typeof(InfoStatusStrip), "Resources.InfoStatusStrip.ico")]
    public partial class InfoStatusStrip : StatusStrip
    {
        //public ToolStripLabel[] itemlist = new ToolStripLabel[7];
        //private SYS_LANGUAGE language;

        public InfoStatusStrip()
        {
            ////language = CliSysMegLag.GetClientLanguage();
            //language = CliUtils.fClientLang;
            InitializeComponent();
        }

        public InfoStatusStrip(IContainer container)
        {
            ////language = CliSysMegLag.GetClientLanguage();
            //language = CliUtils.fClientLang;
            container.Add(this);
            InitializeComponent();
        }

        private string[] statustext 
            = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "InfoStatusStrip", "StripMsg").Split(';');

        private enum StatusType
        {
            NavigatorStatus= 0,
            UserID = 1,
            UserName = 2,
            Date = 3,
            EEPAlias = 4,
            Solution = 5,
            Company = 6,
            Count = 7,
            SolutionName = 8,
        }

        public void SetNavigatorStatus(string text)
        {
            if (this.Items[StatusType.NavigatorStatus.ToString()] != null)
            {
                this.Items[StatusType.NavigatorStatus.ToString()].Text = text;
            }
        }

        private ToolStripLabel CreateLabel(StatusType type, string text)
        {
            ToolStripStatusLabel label = new ToolStripStatusLabel();
            label.Name = type.ToString();
            if ((int)type > 0)
            {
                label.Text = statustext[(int)type - 1] + text;
            }
            else
            {
                label.Text = text;
            }
            label.BorderSides = ToolStripStatusLabelBorderSides.All;
            label.BorderStyle = Border3DStyle.SunkenOuter;
            return label;
        }

        private void SetItem(StatusType type, string text, bool visible)
        {
            string itemname = type.ToString();
            if (visible)
            {
                if (this.Items[itemname] == null)
                {
                    int index = this.Items.Count;
                    for (int i = 0; i < this.Items.Count; i++)
                    {
                        try
                        {
                            StatusType itemtype = (StatusType)Enum.Parse(typeof(StatusType), this.Items[i].Name);
                            if ((int)type < (int)itemtype)
                            {
                                index = i;
                                break;
                            }
                        }
                        catch
                        {
                            index = i;
                            break;
                        }
                    }
                    this.Items.Insert(index, CreateLabel(type, text));
                }
                this.Items[itemname].Visible = true;
            }
            else
            {
                if (this.Items[itemname] != null)
                {
                    this.Items[itemname].Visible = false;
                }
            }
        }

        private bool _ShowNavigatorStatus;
        [Category("Infolight"),
        Description("Indicates whether the infomation of status of InfoNavigator is displayed on InfoStatusStrip")]
        public bool ShowNavigatorStatus
        {
            get { return _ShowNavigatorStatus; }
            set
            {
                _ShowNavigatorStatus = value;
                SetItem(StatusType.NavigatorStatus, string.Empty, _ShowNavigatorStatus);
            }
        }
        
        private bool _ShowUserID;
        [Category("Infolight"),
        Description("Indicates whether the infomation of user's id is displayed on InfoStatusStrip")]
        public bool ShowUserID
        {
            get { return _ShowUserID; }
            set
            {
                _ShowUserID = value;
                SetItem(StatusType.UserID, CliUtils.fLoginUser, _ShowUserID);
            }
        }
    
        private bool _ShowUserName;
        [Category("Infolight"),
        Description("Indicates whether the infomation of user's name is displayed on InfoStatusStrip")]
        public bool ShowUserName
        {
            get { return _ShowUserName; }
            set
            {
                _ShowUserName = value;
                SetItem(StatusType.UserName, CliUtils.fUserName, _ShowUserName);
            }
        }

        private bool _ShowDate;
        [Category("Infolight"),
        Description("Indicates whether the infomation of date is displayed on InfoStatusStrip")]
        public bool ShowDate
        {
            get { return _ShowDate; }
            set
            {
                _ShowDate = value;
                SetItem(StatusType.Date, DateTime.Today.ToShortDateString(), _ShowDate);
            }
        }

        private bool _ShowEEPAlias;
        [Category("Infolight"),
        Description("Indicates whether the infomation of database is displayed on InfoStatusStrip")]
        public bool ShowEEPAlias
        {
            get { return _ShowEEPAlias; }
            set
            {
                _ShowEEPAlias = value;
                SetItem(StatusType.EEPAlias, CliUtils.fLoginDB, _ShowEEPAlias);
            }
        }
        
        private bool _ShowSolution;
        [Category("Infolight"),
        Description("Indicates whether the infomation of solution is displayed on InfoStatusStrip")]
        public bool ShowSolution
        {
            get { return _ShowSolution; }
            set
            {
                _ShowSolution = value;
                SetItem(StatusType.Solution, CliUtils.fCurrentProject, _ShowSolution);
            }
        }

        private bool _ShowSolutionName;
        [Category("Infolight"),
        Description("Indicates whether the infomation of solution is displayed on InfoStatusStrip")]
        public bool ShowSolutionName
        {
            get { return _ShowSolutionName; }
            set
            {
                _ShowSolutionName = value;
                SetItem(StatusType.SolutionName, CliUtils.fCurrentProjectName, _ShowSolutionName);
            }
        }

        private bool _ShowCompany;
        [Category("Infolight"),
        Description("Indicates whether the infomation of company is displayed on InfoStatusStrip")]
        public bool ShowCompany
        {
            get { return _ShowCompany; }
            set
            {
                _ShowCompany = value;
                SetItem(StatusType.Company, CliUtils.fSiteCode, _ShowCompany);
            }
        }

        private bool _ShowProgress;
        [Category("Infolight")]
        public bool ShowProgress
        {
            get { return _ShowProgress; }
            set { _ShowProgress = value; }
        }

        private ToolStripLabel labelprogress;

        public void ShowProgressBar()
        {
            if (labelprogress == null)
            {
                this.ImageScalingSize = new Size(100, 16);
                Assembly assembly = this.GetType().Assembly;
                Bitmap bmp = new Bitmap(assembly.GetManifestResourceStream("Srvtools.Resources.progress.gif"));
                labelprogress = new ToolStripLabel(string.Empty, bmp);
                labelprogress.AutoSize = false;
                labelprogress.Size = new Size(100, 16);
                this.Items.Add(labelprogress);
            }
            else
            {
                labelprogress.Visible = true;
            }

            //ToolStripButton
        }

        public void HideProgressBar()
        {
            if (labelprogress != null)
            {
                labelprogress.Visible = false;
            }
        }
    }
}
