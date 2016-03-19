using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinearAlgebraLibrary.Tests
{
    [TestClass]
    public class VectorTests
    {
        private Vector R1_origin, R2_origin, R3_origin, R100_origin;
        private Vector R1_1, R2_1, R3_1, R100_1;

        [TestInitialize]
        public void TestInitializer()
        {
            R1_origin = new Vector(new double[1]);
            R2_origin = new Vector(new double[2]);
            R3_origin = new Vector(new double[3]);
            R100_origin = new Vector(new double[100]);
            
            R1_1 = new Vector(new double[] {1});
            R2_1 = new Vector(1, 1);
            R3_1 = new Vector(1, 1, 1);
            R100_1 = new Vector(Enumerable.Repeat(1.0, 100).ToArray());
        }

        [TestCategory("Vector Summation"), TestMethod]
        public void SummationWithZeroVectorIsTheSameVector()
        {
            Assert.AreEqual(R1_1, R1_1 + R1_origin);

        }

        [TestCategory("Defaults"), TestCategory("Normalization"), TestMethod]
        public void NormalizedZeroVectorIsStillZeroVector()
        {
            R1_origin.Normalize();
            R2_origin.Normalize();
            R3_origin.Normalize();
            R100_origin.Normalize();

            Assert.IsTrue(R1_origin.IsNull());
            Assert.IsTrue(R2_origin.IsNull());
            Assert.IsTrue(R3_origin.IsNull());
            Assert.IsTrue(R100_origin.IsNull());
        }

        [TestCategory("Defaults"), TestMethod]
        public void DefaultVectorIsAScalarZero()
        {   
            var v = new Vector();

            Assert.AreEqual(1, v.Coordinates.Length);
            Assert.AreEqual(0, v.Coordinates[0]);
        }

        [TestCategory("Defaults"), TestMethod]
        public void OriginIsAlwaysNullVector()
        {
            Assert.IsTrue(R1_origin.IsNull());   
            Assert.IsTrue(R2_origin.IsNull());
            Assert.IsTrue(R3_origin.IsNull());
            Assert.IsTrue(R100_origin.IsNull());
        }

        [TestCategory("Vector Length"), TestMethod]
        public void LengthOfZeroVectorIsZero()
        {
            Assert.AreEqual(0, R1_origin.GetLength());
            Assert.AreEqual(0, R2_origin.GetLength());
            Assert.AreEqual(0, R3_origin.GetLength());
            Assert.AreEqual(0, R100_origin.GetLength());
        }

        [TestCategory("Vector Length"), TestMethod]
        public void LengthOf1VectorIsSqrtN()
        {
            Assert.AreEqual(Math.Sqrt(R1_1.Coordinates.Length), R1_1.GetLength());
            Assert.AreEqual(Math.Sqrt(R2_1.Coordinates.Length), R2_1.GetLength());
            Assert.AreEqual(Math.Sqrt(R3_1.Coordinates.Length), R3_1.GetLength());
            Assert.AreEqual(Math.Sqrt(R100_1.Coordinates.Length), R100_1.GetLength());
        }

        [TestCategory("Vector Length"), TestMethod]
        public void LengthOfNormalizedNonZeroVectorIsOne()
        {
            R1_1.Normalize();
            R2_1.Normalize();
            R3_1.Normalize();
            R100_1.Normalize();

            Assert.AreEqual(1, R1_1.GetLength(), General.Eps);
            Assert.AreEqual(1, R2_1.GetLength(), General.Eps);
            Assert.AreEqual(1, R3_1.GetLength(), General.Eps);
            Assert.AreEqual(1, R100_1.GetLength(), General.Eps);
        }

    }
}