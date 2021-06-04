using Kros.Generators.Flattening.IntegrationTests.Domains;
using Xunit;

namespace Kros.Generators.Flattening.IntegrationTests
{
    public class ComplexDomainClassShould
    {
        [Fact]
        public void BeFlatten()
        {

        }

        [Fact]
        public void SkipDefinedSubProperties()
        {

        }

        [Fact]
        public void DoNotFlatenDefinedProperties()
        {

        }

        [Fact]
        public void RenameDefinedProperties()
        {

        }

        [Flatten(SourceType = typeof(Document))]
        public partial class DocumentFlat
        {

        }

        [Flatten(SourceType = typeof(Document),
            Skip = new string[] { "Supplier.Name", "Supplier.PostalAddress.City" })]
        public partial class DocumentFlatWithSkipSubProperty
        {

        }

        [Flatten(SourceType = typeof(Document),
            DoNotFlatten = new string[] { nameof(Document.Supplier), "Document.Purchaser.Address" })]
        public partial class DocumentFlatWithNoFlattenedProperties
        {

        }

        [Flatten(SourceType = typeof(Document))]
        //[FlattenPropertyName(SourcePropertyName = "Supplier.Address.City", Name = "SupplierAddressTown")]
        //        [FlattenPropertyName(SourcePropertyName = "Purchaser.Address", Prefix = "Purchaser")]
        public partial class DocumentFlatWithRenamedPriperties
        {

        }
    }
}
