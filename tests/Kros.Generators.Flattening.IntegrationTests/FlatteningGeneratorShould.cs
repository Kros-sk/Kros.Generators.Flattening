using AutoBogus;
using FluentAssertions;
using FluentAssertions.Execution;
using Kros.Generators.Flattening.IntegrationTests.Domains;
using Xunit;

namespace Kros.Generators.Flattening.IntegrationTests
{
    public class FlatteningGeneratorShould
    {
        [Fact]
        public void MapFlatInstanceToComplex()
        {
            DocumentFlaForMapping flat = AutoFaker.Generate<DocumentFlaForMapping>();
            Document document = flat;

            using (new AssertionScope())
            {
                document.Created.Should().Be(flat.Created);
                document.DocumentNumber.Should().Be(flat.DocumentNumber);
                document.Id.Should().Be(flat.Id);
                document.IssueDate.Should().Be(flat.IssueDate);
                document.IsTotalPriceInclVat.Should().Be(flat.IsTotalPriceInclVat);
                document.Items.Should().BeEquivalentTo(flat.Items);
                document.Purchaser.Address.City.Should().Be(flat.PurchaserCity);
                document.Purchaser.Address.Street.Should().Be(flat.PurchaserStreet);
                document.Purchaser.BussinessName.Should().Be(flat.PurchaserBussinessName);
                document.Purchaser.Name.Should().Be(flat.PurchaserName);
                document.Purchaser.PostalAddress.City.Should().Be(flat.PurchaserPostalAddressCity);
                document.Purchaser.PostalAddress.Street.Should().Be(flat.PurchaserPostalAddressStreet);
                document.Purchaser.RegistrationId.Should().Be(flat.PurchaserRegistrationId);
                document.Supplier.Address.City.Should().Be(flat.SupplierAddressTown);
                document.Supplier.Address.Street.Should().Be(flat.SupplierAddressStreet);
                document.Supplier.BussinessName.Should().Be(flat.SupplierBussinessName);
                document.Supplier.Name.Should().Be(flat.SupplierName);
                document.Supplier.PostalAddress.City.Should().Be(flat.SupplierPostalAddressCity);
                document.Supplier.PostalAddress.Street.Should().Be(flat.SupplierPostalAddressStreet);
                document.VatPayerType.Should().Be(flat.VatPayerType);
                document.VatTotalPrice.Should().Be(flat.VatTotalPrice);
            }
        }

        [Fact]
        public void MapComplexInstanceToFlat()
        {
            Document document = AutoFaker.Generate<Document>();
            var flat = (DocumentFlaForMapping)document;

            using (new AssertionScope())
            {
                flat.Created.Should().Be(document.Created);
                flat.DocumentNumber.Should().Be(document.DocumentNumber);
                flat.Id.Should().Be(document.Id);
                flat.IssueDate.Should().Be(document.IssueDate);
                flat.IsTotalPriceInclVat.Should().Be(document.IsTotalPriceInclVat);
                flat.Items.Should().BeEquivalentTo(document.Items);
                flat.PurchaserCity.Should().Be(document.Purchaser.Address.City);
                flat.PurchaserStreet.Should().Be(document.Purchaser.Address.Street);
                flat.PurchaserBussinessName.Should().Be(document.Purchaser.BussinessName);
                flat.PurchaserName.Should().Be(document.Purchaser.Name);
                flat.PurchaserPostalAddressCity.Should().Be(document.Purchaser.PostalAddress.City);
                flat.PurchaserPostalAddressStreet.Should().Be(document.Purchaser.PostalAddress.Street);
                flat.PurchaserRegistrationId.Should().Be(document.Purchaser.RegistrationId);
                flat.SupplierAddressTown.Should().Be(document.Supplier.Address.City);
                flat.SupplierAddressStreet.Should().Be(document.Supplier.Address.Street);
                flat.SupplierBussinessName.Should().Be(document.Supplier.BussinessName);
                flat.SupplierName.Should().Be(document.Supplier.Name);
                flat.SupplierPostalAddressCity.Should().Be(document.Supplier.PostalAddress.City);
                flat.SupplierPostalAddressStreet.Should().Be(document.Supplier.PostalAddress.Street);
                flat.VatPayerType.Should().Be(document.VatPayerType);
                flat.VatTotalPrice.Should().Be(document.VatTotalPrice);
            }
        }

        [Fact]
        public void MapComplexInstanceToFlatWhenSomePropertiesAreNull()
        {
            Document document = AutoFaker.Generate<Document>();
            document.Purchaser = null;
            document.Supplier.Address = null;

            var flat = (DocumentFlaForMapping)document;

            using (new AssertionScope())
            {
                flat.Created.Should().Be(document.Created);
                flat.DocumentNumber.Should().Be(document.DocumentNumber);
                flat.Id.Should().Be(document.Id);
                flat.IssueDate.Should().Be(document.IssueDate);
                flat.IsTotalPriceInclVat.Should().Be(document.IsTotalPriceInclVat);
                flat.Items.Should().BeEquivalentTo(document.Items);
                flat.PurchaserCity.Should().BeNull();
                flat.PurchaserStreet.Should().BeNull();
                flat.PurchaserBussinessName.Should().BeNull();
                flat.PurchaserName.Should().BeNull();
                flat.PurchaserPostalAddressCity.Should().BeNull();
                flat.PurchaserPostalAddressStreet.Should().BeNull();
                flat.PurchaserRegistrationId.Should().BeNull();
                flat.SupplierAddressTown.Should().BeNull();
                flat.SupplierAddressStreet.Should().BeNull();
                flat.SupplierBussinessName.Should().Be(document.Supplier.BussinessName);
                flat.SupplierName.Should().Be(document.Supplier.Name);
                flat.SupplierPostalAddressCity.Should().Be(document.Supplier.PostalAddress.City);
                flat.SupplierPostalAddressStreet.Should().Be(document.Supplier.PostalAddress.Street);
                flat.VatPayerType.Should().Be(document.VatPayerType);
                flat.VatTotalPrice.Should().Be(document.VatTotalPrice);
            }
        }
    }

    [Flatten(SourceType = typeof(Document))]
    [FlattenPropertyName(SourcePropertyName = "Supplier.Address.City", Name = "Town")]
    [FlattenPropertyName(SourcePropertyName = "Purchaser.Address", Name = "")]
    public partial class DocumentFlaForMapping
    {

    }
}
