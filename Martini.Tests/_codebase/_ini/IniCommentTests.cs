using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Martini.Tests.IniCommentTests
{
    [TestClass]
    public class IniCommentTests
    {
        [TestMethod]
        public void ctorTests()
        {
            var s = CommentFactory.CreateComment("foo");
            var c = new IniComment(s);

            Assert.AreEqual("foo", c.Text);
            c.Text = "bar";
            Assert.AreEqual("bar", c.Text);
        }
    }
}
