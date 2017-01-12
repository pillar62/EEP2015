using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;

namespace Infolight.EasilyReportTools.Design
{
    public class ReportItemCollectionEditer: UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            return base.EditValue(context, provider, value);
        }
    }
}
