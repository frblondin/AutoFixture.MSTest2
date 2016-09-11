using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.TestFramework.AdvancedDataRow;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.AutoFixture.MSTest2.UnitTest.Extensions
{
    public static class DataAttributeExtensions
    {
        public static ITestMethod ToTestMethod(this DataAttribute source, MethodInfo method)
        {
            var result = Substitute.For<ITestMethod>();
            result.MethodInfo.Returns(method);
            result.GetAttributes<DataAttribute>(false).Returns(new DataAttribute[] { source });
            return result;
        }
    }
}
