using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SQ7
{
    [TestFixture]
    public class Tests
    {

        [Test]
        public void PushItems()
        {
            var model = new Stack<string>();
            model.Push("Max");
            model.Push("Mirta");
            model.Push("Eren");
            Assert.AreEqual(new List<string> { "Max", "Mirta", "Eren" }, model.ListItem);
        }

        [Test]
        public void PopItems()
        {
            var model = new Stack<string>();
            model.Push("Max");
            model.Push("Mirta");
            model.Push("Eren");
            model.Pop();
            Assert.AreEqual(new List<string> { "Max", "Mirta" }, model.ListItem);
        }

        [Test]
        public void RollbackHistory()
        {
            var model = new Stack<string>();
            model.Push("Max");
            model.Pop();
            model.Push("Mirta");
            model.Pop();
            model.Push("Eren");
            model.Push("Isak");
            model.Rollback(3);
            Assert.AreEqual(new List<string> { "Mirta" }, model.ListItem);
        }

        [Test]
        public void RollbackAllHistory()
        {
            var model = new Stack<string>();
            model.Push("Max");
            model.Pop();
            model.Push("Mirta");
            model.Pop();
            model.Push("Eren");
            model.Push("Isak");
            model.Push("Ingrid");
            model.Rollback(0);
            Assert.AreEqual(new List<string> { }, model.ListItem);
        }

        [Test]
        public void ForgetHistory()
        {
            var model = new Stack<string>();
            model.Push("Max");
            model.Pop();
            model.Push("Mirta");
            model.Pop();
            model.Push("Eren");
            model.Forget();

            Assert.AreEqual(new List<string> { "Eren" }, model.ListItem);
        }

        [Test]
        public void ForgetRollbackHistory()
        {
            var model = new Stack<string>();
            model.Push("Max");
            model.Pop();
            model.Push("Mirta");
            model.Pop();
            model.Push("Eren");
            model.Forget();
            model.Rollback(0);
            Assert.AreEqual(new List<string> { "Eren" }, model.ListItem);
        }

        [Test]
        public void ForgetRollbackHistory2()
        {
            var model = new Stack<string>();
            model.Push("Max");
            model.Pop();
            model.Push("Mirta");
            model.Pop();
            model.Push("Eren");
            model.Forget();
            model.Pop();
            model.Rollback(1);
            Assert.AreEqual(new List<string> { }, model.ListItem);
        }

        [Test]
        public void ForgetRollbackHistory3()
        {
            var model = new Stack<string>();
            model.Push("Max");
            model.Pop();
            model.Push("Mirta");
            model.Pop();
            model.Push("Eren");
            model.Forget();
            model.Push("Isaac");
            model.Push("Uni");
            model.Rollback(2);
            Assert.AreEqual(new List<string> { "Eren", "Isaac", "Uni" }, model.ListItem);
        }

        [Test]
        public void AllAction()
        {
            var model = new Stack<string>();
            model.Push("Max");
            model.Pop();
            model.Push("Mirta");
            model.Pop();
            model.Push("Eren");
            model.Forget();
            model.Pop();
            model.Push("Isaac");
            model.Rollback(0);
            model.Push("Uni");

            Assert.AreEqual(new List<string> { "Eren", "Uni" }, model.ListItem);
        }
    }
}