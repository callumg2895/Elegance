using Elegance.Core.Tests.Data.TestEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Elegance.Core.Tests.Data.TestRepository
{
    public class TestEntityARepository : BaseRepository
    {
        private const string _testEntitySelectText = @"
                select  tea.property_bigint                     AS PropertyBigInt,
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
                        tea.property_nullable_enum              AS PropertyNullableEnum

                from    test_entity_a tea (nolock)";

        private const string _testEntitySelectCountText = @"
                select  count(*)
                from    test_entity_a tea (nolock)";

        public TestEntityARepository()
            : base()
        {

        }

        #region GetAll

        public IList<TestEntityA> GetAll_Query()
        {
            using var session = CreateSession();

            return session
                .CreateObjectQuery<TestEntityA>(_testEntitySelectText, CommandType.Text)
                .Results();
        }

        public IList<TestEntityA> GetAll_Query_StoredProcedure(out int status, out string message)
        {
            using var session = CreateSession();

            var query = session
                .CreateObjectQuery<TestEntityA>("GetTestEntityAItems", CommandType.StoredProcedure)
                .SetParameter("@result_status", 1, ParameterDirection.Output)
                .SetParameter<String>("@result_message", null, ParameterDirection.Output);
                
            var results = query.Results();

            status = query.GetParameter<Int32>("@result_status");
            message = query.GetParameter<String>("@result_message");

            return results;
        }

        public IList<TestEntityA> GetAll_Query_Reader()
        {
            using var session = CreateSession();

            var query = session.CreateObjectQuery<TestEntityA>(_testEntitySelectText, CommandType.Text);
            var reader = query.Reader();

            return ReadTestEntities(reader);
        }

        public IList<TestEntityA> GetAll_Standard()
        {
            using var session = CreateSession();
            using var reader = session
                .CreateCommand(_testEntitySelectText)
                .ExecuteReader();

            return ReadTestEntities(reader);
        }

        #endregion

        #region GetAllCount

        public int GetAllCount_Query()
        {
            using var session = CreateSession();

            return session
                .CreateScalarQuery<int>(_testEntitySelectCountText, CommandType.Text)
                .Result();
        }

        public int GetAllCount_Standard()
        {
            using var session = CreateSession();
            using var reader = session
                .CreateCommand(_testEntitySelectCountText)
                .ExecuteReader();

            reader.Read();

            return int.Parse(reader[0].ToString());
        }

        #endregion

        #region GetById

        public TestEntityA GetByAllProperties_Query(TestEntityA testEntity)
        {
            var sql = new StringBuilder()
                .AppendLine(_testEntitySelectText)
                .AppendLine("where tea.property_bigint = @property_bigint")
                .AppendLine("and tea.property_int = @property_int")
                .AppendLine("and tea.property_smallint = @property_smallint")
                .AppendLine("and tea.property_tinyint = @property_tinyint")
                .AppendLine("and tea.property_real = @property_real")
                .AppendLine("and tea.property_float = @property_float")
                .AppendLine("and tea.property_decimal = @property_decimal")
                .AppendLine("and tea.property_varchar = @property_varchar")
                .AppendLine("and tea.property_datetime = @property_datetime")
                .AppendLine("and tea.property_enum = @property_enum")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_bigint = COALESCE(@property_nullable_bigint, tea.property_nullable_bigint)) OR")
                .AppendLine("        (tea.property_nullable_bigint IS NULL AND @property_nullable_bigint IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_int = COALESCE(@property_nullable_int, tea.property_nullable_int)) OR")
                .AppendLine("        (tea.property_nullable_int IS NULL AND @property_nullable_int IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_smallint = COALESCE(@property_nullable_smallint, tea.property_nullable_smallint)) OR")
                .AppendLine("        (tea.property_nullable_smallint IS NULL AND @property_nullable_smallint IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_tinyint = COALESCE(@property_nullable_tinyint, tea.property_nullable_tinyint)) OR")
                .AppendLine("        (tea.property_nullable_tinyint IS NULL AND @property_nullable_tinyint IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_real = COALESCE(@property_nullable_real, tea.property_nullable_real)) OR")
                .AppendLine("        (tea.property_nullable_real IS NULL AND @property_nullable_real IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_float = COALESCE(@property_nullable_float, tea.property_nullable_float)) OR")
                .AppendLine("        (tea.property_nullable_float IS NULL AND @property_nullable_float IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_decimal = COALESCE(@property_nullable_decimal, tea.property_nullable_decimal)) OR")
                .AppendLine("        (tea.property_nullable_decimal IS NULL AND @property_nullable_decimal IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_datetime = COALESCE(@property_nullable_datetime, tea.property_nullable_datetime)) OR")
                .AppendLine("        (tea.property_nullable_datetime IS NULL AND @property_nullable_datetime IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_enum = COALESCE(@property_nullable_enum, tea.property_nullable_enum)) OR")
                .AppendLine("        (tea.property_nullable_enum IS NULL AND @property_nullable_enum IS NULL)")
                .AppendLine("     )")
                .ToString();

            using var session = CreateSession();

            return session
                .CreateObjectQuery<TestEntityA>(sql, CommandType.Text)
                .SetParameter("@property_bigint", testEntity.PropertyBigInt)
                .SetParameter("@property_int", testEntity.PropertyInt)
                .SetParameter("@property_smallint", testEntity.PropertySmallInt)
                .SetParameter("@property_tinyint", testEntity.PropertyTinyInt)
                .SetParameter("@property_real", testEntity.PropertyReal)
                .SetParameter("@property_float", testEntity.PropertyFloat)
                .SetParameter("@property_decimal", testEntity.PropertyDecimal)
                .SetParameter("@property_varchar", testEntity.PropertyVarChar)
                .SetParameter("@property_datetime", testEntity.PropertyDateTime)
                .SetParameter("@property_enum", testEntity.PropertyEnum)
                .SetParameter("@property_nullable_bigint", testEntity.PropertyNullableBigInt)
                .SetParameter("@property_nullable_int", testEntity.PropertyNullableInt)
                .SetParameter("@property_nullable_smallint", testEntity.PropertyNullableSmallInt)
                .SetParameter("@property_nullable_tinyint", testEntity.PropertyNullableTinyInt)
                .SetParameter("@property_nullable_real", testEntity.PropertyNullableReal)
                .SetParameter("@property_nullable_float", testEntity.PropertyNullableFloat)
                .SetParameter("@property_nullable_decimal", testEntity.PropertyNullableDecimal)
                .SetParameter("@property_nullable_datetime", testEntity.PropertyNullableDateTime)
                .SetParameter("@property_nullable_enum", testEntity.PropertyNullableEnum)
                .Result();
        }

        public TestEntityA GetByAllProperties_Query_StoredProcedure(TestEntityA testEntity, out int status, out string message)
        {
            using var session = CreateSession();

            var query = session
                .CreateObjectQuery<TestEntityA>("GetTestEntityAItems", CommandType.StoredProcedure)
                .SetParameter("@result_status", 1, ParameterDirection.Output)
                .SetParameter<String>("@result_message", null, ParameterDirection.Output)
                .SetParameter("@property_bigint", testEntity.PropertyBigInt)
                .SetParameter("@property_int", testEntity.PropertyInt)
                .SetParameter("@property_smallint", testEntity.PropertySmallInt)
                .SetParameter("@property_tinyint", testEntity.PropertyTinyInt)
                .SetParameter("@property_real", testEntity.PropertyReal)
                .SetParameter("@property_float", testEntity.PropertyFloat)
                .SetParameter("@property_decimal", testEntity.PropertyDecimal)
                .SetParameter("@property_varchar", testEntity.PropertyVarChar)
                .SetParameter("@property_datetime", testEntity.PropertyDateTime)
                .SetParameter("@property_enum", testEntity.PropertyEnum)
                .SetParameter("@property_nullable_bigint", testEntity.PropertyNullableBigInt)
                .SetParameter("@property_nullable_int", testEntity.PropertyNullableInt)
                .SetParameter("@property_nullable_smallint", testEntity.PropertyNullableSmallInt)
                .SetParameter("@property_nullable_tinyint", testEntity.PropertyNullableTinyInt)
                .SetParameter("@property_nullable_real", testEntity.PropertyNullableReal)
                .SetParameter("@property_nullable_float", testEntity.PropertyNullableFloat)
                .SetParameter("@property_nullable_decimal", testEntity.PropertyNullableDecimal)
                .SetParameter("@property_nullable_datetime", testEntity.PropertyNullableDateTime)
                .SetParameter("@property_nullable_enum", testEntity.PropertyNullableEnum);

            var result = query.Result();

            status = query.GetParameter<Int32>("@result_status");
            message = query.GetParameter<String>("@result_message");

            return result;
        }

        public TestEntityA GetByAllProperties_Standard(TestEntityA testEntity)
        {
            var sql = new StringBuilder()
                .AppendLine(_testEntitySelectText)
                .AppendLine("where tea.property_bigint = @property_bigint")
                .AppendLine("and tea.property_int = @property_int")
                .AppendLine("and tea.property_smallint = @property_smallint")
                .AppendLine("and tea.property_tinyint = @property_tinyint")
                .AppendLine("and tea.property_real = @property_real")
                .AppendLine("and tea.property_float = @property_float")
                .AppendLine("and tea.property_decimal = @property_decimal")
                .AppendLine("and tea.property_varchar = @property_varchar")
                .AppendLine("and tea.property_datetime = @property_datetime")
                .AppendLine("and tea.property_enum = @property_enum")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_bigint = COALESCE(@property_nullable_bigint, tea.property_nullable_bigint)) OR")
                .AppendLine("        (tea.property_nullable_bigint IS NULL AND @property_nullable_bigint IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_int = COALESCE(@property_nullable_int, tea.property_nullable_int)) OR")
                .AppendLine("        (tea.property_nullable_int IS NULL AND @property_nullable_int IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_smallint = COALESCE(@property_nullable_smallint, tea.property_nullable_smallint)) OR")
                .AppendLine("        (tea.property_nullable_smallint IS NULL AND @property_nullable_smallint IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_tinyint = COALESCE(@property_nullable_tinyint, tea.property_nullable_tinyint)) OR")
                .AppendLine("        (tea.property_nullable_tinyint IS NULL AND @property_nullable_tinyint IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_real = COALESCE(@property_nullable_real, tea.property_nullable_real)) OR")
                .AppendLine("        (tea.property_nullable_real IS NULL AND @property_nullable_real IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_float = COALESCE(@property_nullable_float, tea.property_nullable_float)) OR")
                .AppendLine("        (tea.property_nullable_float IS NULL AND @property_nullable_float IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_decimal = COALESCE(@property_nullable_decimal, tea.property_nullable_decimal)) OR")
                .AppendLine("        (tea.property_nullable_decimal IS NULL AND @property_nullable_decimal IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_datetime = COALESCE(@property_nullable_datetime, tea.property_nullable_datetime)) OR")
                .AppendLine("        (tea.property_nullable_datetime IS NULL AND @property_nullable_datetime IS NULL)")
                .AppendLine("    )")
                .AppendLine("AND (")
                .AppendLine("        (tea.property_nullable_enum = COALESCE(@property_nullable_enum, tea.property_nullable_enum)) OR")
                .AppendLine("        (tea.property_nullable_enum IS NULL AND @property_nullable_enum IS NULL)")
                .AppendLine("     )")
                .ToString();

            using var session = CreateSession();
            using var command = session.CreateCommand(sql);

            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_bigint", Value = testEntity.PropertyBigInt, DbType = DbType.Int64 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_int", Value = testEntity.PropertyInt, DbType = DbType.Int32 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_smallint", Value = testEntity.PropertySmallInt, DbType = DbType.Int16 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_tinyint", Value = testEntity.PropertyTinyInt, DbType = DbType.Byte });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_real", Value = testEntity.PropertyReal, DbType = DbType.Single });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_float", Value = testEntity.PropertyFloat, DbType = DbType.Double });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_decimal", Value = testEntity.PropertyDecimal, DbType = DbType.Decimal });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_varchar", Value = testEntity.PropertyVarChar, DbType = DbType.AnsiString });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_datetime", Value = testEntity.PropertyDateTime, DbType = DbType.DateTime2 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_enum", Value = (int)testEntity.PropertyEnum, DbType = DbType.Int32 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_bigint", Value = testEntity.PropertyNullableBigInt.HasValue ? testEntity.PropertyNullableBigInt.Value : (object)DBNull.Value, DbType = DbType.Int64 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_int", Value = testEntity.PropertyNullableInt.HasValue ? testEntity.PropertyNullableInt.Value : (object)DBNull.Value, DbType = DbType.Int32 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_smallint", Value = testEntity.PropertyNullableSmallInt.HasValue ? testEntity.PropertyNullableSmallInt.Value : (object)DBNull.Value, DbType = DbType.Int16 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_tinyint", Value = testEntity.PropertyNullableTinyInt.HasValue ? testEntity.PropertyNullableTinyInt.Value : (object)DBNull.Value, DbType = DbType.Byte });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_real", Value = testEntity.PropertyNullableReal.HasValue ? testEntity.PropertyNullableReal.Value : (object)DBNull.Value, DbType = DbType.Single });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_float", Value = testEntity.PropertyNullableFloat.HasValue ? testEntity.PropertyNullableFloat.Value : (object)DBNull.Value, DbType = DbType.Double });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_decimal", Value = testEntity.PropertyNullableDecimal.HasValue ? testEntity.PropertyNullableDecimal.Value : (object)DBNull.Value, DbType = DbType.Decimal });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_datetime", Value = testEntity.PropertyNullableDateTime.HasValue ? testEntity.PropertyNullableDateTime.Value : (object)DBNull.Value, DbType = DbType.DateTime2 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_enum", Value = testEntity.PropertyNullableEnum.HasValue ? (int?)testEntity.PropertyNullableEnum.Value : (object)DBNull.Value, DbType = DbType.Int32 });

            using var reader = command.ExecuteReader();

            return ReadTestEntities(reader).FirstOrDefault();
        }

        #endregion

        #region Insert

        public void InsertTestEntity_Standard(TestEntityA testEntity)
        {
            var sql = @"
                
                insert into test_entity_a
                (
                        property_bigint     ,
                        property_int        ,
                        property_smallint   ,
                        property_tinyint    ,
                        property_real      ,
                        property_float     ,
                        property_decimal    ,
                        property_varchar    ,
                        property_datetime   ,
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
                values
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
                )";

            using var session = CreateSession();
            using var command = session.CreateCommand(sql);

            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_bigint", Value = testEntity.PropertyBigInt, DbType = DbType.Int64 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_int", Value = testEntity.PropertyInt, DbType = DbType.Int32 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_smallint", Value = testEntity.PropertySmallInt, DbType = DbType.Int16 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_tinyint", Value = testEntity.PropertyTinyInt, DbType = DbType.Byte });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_real", Value = testEntity.PropertyReal, DbType = DbType.Single });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_float", Value = testEntity.PropertyFloat, DbType = DbType.Double });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_decimal", Value = testEntity.PropertyDecimal, DbType = DbType.Decimal });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_varchar", Value = testEntity.PropertyVarChar, DbType = DbType.AnsiString });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_datetime", Value = testEntity.PropertyDateTime, DbType = DbType.DateTime2 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_enum", Value = (int)testEntity.PropertyEnum, DbType = DbType.Int32 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_bigint", Value = testEntity.PropertyNullableBigInt.HasValue ? testEntity.PropertyNullableBigInt.Value : (object)DBNull.Value , DbType = DbType.Int64 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_int", Value = testEntity.PropertyNullableInt.HasValue ? testEntity.PropertyNullableInt.Value : (object)DBNull.Value, DbType = DbType.Int32 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_smallint", Value = testEntity.PropertyNullableSmallInt.HasValue ? testEntity.PropertyNullableSmallInt.Value : (object)DBNull.Value , DbType = DbType.Int16 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_tinyint", Value = testEntity.PropertyNullableTinyInt.HasValue ? testEntity.PropertyNullableTinyInt.Value : (object)DBNull.Value, DbType = DbType.Byte });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_real", Value = testEntity.PropertyNullableReal.HasValue ? testEntity.PropertyNullableReal.Value : (object)DBNull.Value, DbType = DbType.Single });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_float", Value = testEntity.PropertyNullableFloat.HasValue ? testEntity.PropertyNullableFloat.Value : (object)DBNull.Value, DbType = DbType.Double });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_decimal", Value = testEntity.PropertyNullableDecimal.HasValue ? testEntity.PropertyNullableDecimal.Value : (object)DBNull.Value, DbType = DbType.Decimal });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_datetime", Value = testEntity.PropertyNullableDateTime.HasValue ? testEntity.PropertyNullableDateTime.Value : (object)DBNull.Value, DbType = DbType.DateTime2 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_nullable_enum", Value = testEntity.PropertyNullableEnum.HasValue ? (int?)testEntity.PropertyNullableEnum.Value : (object)DBNull.Value, DbType = DbType.Int32 });

            command.ExecuteNonQuery();
        }

        public void InsertTestEntity_NonQuery_StoredProcedure(TestEntityA testEntity, out int resultStatus, out string resultMessage)
        {
            using var session = CreateSession();

            var nonQuery = session
                .CreateNonQuery("CreateTestEntityAItem", CommandType.StoredProcedure)
                .SetParameter("@result_status", 1, ParameterDirection.Output)
                .SetParameter<String>("@result_message", null, ParameterDirection.Output)
                .SetParameter("@property_bigint", testEntity.PropertyBigInt)
                .SetParameter("@property_int", testEntity.PropertyInt)
                .SetParameter("@property_smallint", testEntity.PropertySmallInt)
                .SetParameter("@property_tinyint", testEntity.PropertyTinyInt)
                .SetParameter("@property_real", testEntity.PropertyReal)
                .SetParameter("@property_float", testEntity.PropertyFloat)
                .SetParameter("@property_decimal", testEntity.PropertyDecimal)
                .SetParameter("@property_varchar", testEntity.PropertyVarChar)
                .SetParameter("@property_datetime", testEntity.PropertyDateTime)
                .SetParameter("@property_enum", testEntity.PropertyEnum)
                .SetParameter("@property_nullable_bigint", testEntity.PropertyNullableBigInt)
                .SetParameter("@property_nullable_int", testEntity.PropertyNullableInt)
                .SetParameter("@property_nullable_smallint", testEntity.PropertyNullableSmallInt)
                .SetParameter("@property_nullable_tinyint", testEntity.PropertyNullableTinyInt)
                .SetParameter("@property_nullable_real", testEntity.PropertyNullableReal)
                .SetParameter("@property_nullable_float", testEntity.PropertyNullableFloat)
                .SetParameter("@property_nullable_decimal", testEntity.PropertyNullableDecimal)
                .SetParameter("@property_nullable_datetime", testEntity.PropertyNullableDateTime)
                .SetParameter("@property_nullable_enum", testEntity.PropertyNullableEnum);

            nonQuery.Execute();

            resultStatus = nonQuery.GetParameter<int>("@result_status");
            resultMessage = nonQuery.GetParameter<String>("@result_message");
        }

        #endregion

        #region Delete

        public void DeleteTestEntities()
        {
            using var session = CreateSession();

            var sql = @"delete from test_entity_a";

            session
                .CreateCommand(sql)
                .ExecuteNonQuery();
        }

        #endregion

        #region Helpers

        private IList<TestEntityA> ReadTestEntities(IDataReader reader)
        {
            var entities = new List<TestEntityA>();

            while (reader.Read())
            {
                var entity = new TestEntityA();

                entity.PropertyBigInt = (long)GetReaderValue(reader, nameof(entity.PropertyBigInt));
                entity.PropertyInt = (int)GetReaderValue(reader, nameof(entity.PropertyInt));
                entity.PropertySmallInt = (short)GetReaderValue(reader, nameof(entity.PropertySmallInt));
                entity.PropertyTinyInt = (byte)GetReaderValue(reader, nameof(entity.PropertyTinyInt));
                entity.PropertyReal = (float)GetReaderValue(reader, nameof(entity.PropertyReal));
                entity.PropertyFloat = (double)GetReaderValue(reader, nameof(entity.PropertyFloat));
                entity.PropertyDecimal = (decimal)GetReaderValue(reader, nameof(entity.PropertyDecimal));
                entity.PropertyVarChar = (string)GetReaderValue(reader, nameof(entity.PropertyVarChar));
                entity.PropertyDateTime = (DateTime)GetReaderValue(reader, nameof(entity.PropertyDateTime));
                entity.PropertyEnum = (TestEnumA)(int)GetReaderValue(reader, nameof(entity.PropertyEnum));

                entity.PropertyNullableBigInt = (long?)GetReaderValue(reader, nameof(entity.PropertyNullableBigInt));
                entity.PropertyNullableInt = (int?)GetReaderValue(reader, nameof(entity.PropertyNullableInt));
                entity.PropertyNullableSmallInt = (short?)GetReaderValue(reader, nameof(entity.PropertyNullableSmallInt));
                entity.PropertyNullableTinyInt = (byte?)GetReaderValue(reader, nameof(entity.PropertyNullableTinyInt));
                entity.PropertyNullableReal = (float?)GetReaderValue(reader, nameof(entity.PropertyNullableReal));
                entity.PropertyNullableFloat = (double?)GetReaderValue(reader, nameof(entity.PropertyNullableFloat));
                entity.PropertyNullableDecimal = (decimal?)GetReaderValue(reader, nameof(entity.PropertyNullableDecimal));
                entity.PropertyNullableDateTime = (DateTime?)GetReaderValue(reader, nameof(entity.PropertyNullableDateTime));
                entity.PropertyNullableEnum = (TestEnumA?)GetReaderValue(reader, nameof(entity.PropertyNullableEnum));

                entities.Add(entity);
            }

            return entities;
        }

        #endregion
    }
}
