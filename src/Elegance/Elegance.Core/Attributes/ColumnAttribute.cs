using System;
using System.Collections.Generic;
using System.Text;

namespace Elegance.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(string name)
        {
            Name = name;
        }

        internal string Name { get; private set; }
    }
}
