IF OBJECT_ID('dbo.test_entity_c', 'U') IS NOT NULL 
  DROP TABLE [dbo].test_entity_c;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE test_entity_c
(
    property_bigint                 bigint
)
