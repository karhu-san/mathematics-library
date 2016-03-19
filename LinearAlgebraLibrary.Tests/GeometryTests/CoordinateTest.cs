using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinearAlgebraLibrary.Tests.GeometryTests
{
    [TestClass]
    public class CoordinateTests
    {
        [TestMethod, TestCategory("Defaults")]
        public void DefaultSimplexIs2DTriangleWith11()
        {
            var s = new Simplex();

            Assert.AreEqual(2, s.Dimension);
            Assert.AreEqual(new Vector(1, 0), s.vertices[0]);
            Assert.AreEqual(new Vector(0, 1), s.vertices[1]);
        }

        [TestMethod, TestCategory("Defaults")]
        [ExpectedException(typeof(ArgumentOutOfRangeException),  "Simplexes with dimension less than 2 not allowed!")]
        public void SimplexWithLessThan2DimensionsThrowsException()
        {
            new Simplex(1);
        }
    }

    public class Simplex
    {
        public Vector[] vertices { get; set; }
        public int? Dimension => vertices?.Length;

        public Simplex(int dimension=2)
        {
            if (dimension < 2)
                throw new ArgumentOutOfRangeException("Simplexes with dimension less than 2 not allowed!");

            vertices = new Vector[dimension];

            for (var i = 0; i < vertices.Length; i++)
                vertices[i] = new Vector(Dimension) {Coordinates = {[i] = 1.0}};
        }
    }
}
