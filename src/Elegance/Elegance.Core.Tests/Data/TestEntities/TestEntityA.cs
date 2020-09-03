using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Tests.Data.TestEntities
{
    public class TestEntityA
    {
        public long PropertyBigInt { get; set; }
        public int PropertyInt { get; set; }
        public short PropertySmallInt { get; set; }
        public byte PropertyTinyInt { get; set; }
        public string PropertyVarChar { get; set; }
        public DateTime PropertyDateTime { get; set; }

        public static bool AreEqual(TestEntityA expected, TestEntityA actual)
        {
            return expected.PropertyBigInt == actual.PropertyBigInt
                && expected.PropertyInt == actual.PropertyInt
                && expected.PropertySmallInt == actual.PropertySmallInt
                && expected.PropertyTinyInt == actual.PropertyTinyInt
                && expected.PropertyVarChar == actual.PropertyVarChar
                && expected.PropertyDateTime == actual.PropertyDateTime;
        }
    }
}
