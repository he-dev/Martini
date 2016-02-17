using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Martini.Tests.IniPropertyTests
{
    [TestClass]
    public class ctor
    {
        [TestMethod]
        public void CreatesProperty()
        {
            var s = PropertyFactory.CreateProperty("foo", "bar", Grammar.DefaultDelimiters);
            var p = new IniProperty(s, null);

            Assert.AreEqual("foo", p.Name);
            Assert.AreEqual("bar", p.Value);
        }
    }

    [TestClass]
    public class Render
    {
        [TestMethod]
        public void RendersTextWithFormattingOptions()
        {
            var s = PropertyFactory.CreateProperty("foo", "bar", Grammar.DefaultDelimiters);
            var p = new IniProperty(s, null);

            Assert.AreEqual("foo=bar", p.Render(FormattingOptions.None));
            Assert.AreEqual("foo =bar", p.Render(FormattingOptions.SpaceBeforePropertyValueDelimiter));
            Assert.AreEqual("foo = bar", p.Render(FormattingOptions.SpaceBeforePropertyValueDelimiter | FormattingOptions.SpaceAfterPropertyValueDelimiter));
        }
    }
}
