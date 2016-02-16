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
            var sentence = SectionFactory.CreateSection("foo");
            var section = new IniSection(sentence, new IniFile(sentence, new IniSettings
            {
                DuplicatePropertyHandling = DuplicatePropertyHandling.Disallow
            }));

            section.AddProperty("bar", "baz");
            section.AddProperty("bar", "qux");
        }

        [TestMethod]
        public void AllowsDuplicateProperties()
        {
            var sentence = SectionFactory.CreateSection("foo");
            var section = new IniSection(sentence, new IniFile(sentence, new IniSettings
            {
                DuplicatePropertyHandling = DuplicatePropertyHandling.Allow
            }));

            section.AddProperty("bar", "baz");
            section.AddProperty("bar", "qux");

            Assert.IsTrue(section.Properties.Count() == 2);
            Assert.IsTrue(section.Properties.Count(x => x.Name.Equals("bar", StringComparison.OrdinalIgnoreCase)) == 2);
        }

        [TestMethod]
        public void KeepsFirstDuplicateProperty()
        {
            var sentence = SectionFactory.CreateSection("foo");
            var section = new IniSection(sentence, new IniFile(sentence, new IniSettings
            {
                DuplicatePropertyHandling = DuplicatePropertyHandling.KeepFirst
            }));

            section.AddProperty("bar", "baz");
            section.AddProperty("bar", "qux");

            Assert.IsTrue(section.Properties.Count() == 1);
            Assert.AreEqual("baz", section["bar"].Value);
        }

        [TestMethod]
        public void KeepsLastDuplicateProperty()
        {
            var sentence = SectionFactory.CreateSection("foo");
            var section = new IniSection(sentence, new IniFile(sentence, new IniSettings
            {
                DuplicatePropertyHandling = DuplicatePropertyHandling.KeepLast
            }));

            section.AddProperty("bar", "baz");
            section.AddProperty("bar", "qux");

            Assert.IsTrue(section.Properties.Count() == 1);
            Assert.AreEqual("qux", section["bar"].Value);
        }

        [TestMethod]
        public void RenamesDuplicateProperty()
        {
            {
                var sentence = SectionFactory.CreateSection("foo");
                var section = new IniSection(sentence, new IniFile(sentence, new IniSettings
                {
                    DuplicatePropertyHandling = DuplicatePropertyHandling.Rename
                }));

                section.AddProperty("bar", "baz");
                section.AddProperty("bar", "qux");

                Assert.IsTrue(section.Properties.Count() == 2);
                Assert.AreEqual("baz", section["bar"].Value);
                Assert.AreEqual("qux", section["bar2"].Value);
            }
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
