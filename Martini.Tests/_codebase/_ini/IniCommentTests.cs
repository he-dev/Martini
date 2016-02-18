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
}
