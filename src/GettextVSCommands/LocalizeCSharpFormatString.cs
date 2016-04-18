// <copyright file="LocalizeCSharpFormatString.cs" company="GettextVSCommands project">
// Copyright (c) GettextVSCommands project. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.
// </copyright>
namespace GettextVSCommands
{
    using System;
    using System.ComponentModel.Design;
    using System.Globalization;

    using EnvDTE;

    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class LocalizeCSharpFormatString
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("905f3286-352b-4d1d-a418-18cc64c061c3");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        private DTE _dte;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizeCSharpFormatString"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private LocalizeCSharpFormatString(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            _dte = (DTE)ServiceProvider.GetService(typeof(SDTE));
            var commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static LocalizeCSharpFormatString Instance { get; private set; }

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
            Instance = new LocalizeCSharpFormatString(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            var document = _dte.ActiveDocument;
            var selection = document.Selection as TextSelection;
            if (selection != null)
            {
                if (selection.Text != null)
                {
                    // TODO: Implement config gathering.
                    var processor = new LocalizeCSharpFormatStringProcessor(new GettextConfig());
                    var replaced = processor.Process(selection.Text);
                    selection.Text = replaced;
                }
            }

            ////var title = "LocalizeCSharpFormatString";
            ////var message = string.Format(
            ////    CultureInfo.CurrentCulture,
            ////    "Inside {0}.MenuItemCallback()",
            ////    this.GetType().FullName);
            //// Show a message box to prove we were here
            //// VsShellUtilities.ShowMessageBox(
            //// this.ServiceProvider,
            //// message,
            //// title,
            //// OLEMSGICON.OLEMSGICON_INFO,
            //// OLEMSGBUTTON.OLEMSGBUTTON_OK,
            //// OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
    }
}