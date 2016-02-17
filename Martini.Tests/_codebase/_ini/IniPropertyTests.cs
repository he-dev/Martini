using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Martini.Tests.IniPropertyTests
{
    [TestClass]
    public class IniPropertyTests
    {
        [TestMethod]
        public void ctorTests()
        {
            var s = PropertyFactory.CreateProperty("foo", "bar", Grammar.DefaultDelimiters);
            var p = new IniProperty(s, null);

            Assert.AreEqual("foo", p.Name);
            Assert.AreEqual("bar", p.Value);
        }
    }
}
