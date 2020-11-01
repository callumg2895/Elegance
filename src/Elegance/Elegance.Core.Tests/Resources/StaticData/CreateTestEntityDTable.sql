IF OBJECT_ID('dbo.test_entity_d', 'U') IS NOT NULL 
  DROP TABLE [dbo].test_entity_d;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE test_entity_d
(
    property_bigint                 bigint,
    property_varchar                varchar(255)
)
