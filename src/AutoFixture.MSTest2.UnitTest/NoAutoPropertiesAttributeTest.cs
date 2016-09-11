using System;
using System.Linq;
using Ploeh.TestTypeFoundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ploeh.AutoFixture.MSTest2.UnitTest
{
    [TestClass]
    public class NoAutoPropertiesAttributeTest
    {
        [TestMethod]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new NoAutoPropertiesAttribute();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(CustomizeAttribute));
        }

        [TestMethod]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Fixture setup
            var sut = new NoAutoPropertiesAttribute();
            // Exercise system and verify the outcome
            Assert.ThrowsException<ArgumentNullException>(() =>
                sut.GetCustomization(null));
        }

        [TestMethod]
        public void GetCustomizationReturnsTheCorrectResult()
        {
            // Fixture setup
            var sut = new NoAutoPropertiesAttribute();
            var parameter = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object) })
                .GetParameters()
                .Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify the outcome
            Assert.IsInstanceOfType(result, typeof(NoAutoPropertiesCustomization));
        }
    }
}
