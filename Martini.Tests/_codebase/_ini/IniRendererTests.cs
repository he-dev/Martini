using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Martini.Tests.IniRendererTests
{
    [TestClass]
    public class Render
    {
        [TestMethod]
        public void RendersIniWithFormattingOptions()
        {
            var iniFile = new IniFile();
            var bar = iniFile.AddSection("bar");
            bar.AddProperty("baz", "baaz");
            bar.AddProperty("qux", "quux");

            var baar = iniFile.AddSection("baar");
            baar.AddProperty("baaz", "baaaz");
            baar.AddProperty("quux", "quuux");

            var iniText = IniRenderer.Render(iniFile.GlobalSection, FormattingOptions.None);
        }
    }

    [TestClass]
    public class RenderComment
    {
        [TestMethod]
        public void RendersCommentWithFormattingOptions()
        {
            var comment = CommentFactory.CreateComment("foo", Grammar.DefaultDelimiters);

            Assert.AreEqual(";foo", IniRenderer.RenderComments(new[] { comment }, FormattingOptions.None));
            Assert.AreEqual("; foo", IniRenderer.RenderComments(new[] { comment }, FormattingOptions.SpaceAfterCommentIndicator));
        }

        [TestMethod]
        public void RendersCommentWithFormattingOptionsAndVariousCommentIndicators()
        {
            var settings = new IniSettings
            {
                CommentIndicator = CommentIndicator.NumberSign
            };
            var comment = CommentFactory.CreateComment("foo", settings.Delimiters);

            Assert.AreEqual("#foo", IniRenderer.RenderComments(new[] { comment }, FormattingOptions.None));
            Assert.AreEqual("# foo", IniRenderer.RenderComments(new[] { comment }, FormattingOptions.SpaceAfterCommentIndicator));
        }
    }

    [TestClass]
    public class RenderSection
    {
        [TestMethod]
        public void RendersTextWithFormattingOptions()
        {
            var section = SectionFactory.CreateSection("foo", Grammar.DefaultDelimiters);

            Assert.AreEqual("[foo]", IniRenderer.RenderSection(section, FormattingOptions.None));
        }

        [TestMethod]
        public void RendersTextWithFormattingOptionsAndVariousDelimiters()
        {
            var settings = new IniSettings
            {
                SectionDelimiters = SectionDelimiter.AngleBrackets
            };
            var section = SectionFactory.CreateSection("foo", settings.Delimiters);

            Assert.AreEqual("<foo>", IniRenderer.RenderSection(section, FormattingOptions.None));
        }
    }

    [TestClass]
    public class RenderProperty
    {
        [TestMethod]
        public void RendersPropertyWithFormattingOptions()
        {
            var property = PropertyFactory.CreateProperty("foo", "bar", Grammar.DefaultDelimiters);

            Assert.AreEqual("foo=bar", IniRenderer.RenderProperties(new[] { property }, FormattingOptions.None));
            Assert.AreEqual("foo =bar", IniRenderer.RenderProperties(new[] { property }, FormattingOptions.SpaceBeforePropertyValueDelimiter));
            Assert.AreEqual("foo = bar", IniRenderer.RenderProperties(new[] { property }, FormattingOptions.SpaceBeforePropertyValueDelimiter | FormattingOptions.SpaceAfterPropertyValueDelimiter));
        }
    }
}
