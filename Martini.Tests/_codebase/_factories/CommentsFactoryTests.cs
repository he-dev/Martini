using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Martini.Tests.CommentFactoryTests
{
    [TestClass]
    public class CommentFactoryTests
    {
        [TestMethod]
        public void CommentCommentTests()
        {
            var s = CommentFactory.CreateComment("foo", Grammar.DefaultDelimiters);
            Assert.AreEqual(";", s.Tokens[0]);
            Assert.AreEqual("foo", s.Tokens[1]);
        }
    }
}
