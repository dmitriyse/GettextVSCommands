// <copyright file="GettextConfig.cs" company="GettextVSCommands project">
// Copyright (c) GettextVSCommands project. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.
// </copyright>
namespace GettextVSCommands
{
    /// <summary>
    /// Configuration for the gettext
    /// </summary>
    public class GettextConfig
    {
        /// <summary>
        /// Localized string start pattern.
        /// </summary>
        public string StartPattern { get; set; } = "[[[";

        /// <summary>
        /// Localized string end pattern.
        /// </summary>
        public string EndPattern { get; set; } = "]]]";

        /// <summary>
        /// Parameters delimiter pattern.
        /// </summary>
        public string ParamsDelimiterPattern { get; set; } = "|||";
    }
}