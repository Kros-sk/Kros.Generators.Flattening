using System;
using Xunit;

namespace Kros.Generators.Flattening.IntegrationTests
{
    public class SimpleSourceClassShould
    {
        [Fact]
        public void BeFlattenToEmptyClassWhenDoesnotHaveProperties()
        {
            var actual = new EmptyClassFlat();

            actual.Should().HaveNoProperties();
        }

        [Fact]
        public void HaveAllProperties()
        {
            var actual = new ClassWithSimplePropertiesFlat();
            actual.Should()
                .HaveProperty(p => p.Id)
                .HaveProperty(p => p.Name)
                .HaveProperty(p => p.BirthDay)
                .HaveProperty(p => p.Salary)
                .HaveProperty(p => p.Age)
                .HaveProperty(p => p.IsMan)
                .NoOther();
        }

        [Fact]
        public void ShouldSkipDefinedProperties()
        {
            var actual = new ClassWithSimplePropertiesFlatWithSkipedProperties();
            actual.Should()
                .HaveProperty(p => p.Id)
                .HaveProperty(p => p.Name)
                .HaveProperty(p => p.BirthDay)
                .HaveProperty(p => p.Age)
                .NoOther();
        }

        [Fact]
        public void RenameDefinedProperties()
        {
            var actual = new ClassWithSimplePropertiesFlatWIthRenamedProperties();
            actual.Should()
                .HaveProperty(p => p.Id)
                .HaveProperty(p => p.FirstName)
                .HaveProperty(p => p.BirthDay)
                .HaveProperty(p => p.Payout)
                .HaveProperty(p => p.Age)
                .HaveProperty(p => p.IsMan)
                .NoOther();
        }

        [Fact]
        public void ShouldAcceptExistingProperties()
        {
            var actual = new ClassWithSimplePropertiesFlatAlreadyContainsProperty();
            actual.Should()
                .HaveProperty(p => p.Id)
                .HaveProperty(p => p.Name)
                .HaveProperty(p => p.BirthDay)
                .HaveProperty(p => p.Salary)
                .HaveProperty(p => p.Age)
                .HaveProperty(p => p.IsMan)
                .NoOther();
        }
    }

    public class EmptyClass { }

    [Flatten(SourceType = typeof(EmptyClass))]
    public partial class EmptyClassFlat : BaseClass { }

    public class ClassWithSimpleProperties : BaseClass
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDay { get; set; }

        public decimal Salary { get; set; }

        public double Age { get; set; }

        internal bool IsMan { get; set; }

        public int NameLength => Name.Length;

        public static string StaticProperty { get; set; }

        private int PrivateProperty { get; set; }

        protected int ProtectedProperty { get; set; }
    }

    [Flatten(SourceType = typeof(ClassWithSimpleProperties))]
    public partial class ClassWithSimplePropertiesFlat : BaseClass
    {
    }

    [Flatten(SourceType = typeof(ClassWithSimpleProperties),
        Skip = new string[] { nameof(ClassWithSimpleProperties.IsMan), nameof(ClassWithSimpleProperties.Salary) })]
    public partial class ClassWithSimplePropertiesFlatWithSkipedProperties : BaseClass
    {
    }

    [Flatten(SourceType = typeof(ClassWithSimpleProperties))]
    [FlattenPropertyName(SourcePropertyName = nameof(ClassWithSimpleProperties.Name), Name = "FirstName")]
    [FlattenPropertyName(SourcePropertyName = nameof(ClassWithSimpleProperties.Salary), Name = "Payout")]
    public partial class ClassWithSimplePropertiesFlatWIthRenamedProperties : BaseClass
    {
    }

    [Flatten(SourceType = typeof(ClassWithSimpleProperties))]
    public partial class ClassWithSimplePropertiesFlatAlreadyContainsProperty : BaseClass
    {
        public double Salary { get; set; }
    }
}
