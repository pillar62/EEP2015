using System;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace Srvtools
{
    class infoCommandDesigner: ComponentDesigner
	{
		private CommandTextOptionDialog editor = null;

		public infoCommandDesigner()
		{
		}

		public override void DoDefaultAction()
		{		
            InfoConnection conn = ((InfoCommand)this.Component).InfoConnection;
            if(conn == null || string.IsNullOrEmpty(conn.EEPAlias))
                return;

            editor = new CommandTextOptionDialog(conn.InternalConnection, ((InfoCommand)this.Component).CommandText);
		    editor.ShowDialog();

            TypeDescriptor.GetProperties(typeof(InfoCommand))["CommandText"].SetValue(this.Component, editor.CommandText);
   
            // ((InfoCommand)this.Component).CommandText = editor.CommandText;
            editor.Dispose();

		}
	}
}
