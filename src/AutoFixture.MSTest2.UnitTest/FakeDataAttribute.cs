using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.TestFramework.AdvancedDataRow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixture.MSTest2.UnitTest
{
    public class FakeDataAttribute : DataAttribute
    {
        private readonly MethodInfo expectedMethod;
        private readonly IEnumerable<object[]> output;

        public FakeDataAttribute(MethodInfo expectedMethod, IEnumerable<object[]> output)
        {
            this.expectedMethod = expectedMethod;
            this.output = output;
        }

        public override IEnumerable<object[]> GetData(ITestMethod methodUnderTest)
        {
            Assert.AreEqual(this.expectedMethod, methodUnderTest.MethodInfo);

            return this.output;
        }
    }
}