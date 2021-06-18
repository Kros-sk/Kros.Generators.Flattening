using System.Collections.Generic;

namespace Kros.Generators.Flattening
{
    internal class SourcePropertyModel
    {
        public string Name { get; set; }

        public string TargetPropertyName { get; set; }

        public List<SourcePropertyModel> SubProperties { get; set; } = new();

        public override string ToString() => $"{Name} -> {TargetPropertyName}";
    }
}
