using System;
using System.Linq;

namespace LinearAlgebraLibrary
{
    public class Matrix
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public double[,] Values { get; set; }

        public Matrix(int height=1, int width=1)
        {

            if (height<1 || width<1)
                throw new ArgumentOutOfRangeException("Matrix dimensions must be positive");

            Values = new double[height, width];

            Height = Values.GetLength(0);
            Width = Values.GetLength(1);
        }

        public double GetDeterminant()
        {
            if (Height != Width)
                throw new InvalidOperationException("The determinant of a non-square matrix is not defined.");

            if (Height == 1 && Width == 1)
                return Values[0, 0];
            
            if (Height == 2 && Width == 2)
                return GetDeterminantADBC(Values[0, 0], Values[1, 1], Values[0, 1], Values[1, 0]);

            for (var i = 0; i < Height; i++ )
            {
                var row = GetRow(i);
                if (Vector.CheckIfNullVector(row))
                    return 0;
            }

            for (var j = 0; j < Width; j++)
            {
                var col = GetColumn(j);
                if (Vector.CheckIfNullVector(col))
                    return 0;
            }

            var submatrices = new Matrix[Width];

            for (var i = 0; i < Width; i++)
                submatrices[i] = GetSubmatrix(0, i);

            return submatrices.Select((t, i) => Math.Pow(-1, i)*Values[0, i]*t.GetDeterminant()).Sum();
        }

        public Matrix GetSubmatrix(int rowRemove, int colRemove)
        {
            if (Height == 1 || Width == 1)
                throw new InvalidOperationException("Submatrix of a matrix with dimension 1 is undefined.");

            if (rowRemove < 0 || colRemove < 0)
                throw new ArgumentOutOfRangeException($"Row {rowRemove} and/or column {colRemove} to remove are negative.");

            if (rowRemove > Height-1 || colRemove > Width-1)
                throw new ArgumentOutOfRangeException($"{rowRemove},{colRemove} is outside of the given matrix dimensions {Height-1},{Width-1}");

            var reducedMatrix = new Matrix(Height-1, Width-1);

            for (var row = 0; row < Height; row++)
                for (var col = 0; col < Width; col++)
                {
                    if (row == rowRemove || col == colRemove)
                        continue;

                    if (row < rowRemove)
                    {
                        if (col < colRemove)
                            reducedMatrix.Values[row, col] = Values[row, col];
                        if (col > colRemove)
                            reducedMatrix.Values[row, col-1] = Values[row, col];
                    }
                    else if (row > rowRemove)
                    {
                        if (col < colRemove)
                            reducedMatrix.Values[row - 1, col] = Values[row, col];
                        if (col > colRemove)
                            reducedMatrix.Values[row - 1, col - 1] = Values[row, col];
                    }
                }

            return reducedMatrix;
        }

        private static double GetDeterminantADBC(double a, double d, double b, double c)
        {
            return a * d - b * c;
        }
        
        private double[] GetColumn(int colnumber)
        {
            var col = new double[Height];

            for (var j = 0; j < Height; j++)
                col[j] = Values[j, colnumber];

            return col;
        }

        private double [] GetRow (int rownumber) {

            var row = new double[Width];

            for (var i = 0; i < Width; i++)
                row[i] = Values[rownumber, i];

            return row;
        }

        public static Matrix GetIdentity(int rank)
        {
            var I = new Matrix(rank, rank);

            for (var i = 0; i < rank; i++)
                I.Values[i, i] = 1;

            return I;
        }

        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (left.Width != right.Width || left.Height != right.Height)
                throw new ArgumentException("Cannot add two matrices with incompatible dimensions");

            var sum = new Matrix(left.Height, left.Width);

            for (var i=0; i<sum.Height; i++)
                for (var j = 0; j < sum.Width; j++)
                    sum.Values[i, j] = left.Values[i, j] + right.Values[i, j];

            return sum;
        }
    
        public static Vector operator *(Matrix left, Vector right)
        {
            if (left.Width != right.Dimension)
                throw new ArgumentException("Multiplication of matrix and vector undefined!");

            var mult = new Vector(left.Height);

            for (var i=0; i<left.Height; i++)
                for (var j = 0; j < left.Width; j++)
                {
                    var sum = 0.0;
                    sum += right.Coordinates[j]*left.Values[i, j];

                    mult.Coordinates[i] = sum;
                }

            return mult;
        }

        public static Matrix operator *(double scalar, Matrix matrix)
        {
            var mult = new Matrix(matrix.Height, matrix.Width);

            for (var i=0; i<matrix.Height; i++)
                for (var j = 0; j < matrix.Width; j++)
                {
                    mult.Values[i, j] = scalar*matrix.Values[i, j];
                }

            return mult;
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left.Width != right.Height)
                throw new InvalidOperationException("Matrix multiplication not defined for these matrices");

            var mult = new Matrix(left.Height, right.Width);

            for (var i = 0; i < mult.Height; i++)
            {
                for (var j = 0; j < mult.Width; j++)
                {
                    var sum = 0.0;

                    for (var h = 0; h < right.Height; h++)
                        sum += left.Values[i, h]*right.Values[h, j];
                    
                    mult.Values[i, j] = sum;
                }
            }

            return mult;
        }

        //for k = 1 ... min(m,n):
        //    Find the k-th pivot:
        //    i_max  := argmax (i = k ... m, abs(A[i, k]))
        //    if A[i_max, k] = 0
        //        error "Matrix is singular!"
        //    swap rows(k, i_max)
        //    Do for all rows below pivot:
        //    for i = k + 1 ... m:
        //        m := A[i, k] / A[k, k]
        //    Do for all remaining elements in current row:
        //    for j = k + 1 ... n:
        //        A[i, j]  := A[i, j] - A[k, j] * m
        //        Fill lower triangular matrix with zeros:
        //    A[i, k]  := 0

        public override bool Equals(object obj)
        {
            var other = obj as Matrix;

            if (other == null)
                return false;
        
            for (var i=0; i<other.Height; i++)
                for (var j = 0; j < other.Width; j++)
                {
                    if (!General.FEquals(Values[i, j], other.Values[i, j]))
                        return false;
                }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ (int)GetDeterminant();
        }

    }
}
