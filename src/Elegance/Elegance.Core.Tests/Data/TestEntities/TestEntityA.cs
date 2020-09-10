using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Tests.Data.TestEntities
{
    public enum TestEnumA : int
    {
        Value1 = 0,
        Value2 = 1,
        Value3 = 2,
    }

    public class TestEntityA
    {
        public long PropertyBigInt { get; set; }
        public int PropertyInt { get; set; }
        public short PropertySmallInt { get; set; }
        public byte PropertyTinyInt { get; set; }
        public string PropertyVarChar { get; set; }
        public DateTime PropertyDateTime { get; set; }
        public TestEnumA PropertyEnum { get; set; }
    }
}
