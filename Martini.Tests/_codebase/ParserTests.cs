using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Martini.Tests.ParserTests
{
    [TestClass]
    public class Parse
    {
        [TestMethod]
        public void ParsesBlankLine()
        {
            var sentence = Parser.Parse("   ", new IniSettings());
            Assert.IsTrue(sentence.Next.Type == SentenceType.Blank);
        }

        [TestMethod]
        public void ParsesInvalidLine()
        {
            var sentence = Parser.Parse("foo;bar", new IniSettings());
            Assert.IsTrue(sentence.Next.Type == SentenceType.Invalid);
        }

        [TestMethod]
        public void ParsesComment()
        {
            // skip global section
            var sentence = Parser.Parse(";foo", new IniSettings()).Next;
            Assert.IsTrue(sentence.Type == SentenceType.Comment);
            Assert.IsTrue(sentence.Tokens.Count == 2);
            Assert.IsTrue(sentence.Tokens[0].Type == TokenType.CommentIndicator);
            Assert.IsTrue(sentence.Tokens[1].Type == TokenType.Comment);
            Assert.IsTrue(sentence.Tokens[1].Value == "foo");
        }

        [TestMethod]
        public void ParsesSection()
        {
            var sentence = Parser.Parse("[foo]", new IniSettings()).Next;
            Assert.IsTrue(sentence.Type == SentenceType.Section);
            Assert.IsTrue(sentence.Tokens.Count == 3);
            Assert.IsTrue(sentence.Tokens[0].Type == TokenType.LeftSectionDelimiter);
            Assert.IsTrue(sentence.Tokens[1].Type == TokenType.Section);
            Assert.IsTrue(sentence.Tokens[2].Type == TokenType.RightSectionDelimiter);
            Assert.IsTrue(sentence.Tokens[1].Value == "foo");
        }

        [TestMethod]
        public void ParsesProperty()
        {
            var sentence = Parser.Parse("foo=bar", new IniSettings()).Next;
            Assert.IsTrue(sentence.Type == SentenceType.Property);
            Assert.IsTrue(sentence.Tokens.Count == 3);
            Assert.IsTrue(sentence.Tokens[0].Type == TokenType.Property);
            Assert.IsTrue(sentence.Tokens[1].Type == TokenType.ProperetyValueDelimiter);
            Assert.IsTrue(sentence.Tokens[2].Type == TokenType.Value);
            Assert.IsTrue(sentence.Tokens[0].Value == "foo");
            Assert.IsTrue(sentence.Tokens[2].Value == "bar");
        }

        [TestMethod]
        public void ParsesWellFormedIni()
        {
            var ini = @"
;foo
[bar]
baz=qux
[barbar]
bazbaz=quxqux";

            var sentence = (Sentence)Parser.Parse(ini, new IniSettings()).Next;

            Assert.IsTrue(sentence.After.Count() == 6);
            Assert.IsTrue(sentence.After.ElementAt(0).Type == SentenceType.Blank);
            Assert.IsTrue(sentence.After.ElementAt(1).Type == SentenceType.Comment);
            Assert.IsTrue(sentence.After.ElementAt(2).Type == SentenceType.Section);
            Assert.IsTrue(sentence.After.ElementAt(3).Type == SentenceType.Property);
            Assert.IsTrue(sentence.After.ElementAt(4).Type == SentenceType.Section);
            Assert.IsTrue(sentence.After.ElementAt(5).Type == SentenceType.Property);
        }

        [TestMethod]
        public void AllowsDuplicateSections()
        {
            var ini = @"
;quux
[foo]
bar=baar
[foo]
baz=baaz";

            var sentence = (Sentence)Parser.Parse(ini, new IniSettings
            {
                DuplicateSectionHandling = DuplicateSectionHandling.Allow
            });

            var afterGlobal = sentence.After.Skip(1).ToList();

            Assert.IsTrue(afterGlobal.Count() == 6);
            Assert.IsTrue(afterGlobal.ElementAt(0).Type == SentenceType.Blank);
            Assert.IsTrue(afterGlobal.ElementAt(1).Type == SentenceType.Comment);
            Assert.IsTrue(afterGlobal.ElementAt(2).Type == SentenceType.Section);
            Assert.IsTrue(afterGlobal.ElementAt(3).Type == SentenceType.Property);
            Assert.IsTrue(afterGlobal.ElementAt(4).Type == SentenceType.Section);
            Assert.IsTrue(afterGlobal.ElementAt(5).Type == SentenceType.Property);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateSectionsException))]
        public void DisallowsDuplicateSections()
        {
            var ini = @"
;quux
[foo]
bar=baar
[foo]
baz=baaz";

            var sentence = (Sentence)Parser.Parse(ini, new IniSettings
            {
                DuplicateSectionHandling = DuplicateSectionHandling.Disallow
            });
        }

        [TestMethod]
        public void MergesDuplicateSections()
        {
            var ini = @"
;quux
[foo]
bar=baar
[foo]
baz=baaz";

            var sentence = (Sentence)Parser.Parse(ini, new IniSettings
            {
                DuplicateSectionHandling = DuplicateSectionHandling.Merge
            });

            var afterGlobal = sentence.After.Skip(1).ToList();

            Assert.IsTrue(afterGlobal.Count() == 5);
            Assert.IsTrue(afterGlobal.ElementAt(0).Type == SentenceType.Blank);
            Assert.IsTrue(afterGlobal.ElementAt(1).Type == SentenceType.Comment);
            Assert.IsTrue(afterGlobal.ElementAt(2).Type == SentenceType.Section);
            Assert.IsTrue(afterGlobal.ElementAt(3).Type == SentenceType.Property);
            Assert.IsTrue(afterGlobal.ElementAt(4).Type == SentenceType.Property);
        }

        [TestMethod]
        public void AllowsDuplicateProperties()
        {
            var ini = @"
;quux
[foo]
bar=baar
bar=baaz";

            var sentence = (Sentence)Parser.Parse(ini, new IniSettings
            {
                DuplicatePropertyHandling = DuplicatePropertyHandling.Allow
            });

            var afterGlobal = sentence.After.Skip(1).ToList();

            Assert.IsTrue(afterGlobal.Count() == 5);
            Assert.IsTrue(afterGlobal.ElementAt(0).Type == SentenceType.Blank);
            Assert.IsTrue(afterGlobal.ElementAt(1).Type == SentenceType.Comment);
            Assert.IsTrue(afterGlobal.ElementAt(2).Type == SentenceType.Section);
            Assert.IsTrue(afterGlobal.ElementAt(3).Type == SentenceType.Property);
            Assert.IsTrue(afterGlobal.ElementAt(4).Type == SentenceType.Property);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicatePropertiesException))]
        public void DisallowsDuplicateProperties()
        {
            var ini = @"
;quux
[foo]
bar=baar
bar=baaz";

            var sentence = (Sentence)Parser.Parse(ini, new IniSettings
            {
                DuplicatePropertyHandling = DuplicatePropertyHandling.Disallow
            });            
        }

        [TestMethod]
        public void KeepsFirstDuplicateProperty()
        {
            var ini = @"
;quux
[foo]
bar=baar
bar=baaz";

            var sentence = (Sentence)Parser.Parse(ini, new IniSettings
            {
                DuplicatePropertyHandling = DuplicatePropertyHandling.KeepFirst
            });

            var afterGlobal = sentence.After.Skip(1).ToList();

            Assert.IsTrue(afterGlobal.Count() == 4);
            Assert.IsTrue(afterGlobal.ElementAt(0).Type == SentenceType.Blank);
            Assert.IsTrue(afterGlobal.ElementAt(1).Type == SentenceType.Comment);
            Assert.IsTrue(afterGlobal.ElementAt(2).Type == SentenceType.Section);
            Assert.IsTrue(afterGlobal.ElementAt(3).Type == SentenceType.Property);
            Assert.IsTrue(afterGlobal.ElementAt(3).Tokens.ValueToken() == "baar");
        }

        [TestMethod]
        public void KeepsLastDuplicateProperty()
        {
            var ini = @"
;quux
[foo]
bar=baar
bar=baaz";

            var sentence = (Sentence)Parser.Parse(ini, new IniSettings
            {
                DuplicatePropertyHandling = DuplicatePropertyHandling.KeepLast
            });

            var afterGlobal = sentence.After.Skip(1).ToList();

            Assert.IsTrue(afterGlobal.Count() == 4);
            Assert.IsTrue(afterGlobal.ElementAt(0).Type == SentenceType.Blank);
            Assert.IsTrue(afterGlobal.ElementAt(1).Type == SentenceType.Comment);
            Assert.IsTrue(afterGlobal.ElementAt(2).Type == SentenceType.Section);
            Assert.IsTrue(afterGlobal.ElementAt(3).Type == SentenceType.Property);
            Assert.IsTrue(afterGlobal.ElementAt(3).Tokens.ValueToken() == "baaz");
        }

        [TestMethod]
        public void RenamesDuplicateProperty()
        {
            var ini = @"
;quux
[foo]
bar=baar
bar=baaz";

            var sentence = (Sentence)Parser.Parse(ini, new IniSettings
            {
                DuplicatePropertyHandling = DuplicatePropertyHandling.Rename
            });

            var afterGlobal = sentence.After.Skip(1).ToList();

            Assert.IsTrue(afterGlobal.Count() == 5);
            Assert.IsTrue(afterGlobal.ElementAt(0).Type == SentenceType.Blank);
            Assert.IsTrue(afterGlobal.ElementAt(1).Type == SentenceType.Comment);
            Assert.IsTrue(afterGlobal.ElementAt(2).Type == SentenceType.Section);
            Assert.IsTrue(afterGlobal.ElementAt(3).Type == SentenceType.Property);
            Assert.IsTrue(afterGlobal.ElementAt(3).Type == SentenceType.Property);
            Assert.IsTrue(afterGlobal.ElementAt(3).Tokens.PropertyToken() == "bar1");
            Assert.IsTrue(afterGlobal.ElementAt(4).Tokens.PropertyToken() == "bar2");
        }
    }
}
