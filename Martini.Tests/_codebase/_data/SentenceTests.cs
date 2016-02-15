using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Martini.Tests.SentenceTests
{
    [TestClass]
    public class SentenceTests
    {
        [TestMethod]
        public void TokensSetsParentSentence()
        {
            var tokens = new List<Token>
            {
                new Token("foo"),
                new Token("bar")
            };

            var s = new Sentence
            {
                Tokens = tokens
            };

            Assert.AreSame(s, tokens[0].Sentence);
            Assert.AreSame(s, tokens[1].Sentence);
        }

        [TestMethod]
        public void NextAndAfterTests()
        {
            var s1 = new Sentence();
            var s2 = new Sentence();
            s1.Next = s2;
            Assert.AreSame(s2, s1.Next);
            Assert.IsNull(s2.Next);
            CollectionAssert.AreEqual(new[] { s1, s2 }, s1.After.ToList());
        }

        [TestMethod]
        public void PreviousAndBeforeTests()
        {
            var s1 = new Sentence();
            var s2 = new Sentence();
            s1.Previous = s2;
            Assert.AreSame(s2, s1.Previous);
            Assert.IsNull(s1.Next);
            CollectionAssert.AreEqual(new[] { s1, s2 }, s1.Before.ToList());
        }

        [TestMethod]
        public void RemoveTests()
        {
            var s1 = new Sentence();
            var s2 = new Sentence();
            var s3 = new Sentence();

            s1.Next = s2;
            s2.Next = s3;

            Assert.AreSame(s2, s1.Next);
            Assert.AreSame(s3, s2.Next);
            Assert.IsNull(s3.Next);

            CollectionAssert.AreEqual(new[] { s1, s2, s3 }, s1.After.ToList());

            s2.Remove();

            Assert.AreSame(s3, s1.Next);
            Assert.IsNull(s3.Next);

            Assert.IsNull(s2.Previous);
            Assert.IsNull(s2.Next);
        }
    }
}
