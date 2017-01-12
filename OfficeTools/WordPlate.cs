using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using OfficeTools.RunTime;

namespace OfficeTools
{
    //WordPlate 控件类
    /// <summary>
    /// The class of WordPlate
    /// </summary>
    [ToolboxItem(true)]
    public class WordPlate : OfficePlate
    {
        /// <summary>
        /// Create a new instance of WordPlate
        /// </summary>
        public WordPlate():base()
        {

        }

        /// <summary>
        /// Create a new instance of WordPlate
        /// </summary>
        /// <param name="container">The container of the component</param>
        public WordPlate(System.ComponentModel.IContainer container)
            : this()
        {
            container.Add(this);
        }

        /// <summary>
        /// The template word file
        /// </summary>
        [Category("Infolight"),
        Description("The template word file")]
        public string WordFile
        {
            get { return base.OfficeFile; }
            set { base.OfficeFile = value; }
        }

        /// <summary>
        /// The user-defined Tag palte will use to output
        /// </summary>
        [Category("Infolight"),
        Description("The user-defined Tag palte will use to output")]
        public TagCollections WordTags
        {
            get { return base.Tags; }
            set { base.Tags = value; }
        }

        /// <summary>
        /// The basic function of WordPlate, used to output
        /// </summary>
        /// <param name="Mode">The type of output</param>
        /// <returns>The flag indicates whether output is successful</returns>
        public override bool Output(int Mode)
        {
            if (base.Output(Mode))
            {
                OutputModeType type = this.OutputMode;
                if (Mode == 1)
                {
                    frmOutputMode fom = new frmOutputMode("LaunchWord");
                    if (fom.ShowDialog() == DialogResult.OK)
                    {
                        switch (fom.cbMode.Text)
                        {
                            case "None": type = OutputModeType.None; break;
                            case "LaunchWord": type = OutputModeType.Launch; break;
                            case "Email": type = OutputModeType.Email; break;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                IAutomation wa;
                if (this.PlateMode == PlateModeType.Xml)
                {
                    wa = new WordAutomationXml(this.FilePath, this);
                }
                else
                {
                    wa = new WordAutomationCom(this.FilePath, this);
                }
                frmProgress fp = new frmProgress("Word Plate", wa, type);
                fp.FormClosed += delegate(object sender, FormClosedEventArgs e)
                {
                    if (fp.modetype == OutputModeType.Launch)
                    {
                        Process.Start("winword.exe", "\"" + this.FilePath + "\"");
                    }
                    else if (fp.modetype == OutputModeType.Email)
                    {
                        frmEmail fe = new frmEmail(EmailAddress, this.FilePath, EmailTitle);
                        fe.ShowDialog((Srvtools.InfoForm)this.OwnerComp);
                    }
                };
                fp.Show();
                if (!ShowAction)
                {
                    fp.Hide();
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
