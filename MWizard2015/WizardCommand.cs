//------------------------------------------------------------------------------
// <copyright file="WizardCommand.cs" company="Microsoft">
//     Copyright (c) Microsoft.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE80;
using EnvDTE;
using System.Threading;

namespace MWizard2015
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class WizardCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid MenuGroup = new Guid("7ef19bcd-b381-4211-9f15-c5e7c03bd3e5");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private WizardCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                CommandID menuCommandID = new CommandID(MenuGroup, CommandId);
                EventHandler eventHandler = this.ShowMessageBox;
                MenuCommand menuItem = new MenuCommand(eventHandler, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static WizardCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new WizardCommand(package);
        }

        private EEPWizard FEEPWizard = null;

        /// <summary>
        /// Shows a message box when the menu item is clicked.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void ShowMessageBox(object sender, EventArgs e)
        {
            DTE2 dte = (DTE2)ServiceProvider.GetService(typeof(DTE));
            FEEPWizard = new MWizard2015.EEPWizard(dte, null);

            fmWizardMain MainForm = new fmWizardMain();
            String Result = MainForm.ShowEEPWizard();
            switch (Result)
            {
                case "Server":
                    try
                    {
                        FEEPWizard.ServerWizard.ShowServerWizard();
                    }
                    catch (Exception ex)
                    {
                        WzdUtils.Application_ThreadException(null, new ThreadExceptionEventArgs(ex));
                    }
                    break;
                case "Client":
                    try
                    {
                        FEEPWizard.ClientWizard.ShowClientWizard();
                    }
                    catch (Exception ex)
                    {
                        WzdUtils.Application_ThreadException(null, new ThreadExceptionEventArgs(ex));
                    }
                    break;
                case "Web":
                    try
                    {
                        FEEPWizard.WebWizard.ShowWebClientWizard();
                    }
                    catch (Exception ex)
                    {
                        WzdUtils.Application_ThreadException(null, new ThreadExceptionEventArgs(ex));
                    }
                    break;
                case "NewEmptySolution":
                    try
                    {
                        FEEPWizard.NewEmptySolution.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        WzdUtils.Application_ThreadException(null, new ThreadExceptionEventArgs(ex));
                    }
                    break;
                case "WebReport":
                case "WinReport":
                    try
                    {
                        fmReportTypeSelect frts = new fmReportTypeSelect(Result, dte, null);
                        frts.ShowDialog();
                        //bool isWebRpt = (Result == "WebReport");
                        //frmEEPReport bForm = new frmEEPReport(applicationObject, addInInstance, isWebRpt);
                        //bForm.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        WzdUtils.Application_ThreadException(null, new ThreadExceptionEventArgs(ex));
                    }
                    break;
                case "ServerPath":
                    try
                    {
                        FEEPWizard.ServerPath.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        WzdUtils.Application_ThreadException(null, new ThreadExceptionEventArgs(ex));
                    }
                    break;
                case "JQuery":
                    try
                    {
                        FEEPWizard.JQueryWebFormWizard.ShowJQueryWebForm();
                    }
                    catch (Exception ex)
                    {
                        WzdUtils.Application_ThreadException(null, new ThreadExceptionEventArgs(ex));
                    }
                    break;
                case "JQMobile":
                    try
                    {
                        FEEPWizard.JQMobileFormWizard.ShowJQMobileForm();
                    }
                    catch (Exception ex)
                    {
                        WzdUtils.Application_ThreadException(null, new ThreadExceptionEventArgs(ex));
                    }
                    break;
                case "JQueryToJQMobile":
                    try
                    {
                        FEEPWizard.JQueryToJQMobileWizard.ShowJQueryToJQMobileWebForm();
                    }
                    catch (Exception ex)
                    {
                        WzdUtils.Application_ThreadException(null, new ThreadExceptionEventArgs(ex));
                    }
                    break;
                case "RDLC":
                    try
                    {
                        FEEPWizard.RDLCWizard.ShowRDLCWizard();
                    }
                    catch (Exception ex)
                    {
                        WzdUtils.Application_ThreadException(null, new ThreadExceptionEventArgs(ex));
                    }
                    break;
            }

        }
    }
}
