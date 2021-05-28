﻿using System;
using System.Collections.Generic;

namespace Kros.Generators.Flattening.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public MyEnum Foo { get; set; }

        string WithoutModifier { get; set; }

        protected string Protected { get; set; }

        internal string Internal { get; set; }

        private string Private { get; set; }

        public List<string> Addresses { get; set; }
    }

    [Flatten(SourceType = typeof(Person))]
    public partial class PersonFlat
    {

    }

    public enum MyEnum
    {
        Value1
    }
}
