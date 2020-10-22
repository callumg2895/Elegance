using Elegance.Core.Attributes;
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
        [Column("property_bigint")] 
        [AlternateAlias("PropertyBigInt")]
        public long PropertyBigInt { get; set; }

        [Column("property_int")]
        [AlternateAlias("PropertyInt")]
        public int PropertyInt { get; set; }

        [Column("property_smallint")]
        [AlternateAlias("PropertySmallInt")]
        public short PropertySmallInt { get; set; }

        [Column("property_tinyint")]
        [AlternateAlias("PropertyTinyInt")]
        public byte PropertyTinyInt { get; set; }

        [Column("property_real")]
        [AlternateAlias("PropertyReal")]
        public float PropertyReal { get; set; }

        [Column("property_float")]
        [AlternateAlias("PropertyFloat")]
        public double PropertyFloat { get; set; }

        [Column("property_decimal")]
        [AlternateAlias("PropertyDecimal")]
        public decimal PropertyDecimal { get; set; }

        [Column("property_varchar")]
        [AlternateAlias("PropertyVarChar")]
        public string PropertyVarChar { get; set; }

        [Column("property_datetime")]
        [AlternateAlias("PropertyDateTime")]
        public DateTime PropertyDateTime { get; set; }

        [Column("property_enum")]
        [AlternateAlias("PropertyEnum")]
        public TestEnumA PropertyEnum { get; set; }

        [Column("property_nullable_bigint")]
        [AlternateAlias("PropertyNullableBigInt")]
        public long? PropertyNullableBigInt { get; set; }

        [Column("property_nullable_int")]
        [AlternateAlias("PropertyNullableInt")]
        public int? PropertyNullableInt { get; set; }

        [Column("property_nullable_smallint")]
        [AlternateAlias("PropertyNullableSmallInt")]
        public short? PropertyNullableSmallInt { get; set; }

        [Column("property_nullable_tinyint")]
        [AlternateAlias("PropertyNullableTinyInt")]
        public byte? PropertyNullableTinyInt { get; set; }

        [Column("property_nullable_real")]
        [AlternateAlias("PropertyNullableReal")]
        public float? PropertyNullableReal { get; set; }

        [Column("property_nullable_float")]
        [AlternateAlias("PropertyNullableFloat")]
        public double? PropertyNullableFloat { get; set; }

        [Column("property_nullable_decimal")]
        [AlternateAlias("PropertyNullableDecimal")]
        public decimal? PropertyNullableDecimal { get; set; }

        [Column("property_nullable_datetime")]
        [AlternateAlias("PropertyNullableDateTime")]
        public DateTime? PropertyNullableDateTime { get; set; }

        [Column("property_nullable_enum")]
        [AlternateAlias("PropertyNullableEnum")]
        public TestEnumA? PropertyNullableEnum { get; set; }

        [AlternateAlias("TestEntityB")]
        public TestEntityB TestEntityB { get; set; }
    }
}
