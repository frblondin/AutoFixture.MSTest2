using System;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ploeh.AutoFixture.MSTest2.UnitTest
{
    [TestClass]
    public class GreedyAttributeTest
    {
        [TestMethod]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new GreedyAttribute();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(CustomizeAttribute));
            // Teardown
        }

        [TestMethod]
        public void GetCustomizationFromNullParamterThrows()
        {
            // Fixture setup
            var sut = new GreedyAttribute();
            // Exercise system and verify outcome
            Assert.ThrowsException<ArgumentNullException>(() =>
                sut.GetCustomization(null));
            // Teardown
        }

        [TestMethod]
        public void GetCustomizationReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new GreedyAttribute();
            var parameter = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) }).GetParameters().Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            Assert.IsInstanceOfType(result, typeof(ConstructorCustomization));
            var invoker = (ConstructorCustomization)result;
            Assert.AreEqual(parameter.ParameterType, invoker.TargetType);
            Assert.IsInstanceOfType(invoker.Query, typeof(GreedyConstructorQuery));
            // Teardown
        }
    }
}
