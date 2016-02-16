using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Martini.Tests.IniSectionTests
{
    [TestClass]
    public class ctor
    {
        [TestMethod]
        public void CreatesSection()
        {
            var sentence = SectionFactory.CreateSection("foo");
            var section = new IniSection(sentence, null);

            Assert.AreEqual("foo", section.Name);
            Assert.IsTrue(!section.Comments.Any());
            Assert.IsTrue(!section.Properties.Any());
        }
    }

    [TestClass]
    public class AddProperty
    {
        [TestMethod]
        public void AddsProperty()
        {
            var sentence = SectionFactory.CreateSection("foo");
            var section = new IniSection(sentence, null);

            Assert.AreEqual("foo", section.Name);
            Assert.IsFalse(section.Comments.Any());
            Assert.IsFalse(section.Properties.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicatePropertiesException))]
        public void DisallowsDuplicateProperties()
        {

        }

        [TestMethod]
        public void AllowsDuplicateProperties()
        {
        }

        [TestMethod]
        public void KeepsFirstDuplicateProperty()
        {
        }

        [TestMethod]
        public void KeepsLastDuplicateProperty()
        {
        }

        [TestMethod]
        public void RenamesDuplicateProperty()
        {
        }
    }

    [TestClass]
    public class AddComment
    {
        [TestMethod]
        public void AddPropertyTests()
        {
            var sentence = SectionFactory.CreateSection("foo");
            var section = new IniSection(sentence, null);

            Assert.AreEqual("foo", section.Name);
            Assert.IsTrue(!section.Comments.Any());
            Assert.IsTrue(!section.Properties.Any());
        }
    }
}
