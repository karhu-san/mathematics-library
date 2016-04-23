using System;
using System.Collections.Generic;
using LinearAlgebraLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VectorAnalysis.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestCategory("Defaults"), TestMethod]
        public void DefaultIntervalIsR2UnitSquareInFirstQuadrant()
        {
            var I = new Interval();

            Assert.AreEqual(2, I.Dimension);
            
            Assert.AreEqual(0, I.x[0].Low);
            Assert.AreEqual(1, I.x[0].High);

            Assert.AreEqual(0, I.x[1].Low);
            Assert.AreEqual(1, I.x[1].High);

        }
    }

    public class Interval
    {
        public List<Interval> x;
        public int Dimension
        {
            get { return x.Count; }
        }

        public double Low, High;

        public Interval(double low, double high)
        {
            Low = low;
            High = high;
        }

        public Interval()
        {
            x = new List<Interval>();

            var dim = new Interval(0, 1);
            x.Add(dim);
            
            dim = new Interval(0, 1);
            x.Add(dim);
        }
    }
}
