using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Martini.Tests.IniFileTests
{
    [TestClass]
    public class ctor
    {
        [TestMethod]
        public void CreatesIniFile()
        {
            var iniFile = new IniFile(new Sentence());
            Assert.IsNull(iniFile["foo"]);
            Assert.IsFalse(iniFile.Sections.Any());

            IniSection sec = ((dynamic)iniFile).foo;
            Assert.IsNull(sec);
        }
    }

    [TestClass]
    public class AddSection
    {
        [TestMethod]
        public void AddsSection()
        {
            var iniFile = new IniFile(new Sentence());

            var s1 = iniFile.AddSection("foo");
            Assert.AreEqual(s1, iniFile["foo"]);
            Assert.AreEqual(s1, ((dynamic)iniFile).foo);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateSectionsException))]
        public void DisallowsDuplicateSection()
        {
            var iniFile = new IniFile(new Sentence(), new IniSettings());
            iniFile.AddSection("foo");
            iniFile.AddSection("foo");
        }

        [TestMethod]
        public void AllowsDuplicateSection()
        {
            var iniFile = new IniFile(new Sentence(), new IniSettings
            {
                DuplicateSectionHandling = DuplicateSectionHandling.Allow
            });

            iniFile.AddSection("foo");
            iniFile.AddSection("foo");

            Assert.IsTrue(iniFile.Sections.Count() == 2);
            Assert.IsTrue(iniFile.Sections.Count(x => x == "foo") == 2);
        }

        [TestMethod]
        public void MergesDuplicateSection()
        {
            var iniFile = new IniFile(new Sentence(), new IniSettings
            {
                DuplicateSectionHandling = DuplicateSectionHandling.Merge
            });

            iniFile.AddSection("foo");
            iniFile.AddSection("foo");

            Assert.IsTrue(iniFile.Sections.Count() == 1);
            Assert.IsTrue(iniFile.Sections.Count(x => x == "foo") == 1);
        }

        
    }
}
