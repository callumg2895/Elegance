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
                                                @property_enum int
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
            property_enum
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
        @property_enum
    )
END
GO
