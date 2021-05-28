using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;

namespace Kros.Generators.Flattening.IntegrationTests
{
    public static class Helpers
    {
        public class AssertionHelper<T> where T : class
        {

            private HashSet<string> _properties = new();

            public AssertionHelper<T> HaveProperty<TProperty>(Expression<Func<T, TProperty>> propertyExptession)
            {
                _properties.Add(PropertyName<T>.GetPropertyName(propertyExptession));

                return this;
            }

            public void HaveNoProperties()
            {
                var actual = GetProperties();
                actual.Should().BeEmpty();
            }

            public void NoOther()
            {
                var actual = GetProperties();
                actual.Should().BeEquivalentTo(_properties);
            }

            private static IEnumerable<string> GetProperties()
                => typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(p => p.Name);
        }

        public static AssertionHelper<T> Should<T>(this T _)
            where T : BaseClass
            => new();
    }
}
