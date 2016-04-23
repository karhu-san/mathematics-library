using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinearAlgebraLibrary.Tests.GeometryTests
{
    [TestClass]
    public class CoordinateTests
    {
        private readonly Simplex defaultsimplex = new Simplex();
        private readonly Simplex SO2 = new Simplex(new Vector(2), new Vector(2), new Vector(2));

#region defaults
        [TestMethod, TestCategory("Defaults")]
        public void DefaultSimplexIs2DTriangleWith_00_01_10()
        {
            Assert.AreEqual(2, defaultsimplex.SpaceDimension);
            Assert.AreEqual(3, defaultsimplex.VertexCount);

            Assert.AreEqual(new Vector(0, 0), defaultsimplex.vertices[0]);
            Assert.AreEqual(new Vector(1, 0), defaultsimplex.vertices[1]);
            Assert.AreEqual(new Vector(0, 1), defaultsimplex.vertices[2]);
        }

        //[TestMethod, TestCategory("Defaults")]
        //public void APointIsALegalSimplex()
        //{
            
        //}

        [TestMethod, TestCategory("Defaults")]
        [ExpectedException(typeof(ArgumentOutOfRangeException),  "Simplexes with dimension less than 2 not allowed!")]
        public void SimplexWithLessThan2DimensionsThrowsException()
        {
            new Simplex(1);
        }
        
#endregion

#region centroid

        [TestMethod, TestCategory("Centroid")]
        public void CentroidOfDefaultSimplexIs_3rd_3rd()
        {
            Assert.AreEqual(expected: 1.0/3, actual: defaultsimplex.GetCentroid().Coordinates[0], delta: General.Eps);
            Assert.AreEqual(expected: 1.0/3, actual: defaultsimplex.GetCentroid().Coordinates[1], delta: General.Eps);
        }

        [TestMethod, TestCategory("Centroid")]
        public void DimensionOfCentroidIsSameAsSpaceDimension()
        {
            Assert.AreEqual(defaultsimplex.SpaceDimension, defaultsimplex.GetCentroid().Dimension);
        }

#endregion

    }

    public class Simplex
    {
        public Vector[] vertices { get; set; }
        public int? SpaceDimension => VertexCount - 1;
        public int? VertexCount => vertices?.Length;

        public Vector GetCentroid()
        {
            var centroid = new Vector(SpaceDimension);

            for (int v = 0; v < VertexCount; v++)
            {
                for (int n = 0; n < SpaceDimension; n++)
                {
                    centroid.Coordinates[n] += vertices[v].Coordinates[n];
                }
            }
            if (SpaceDimension != null)
                centroid = 1.0/VertexCount.GetValueOrDefault() * centroid;

            return centroid;
        }

        public Simplex(params Vector[] coordinates)
        {
            vertices = (coordinates.Length < 3) ? new Vector[] {new Vector(0,0), new Vector(0,1), new Vector(1,0)} : coordinates;
            
        }

        // TODO: implement this for consistency?
        //public Vector(int? dimension)
        //{
        //    Coordinates = new double[dimension.GetValueOrDefault()];    
        //}

        public Simplex(int dimension=2)
        {
            if (dimension < 2)
                throw new ArgumentOutOfRangeException("Simplexes with dimension less than 2 not allowed!");

            // TODO: Make this constructor chaining so that the real call is always using Vectors

            vertices = new Vector[dimension+1];
            
            vertices[0] = new Vector(SpaceDimension);

            for (var i = 1; i < VertexCount; i++)
                vertices[i] = new Vector(SpaceDimension) {Coordinates = {[i - 1] = 1}};
        }
    }
}
