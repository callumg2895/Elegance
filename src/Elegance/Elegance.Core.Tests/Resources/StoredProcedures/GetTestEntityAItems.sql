IF EXISTS (SELECT name FROM sysobjects WHERE  name = N'GetTestEntityAItems' AND type = 'P')
	DROP PROCEDURE [dbo].GetTestEntityAItems
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].GetTestEntityAItems  @result_status	int				            = null out,
											@result_message varchar(255)	            = null out,
                                            @property_bigint bigint                     = null,
                                            @property_int int                           = null,
                                            @property_smallint smallint                 = null,
                                            @property_tinyint tinyint                   = null,
                                            @property_real real                         = null,
                                            @property_float float                       = null,
                                            @property_decimal decimal                   = null,
                                            @property_varchar varchar(255)              = null,
                                            @property_datetime datetime2                = null,
                                            @property_enum int                          = null,
                                            @property_nullable_bigint bigint            = null,
                                            @property_nullable_int int                  = null,
                                            @property_nullable_smallint smallint        = null,
                                            @property_nullable_tinyint tinyint          = null,
                                            @property_nullable_real real                = null,
                                            @property_nullable_float float              = null,
                                            @property_nullable_decimal decimal          = null,
                                            @property_nullable_datetime datetime2       = null,
                                            @property_nullable_enum int                 = null
AS
BEGIN
    SET @result_status = 0;
    SET @result_message = 'success';

    SELECT      tea.property_bigint                     AS PropertyBigInt,
                tea.property_int                        AS PropertyInt,
                tea.property_smallint                   AS PropertySmallInt,
                tea.property_tinyint                    AS PropertyTinyInt,
                tea.property_real                       AS PropertyReal,
                tea.property_float                      AS PropertyFloat,
                tea.property_decimal                    AS PropertyDecimal,
                tea.property_varchar                    AS PropertyVarChar,
                tea.property_datetime                   AS PropertyDateTime,
                tea.property_enum                       AS PropertyEnum,
                tea.property_nullable_bigint            AS PropertyNullableBigInt,
                tea.property_nullable_int               AS PropertyNullableInt,
                tea.property_nullable_smallint          AS PropertyNullableSmallInt,
                tea.property_nullable_tinyint           AS PropertyNullableTinyInt,
                tea.property_nullable_real              AS PropertyNullableReal,
                tea.property_nullable_float             AS PropertyNullableFloat,
                tea.property_nullable_decimal           AS PropertyNullableDecimal,
                tea.property_nullable_datetime          AS PropertyNullableDateTime,
                tea.property_nullable_enum              AS PropertyNullableEnum,
                teb.property_bigint                     AS TestEntityB_PropertyBigInt,
                tec.property_bigint                     AS TestEntityC_PropertyBigInt,
                ted.property_bigint                     AS TestEntityD_PropertyBigInt,
                ted.property_varchar                    AS TestEntityD_PropertyVarChar,
                tee.property_bigint                     AS TestEntityE_PropertyBigInt,
                tee.property_varchar                    AS TestEntityE_PropertyVarChar

    FROM        test_entity_a tea (NOLOCK)
    LEFT JOIN   test_entity_b teb (NOLOCK)              ON tea.property_bigint = teb.property_bigint
    LEFT JOIN   test_entity_c tec (NOLOCK)              ON tea.property_bigint = tec.property_bigint
    LEFT JOIN   test_entity_d ted (NOLOCK)              ON tea.property_bigint = ted.property_bigint
    LEFT JOIN   test_entity_e tee (NOLOCK)              ON tea.property_bigint = tee.property_bigint

    WHERE       0=0
    AND         tea.property_bigint = COALESCE(@property_bigint, tea.property_bigint)
    AND         tea.property_int = COALESCE(@property_int, tea.property_int)
    AND         tea.property_smallint = COALESCE(@property_smallint, tea.property_smallint)
    AND         tea.property_tinyint = COALESCE(@property_tinyint, tea.property_tinyint)
    AND         tea.property_real = COALESCE(@property_real, tea.property_real)
    AND         tea.property_float = COALESCE(@property_float, tea.property_float)
    AND         tea.property_decimal = COALESCE(@property_decimal, tea.property_decimal)
    AND         tea.property_varchar = COALESCE(@property_varchar, tea.property_varchar)
    AND         tea.property_datetime = COALESCE(@property_datetime, tea.property_datetime)
    AND         tea.property_enum = COALESCE(@property_enum, tea.property_enum)
    AND         (
                    (tea.property_nullable_bigint = COALESCE(@property_nullable_bigint, tea.property_nullable_bigint)) OR
                    (tea.property_nullable_bigint IS NULL AND @property_nullable_bigint IS NULL)
                )
    AND         (
                    (tea.property_nullable_int = COALESCE(@property_nullable_int, tea.property_nullable_int)) OR
                    (tea.property_nullable_int IS NULL AND @property_nullable_int IS NULL)
                )
    AND         (
                    (tea.property_nullable_smallint = COALESCE(@property_nullable_smallint, tea.property_nullable_smallint)) OR
                    (tea.property_nullable_smallint IS NULL AND @property_nullable_smallint IS NULL)
                )
    AND         (
                    (tea.property_nullable_tinyint = COALESCE(@property_nullable_tinyint, tea.property_nullable_tinyint)) OR
                    (tea.property_nullable_tinyint IS NULL AND @property_nullable_tinyint IS NULL)
                )
    AND         (
                    (tea.property_nullable_real = COALESCE(@property_nullable_real, tea.property_nullable_real)) OR
                    (tea.property_nullable_real IS NULL AND @property_nullable_real IS NULL)
                )
    AND         (
                    (tea.property_nullable_float = COALESCE(@property_nullable_float, tea.property_nullable_float)) OR
                    (tea.property_nullable_float IS NULL AND @property_nullable_float IS NULL)
                )
    AND         (
                    (tea.property_nullable_decimal = COALESCE(@property_nullable_decimal, tea.property_nullable_decimal)) OR
                    (tea.property_nullable_decimal IS NULL AND @property_nullable_decimal IS NULL)
                )
    AND         (
                    (tea.property_nullable_datetime = COALESCE(@property_nullable_datetime, tea.property_nullable_datetime)) OR
                    (tea.property_nullable_datetime IS NULL AND @property_nullable_datetime IS NULL)
                )
    AND         (
                    (tea.property_nullable_enum = COALESCE(@property_nullable_enum, tea.property_nullable_enum)) OR
                    (tea.property_nullable_enum IS NULL AND @property_nullable_enum IS NULL)
                )
END
GO
