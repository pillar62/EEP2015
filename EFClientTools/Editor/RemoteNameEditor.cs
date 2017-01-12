using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace EFClientTools.Editor
{
    public class RemoteNameEditor : ModalSelectorEditor
    {
        protected override EditorDialog CreateDialog(Control control, ITypeDescriptorContext context)
        {
            string remoteName = string.Empty;
            if (control is IEFDataSource)
            {
                remoteName = ((IEFDataSource)control).RemoteName;
            }
            return new RemoteNameEditorDialog(remoteName);
        }
    }
}
