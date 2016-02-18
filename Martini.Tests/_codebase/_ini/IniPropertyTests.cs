using System;
using System.Linq;
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
    public class AddComment
    {
        [TestMethod]
        public void AddsComment()
        {
            var s = PropertyFactory.CreateProperty("foo", "bar", Grammar.DefaultDelimiters);
            var p = new IniProperty(s, new IniFile());

            p.AddComment("baz");
            p.AddComment("qux");

            Assert.IsTrue(p.Comments.Count() == 2);
            Assert.IsTrue(p.Comments.ElementAt(0) == "baz");
            Assert.IsTrue(p.Comments.ElementAt(1) == "qux");
        }
    }
}
