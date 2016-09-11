using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ploeh.AutoFixture.MSTest2.UnitTest
{
    [TestClass]
    public class FavorArraysAttributeTest
    {
        [TestMethod]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new FavorArraysAttribute();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(CustomizeAttribute));
            // Teardown
        }

        [TestMethod]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Fixture setup
            var sut = new FavorArraysAttribute();
            // Exercise system and verify outcome
            Assert.ThrowsException<ArgumentNullException>(() =>
                sut.GetCustomization(null));
            // Teardown
        }

        [TestMethod]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new FavorArraysAttribute();
            var parameter = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) }).GetParameters().Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            Assert.IsInstanceOfType(result, typeof(ConstructorCustomization));
            var invoker = (ConstructorCustomization)result;
            Assert.AreEqual(parameter.ParameterType, invoker.TargetType);
            Assert.IsInstanceOfType(invoker.Query, typeof(ArrayFavoringConstructorQuery));
            // Teardown
        }
    }
}
