using System;
using System.Linq.Expressions;

namespace Kros.Generators.Flattening.IntegrationTests
{
    internal static class PropertyName<P> where P : class
    {
        public static string GetPropertyName<T>(Expression<Func<P, T>> expression)
        {
            var memberExpression = (MemberExpression)expression.Body;
            var propertyName = memberExpression.Member.Name;

            return propertyName;
        }
    }
}
