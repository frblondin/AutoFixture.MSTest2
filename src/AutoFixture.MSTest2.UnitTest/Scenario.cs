using System;
using System.Linq;
using Ploeh.TestTypeFoundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.TestFramework.AdvancedDataRow;

namespace Ploeh.AutoFixture.MSTest2.UnitTest
{
    public class Scenario
    {
        [AdvancedDataTestMethod, AutoData]
        public void AutoDataProvidesCorrectInteger(int primitiveValue)
        {
            Assert.AreNotEqual(0, primitiveValue);
        }

        [AdvancedDataTestMethod, AutoData]
        public void AutoDataProvidesCorrectString(string text)
        {
            Assert.IsTrue(text.StartsWith("text"));
        }

        [AdvancedDataTestMethod, AutoData]
        public void AutoDataProvidesCorrectObject(PropertyHolder<Version> ph)
        {
            Assert.IsNotNull(ph);
            Assert.IsNotNull(ph.Property);
        }

        [AdvancedDataTestMethod, AutoData]
        public void AutoDataProvidesMultipleObjects(PropertyHolder<Version> ph, SingleParameterType<OperatingSystem> spt)
        {
            Assert.IsNotNull(ph);
            Assert.IsNotNull(ph.Property);

            Assert.IsNotNull(spt);
            Assert.IsNotNull(spt.Parameter);
        }

#pragma warning disable 618
        [AdvancedDataTestMethod, AutoData(typeof(CustomizedFixture))]
#pragma warning restore 618
        public void AutoDataProvidesCustomizedObject(PropertyHolder<string> ph)
        {
            Assert.AreEqual("Ploeh", ph.Property);
        }

        [AdvancedDataTestMethod]
        [InlineAutoData("foo")]
        [InlineAutoData("foo", "bar")]
        public void InlineAutoDataUsesSuppliedDataValues(string s1, string s2)
        {
            Assert.AreEqual("foo", s1);
            Assert.IsNotNull(s2);
        }

        [AdvancedDataTestMethod]
        [InlineAutoData("foo")]
        [InlineAutoData("foo", "bar")]
        public void InlineAutoDataSuppliesDataSpecimens(string s1, string s2, MyClass myClass)
        {
            Assert.AreEqual("foo", s1);
            Assert.IsNotNull(s2);
            Assert.IsNotNull(myClass);
        }

        [AdvancedDataTestMethod]
        [InlineAutoData("foo")]
        [InlineAutoData("foo", "bar")]
        public void InlineAutoDataSuppliesDataSpecimensOnlyForNonProvidedValues(string s1, string s2, string s3)
        {
            Assert.AreEqual("foo", s1);
            Assert.IsNotNull(s2);
            Assert.AreNotEqual("foo", s3);
            Assert.AreNotEqual("bar", s3);
        }

        // This test and its associated types is used to document one of the
        // InlineAutoDataAttribute constructor overloads.
        [AdvancedDataTestMethod]
        [MyCustomInlineAutoData(1337)]
        [MyCustomInlineAutoData(1337, 7)]
        [MyCustomInlineAutoData(1337, 7, 42)]
        public void CustomInlineDataSuppliesExtraValues(int x, int y, int z)
        {
            Assert.AreEqual(1337, x);
            // y can vary, so we can't express any meaningful assertion for it.
            Assert.AreEqual(42, z);
        }

        private class MyCustomInlineAutoDataAttribute : InlineAutoDataAttribute
        {
            public MyCustomInlineAutoDataAttribute(params object[] values) :
                base(new MyCustomAutoDataAttribute(), values)
            {
            }
        }

        private class MyCustomAutoDataAttribute : AutoDataAttribute
        {
            public MyCustomAutoDataAttribute() :
                base(new Fixture().Customize(new TheAnswer()))
            {
            }

