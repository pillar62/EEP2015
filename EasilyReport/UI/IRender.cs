using System;
using System.Collections.Generic;
using System.Text;

namespace Infolight.EasilyReportTools.UI
{
    internal interface IRender
    {
        void CreateChildControls();
        void InitialView(WebEasilyReport report);
    }
}
