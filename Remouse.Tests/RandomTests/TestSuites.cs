using Remouse.Math;
using NUnit.Framework;

namespace Shared.Test.RandomTests
{
    public class TestSuites
    {
        [Test]
        public void TestRandomGenerateCorrectValues()
        {
            var fastRandom = new DeterminedRandom();

            var value = fastRandom.Next(0, 20);
            
            Assert.IsTrue(value <= 20 && value > 0, "value out of bounds");
        }
        
        [Test]
        public void TestRandomGenerateSameValuesWithSameSeed()
        {
            var fastRandom1 = new DeterminedRandom(100);
            var fastRandom2 = new DeterminedRandom(100);
            
            Assert.AreEqual(fastRandom1.Next(0, 100), fastRandom2.Next(0, 100), "value are different");
        }
        
        [Test]
        public void TestRandomGenerateDifferentValuesWithNotFullyCopiedSeeds()
        {
            var fastRandom1 = new DeterminedRandom(100);

            fastRandom1.Next(0, 200);
            fastRandom1.Next(0, 200);
            fastRandom1.Next(0, 200);
            
            var fastRandom2 = new DeterminedRandom(fastRandom1.x);
            
            Assert.AreNotEqual(fastRandom2.Next(0, 100), fastRandom1.Next(0, 100), "value are different");
        }
        
        [Test]
        public void TestRandomGenerateSameValuesWithReinitializationAllValues()
        {
            var fastRandom1 = new DeterminedRandom(100);

            fastRandom1.Next(0, 200);
            fastRandom1.Next(0, 200);
            fastRandom1.Next(0, 200);
            
            var fastRandom2 = new DeterminedRandom(fastRandom1.x, fastRandom1.y, fastRandom1.z, fastRandom1.w);
            
            Assert.AreEqual(fastRandom2.Next(0, 100), fastRandom1.Next(0, 100), "value are different");
        }
    }
}