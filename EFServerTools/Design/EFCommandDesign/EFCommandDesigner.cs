using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using EFBase.Design;

namespace EFServerTools.Design.EFCommandDesign
{
    /// <summary>
    /// Designer of EFCommand
    /// </summary>
    internal class EFCommandDesigner : ComponentDesigner
    {
        /// <summary>
        /// Default event on the component and navigates the user's cursor to that location.
        /// </summary>
        public override void DoDefaultAction()
        {
            if (this.Component is EFCommand)
            {
                try
                {
                    var command = this.Component as EFCommand;
                    var editor = new EFCommandEditor(DTE.CurrentDirectory, command.CommandText, command.MetadataFile);
                    if (editor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        TypeDescriptor.GetProperties(typeof(EFCommand))["CommandText"].SetValue(this.Component, editor.CommandText);
                        TypeDescriptor.GetProperties(typeof(EFCommand))["MetadataFile"].SetValue(this.Component, editor.MetadataFile);
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message, "Error"
                        , System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }
    }
}
