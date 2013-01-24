using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ab.Tests
{
    [TestFixture]
    public class AudienceTests
    {
        [Test]
        public void Splits_have_acceptable_distribution()
        {
            const int total = 1000;
            var items = new List<string>();
            for(var i = 0; i < total; i++)
            {
                items.Add(Guid.NewGuid().ToString());
            }

            var a = new List<string>();
            var b = new List<string>();
            foreach(var item in items)
            {
                if(Audience.SplitTwo.Value(item))
                {
                    b.Add(item);
                }
                else
                {
                    a.Add(item);
                }
            }

            var aPercent = (float) a.Count/total;
            var bPercent = (float)b.Count / total;
            var delta = Math.Abs(aPercent - bPercent);

            Console.WriteLine(a.Count + ": " + aPercent);
            Console.WriteLine(b.Count + ": " + bPercent);
            Console.WriteLine("Within " + delta);

            Assert.AreEqual(total, a.Count + b.Count);
            Assert.IsTrue(delta < 0.01f); // One percentage point
        }
    }
}
