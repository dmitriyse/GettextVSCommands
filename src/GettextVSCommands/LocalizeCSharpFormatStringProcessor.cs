// <copyright file="LocalizeCSharpFormatStringProcessor.cs" company="GettextVSCommands project">
// Copyright (c) GettextVSCommands project. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.
// </copyright>
namespace GettextVSCommands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Localizes C# format string.
    /// </summary>
    internal class LocalizeCSharpFormatStringProcessor
    {
        /// <summary>
        /// CSharp format string parameter pattern.
        /// </summary>
        internal static readonly Regex CSharpFormatParamPattern = new Regex(
            "\\{(?<index>\\d+(\\:[^\\}]*)?)\\}", 
            RegexOptions.Compiled);

        private readonly GettextConfig _gettextConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizeCSharpFormatStringProcessor"/> class.
        /// </summary>
        /// <param name="gettextConfig">Gettext config.</param>
        public LocalizeCSharpFormatStringProcessor(GettextConfig gettextConfig)
        {
            _gettextConfig = gettextConfig;
        }

        /// <summary>
        /// Performs format string localization.
        /// </summary>
        /// <param name="inputText">Text, that contains format string.</param>
        public string Process(string inputText)
        {
            var startDoubleQuotePos = inputText.IndexOf('"');
            if (startDoubleQuotePos == -1)
            {
                return inputText;
            }

            ////if (startDoubleQuote > 0)
            ////{
            ////    // TODO: add support for verbatim string text.
            ////    if (inputText[startDoubleQuote - 1] == '@')
            ////    {
            ////        return inputText;
            ////    }
            ////}
            int? endDoubleQuotePos = null;

            for (var pos = startDoubleQuotePos; pos < inputText.Length; pos++)
            {
                if (inputText[pos] == '"')
                {
                    if (pos != startDoubleQuotePos)
                    {
                        if (inputText[pos - 1] != '"' && inputText[pos - 1] != '\\')
                        {
                            endDoubleQuotePos = pos;
                            break;
                        }

                        if (inputText[pos - 1] == '"')
                        {
                            if (pos - 2 > startDoubleQuotePos && inputText[pos - 2] == '\\')
                            {
                                endDoubleQuotePos = pos;
                                break;
                            }
                        }
                    }
                }
            }

            if (endDoubleQuotePos == null)
            {
                return inputText;
            }

            // Writing before string part
            var result = new StringBuilder();
            result.Append(inputText.Substring(0, startDoubleQuotePos + 1));

            // Writing localized start pattern
            result.Append(_gettextConfig.StartPattern);

            // Writing sring with replaced parameters
            var stringContent = inputText.Substring(
                startDoubleQuotePos + 1, 
                endDoubleQuotePos.Value - startDoubleQuotePos - 1);

            var replacedContent = CSharpFormatParamPattern.Replace(stringContent, "%${index}");
            result.Append(replacedContent);

            var matches = CSharpFormatParamPattern.Matches(inputText);
            var parameters = new List<Tuple<int, Match>>();
            for (var i = 0; i < matches.Count; i++)
            {
                int indexInt;
                if (!int.TryParse(matches[i].Groups["index"].Value, out indexInt))
                {
                    return inputText;
                }

                parameters.Add(new Tuple<int, Match>(indexInt, matches[i]));
            }

            parameters = parameters.OrderBy(x => x.Item1).ToList();
            foreach (var parameter in parameters)
            {
                result.Append(_gettextConfig.ParamsDelimiterPattern);
                result.Append(parameter.Item2.Value);
            }

            result.Append(_gettextConfig.EndPattern);

            result.Append(inputText.Substring(endDoubleQuotePos.Value));
            return result.ToString();
        }
    }
}