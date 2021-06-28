using AutoBogus;
using AutoBogus.Conventions;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace Kros.Generators.Flattening.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoFaker.Configure(builder =>
            {
                builder.WithConventions();
            });

            DocumentFlat flat = AutoFaker.Generate<DocumentFlat>();

            Document document = flat;

            DocumentFlat flat2 = (DocumentFlat)document;

            IEnumerable<DocumentFlat> flatDocuments = AutoFaker.Generate<DocumentFlat>(10);
            IEnumerable<Document> documents = flatDocuments.ToComplex<DocumentFlat, Document>();
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

    public record Address(string City, string Street);

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
    }
}
