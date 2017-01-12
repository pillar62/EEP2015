using System;
using System.Collections.Generic;
using System.Text;

namespace Infolight.EasilyReportTools.UI
{
    internal class ExtJsRender: IRender
    {
        private WebEasilyReport report;

        public ExtJsRender(WebEasilyReport _report)
        {
            report = _report;
        }

        #region IRender Members

        public void CreateChildControls()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void InitialView(WebEasilyReport report)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
