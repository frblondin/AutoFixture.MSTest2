using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.TestFramework.AdvancedDataRow;
using System.Linq;

namespace Ploeh.AutoFixture.MSTest2.UnitTest
{
    public class DependencyConstraints
    {
        [AdvancedDataTestMethod]
        [InlineData("FakeItEasy")]
        [InlineData("Foq")]
        [InlineData("FsCheck")]
        [InlineData("Moq")]
        [InlineData("NSubstitute")]
        [InlineData("nunit.framework")]
        [InlineData("Rhino.Mocks")]
        [InlineData("Unquote")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void AutoFixtureXunit2DoesNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = typeof(AutoDataAttribute).Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.IsFalse(references.Any(an => an.Name == assemblyName));
            // Teardown
        }

        [AdvancedDataTestMethod]
        [InlineData("FakeItEasy")]
        [InlineData("Foq")]
        [InlineData("FsCheck")]
        [InlineData("Moq")]
        [InlineData("NSubstitute")]
        [InlineData("nunit.framework")]
        [InlineData("Rhino.Mocks")]
        [InlineData("Unquote")]
        [InlineData("xunit")]
        [InlineData("xunit.extensions")]
        public void AutoFixtureXunit2UnitTestsDoNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = this.GetType().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.IsFalse(references.Any(an => an.Name == assemblyName));
            // Teardown
        }
    }
}