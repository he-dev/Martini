using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Martini.Tests.PropertyFactoryTests
{
    [TestClass]
    public class PropertyFactoryTests
    {
        [TestMethod]
        public void CreatePropertyTests()
        {
            var p = PropertyFactory.CreateProperty("foo", "bar", Grammar.DefaultDelimiters);
            Assert.AreEqual("foo", p.Tokens[0]);
            Assert.AreEqual("=", p.Tokens[1]); // todo should be dynamic
            Assert.AreEqual("bar", p.Tokens[2]);
        }
    }
}
