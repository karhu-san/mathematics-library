using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Numerics.Tests
{
    [TestClass]
    public class NelderMeadDevelopment
    {
        [TestMethod, TestCategory("Defaults")]
        public void NelderMeadConstructor()
        {
            var x = new NelderMeadSimplexImplementation();

        }
    }
}
