using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace OfficeTools
{
    /// <summary>
    /// The class of WebWordPlate
    /// </summary>
    public class WebWordPlate: WebOfficePlate
    {
        /// <summary>
        /// Create a new instance of WebOfficePlate
        /// </summary>
        public WebWordPlate()
        {

        }

        /// <summary>
        /// The template word file
        /// </summary>
        [Category("Infolight"),
        Description("The template excel file")]
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
        }

        /// <summary>
        /// The basic function of WordPlate, used to output
        /// </summary>
        /// <returns>The flag indicates whether output is successful</returns>
        public override bool Output(int Mode)
        {
            base.Output(Mode);
            IAutomation ea;
            if (this.PlateMode == PlateModeType.Xml)
            {
                ea = new WordAutomationXml(Page.Server.MapPath(this.FilePath), this);
            }
            else
            {
                ea = new WordAutomationCom(Page.Server.MapPath(this.FilePath), this);
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
