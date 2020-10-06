IF EXISTS (SELECT name FROM sysobjects WHERE  name = N'CreateTestEntityAItem' AND type = 'P')
	DROP PROCEDURE [dbo].CreateTestEntityAItem
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].CreateTestEntityAItem    @result_status	int				= null out,
                                                @result_message varchar(255)	= null out,
                                                @property_bigint bigint,
                                                @property_int int,
                                                @property_smallint smallint,
                                                @property_tinyint tinyint,
                                                @property_real real,
                                                @property_float float,
                                                @property_decimal decimal,
                                                @property_varchar varchar(255),
                                                @property_datetime datetime2,
                                                @property_enum int,
                                                @property_nullable_bigint bigint,
                                                @property_nullable_int int,
                                                @property_nullable_smallint smallint,
                                                @property_nullable_tinyint tinyint,
                                                @property_nullable_real real,
                                                @property_nullable_float float,
                                                @property_nullable_decimal decimal,
                                                @property_nullable_datetime datetime2,
                                                @property_nullable_enum int
AS
BEGIN
    SET @result_status = 0;
    SET @result_message = 'success';

    INSERT INTO test_entity_a
    (
            property_bigint,
            property_int,
            property_smallint,
            property_tinyint,
            property_real,
            property_float,
            property_decimal,
            property_varchar,
            property_datetime,
            property_enum,
            property_nullable_bigint,
            property_nullable_int,
            property_nullable_smallint,
            property_nullable_tinyint,
            property_nullable_real,
            property_nullable_float,
            property_nullable_decimal,
            property_nullable_datetime,
            property_nullable_enum
    )
    VALUES
    (
        @property_bigint,
        @property_int,
        @property_smallint,
        @property_tinyint,
        @property_real,
        @property_float,
        @property_decimal,
        @property_varchar,
        @property_datetime,
        @property_enum,
        @property_nullable_bigint,
        @property_nullable_int,
        @property_nullable_smallint,
        @property_nullable_tinyint,
        @property_nullable_real,
        @property_nullable_float,
        @property_nullable_decimal,
        @property_nullable_datetime,
        @property_nullable_enum
    )
END
GO
