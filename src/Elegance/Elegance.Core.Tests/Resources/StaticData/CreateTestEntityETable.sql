IF OBJECT_ID('dbo.test_entity_e', 'U') IS NOT NULL 
  DROP TABLE [dbo].test_entity_e;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE test_entity_e
(
    property_bigint                 bigint,
    property_varchar                varchar(255)
)
