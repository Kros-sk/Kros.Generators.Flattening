namespace Kros.Generators.Flattening.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            DocumentFlat document = new();

            document.OwnerAddressTown = "Žilina";
            document.OwnerAddressStreet = "Rudnaya";
            document.OwnerName = "Milan";
            document.CollaboratorCity = "Skalité";
            document.CollaboratorStreet = "Rieky";
            document.CollaboratorName = "Jano";
            document.Name = "new document";
        }
    }

    public class Document
    {
        public Contact Owner { get; set; }

        public Contact Collaborator { get; set; }

        public string Name { get; set; }
    }

    public class Contact
    {
        public string Name { get; set; }

        public Address Address { get; set; }
    }

    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
    }

    [Flatten(SourceType = typeof(Document))]
    [FlattenPropertyName(SourcePropertyName = "Owner.Address.City", Name = "Town")]
    [FlattenPropertyName(SourcePropertyName = "Collaborator.Address", Name = "")]
    public partial class DocumentFlat
    {
    }
}
