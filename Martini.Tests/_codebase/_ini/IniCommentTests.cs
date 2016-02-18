using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Martini.Tests.IniCommentTests
{
    [TestClass]
    public class ctor
    {
        [TestMethod]
        public void CreatesComment()
        {
            var s = CommentFactory.CreateComment("foo", Grammar.DefaultDelimiters);
            var c = new IniComment(s, null);

            Assert.AreEqual("foo", c.Text);
            c.Text = "bar";
            Assert.AreEqual("bar", c.Text);
        }
    }

    [TestClass]
    public class Render
    {
        [TestMethod]
        public void RendersTextWithFormattingOptions()
        {
            var s = CommentFactory.CreateComment("foo", Grammar.DefaultDelimiters);
            var c = new IniComment(s, null);

            Assert.AreEqual(";foo", c.Render(FormattingOptions.None));
            Assert.AreEqual("; foo", c.Render(FormattingOptions.SpaceAfterCommentIndicator));
        }

        [TestMethod]
        public void RendersTextWithFormattingOptionsAndVariousCommentIndicators()
        {
            var settings = new IniSettings
            {
                CommentIndicator = CommentIndicator.NumberSign
            };
            var s = CommentFactory.CreateComment("foo", settings.Delimiters);
            var c = new IniComment(s, null);

            Assert.AreEqual("#foo", c.Render(FormattingOptions.None));
            Assert.AreEqual("# foo", c.Render(FormattingOptions.SpaceAfterCommentIndicator));
        }
    }
}
