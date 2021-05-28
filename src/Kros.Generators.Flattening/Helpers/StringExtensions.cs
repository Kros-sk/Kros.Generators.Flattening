namespace Kros.Generators.Flattening
{
    internal static class StringExtensions
    {
        public static string TrimEnd(this string source, string value)
            => !source.EndsWith(value) ? source : source.Remove(source.LastIndexOf(value));
    }
}
