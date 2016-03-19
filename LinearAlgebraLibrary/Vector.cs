using System;
using System.Linq;

namespace LinearAlgebraLibrary
{
    public class Vector
    {
        public double[] Coordinates { get; private set; }
        public int? Dimension => Coordinates?.Length;
        
        public static Vector operator +(Vector left, Vector right)
        {
            if (left.Dimension != right.Dimension)
                throw new ArgumentException("Cannot add two vectors with incompatible dimensions");

            var sum = new Vector(left.Dimension);

            for (var i = 0; i < left.Dimension; i++)
                sum.Coordinates[i] = left.Coordinates[i] + right.Coordinates[i];

            return sum;
        }

        public static Vector operator *(double scalar, Vector vector)
        {
            var mult = new Vector(vector.Dimension);

            for (var i = 0; i < vector.Dimension; i++)
                mult.Coordinates[i] = scalar*vector.Coordinates[i];

            return mult;
        }

        public double GetLength()
        {
            if (IsNull())
                return 0;

            var squaresum = Coordinates.Sum(x => Math.Pow(x, 2));

            return Math.Sqrt(squaresum);
        }

        public Vector(int dimension)
        {
            Coordinates = new double[dimension];
        }

        public Vector(int? dimension)
        {
            Coordinates = new double[dimension.GetValueOrDefault()];    
        }

        public Vector(params double[] coordinates)
        {
            Coordinates = (coordinates.Length == 0) ? new double[] { 0 } : coordinates;
        }

        public bool IsNull()
        {
            return CheckIfNullVector(Coordinates);
        }

        internal static bool CheckIfNullVector(double[] vector)
        {
            return vector.All(x => General.FEquals(x, 0));
        }

        public void Normalize()
        {
            if (IsNull()) return;

            var length = GetLength();

            for (var i = 0; i < Coordinates.Length; i++)
                Coordinates[i] /= length;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Vector;

            if (other == null)
                return false;
        
            for (var i=0; i<other.Dimension; i++)
                if (!General.FEquals(Coordinates[i], other.Coordinates[i]))
                    return false;
            
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ (int)(1+GetLength());
        }
    }


    public class General
    {
        public static double Eps = 1e-6;

        public static bool FEquals(double a, double b)
        {
            return (Math.Abs(a - b) < Eps);
        }
    }
}
