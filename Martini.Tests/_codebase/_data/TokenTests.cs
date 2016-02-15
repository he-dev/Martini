using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Martini.Tests.TokenTests
{
    [TestClass]
    public class TokenTests
    {
        [TestMethod]
        public void EqualityOperatorTokenStringTests()
        {
            Assert.IsTrue(new Token("foo") == "foo");
            Assert.IsTrue(new Token("foo") == "Foo");
            Assert.IsFalse(new Token("foo") == "Bar");
            Assert.IsFalse(new Token("foo") == (string)null);
        }
    }
}
