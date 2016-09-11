using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.TestFramework.AdvancedDataRow;
using Ploeh.AutoFixture.MSTest2.UnitTest.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.MSTest2.UnitTest
{
    [TestClass]
    public class CompositeDataAttributeTest
    {
        [TestMethod]
        public void SutIsDataAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeDataAttribute();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(DataAttribute));
            // Teardown
        }

        [TestMethod]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.ThrowsException<ArgumentNullException>(() =>
                new CompositeDataAttribute(null));
            // Teardown
        }

        [TestMethod]
        public void AttributesIsCorrectWhenInitializedWithArray()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;

            var attributes = new[]
            {
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeDataAttribute(attributes);
            // Exercise system
            IEnumerable<DataAttribute> result = sut.Attributes;
            // Verify outcome
            Assert.IsTrue(attributes.SequenceEqual(result));
            // Teardown
        }

        [TestMethod]
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.ThrowsException<ArgumentNullException>(() =>
                new CompositeDataAttribute((IReadOnlyCollection<DataAttribute>)null));
            // Teardown
        }

        [TestMethod]
        public void AttributesIsCorrectWhenInitializedWithEnumerable()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;

            var attributes = new[]
            {
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeDataAttribute(attributes);
            // Exercise system
            var result = sut.Attributes;
            // Verify outcome
            Assert.IsTrue(attributes.SequenceEqual(result));
            // Teardown
        }

        [TestMethod]
        public void GetDataWithNullMethodThrows()
        {
            // Fixture setup
            var sut = new CompositeDataAttribute();
            // Exercise system and verify outcome
            Assert.ThrowsException<ArgumentNullException>(() =>
                sut.GetData(null).ToList());
            // Teardown
        }

        [TestMethod]
        public void GetDataOnMethodWithNoParametersReturnsNoTheory()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var sut = new CompositeDataAttribute(
               new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, Enumerable.Empty<object[]>())
               );

            // Exercise system and verify outcome
            var testMethod = sut.ToTestMethod(a.Method);
            var result = sut.GetData(testMethod);
            Array.ForEach(result.ToArray(), d => Assert.IsTrue(d.Length == 0));
            // Teardown
        }
    }
}