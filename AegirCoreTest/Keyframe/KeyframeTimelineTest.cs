using AegirLib.Keyframe;
using AegirLib.Scene;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace AegirCoreTest
{
    [TestClass]
    public class KeyframeTimelineTest
    {
        /// <summary>
        /// Asserts that the last keyframe of the timeline is returned
        /// when requested timeline position is greater than the position of the
        /// last keyframe
        /// </summary>
        [TestMethod]
        public void GetClosestReturnsLastKeyframe()
        {
            int keyPos = 10;
            int getAtPos = keyPos + 5;
            KeyframeTimeline testCollection = new KeyframeTimeline();
            KeyframePropertyInfo testProperty = new KeyframePropertyInfo(null, PropertyType.Executable);
            Node testNode = new Node();
            //Add some keys
            testCollection.AddKeyframe(new ValueKeyframe(testProperty, null, null), keyPos, testNode);
            testCollection.AddKeyframe(new ValueKeyframe(testProperty, null, null), keyPos - 2, testNode);
            testCollection.AddKeyframe(new ValueKeyframe(testProperty, null, null), keyPos - 4, testNode);
            Tuple<int, int> closest = testCollection.GetClosestKeys(testProperty, getAtPos);

            Assert.AreEqual(keyPos, closest.Item1);
            Assert.AreEqual(keyPos, closest.Item2);
        }

        /// <summary>
        /// Asserts that the first keyframe of the timeline is returned
        /// when requested timeline position is less than the position of the
        /// first keyframe
        /// </summary>
        [TestMethod]
        public void GetClosestReturnsFirstKeyframe()
        {
            int keyPos = 10;
            int getAtPos = keyPos - 5;
            KeyframeTimeline testCollection = new KeyframeTimeline();
            KeyframePropertyInfo testProperty = new KeyframePropertyInfo(null, PropertyType.Executable);
            Node testNode = new Node();
            //Add some keys
            testCollection.AddKeyframe(new ValueKeyframe(testProperty, null, null), keyPos, testNode);
            testCollection.AddKeyframe(new ValueKeyframe(testProperty, null, null), keyPos + 2, testNode);
            testCollection.AddKeyframe(new ValueKeyframe(testProperty, null, null), keyPos + 4, testNode);
            Tuple<int, int> closest = testCollection.GetClosestKeys(testProperty, getAtPos);

            Assert.AreEqual(keyPos, closest.Item1);
            Assert.AreEqual(keyPos, closest.Item2);
        }
    }
}