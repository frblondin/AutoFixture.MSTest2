using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Ploeh.AutoFixture.MSTest2.UnitTest
{
    [TestClass]
    public class CustomizeAttributeTest
    {
        [TestMethod]
        public void TestableSutIsSut()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingCustomizeAttribute();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(CustomizeAttribute));
            // Teardown
        }

        [TestMethod]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new DelegatingCustomizeAttribute();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(Attribute));
            // Teardown
        }
    }
}
