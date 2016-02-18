using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Martini.Tests.TokenizerTests
{
    [TestClass]
    public class TokenizeLine
    {
        [TestMethod]
        public void TokenizesComment()
        {
            var tokens = Tokenizer.TokenizeLine(";foo", Grammar.DefaultDelimiters);

            Assert.IsTrue(tokens.Count == 2);
            Assert.IsTrue(tokens[0].Type == TokenType.CommentIndicator);
            Assert.IsTrue(tokens[1].Type == TokenType.Text);

            Assert.AreEqual(Grammar.DefaultDelimiters.TokenTypes[TokenType.CommentIndicator].ToString(), tokens[0].ToString());
            Assert.AreEqual("foo", tokens[1].ToString());
        }

        [TestMethod]
        public void TokenizesSection()
        {
            var tokens = Tokenizer.TokenizeLine("[foo]", Grammar.DefaultDelimiters);

            Assert.IsTrue(tokens.Count == 3);
            Assert.IsTrue(tokens[0].Type == TokenType.LeftSectionDelimiter);
            Assert.IsTrue(tokens[1].Type == TokenType.Text);
            Assert.IsTrue(tokens[2].Type == TokenType.RightSectionDelimiter);

            Assert.AreEqual(Grammar.DefaultDelimiters.TokenTypes[TokenType.LeftSectionDelimiter].ToString(), tokens[0].ToString());
            Assert.AreEqual("foo", tokens[1].ToString());
            Assert.AreEqual(Grammar.DefaultDelimiters.TokenTypes[TokenType.RightSectionDelimiter].ToString(), tokens[2].ToString());
        }

        [TestMethod]
        public void TokenizesProperty()
        {
            var tokens = Tokenizer.TokenizeLine("foo=bar", Grammar.DefaultDelimiters);

            Assert.IsTrue(tokens.Count == 3);
            Assert.IsTrue(tokens[0].Type == TokenType.Text);
            Assert.IsTrue(tokens[1].Type == TokenType.ProperetyValueDelimiter);
            Assert.IsTrue(tokens[2].Type == TokenType.Text);

            Assert.AreEqual("foo", tokens[0].ToString());
            Assert.AreEqual(Grammar.DefaultDelimiters.TokenTypes[TokenType.ProperetyValueDelimiter].ToString(), tokens[1].ToString());
            Assert.AreEqual("bar", tokens[2].ToString());
        }

        [TestMethod]
        public void TokenizesBlankLine()
        {
            var tokens = Tokenizer.TokenizeLine("", Grammar.DefaultDelimiters);

            Assert.IsTrue(tokens.Count == 0);
        }

        [TestMethod]
        public void IgnoresInlineCommentIndicator()
        {
            var tokens = Tokenizer.TokenizeLine("foo;bar", Grammar.DefaultDelimiters);
            Assert.IsTrue(tokens.Count == 1);
            Assert.IsTrue(tokens[0].Type == TokenType.Text);
        }

        [TestMethod]
        public void IgnoresEscapedCommentIndicator()
        {
            var tokens = Tokenizer.TokenizeLine("\\;foo=bar", Grammar.DefaultDelimiters);
            Assert.IsTrue(tokens.Count == 3);
            Assert.IsTrue(tokens[0].Type == TokenType.Text);
            Assert.IsTrue(tokens[1].Type == TokenType.ProperetyValueDelimiter);
            Assert.IsTrue(tokens[2].Type == TokenType.Text);
        }

        [TestMethod]
        public void IgnoresInlineSectionDelimiters()
        {
            var tokens = Tokenizer.TokenizeLine("foo[bar]baz", Grammar.DefaultDelimiters);
            Assert.IsTrue(tokens.Count == 1);
            Assert.IsTrue(tokens[0].Type == TokenType.Text);
        }

        [TestMethod]
        public void IgnoresStartsWithPropertyValueDelimiter()
        {
            var tokens = Tokenizer.TokenizeLine("=foo", Grammar.DefaultDelimiters);
            Assert.IsTrue(tokens.Count == 1);
            Assert.IsTrue(tokens[0].Type == TokenType.Text);
        }
    }

    [TestClass]
    public class Tokenize
    {

        [TestMethod]
        public void TokenizesIni()
        {
            var ini = @"
;foo
[bar]
baz=qux
[barbar]
bazbaz=quxqux";

            var sentence = (Sentence)Tokenizer.Tokenize(ini, Grammar.DefaultDelimiters);

            Assert.IsTrue(sentence.After.Count() == 6);
            Assert.IsTrue(sentence.After.All(x => x.Type == SentenceType.Uninitialized));
        }
    }
}
