using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Tests.Data.TestEntities
{
    public class TestEntityA
    {
        public long Id { get; set; }

        public string Name { get; set; }


        public static bool AreEqual(TestEntityA expected, TestEntityA actual)
        {
            return expected.Id == actual.Id 
                && expected.Name == actual.Name;
        }
    }
}
