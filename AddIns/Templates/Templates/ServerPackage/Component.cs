using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Reflection;
using Microsoft.Win32;
using System.IO;
using Srvtools;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace TAG_NAMESPACE
{
	/// <summary>
	/// Summary description for Component.
	/// </summary>
    public class Component : DataModule
    {
        private ServiceManager serviceManager;
		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.IContainer components;

		public Component(System.ComponentModel.IContainer container)
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			container.Add(this);
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public Component()
		{
			///
			/// This call is required by the Windows.Forms Designer.
			///
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

        #region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.serviceManager = new Srvtools.ServiceManager(this.components);

		}

		#endregion
	}
}
