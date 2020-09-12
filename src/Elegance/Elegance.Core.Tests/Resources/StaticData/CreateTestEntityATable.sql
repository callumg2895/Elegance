IF OBJECT_ID('dbo.test_entity_a', 'U') IS NOT NULL 
  DROP TABLE [dbo].test_entity_a;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE test_entity_a 
(
    property_bigint     bigint,
    property_int        int,
    property_smallint   smallint,
    property_tinyint    tinyint,
    property_real       real,
    property_float      float,
    property_decimal    decimal,
    property_varchar    varchar(255),
    property_datetime   datetime2,
    property_enum       int
)