            private class TheAnswer : ICustomization
            {
                public void Customize(IFixture fixture)
                {
                    fixture.Inject(42);
                }
            }
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameter([Frozen]Guid g1, Guid g2)
        {
            Assert.AreEqual(g1, g2);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeSecondParameterOnlyFreezesSubsequentParameters(Guid g1, [Frozen]Guid g2, Guid g3)
        {
            Assert.AreNotEqual(g1, g2);
            Assert.AreNotEqual(g1, g3);

            Assert.AreEqual(g2, g3);
        }

        [AdvancedDataTestMethod, AutoData]
        public void IntroductoryTest(
            int expectedNumber, MyClass sut)
        {
            // Fixture setup
            // Exercise system
            int result = sut.Echo(expectedNumber);
            // Verify outcome
            Assert.AreEqual(expectedNumber, result);
            // Teardown
        }

        [AdvancedDataTestMethod, AutoData]
        public void ModestCreatesParameterWithModestConstructor([Modest]MultiUnorderedConstructorType p)
        {
            Assert.IsTrue(string.IsNullOrEmpty(p.Text));
            Assert.AreEqual(0, p.Number);
        }

        [AdvancedDataTestMethod, AutoData]
        public void GreedyCreatesParameterWithGreedyConstructor([Greedy]MultiUnorderedConstructorType p)
        {
            Assert.IsFalse(string.IsNullOrEmpty(p.Text));
            Assert.AreNotEqual(0, p.Number);
        }

        [AdvancedDataTestMethod, AutoData]
        public void BothFrozenAndGreedyAttributesCanBeAppliedToSameParameter([Frozen][Greedy]MultiUnorderedConstructorType p1, MultiUnorderedConstructorType p2)
        {
            Assert.IsFalse(string.IsNullOrEmpty(p2.Text));
            Assert.AreNotEqual(0, p2.Number);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FavorArraysCausesArrayConstructorToBeInjectedWithFrozenItems([Frozen]int[] numbers, [FavorArrays]ItemContainer<int> container)
        {
            Assert.IsTrue(numbers.SequenceEqual(container.Items));
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterAsBaseTypeAssignsSameInstanceToSecondParameterOfThatBaseType(
#pragma warning disable 0618
            [Frozen(As = typeof(AbstractType))]ConcreteType p1,
#pragma warning restore 0618
            AbstractType p2)
        {
            Assert.AreSame(p1, p2);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterAsNullTypeAssignsSameInstanceToSecondParameterOfSameType(
#pragma warning disable 0618
            [Frozen(As = null)]ConcreteType p1,
#pragma warning restore 0618
            ConcreteType p2)
        {
            Assert.AreSame(p1, p2);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterShouldAssignSameInstanceToSecondParameter(
            [Frozen]string p1,
            string p2)
        {
            Assert.AreEqual(p1, p2);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByExactTypeShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ExactType)]ConcreteType p1,
            ConcreteType p2)
        {
            Assert.AreEqual(p1, p2);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByExactTypeShouldNotAssignSameInstanceToSecondParameterOfDifferentType(
            [Frozen(Matching.ExactType)]ConcreteType p1,
            object p2)
        {
            Assert.AreNotEqual(p1, p2);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByDirectBaseTypeShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.DirectBaseType)]ConcreteType p1,
            AbstractType p2)
        {
            Assert.AreEqual(p1, p2);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByDirectBaseTypeShouldNotAssignSameInstanceToSecondParameterOfIndirectBaseType(
            [Frozen(Matching.DirectBaseType)]ConcreteType p1,
            object p2)
        {
            Assert.AreNotEqual(p1, p2);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByDirectBaseTypeShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.DirectBaseType)]ConcreteType p1,
            ConcreteType p2)
        {
            Assert.AreNotEqual(p1, p2);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByExactOrDirectBaseTypeShouldAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.ExactType | Matching.DirectBaseType)]ConcreteType p1,
            ConcreteType p2)
        {
            Assert.AreEqual(p1, p2);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByInterfaceShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ImplementedInterfaces)]NoopInterfaceImplementer p1,
            IInterface p2)
        {
            Assert.AreEqual(p1, p2);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByInterfaceShouldNotAssignSameInstanceToSecondParameterOfNonInterfaceType(
            [Frozen(Matching.ImplementedInterfaces)]NoopInterfaceImplementer p1,
            object p2)
        {
            Assert.AreNotEqual(p1, p2);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByInterfaceShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.ImplementedInterfaces)]NoopInterfaceImplementer p1,
            NoopInterfaceImplementer p2)
        {
            Assert.AreNotEqual(p1, p2);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByDirectOrInterfaceShouldAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.ExactType | Matching.ImplementedInterfaces)]NoopInterfaceImplementer p1,
            NoopInterfaceImplementer p2)
        {
            Assert.AreEqual(p1, p2);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByParameterWithSameNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ParameterName)]string parameter,
            SingleParameterType<object> p2)
        {
            Assert.AreEqual(parameter, p2.Parameter);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByParameterWithDifferentNameShouldNotAssignSameInstanceToSecondParameter(
            [Frozen(Matching.ParameterName)]string p1,
            SingleParameterType<object> p2)
        {
            Assert.AreNotEqual(p1, p2.Parameter);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByParameterWithDifferentNameShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.ParameterName)]string p1,
            SingleParameterType<string> p2)
        {
            Assert.AreNotEqual(p1, p2.Parameter);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByPropertyWithSameNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.PropertyName)]string property,
            PropertyHolder<object> p2)
        {
            Assert.AreEqual(property, p2.Property);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByPropertyWithDifferentNameShouldNotAssignSameInstanceToSecondParameter(
            [Frozen(Matching.PropertyName)]string p1,
            PropertyHolder<object> p2)
        {
            Assert.AreNotEqual(p1, p2.Property);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByPropertyWithDifferentNameShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.PropertyName)]string p1,
            PropertyHolder<string> p2)
        {
            Assert.AreNotEqual(p1, p2.Property);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByFieldWithSameNameShouldAssignSameInstanceToSecondParameter(
            [Frozen(Matching.FieldName)]string field,
            FieldHolder<object> p2)
        {
            Assert.AreEqual(field, p2.Field);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByFieldWithDifferentNameShouldNotAssignSameInstanceToSecondParameter(
            [Frozen(Matching.FieldName)]string p1,
            FieldHolder<object> p2)
        {
            Assert.AreNotEqual(p1, p2.Field);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByFieldWithDifferentNameShouldNotAssignSameInstanceToSecondParameterOfSameType(
            [Frozen(Matching.FieldName)]string p1,
            FieldHolder<string> p2)
        {
            Assert.AreNotEqual(p1, p2.Field);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingParameter(
            [Frozen(Matching.MemberName)]string parameter,
            SingleParameterType<object> p2)
        {
            Assert.AreEqual(parameter, p2.Parameter);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToParameter(
            [Frozen(Matching.MemberName)]string p1,
            SingleParameterType<object> p2)
        {
            Assert.AreNotEqual(p1, p2.Parameter);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToParameterOfSameType(
            [Frozen(Matching.MemberName)]string p1,
            SingleParameterType<string> p2)
        {
            Assert.AreNotEqual(p1, p2.Parameter);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingProperty(
            [Frozen(Matching.MemberName)]string property,
            PropertyHolder<object> p2)
        {
            Assert.AreEqual(property, p2.Property);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToProperty(
            [Frozen(Matching.MemberName)]string p1,
            PropertyHolder<object> p2)
        {
            Assert.AreNotEqual(p1, p2.Property);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToPropertyOfSameType(
            [Frozen(Matching.MemberName)]string p1,
            PropertyHolder<string> p2)
        {
            Assert.AreNotEqual(p1, p2.Property);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByMemberWithSameNameShouldAssignSameInstanceToMatchingField(
            [Frozen(Matching.MemberName)]string field,
            FieldHolder<object> p2)
        {
            Assert.AreEqual(field, p2.Field);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToField(
            [Frozen(Matching.MemberName)]string p1,
            FieldHolder<object> p2)
        {
            Assert.AreNotEqual(p1, p2.Field);
        }

        [AdvancedDataTestMethod, AutoData]
        public void FreezeFirstParameterByMemberWithDifferentNameShouldNotAssignSameInstanceToFieldOfSameType(
            [Frozen(Matching.MemberName)]string p1,
            FieldHolder<string> p2)
        {
            Assert.AreNotEqual(p1, p2.Field);
        }
    }
}