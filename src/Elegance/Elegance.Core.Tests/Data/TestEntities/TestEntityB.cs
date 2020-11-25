using Elegance.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elegance.Core.Tests.Data.TestEntities
{
    public class TestEntityB
    {
        [Column("property_bigint")]
        [AlternateAlias("PropertyBigInt")]
        public long PropertyBigInt { get; set; }
    }
}
