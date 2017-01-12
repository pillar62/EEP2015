using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using OfficeTools.RunTime;

namespace OfficeTools
{
    /// <summary>
    /// The class of ExcelPlate
    /// </summary>
    [ToolboxItem(true)]
    public class ExcelPlate : OfficePlate
    {
        /// <summary>
        /// Create a new instance of ExcelPlate
        /// </summary>
        public ExcelPlate()
            : base()
        {

        }

        /// <summary>
        /// Create a new instance of ExcelPlate
        /// </summary>
        /// <param name="container">The container of the component</param>
        public ExcelPlate(System.ComponentModel.IContainer container)
            : this()
        {
            container.Add(this);
        }

        /// <summary>
        /// The template excel file
        /// </summary>
        [Category("Infolight"),
        Description("The template excel file")]
        public string ExcelFile
        {
            get { return base.OfficeFile; }
            set { base.OfficeFile = value; }
        }

        /// <summary>
        /// The user-defined Tag palte will use to output
        /// </summary>
        [Category("Infolight"),
        Description("The user-defined Tag palte will use to output")]
        public TagCollections ExcelTags
        {
            get { return base.Tags; }
            set { base.Tags = value; }
        }

        /// <summary>
        /// The basic function of ExcelPlate, used to output
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
                    frmOutputMode fom = new frmOutputMode("LaunchExcel");
                    if (fom.ShowDialog() == DialogResult.OK)
                    {
                        switch (fom.cbMode.Text)
                        {
                            case "None": type = OutputModeType.None; break;
                            case "LaunchExcel": type = OutputModeType.Launch; break;
                            case "Email": type = OutputModeType.Email; break;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                IAutomation ea;
                if (this.PlateMode == PlateModeType.Xml)
                {
                    ea = new ExcelAutomationXml(this.FilePath, this);
                }
                else
                {
                    ea = new ExcelAutomationCom(this.FilePath, this);
                }
                frmProgress fp = new frmProgress("Excel Plate", ea, type);
                fp.FormClosed += delegate(object sender, FormClosedEventArgs e)
                {
                    if (fp.modetype == OutputModeType.Launch)
                    {
                        Process.Start("excel.exe", "\"" + this.FilePath + "\"");
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
