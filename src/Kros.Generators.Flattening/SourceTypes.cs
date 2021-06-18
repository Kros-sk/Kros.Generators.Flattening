using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Gets or set the type of the source class.
        /// </summary>
        public Type SourceType { get; init; }

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
        /// Gets or set the name of the source property. (e.g.: `Contact.Addres.Streed`)
        /// </summary>
        public string SourcePropertyName { get; init; }

        /// <summary>
        /// Get or set new name for property.
        /// </summary>
        public string Name { get; init; }
    }

    /// <summary>
    /// Interface which describe flat class.
    /// </summary>
    /// <typeparam name="TDest">The type of the dest.</typeparam>
    public interface IFlat<TSource>
    {
        /// <summary>
        /// Converts to full domain.
        /// </summary>
        TSource ToFull();
    }

    /// <summary>
    /// <see cref="IEnumerable{T}"/> extensions.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Converts to full domains.
        /// </summary>
        /// <typeparam name="TFlat">The type of the flat class.</typeparam>
        /// <typeparam name="TFull">The type of the full domain class.</typeparam>
        /// <param name="flats">The flats.</param>
        public static IEnumerable<TFull> ToFull<TFlat, TFull>(this IEnumerable<TFlat> flats)
            where TFlat : IFlat<TFull>
            => flats.Select(f => f.ToFull());
    }
}
