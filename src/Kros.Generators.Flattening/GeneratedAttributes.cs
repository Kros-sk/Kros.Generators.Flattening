using System;

namespace Kros.Generators.Flattening
{
    /// <summary>
    /// An attribute that indicates a flatt class.
    /// Properties from the domain source class will then be generated to partial class of this.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class FlattenAttribute: Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlattenAttribute"/> class.
        /// </summary>
        /// <param name="source">The source class type.</param>
        public FlattenAttribute(Type sourceType)
        {
            SourceType = sourceType;
        }

        /// <summary>
        /// Gets the type of the source class.
        /// </summary>
        public Type SourceType { get; }

        /// <summary>
        /// Gets or sets the properties for skip. These properties will not be generated to the flat class.
        /// </summary>
        /// <value>
        /// The skip.
        /// </value>
        public string[] Skip { get; init; }

        /// <summary>
        /// Gets or set properties that do not be flattened. These properties are taken as defined in the source class.
        /// </summary>
        public string[] DoNotFlatten { get; init; }
    }

    /// <summary>
    /// An attribute for renaming generated properties.
    /// Properties from the domain source class will then be generated to partial class of this.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class FlattenPropertyNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlattenPropertyNameAttribute"/> class.
        /// </summary>
        /// <param name="source">The source property name.</param>
        public FlattenPropertyNameAttribute(string sourcePropertyName)
        {
            SourcePropertyName = sourcePropertyName;
        }

        /// <summary>
        /// Gets the name of the source property. (e.g.: `Contact.Addres.Streed`)
        /// </summary>
        public string SourcePropertyName { get; }

        /// <summary>
        /// Get or set new name for property.
        /// </summary>
        /// <remarks>
        /// Exactly one of the <see cref="Name"/> or <see cref="Prefix"/> properties must be set.
        /// </remarks>
        public string Name { get; init; }

        /// <summary>
        /// Gets or sets the prefix. (e.g.: `Contact`)
        /// </summary>
        /// <remarks>
        /// Exactly one of the <see cref="Name"/> or <see cref="Prefix"/> properties must be set.
        /// </remarks>
        public string Prefix { get; init; }
    }
}
