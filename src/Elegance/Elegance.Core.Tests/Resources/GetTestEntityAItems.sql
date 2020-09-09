IF EXISTS (SELECT name FROM sysobjects WHERE  name = N'GetTestEntityAItems' AND type = 'P')
	DROP PROCEDURE [dbo].GetTestEntityAItems
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].GetTestEntityAItems  @result_status	int				= null out,
											@result_message varchar(255)	= null out
AS
BEGIN
    set @result_status = 0;
    set @result_message = 'success';

    SELECT  tea.property_bigint      AS PropertyBigInt,
            tea.property_int         AS PropertyInt,
            tea.property_smallint    AS PropertySmallInt,
            tea.property_tinyint     AS PropertyTinyInt,
            tea.property_varchar     AS PropertyVarChar,
            tea.property_datetime    AS PropertyDateTime,
            tea.property_enum        AS PropertyEnum

    FROM    test_entity_a tea (NOLOCK)
END
GO
