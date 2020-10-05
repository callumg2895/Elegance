using System;
using System.Collections.Generic;
using System.Text;

namespace Elegance.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AlternateAliasAttribute : Attribute
    {
        public AlternateAliasAttribute(string name)
        {
            Name = name;
        }

        internal string Name { get; private set; }
    }
}
