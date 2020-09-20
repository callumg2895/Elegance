IF EXISTS (SELECT name FROM sysobjects WHERE  name = N'GetTestEntityAItems' AND type = 'P')
	DROP PROCEDURE [dbo].GetTestEntityAItems
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].GetTestEntityAItems  @result_status	int				= null out,
											@result_message varchar(255)	= null out,
                                            @property_bigint bigint         = null,
                                            @property_int int               = null,
                                            @property_smallint smallint     = null,
                                            @property_tinyint tinyint       = null,
                                            @property_real real            = null,
                                            @property_float float          = null,
                                            @property_decimal decimal       = null,
                                            @property_varchar varchar(255)  = null,
                                            @property_datetime datetime2    = null,
                                            @property_enum int              = null
AS
BEGIN
    SET @result_status = 0;
    SET @result_message = 'success';

    SELECT  tea.property_bigint         AS PropertyBigInt,
            tea.property_int            AS PropertyInt,
            tea.property_smallint       AS PropertySmallInt,
            tea.property_tinyint        AS PropertyTinyInt,
            tea.property_real           AS PropertyReal,
            tea.property_float          AS PropertyFloat,
            tea.property_decimal        AS PropertyDecimal,
            tea.property_varchar        AS PropertyVarChar,
            tea.property_datetime       AS PropertyDateTime,
            tea.property_enum           AS PropertyEnum

    FROM    test_entity_a tea (NOLOCK)

    WHERE   0=0
    AND     tea.property_bigint = COALESCE(@property_bigint, tea.property_bigint)
    AND     tea.property_int = COALESCE(@property_int, tea.property_int)
    AND     tea.property_smallint = COALESCE(@property_smallint, tea.property_smallint)
    AND     tea.property_tinyint = COALESCE(@property_tinyint, tea.property_tinyint)
    AND     tea.property_real = COALESCE(@property_real, tea.property_real)
    AND     tea.property_float = COALESCE(@property_float, tea.property_float)
    AND     tea.property_decimal = COALESCE(@property_decimal, tea.property_decimal)
    AND     tea.property_varchar = COALESCE(@property_varchar, tea.property_varchar)
    AND     tea.property_datetime = COALESCE(@property_datetime, tea.property_datetime)
    AND     tea.property_enum = COALESCE(@property_enum, tea.property_enum)
END
GO
