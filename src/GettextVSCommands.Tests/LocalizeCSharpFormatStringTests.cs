// <copyright file="LocalizeCSharpFormatStringTests.cs" company="GettextVSCommands project">
// Copyright (c) GettextVSCommands project. All rights reserved.
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.
// </copyright>
namespace GettextVSCommands.Tests
{
    using FluentAssertions;

    using NUnit.Framework;

    /// <summary>
    /// <see cref="LocalizeCSharpFormatStringProcessor"/> class tests.
    /// </summary>
    [TestFixture]
    public class LocalizeCSharpFormatStringTests
    {
        /// <summary>
        /// Localize replacement test.
        /// </summary>
        [Test]
        [TestCase(@" asdf "" This is the {0} item""   ", @" asdf ""[[[ This is the %0 item|||{0}]]]""   ")]
        public void LocalizeTest(string inputText, string expectedReplacement)
        {
            var processor = new LocalizeCSharpFormatStringProcessor(new GettextConfig());
            var result = processor.Process(inputText);
            result.Should().Be(expectedReplacement);
        }
    }
}