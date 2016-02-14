using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Martini.Tests.LinkedObjectTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void NextTests()
        {
            var foo = new LinkedObject<string>("foo");
            var bar = new LinkedObject<string>("bar");
            var baz = new LinkedObject<string>("baz");
            var qux = new LinkedObject<string>("qux");

            // create chain
            
            // foo, bar, baz, qux

            foo.Next = bar;
            bar.Next = baz;
            baz.Next = qux;

            // check chain

            Assert.AreSame(bar, foo.Next);
            Assert.AreSame(baz, bar.Next);
            Assert.AreSame(qux, baz.Next);
            Assert.IsNull(qux.Next);

            Assert.AreSame(baz, qux.Previous);
            Assert.AreSame(bar, baz.Previous);
            Assert.AreSame(foo, bar.Previous);
            Assert.IsNull(foo.Previous);

            CollectionAssert.AreEqual(new[] { foo, bar, baz, qux }, foo.After.ToList());
            CollectionAssert.AreEqual(new[] { foo }, foo.Before.ToList());
            CollectionAssert.AreEqual(new[] { qux, baz, bar, foo }, qux.Before.ToList());

            // move qux back
            
            // foo, qux, bar, baz

            foo.Next = qux;

            Assert.AreSame(qux, foo.Next);
            Assert.AreSame(baz, bar.Next);
            Assert.AreSame(bar, qux.Next);
            Assert.IsNull(baz.Next);

            Assert.AreSame(foo, qux.Previous);
            Assert.AreSame(bar, baz.Previous);
            Assert.AreSame(qux, bar.Previous);
            Assert.IsNull(foo.Previous);

            CollectionAssert.AreEqual(new[] { foo, qux, bar, baz }, foo.After.ToList());
            CollectionAssert.AreEqual(new[] { foo }, foo.Before.ToList());
            CollectionAssert.AreEqual(new[] { qux, foo }, qux.Before.ToList());
            CollectionAssert.AreEqual(new[] { baz, bar, qux, foo }, baz.Before.ToList());
        }

        [TestMethod]
        public void PreviousTests()
        {
            var foo = new LinkedObject<string>("foo");
            var bar = new LinkedObject<string>("bar");
            var baz = new LinkedObject<string>("baz");

            foo.Previous = bar;
            bar.Previous = baz;

            Assert.AreSame(bar, foo.Previous);
            Assert.AreSame(baz, bar.Previous);
            Assert.IsNull(baz.Previous);

            Assert.AreSame(bar, baz.Next);
            Assert.AreSame(foo, bar.Next);
            Assert.IsNull(foo.Next);

            CollectionAssert.AreEqual(new[] { foo }, foo.After.ToList());
            CollectionAssert.AreEqual(new[] { foo, bar, baz }, foo.Before.ToList());
            CollectionAssert.AreEqual(new[] { baz }, baz.Before.ToList());
        }
       
        [TestMethod]
        public void RemoveTests()
        {
            var foo = new LinkedObject<string>("foo");
            var bar = new LinkedObject<string>("bar");
            var baz = new LinkedObject<string>("baz");

            foo.Next = bar;
            bar.Next = baz;

            bar.Remove();

            Assert.IsNull(foo.Previous);
            Assert.AreSame(baz, foo.Next);

            Assert.AreSame(foo, baz.Previous);
            Assert.IsNull(baz.Next);

            Assert.IsNull(bar.Previous);
            Assert.IsNull(bar.Next);
        }
    }
}
