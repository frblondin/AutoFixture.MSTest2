using System;
using System.Linq;
using Ploeh.TestTypeFoundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ploeh.AutoFixture.MSTest2.UnitTest
{
    [TestClass]
    public class FrozenAttributeTest
    {
        [TestMethod]
        public void SutIsAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new FrozenAttribute();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(CustomizeAttribute));
            // Teardown
        }

        [TestMethod]
        public void GetCustomizationFromNullParameterThrows()
        {
            // Fixture setup
            var sut = new FrozenAttribute();
            // Exercise system and verify outcome
            Assert.ThrowsException<ArgumentNullException>(() =>
                sut.GetCustomization(null));
            // Teardown
        }

        [TestMethod]
        public void GetCustomizationWithSpecificRegisteredTypeReturnsCorrectResult()
        {
            // Fixture setup
            var registeredType = typeof(AbstractType);
#pragma warning disable 0618
            var sut = new FrozenAttribute { As = registeredType };
#pragma warning restore 0618
            var parameter = typeof(TypeWithConcreteParameterMethod)
                .GetMethod("DoSomething", new[] { typeof(ConcreteType) })
                .GetParameters()
                .Single();
            // Exercise system
            var result = sut.GetCustomization(parameter);
            // Verify outcome
            Assert.IsInstanceOfType(result, typeof(FreezingCustomization));
            var freezer = (FreezingCustomization)result;
            Assert.AreEqual(registeredType, freezer.RegisteredType);
            // Teardown
        }

        [TestMethod]
        public void GetCustomizationWithIncompatibleRegisteredTypeThrowsArgumentException()
        {
            // Fixture setup
            var registeredType = typeof(string);
#pragma warning disable 0618
            var sut = new FrozenAttribute { As = registeredType };
#pragma warning restore 0618
            var parameter = typeof(TypeWithConcreteParameterMethod)
                .GetMethod("DoSomething", new[] { typeof(ConcreteType) })
                .GetParameters()
                .Single();
            // Exercise system and verify outcome
            Assert.ThrowsException<ArgumentException>(() => sut.GetCustomization(parameter));
            // Teardown
        }
    }
}
