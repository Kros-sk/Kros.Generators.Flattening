using System.Collections.Generic;
using System.Linq;

namespace Kros.Generators.Flattening.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            DocumentFlat flat = new();

            flat.OwnerAddressTown = "Žilina";
            flat.OwnerAddressStreet = "Rudnaya";
            flat.OwnerName = "Milan";
            flat.CollaboratorCity = "Skalité";
            flat.CollaboratorStreet = "Rieky";
            flat.CollaboratorName = "Jano";
            flat.Name = "new document";

            Document document = flat;

            DocumentFlat f = (DocumentFlat)document;
        }
    }

    public class Document
    {
        public Contact Owner { get; set; }

        public Contact Collaborator { get; set; }

        public string Name { get; set; }

        public Settings Settings { get; set; }
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

    public class Settings
    {
        public string Currency { get; set; }
    }

    [Flatten(SourceType = typeof(Document),
        DoNotFlatten = new string[] { nameof(Document.Settings) })]
    [FlattenPropertyName(SourcePropertyName = "Owner.Address.City", Name = "Town")]
    [FlattenPropertyName(SourcePropertyName = "Collaborator.Address", Name = "")]
    public partial class DocumentFlat
    {
        public virtual DocumentFlat From(Document document)
        {
            DocumentFlat ret = new();

            ret.CollaboratorCity = document.Collaborator.Address.City;

            return ret;
        }
    }
}
