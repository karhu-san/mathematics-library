using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinearAlgebraLibrary.Tests.MatrixTests
{
    [TestClass]
    [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
    public class MatrixTests
    {
        #region initialization

        private Matrix M11_PI;
        private Matrix M22_sqrts;
        private Matrix M33;
        private Matrix M33_sqrts;
        private Matrix M33_zero_row, M33_zero_col;

        private Matrix I1, I2, I3, I100;
        private Matrix Zero1, Zero2, Zero3;

        private Matrix M12_ones, M21_ones;

        [TestInitialize]
        public void TestInitialize()
        {
            Zero1 = new Matrix();
            Zero2 = new Matrix(2, 2);
            Zero3 = new Matrix(3, 3);

            M11_PI = new Matrix
            {
                Values = new[,] { { Math.PI } }
            };

            M22_sqrts = new Matrix(2, 2)
            {
                Values = new[,] { { Math.Sqrt(2), Math.Sqrt(3) }, { Math.Sqrt(5), Math.Sqrt(6) } }
            };

            M33 = new Matrix(3, 3)
            {
                Values = new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } }
            };

            M33_sqrts = new Matrix(3, 3)
            {
                Values = new[,] {
                    { Math.Sqrt(2), Math.Sqrt(3), Math.Sqrt(5) },
                    { Math.Sqrt(6), Math.Sqrt(7), Math.Sqrt(8) },
                    { Math.Sqrt(10), Math.Sqrt(11), Math.Sqrt(12)}}
            };

            M33_zero_row = new Matrix(3, 3)
            {
                Values = new[,] {
                    { Math.Pow(Math.E, 1),  Math.Pow(Math.E, 2),  Math.Pow(Math.E, 3) },
                    { 0, 0, 0 },
                    { Math.Pow(Math.PI, 1), Math.Pow(Math.PI, 2), Math.Pow(Math.PI, 3) } }
            };

            M33_zero_col = new Matrix(3, 3)
            {
                Values = new double[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -3, 0, 3 } }
            };

            M12_ones = new Matrix(1, 2)
            {
                Values = new double[,] { { 1, 1 } }
            };

            M21_ones = new Matrix(2, 1)
            {
                Values = new double[,] { { 1 }, { 1 } }
            };

            I1 = Matrix.GetIdentity(1);
            I2 = Matrix.GetIdentity(2);
            I3 = Matrix.GetIdentity(3);
            I100 = Matrix.GetIdentity(100);
        }

        #endregion

        #region summation
        [TestCategory("Summation"), TestMethod]
        public void SummingZeroMatrixDoesntChangeMatrix()
        {
            Assert.AreEqual(M11_PI,    M11_PI + Zero1);
            Assert.AreEqual(M22_sqrts, M22_sqrts + Zero2);
            Assert.AreEqual(M33_sqrts, M33_sqrts + Zero3);
        }

        [TestCategory("Summation"), TestMethod]
        public void SummingMatrixAndItsAdditiveInverseIsZeroMatrix()
        {
            Assert.AreEqual(Zero1, M11_PI    + (-1)*M11_PI);
            Assert.AreEqual(Zero2, M22_sqrts + (-1)*M22_sqrts);
            Assert.AreEqual(Zero3, M33_sqrts + (-1)*M33_sqrts);
        }

        #endregion

        #region Multiplication

        [TestCategory("Multiplication"), TestMethod]
        public void MultiplicationOfZeroVectorFromLeftIsZeroVector()
        {
            for (int i = 1; i < 10; i++)
            {
                var zero = new Vector(i);
                
                Assert.AreEqual(zero, Matrix.GetIdentity(i)*zero);
            }
        }

        [TestCategory("Multiplication"), TestMethod]
        public void MultipliationOfAnyVectorWithZeroMatrixIsZeroVector()
        {
            for (int i = 0; i < 10; i++)
            {
                var zero = new Matrix(i+1, i+1);
                var x = new Vector(i+1);
                x.Coordinates[i] = (i + 1)*(i + 1);

                Assert.AreEqual(new Vector(i+1), zero*x);
            }
        }

        [TestCategory("Multiplication"), TestMethod]
        public void MultipliationOfAnyVectorWithIdentityMatrixIsTheSame()
        {
            for (int i = 0; i < 10; i++)
            {
                var x = new Vector(i + 1);
                x.Coordinates[i] = (i + 1) * (i + 1);

                Assert.AreEqual(x, Matrix.GetIdentity(i+1) * x);
            }
        }

        [TestCategory("Multiplication"), TestMethod]
        public void Multiplication3x3ByItself()
        {
            var result = new Matrix(3, 3);
            var v = M33_sqrts.Values;

            result.Values[0, 0] = v[0, 0] * v[0, 0] + v[0, 1] * v[1, 0] + v[0, 2] * v[2, 0];
            result.Values[0, 1] = v[0, 0] * v[0, 1] + v[0, 1] * v[1, 1] + v[0, 2] * v[2, 1];
            result.Values[0, 2] = v[0, 0] * v[0, 2] + v[0, 1] * v[1, 2] + v[0, 2] * v[2, 2];

            result.Values[1, 0] = v[1, 0] * v[0, 0] + v[1, 1] * v[1, 0] + v[1, 2] * v[2, 0];
            result.Values[1, 1] = v[1, 0] * v[0, 1] + v[1, 1] * v[1, 1] + v[1, 2] * v[2, 1];
            result.Values[1, 2] = v[1, 0] * v[0, 2] + v[1, 1] * v[1, 2] + v[1, 2] * v[2, 2];

            result.Values[2, 0] = v[2, 0] * v[0, 0] + v[2, 1] * v[1, 0] + v[2, 2] * v[2, 0];
            result.Values[2, 1] = v[2, 0] * v[0, 1] + v[2, 1] * v[1, 1] + v[2, 2] * v[2, 1];
            result.Values[2, 2] = v[2, 0] * v[0, 2] + v[2, 1] * v[1, 2] + v[2, 2] * v[2, 2];

            Assert.AreEqual(result, M33_sqrts*M33_sqrts);
        }

        [TestCategory("Multiplication"), TestMethod]
        public void MultiplicationMightNotBeCommutative()
        {
            Assert.AreNotEqual(M33*M33_sqrts, M33_sqrts*M33);
        }

        [TestCategory("Multiplication"), TestMethod]
        public void Multiplication3x3ByAnother3x3()
        {
            var result = new Matrix(3, 3);
            var v = M33.Values;
            var u = M33_sqrts.Values;

            result.Values[0, 0] = v[0, 0] * u[0, 0] + v[0, 1] * u[1, 0] + v[0, 2] * u[2, 0];
            result.Values[0, 1] = v[0, 0] * u[0, 1] + v[0, 1] * u[1, 1] + v[0, 2] * u[2, 1];
            result.Values[0, 2] = v[0, 0] * u[0, 2] + v[0, 1] * u[1, 2] + v[0, 2] * u[2, 2];

            result.Values[1, 0] = v[1, 0] * u[0, 0] + v[1, 1] * u[1, 0] + v[1, 2] * u[2, 0];
            result.Values[1, 1] = v[1, 0] * u[0, 1] + v[1, 1] * u[1, 1] + v[1, 2] * u[2, 1];
            result.Values[1, 2] = v[1, 0] * u[0, 2] + v[1, 1] * u[1, 2] + v[1, 2] * u[2, 2];

            result.Values[2, 0] = v[2, 0] * u[0, 0] + v[2, 1] * u[1, 0] + v[2, 2] * u[2, 0];
            result.Values[2, 1] = v[2, 0] * u[0, 1] + v[2, 1] * u[1, 1] + v[2, 2] * u[2, 1];
            result.Values[2, 2] = v[2, 0] * u[0, 2] + v[2, 1] * u[1, 2] + v[2, 2] * u[2, 2];

            Assert.AreEqual(result, M33 * M33_sqrts);
        }

        [TestCategory("Multiplication"), TestMethod]
        public void MultiplicationByZeroMatrixIsZeroFromBothDirections()
        {
            Assert.AreEqual(Zero1, Zero1*M11_PI);
            Assert.AreEqual(Zero1, M11_PI*Zero1);

            Assert.AreEqual(Zero2, Zero2*M22_sqrts);
            Assert.AreEqual(Zero2, M22_sqrts*Zero2);

            Assert.AreEqual(Zero3, Zero3 * M33_sqrts);
            Assert.AreEqual(Zero3, M33_sqrts * Zero3);

            Assert.AreEqual(new Matrix(100,100), new Matrix(100,100)*I100);
            Assert.AreEqual(new Matrix(100, 100), I100 * new Matrix(100, 100));
        }

        [TestCategory("Multiplication"), TestMethod,
         ExpectedException(typeof(InvalidOperationException), "Multiplication for these matrices is not defined.")]
        public void MultiplicationOfWrongDimensionsThrowsException()
        {
            var t = (M33_sqrts)* (this.M22_sqrts);
        }

        [TestCategory("Multiplication"), TestMethod]
        public void MultiplicativeIdentityMatrixWorksBothDirections()
        {
            Assert.AreEqual(M11_PI, I1*M11_PI);
            Assert.AreEqual(M11_PI, M11_PI*I1);

            Assert.AreEqual(M22_sqrts, I2 * M22_sqrts);
            Assert.AreEqual(M22_sqrts, M22_sqrts * I2);

            Assert.AreEqual(M33_sqrts, I3 * M33_sqrts);
            Assert.AreEqual(M33_sqrts, M33_sqrts * I3);
        }

        [TestCategory("Multiplication"), TestMethod]
        public void MultiplicationByOneIsTheSameMatrix()
        {
            Assert.AreEqual(M11_PI, 1*M11_PI);   
        }

        [TestCategory("Multiplication"), TestMethod]
        public void MultiplicationByRealZeroIsZeroMatrix()
        {
            Assert.AreEqual(Zero1, 0*M11_PI);
            Assert.AreEqual(Zero2, 0*M22_sqrts);
            Assert.AreEqual(Zero3, 0*M33_sqrts);
        }

        [TestCategory("Multiplication"), TestMethod]
        public void ScalarMultiplicationByOneKeepsMatrix()
        {
            Assert.AreEqual(M11_PI,    1*M11_PI);
            Assert.AreEqual(M22_sqrts, 1*M22_sqrts);
            Assert.AreEqual(M33_sqrts, 1*M33_sqrts);
        }

        #endregion

        #region submatrix

        [TestCategory("Submatrix"), TestMethod]
        public void SubmatrixOf2x2MatrixIsCorrectElement()
        {
            Assert.AreEqual(M22_sqrts.Values[1, 1], M22_sqrts.GetSubmatrix(0, 0).Values[0, 0]);
            Assert.AreEqual(M22_sqrts.Values[0, 0], M22_sqrts.GetSubmatrix(1, 1).Values[0, 0]);
            Assert.AreEqual(M22_sqrts.Values[0, 1], M22_sqrts.GetSubmatrix(1, 0).Values[0, 0]);
            Assert.AreEqual(M22_sqrts.Values[1, 0], M22_sqrts.GetSubmatrix(0, 1).Values[0, 0]);
        }

        [TestCategory("Submatrix"), TestMethod]
        public void SubmatrixOf3x3MatrixIsCorrectElement()
        {
            Assert.AreEqual(M33.Values[1, 1], M33.GetSubmatrix(0, 0).Values[0, 0]);
            Assert.AreEqual(M33.Values[1, 2], M33.GetSubmatrix(0, 0).Values[0, 1]);
            Assert.AreEqual(M33.Values[2, 1], M33.GetSubmatrix(0, 0).Values[1, 0]);
            Assert.AreEqual(M33.Values[2, 2], M33.GetSubmatrix(0, 0).Values[1, 1]);

            Assert.AreEqual(M33.Values[1, 0], M33.GetSubmatrix(0, 1).Values[0, 0]);
            Assert.AreEqual(M33.Values[1, 2], M33.GetSubmatrix(0, 1).Values[0, 1]);
            Assert.AreEqual(M33.Values[2, 0], M33.GetSubmatrix(0, 1).Values[1, 0]);
            Assert.AreEqual(M33.Values[2, 2], M33.GetSubmatrix(0, 1).Values[1, 1]);

            Assert.AreEqual(M33.Values[1, 0], M33.GetSubmatrix(0, 2).Values[0, 0]);
            Assert.AreEqual(M33.Values[1, 1], M33.GetSubmatrix(0, 2).Values[0, 1]);
            Assert.AreEqual(M33.Values[2, 0], M33.GetSubmatrix(0, 2).Values[1, 0]);
            Assert.AreEqual(M33.Values[2, 1], M33.GetSubmatrix(0, 2).Values[1, 1]);

            Assert.AreEqual(M33.Values[0, 1], M33.GetSubmatrix(1, 0).Values[0, 0]);
            Assert.AreEqual(M33.Values[0, 2], M33.GetSubmatrix(1, 0).Values[0, 1]);
            Assert.AreEqual(M33.Values[2, 1], M33.GetSubmatrix(1, 0).Values[1, 0]);
            Assert.AreEqual(M33.Values[2, 2], M33.GetSubmatrix(1, 0).Values[1, 1]);

            Assert.AreEqual(M33.Values[0, 0], M33.GetSubmatrix(1, 1).Values[0, 0]);
            Assert.AreEqual(M33.Values[0, 2], M33.GetSubmatrix(1, 1).Values[0, 1]);
            Assert.AreEqual(M33.Values[2, 0], M33.GetSubmatrix(1, 1).Values[1, 0]);
            Assert.AreEqual(M33.Values[2, 2], M33.GetSubmatrix(1, 1).Values[1, 1]);

            Assert.AreEqual(M33.Values[0, 0], M33.GetSubmatrix(1, 2).Values[0, 0]);
            Assert.AreEqual(M33.Values[0, 1], M33.GetSubmatrix(1, 2).Values[0, 1]);
            Assert.AreEqual(M33.Values[2, 0], M33.GetSubmatrix(1, 2).Values[1, 0]);
            Assert.AreEqual(M33.Values[2, 1], M33.GetSubmatrix(1, 2).Values[1, 1]);

            Assert.AreEqual(M33.Values[0, 1], M33.GetSubmatrix(2, 0).Values[0, 0]);
            Assert.AreEqual(M33.Values[0, 2], M33.GetSubmatrix(2, 0).Values[0, 1]);
            Assert.AreEqual(M33.Values[1, 1], M33.GetSubmatrix(2, 0).Values[1, 0]);
            Assert.AreEqual(M33.Values[1, 2], M33.GetSubmatrix(2, 0).Values[1, 1]);

            Assert.AreEqual(M33.Values[0, 0], M33.GetSubmatrix(2, 1).Values[0, 0]);
            Assert.AreEqual(M33.Values[0, 2], M33.GetSubmatrix(2, 1).Values[0, 1]);
            Assert.AreEqual(M33.Values[1, 0], M33.GetSubmatrix(2, 1).Values[1, 0]);
            Assert.AreEqual(M33.Values[1, 2], M33.GetSubmatrix(2, 1).Values[1, 1]);

            Assert.AreEqual(M33.Values[0, 0], M33.GetSubmatrix(2, 2).Values[0, 0]);
            Assert.AreEqual(M33.Values[0, 1], M33.GetSubmatrix(2, 2).Values[0, 1]);
            Assert.AreEqual(M33.Values[1, 0], M33.GetSubmatrix(2, 2).Values[1, 0]);
            Assert.AreEqual(M33.Values[1, 1], M33.GetSubmatrix(2, 2).Values[1, 1]);
        }

        [TestCategory("Submatrix"), TestMethod,
         ExpectedException(typeof(InvalidOperationException))]
        public void SubmatrixOfMatrixWith1x1ThrowsException()
        {
            M11_PI.GetSubmatrix(0, 0);
        }

        [TestCategory("Submatrix"), TestMethod, 
         ExpectedException(typeof(InvalidOperationException))]
        public void SubmatrixOfMatrixWith1x2ThrowsException()
        {
            M12_ones.GetSubmatrix(0, 0);
        }

        [TestCategory("Submatrix"), TestMethod,
         ExpectedException(typeof(InvalidOperationException))]
        public void SubmatrixOfMatrixWith2x1ThrowsException()
        {
            M21_ones.GetSubmatrix(0, 0);
        }

        [TestCategory("Submatrix"), TestMethod]
        public void SizeOfValidSubmatrixIs1x1Less()
        {
            Assert.AreEqual(-1 + M22_sqrts.Height, M22_sqrts.GetSubmatrix(0, 0).Height);
            Assert.AreEqual(-1 + M22_sqrts.Width, M22_sqrts.GetSubmatrix(0, 0).Width);

            Assert.AreEqual(-1 + M33_zero_col.Height, M33_zero_col.GetSubmatrix(0, 0).Height);
            Assert.AreEqual(-1 + M33_zero_col.Width, M33_zero_col.GetSubmatrix(0, 0).Width);

            Assert.AreEqual(-1 + I100.Height, I100.GetSubmatrix(0, 0).Height);
            Assert.AreEqual(-1 + I100.Width, I100.GetSubmatrix(0, 0).Width);
        }

        [TestCategory("Submatrix"), TestMethod,
         ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SubmatrixAgainstNegativeDimensionsThrowsException()
        {
            M22_sqrts.GetSubmatrix(-1, -1);
        }

        [TestCategory("Submatrix"), TestMethod,
         ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SubmatrixAgainstTooLargeDimensionsThrowsException()
        {
            M22_sqrts.GetSubmatrix(M12_ones.Height, M12_ones.Width);
        }

#endregion

        #region determinant

        [TestCategory("Determinant"), TestMethod]
        public void DeterminantOf3x3IrrationalRoots()
        {
            Assert.AreEqual(-0.052407645760034, M33_sqrts.GetDeterminant(), General.Eps);
        }

        [TestCategory("Determinant"), TestMethod]
        public void DeterminantOfLDMatrixIs0()
        {
            Assert.AreEqual(0, M33.GetDeterminant());
        }

        [TestCategory("Determinant"), TestMethod]
        public void DeterminantOfRealNumberIsTheNumberPrecisely()
        {
            Assert.AreEqual(M11_PI.Values[0, 0], M11_PI.GetDeterminant());
        }

        [TestCategory("Determinant"), TestMethod]
        public void DeterminantOf2x2MatrixIs_ad_bc()
        {
            var ad_bc = M22_sqrts.Values[0, 0] * M22_sqrts.Values[1, 1] - M22_sqrts.Values[1, 0] * M22_sqrts.Values[0, 1];
            Assert.AreEqual(ad_bc, M22_sqrts.GetDeterminant(), General.Eps);
        }

        [TestCategory("Determinant"), TestMethod]
        public void DeterminantOfIdentityMatrixIs1()
        {
            Assert.AreEqual(1, I2.GetDeterminant());
            Assert.AreEqual(1, I3.GetDeterminant());
            Assert.AreEqual(1, I100.GetDeterminant());
        }

        [TestCategory("Determinant"), TestMethod]
        public void DetABEqualsDetADetB()
        {
            Assert.AreEqual((M33*M33_sqrts).GetDeterminant(), M33.GetDeterminant()*M33_sqrts.GetDeterminant(), General.Eps);
        }

        [TestCategory("Determinant"), TestMethod]
        public void DeterminantOfMatrixWithZeroRowIsZero()
        {
            Assert.AreEqual(0, M33_zero_row.GetDeterminant());
        }

        [TestCategory("Determinant"), TestMethod]
        public void DeterminantOfMatrixWithZeroColumnIsZero()
        {
            Assert.AreEqual(0, M33_zero_col.GetDeterminant());
        }

        [TestCategory("Determinant"), TestMethod,
         ExpectedException(typeof(InvalidOperationException), "The determinant of a non-square matrix is not defined.")]
        public void DeterminantOfNonSquareMatrixThrowsException()
        {
            M12_ones.GetDeterminant();
        }

#endregion 
    }  

}
