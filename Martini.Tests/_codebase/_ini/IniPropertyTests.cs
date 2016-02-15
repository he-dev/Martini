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
            var s = PropertyFactory.CreateProperty("foo", "bar");
            var p = new IniProperty(s);

            Assert.AreEqual("foo", p.Name);
            Assert.AreEqual("bar", p.Value);
        }
    }
}
