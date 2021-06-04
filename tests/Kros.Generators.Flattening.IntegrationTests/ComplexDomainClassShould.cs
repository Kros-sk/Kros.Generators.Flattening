using Kros.Generators.Flattening.IntegrationTests.Domains;
using Xunit;

namespace Kros.Generators.Flattening.IntegrationTests
{
    public class ComplexDomainClassShould
    {
        [Fact]
        public void BeFlatten()
        {
            var actual = new DocumentFlat();
            actual.Should()
                .HaveProperty(p => p.Id)
                .HaveProperty(p => p.Created)
                .HaveProperty(p => p.DocumentNumber)
                .HaveProperty(p => p.IssueDate)
                .HaveProperty(p => p.IsTotalPriceInclVat)
                .HaveProperty(p => p.Items)
                .HaveProperty(p => p.PurchaserBussinessName)
                .HaveProperty(p => p.PurchaserName)
                .HaveProperty(p => p.PurchaserAddressCity)
                .HaveProperty(p => p.PurchaserAddressStreet)
                .HaveProperty(p => p.PurchaserPostalAddressCity)
                .HaveProperty(p => p.PurchaserPostalAddressStreet)
                .HaveProperty(p => p.PurchaserRegistrationId)
                .HaveProperty(p => p.SupplierBussinessName)
                .HaveProperty(p => p.SupplierName)
                .HaveProperty(p => p.SupplierAddressCity)
                .HaveProperty(p => p.SupplierAddressStreet)
                .HaveProperty(p => p.SupplierPostalAddressCity)
                .HaveProperty(p => p.SupplierPostalAddressStreet)
                .HaveProperty(p => p.VatPayerType)
                .HaveProperty(p => p.VatTotalPrice)
                .NoOther();
        }

        [Fact]
        public void SkipDefinedSubProperties()
        {
            var actual = new DocumentFlatWithSkipSubProperty();
            actual.Should()
                .HaveProperty(p => p.Id)
                .HaveProperty(p => p.Created)
                .HaveProperty(p => p.DocumentNumber)
                .HaveProperty(p => p.IssueDate)
                .HaveProperty(p => p.IsTotalPriceInclVat)
                .HaveProperty(p => p.Items)
                .HaveProperty(p => p.PurchaserBussinessName)
                .HaveProperty(p => p.PurchaserName)
                .HaveProperty(p => p.PurchaserAddressCity)
                .HaveProperty(p => p.PurchaserAddressStreet)
                .HaveProperty(p => p.PurchaserPostalAddressCity)
                .HaveProperty(p => p.PurchaserPostalAddressStreet)
                .HaveProperty(p => p.PurchaserRegistrationId)
                .HaveProperty(p => p.SupplierBussinessName)
                .HaveProperty(p => p.SupplierAddressCity)
                .HaveProperty(p => p.SupplierAddressStreet)
                .HaveProperty(p => p.SupplierPostalAddressStreet)
                .HaveProperty(p => p.VatPayerType)
                .HaveProperty(p => p.VatTotalPrice)
                .NoOther();
        }

        [Fact]
        public void DoNotFlatenDefinedProperties()
        {
            var actual = new DocumentFlatWithNoFlattenedProperties();
            actual.Should()
                .HaveProperty(p => p.Id)
                .HaveProperty(p => p.Created)
                .HaveProperty(p => p.DocumentNumber)
                .HaveProperty(p => p.IssueDate)
                .HaveProperty(p => p.IsTotalPriceInclVat)
                .HaveProperty(p => p.Items)
                .HaveProperty(p => p.PurchaserBussinessName)
                .HaveProperty(p => p.PurchaserName)
                .HaveProperty(p => p.PurchaserAddress)
                .HaveProperty(p => p.PurchaserPostalAddressCity)
                .HaveProperty(p => p.PurchaserPostalAddressStreet)
                .HaveProperty(p => p.PurchaserRegistrationId)
                .HaveProperty(p => p.Supplier)
                .HaveProperty(p => p.VatPayerType)
                .HaveProperty(p => p.VatTotalPrice)
                .NoOther();
        }

        [Fact]
        public void RenameDefinedProperties()
        {
            var actual = new DocumentFlatWithRenamedPriperties();
            actual.Should()
                .HaveProperty(p => p.Id)
                .HaveProperty(p => p.Created)
                .HaveProperty(p => p.DocumentNumber)
                .HaveProperty(p => p.IssueDate)
                .HaveProperty(p => p.IsTotalPriceInclVat)
                .HaveProperty(p => p.Items)
                .HaveProperty(p => p.PurchaserBussinessName)
                .HaveProperty(p => p.PurchaserName)
                .HaveProperty(p => p.PurchaserCity)
                .HaveProperty(p => p.PurchaserStreet)
                .HaveProperty(p => p.PurchaserPostalAddressCity)
                .HaveProperty(p => p.PurchaserPostalAddressStreet)
                .HaveProperty(p => p.PurchaserRegistrationId)
                .HaveProperty(p => p.SupplierBussinessName)
                .HaveProperty(p => p.SupplierName)
                .HaveProperty(p => p.SupplierAddressStreet)
                .HaveProperty(p => p.SupplierAddressTown)
                .HaveProperty(p => p.SupplierPostalAddressCity)
                .HaveProperty(p => p.SupplierPostalAddressStreet)
                .HaveProperty(p => p.VatPayerType)
                .HaveProperty(p => p.VatTotalPrice)
                .NoOther();
        }
    }

    [Flatten(SourceType = typeof(Document))]
    public partial class DocumentFlat : BaseClass
    {

    }

    [Flatten(SourceType = typeof(Document),
        Skip = new string[] { "Supplier.Name", "Supplier.PostalAddress.City" })]
    public partial class DocumentFlatWithSkipSubProperty : BaseClass
    {

    }

    [Flatten(SourceType = typeof(Document),
        DoNotFlatten = new string[] { nameof(Document.Supplier), "Purchaser.Address" })]
    public partial class DocumentFlatWithNoFlattenedProperties : BaseClass
    {

    }

    [Flatten(SourceType = typeof(Document))]
    [FlattenPropertyName(SourcePropertyName = "Supplier.Address.City", Name = "Town")]
    [FlattenPropertyName(SourcePropertyName = "Purchaser.Address", Name = "")]
    public partial class DocumentFlatWithRenamedPriperties : BaseClass
    {

    }
}
