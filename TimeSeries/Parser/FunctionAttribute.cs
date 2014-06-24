using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Reclamation.TimeSeries.Parser
{

    // adapted from http://oreilly.com/catalog/progcsharp/chapter/ch18.html


    /// <summary>
    /// Contains the name, description, and example for Pisces functions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class |
       AttributeTargets.Constructor |
       AttributeTargets.Field |
       AttributeTargets.Method |
       AttributeTargets.Property,
       AllowMultiple = true)]
    public class FunctionAttribute: System.Attribute
    {
        
        public FunctionAttribute(string description, string example, string name="")
        {
            this.Description = description;
            this.Example = example;
            Name = name;
        }

        public string Name { get; set; }
        public string Example { get; set; }

        public string Description { get; set; }
    }
}
