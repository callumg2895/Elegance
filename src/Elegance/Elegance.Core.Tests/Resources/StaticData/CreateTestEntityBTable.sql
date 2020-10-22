IF OBJECT_ID('dbo.test_entity_b', 'U') IS NOT NULL 
  DROP TABLE [dbo].test_entity_b;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE test_entity_b 
(
    property_bigint                 bigint
)
