using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Web.UI.Design;
using System.ComponentModel.Design;

namespace OfficeTools
{
    /// <summary>
    /// The class of WebExcelPlate
    /// </summary>
    public class WebExcelPlate: WebOfficePlate
    {
        /// <summary>
        /// Create a new instance of WebExcelPlate
        /// </summary>
        public WebExcelPlate()
        { 
        
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
        }

        /// <summary>
        /// The basic function of ExcelPlate, used to output
        /// </summary>
        /// <returns>The flag indicates whether output is successful</returns>
        public override bool Output(int Mode)
        {
            base.Output(Mode);
            IAutomation ea;
            if (this.PlateMode == PlateModeType.Xml)
            {
                ea = new ExcelAutomationXml(Page.Server.MapPath(this.FilePath), this);
            }
            else
            {
                ea = new ExcelAutomationCom(Page.Server.MapPath(this.FilePath), this);
            }
            try
            {
                (ea as IAutomation).Run();
            }
            finally
            {
                (ea as OfficeAutomation).Plate.OnAfterOutput(new EventArgs());
            }
            return true;
        }
    }
}
