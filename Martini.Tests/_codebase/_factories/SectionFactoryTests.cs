using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Martini.Tests.SectionFactoryTests
{
    [TestClass]
    public class SectionFactoryTests
    {
        [TestMethod]
        public void CreateSectionTests()
        {
            var s = SectionFactory.CreateSection("foo");
            Assert.AreEqual("[", s.Tokens[0]);
            Assert.AreEqual("foo", s.Tokens[1]);
            Assert.AreEqual("]", s.Tokens[2]);
        }
    }
}
