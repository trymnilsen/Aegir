using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using AegirCore.Keyframe;

namespace AegirCoreTest
{
    [TestClass]
    public class KeyframeTimelineTest
    {
        //[TestMethod]
        //public void TestGetPastAnyGivesSameClosestKey()
        //{
        //    Test testCollection = GetTestCollection();
        //    Tuple <int,int> closest = testCollection.GetClosest(120);

        //    Assert.AreEqual(64, closest.Item1);
        //    Assert.AreEqual(64, closest.Item2);
        //}
        //[TestMethod]
        //public void TestGetBeforeAnyGivesSameClosestKey()
        //{
        //    Test testCollection = GetTestCollection();
        //    Tuple<int, int> closest = testCollection.GetClosest(2);

        //    Assert.AreEqual(3, closest.Item1);
        //    Assert.AreEqual(3, closest.Item2);
        //}
        //[TestMethod]
        //public void TestGetInBetweenGivesBeforeAndAfter()
        //{
        //    Test testCollection = GetTestCollection();
        //    Tuple<int, int> closest = testCollection.GetClosest(27);

        //    Assert.AreEqual(20, closest.Item1);
        //    Assert.AreEqual(47, closest.Item2);
        //}

        //private Test GetTestCollection()
        //{
        //    Test testCollection = new Test();
        //    testCollection.AddItem(3, "foo");
        //    testCollection.AddItem(8, "bar");
        //    testCollection.AddItem(20, "faz");
        //    testCollection.AddItem(47, "baz");
        //    testCollection.AddItem(64, "foobar");

        //    return testCollection;
        //}
    }
}
