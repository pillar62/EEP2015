using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EFBase.Design;

namespace EFServerTools.Design.EFCommandDesign
{
    public partial class EFParameterEditor: PropertyModalEditor
    {
        public override ModalForm GetModalForm(ITypeDescriptorContext context)
        {
            return new EFParameterEditorDialog();
        }
    }
}
