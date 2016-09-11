using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.TestFramework.AdvancedDataRow;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.MSTest2.UnitTest
{
    [TestClass]
    public class InlineAutoDataAttributeTest
    {
        [TestMethod]
        public void SutIsCompositeDataAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new InlineAutoDataAttribute();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(CompositeDataAttribute));
            // Teardown
        }

        [TestMethod]
        public void SutComposesDataAttributesInCorrectOrder()
        {
            // Fixture setup
            var sut = new InlineAutoDataAttribute();
            var expected = new[] { typeof(InlineDataAttribute), typeof(AutoDataAttribute) };
            // Exercise system
            IEnumerable<DataAttribute> result = sut.Attributes;
            // Verify outcome
            Assert.IsTrue(result.Select(d => d.GetType()).SequenceEqual(expected));
            // Teardown
        }

        [TestMethod]
        public void AttributesContainsAttributeWhenConstructedWithExplicitAutoDataAttribute()
        {
            // Fixture setup
            var autoDataAttribute = new AutoDataAttribute();
            var sut = new InlineAutoDataAttribute(autoDataAttribute);
            // Exercise system
            var result = sut.Attributes;
            // Verify outcome
            CollectionAssert.Contains((System.Collections.ICollection)result, autoDataAttribute);
            // Teardown
        }

        [TestMethod]
        public void AttributesContainsCorrectAttributeTypesWhenConstructorWithExplicitAutoDataAttribute()
        {
            // Fixture setup
            var autoDataAttribute = new AutoDataAttribute();
            var sut = new InlineAutoDataAttribute(autoDataAttribute);
            // Exercise system
            var result = sut.Attributes;
            // Verify outcome
            var expected = new[] { typeof(InlineDataAttribute), autoDataAttribute.GetType() };
            Assert.IsTrue(result.Select(d => d.GetType()).SequenceEqual(expected));
            // Teardown
        }

        [TestMethod]
        public void ValuesWillBeEmptyWhenSutIsCreatedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new InlineAutoDataAttribute();
            var expected = Enumerable.Empty<object>();
            // Exercise system
            var result = sut.Values;
            // Verify outcome
            CollectionAssert.AreEqual((System.Collections.ICollection)expected, (System.Collections.ICollection)result);
            // Teardown
        }

        [TestMethod]
        public void ValuesWillNotBeEmptyWhenSutIsCreatedWithConstructorArguments()
        {
            // Fixture setup
            var expectedValues = new[] { new object(), new object(), new object() };
            var sut = new InlineAutoDataAttribute(expectedValues);
            // Exercise system
            var result = sut.Values;
            // Verify outcome
            Assert.IsTrue(result.SequenceEqual(expectedValues));
            // Teardown
        }

        [TestMethod]
        public void ValuesAreCorrectWhenConstructedWithExplicitAutoDataAttribute()
        {
            // Fixture setup
            var dummyAutoDataAttribute = new AutoDataAttribute();
            var expectedValues = new[] { new object(), new object(), new object() };
            var sut = new InlineAutoDataAttribute(dummyAutoDataAttribute, expectedValues);
            // Exercise system
            var result = sut.Values;
            // Verify outcome
            Assert.IsTrue(expectedValues.SequenceEqual(result));
            // Teardown
        }

        [TestMethod]
        public void AutoDataAttributeIsCorrectWhenCreatedWithModestConstructor()
        {
            // Fixture setup
            var sut = new InlineAutoDataAttribute();
            // Exercise system
            AutoDataAttribute result = sut.AutoDataAttribute;
            // Verify outcome
            Assert.IsNotNull(result);
            // Teardown
        }

        [TestMethod]
        public void AutoDataAttributeIsCorrectWhenCreatedExplicitlyByConstructor()
        {
            // Fixture setup
            var expected = new AutoDataAttribute();
            var sut = new InlineAutoDataAttribute(expected);
            // Exercise system
            var result = sut.AutoDataAttribute;
            // Verify outcome
            Assert.AreEqual(expected, result);
            // Teardown
        }
    }
}