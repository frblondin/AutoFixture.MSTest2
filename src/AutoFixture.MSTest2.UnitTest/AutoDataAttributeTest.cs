using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.TestTypeFoundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.TestFramework.AdvancedDataRow;
using Ploeh.AutoFixture.MSTest2.UnitTest.Extensions;

namespace Ploeh.AutoFixture.MSTest2.UnitTest
{
    [TestClass]
    public class AutoDataAttributeTest
    {
        [TestMethod]
        public void SutIsDataAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new AutoDataAttribute();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(DataAttribute));
            // Teardown
        }

        [TestMethod]
        public void InitializedWithDefaultConstructorHasCorrectFixture()
        {
            // Fixture setup
            var sut = new AutoDataAttribute();
            // Exercise system
            IFixture result = sut.Fixture;
            // Verify outcome
            Assert.IsInstanceOfType(result, typeof(Fixture));
            // Teardown
        }

        [TestMethod]
        public void InitializeWithNullFixtureThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.ThrowsException<ArgumentNullException>(() =>
                new AutoDataAttribute((IFixture)null));
            // Teardown
        }

        [TestMethod]
        public void InitializedWithComposerHasCorrectComposer()
        {
            // Fixture setup
            var expectedComposer = new DelegatingFixture();
            var sut = new AutoDataAttribute(expectedComposer);
            // Exercise system
            var result = sut.Fixture;
            // Verify outcome
            Assert.AreEqual(expectedComposer, result);
            // Teardown
        }

        [TestMethod]
        public void InitializeWithNullTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.ThrowsException<ArgumentNullException>(() =>
#pragma warning disable 618
                new AutoDataAttribute((Type)null));
#pragma warning disable 618
            // Teardown
        }

        [TestMethod]
        public void InitializeWithNonComposerTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.ThrowsException<ArgumentException>(() =>
                new AutoDataAttribute(typeof(object)));
            // Teardown
        }

        [TestMethod]
        public void InitializeWithComposerTypeWithoutDefaultConstructorThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.ThrowsException<ArgumentException>(() =>
                new AutoDataAttribute(typeof(ComposerWithoutADefaultConstructor)));
            // Teardown
        }

        [TestMethod]
        public void InitializedWithCorrectComposerTypeHasCorrectComposer()
        {
            // Fixture setup
            var composerType = typeof(DelegatingFixture);
            var sut = new AutoDataAttribute(composerType);
            // Exercise system
            var result = sut.Fixture;
            // Verify outcome
            Assert.IsInstanceOfType(result, composerType);
            // Teardown
        }

        [TestMethod]
        public void FixtureTypeIsCorrect()
        {
            // Fixture setup
            var composerType = typeof(DelegatingFixture);
            var sut = new AutoDataAttribute(composerType);
            // Exercise system
            var result = sut.FixtureType;
            // Verify outcome
            Assert.AreEqual(composerType, result);
            // Teardown
        }

        [TestMethod]
        public void GetDataWithNullMethodThrows()
        {
            // Fixture setup
            var sut = new AutoDataAttribute();
            var dummyTypes = Type.EmptyTypes;
            // Exercise system and verify outcome
            Assert.ThrowsException<ArgumentNullException>(() =>
                sut.GetData(null));
            // Teardown
        }

        [TestMethod]
        public void GetDataReturnsCorrectResult()
        {
            // Fixture setup
            var method = typeof(TypeWithOverloadedMembers).GetMethod("DoSomething", new[] { typeof(object) });
            var parameters = method.GetParameters();

            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) =>
                    {
                        Assert.AreEqual(parameters.Single(), r);
                        Assert.IsNotNull(c);
                        return expectedResult;
                    }
            };
            var composer = new DelegatingFixture { OnCreate = builder.OnCreate };

            var sut = new AutoDataAttribute(composer);
            // Exercise system
            var testMethod = sut.ToTestMethod(method);
            var result = sut.GetData(testMethod);
            // Verify outcome
            Assert.IsTrue(new[] { expectedResult }.SequenceEqual(result.Single()));
            // Teardown
        }

        [AdvancedDataTestMethod]
        [InlineData("CreateWithFrozenAndFavorArrays")]
        [InlineData("CreateWithFavorArraysAndFrozen")]
        [InlineData("CreateWithFrozenAndFavorEnumerables")]
        [InlineData("CreateWithFavorEnumerablesAndFrozen")]
        [InlineData("CreateWithFrozenAndFavorLists")]
        [InlineData("CreateWithFavorListsAndFrozen")]
        [InlineData("CreateWithFrozenAndGreedy")]
        [InlineData("CreateWithGreedyAndFrozen")]
        [InlineData("CreateWithFrozenAndModest")]
        [InlineData("CreateWithModestAndFrozen")]
        [InlineData("CreateWithFrozenAndNoAutoProperties")]
        [InlineData("CreateWithNoAutoPropertiesAndFrozen")]
        public void GetDataOrdersCustomizationAttributes(string methodName)
        {
            // Fixture setup
            var method = typeof(TypeWithCustomizationAttributes).GetMethod(methodName, new[] { typeof(ConcreteType) });
            var customizationLog = new List<ICustomization>();
            var fixture = new DelegatingFixture();
            fixture.OnCustomize = c =>
            {
                customizationLog.Add(c);
                return fixture;
            };
            var sut = new AutoDataAttribute(fixture);
            // Exercise system
            var testMethod = sut.ToTestMethod(method);
            var result = sut.GetData(testMethod);
            // Verify outcome
            Assert.IsFalse(customizationLog[0] is FreezeOnMatchCustomization);
            Assert.IsTrue(customizationLog[1] is FreezeOnMatchCustomization);
            // Teardown
        }
    }
}