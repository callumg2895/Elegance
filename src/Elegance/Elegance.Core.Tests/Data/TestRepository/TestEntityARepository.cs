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
                select  tea.property_bigint         as PropertyBigInt,
                        tea.property_int            as PropertyInt,
                        tea.property_smallint       as PropertySmallInt,
                        tea.property_tinyint        as PropertyTinyInt,
                        tea.property_real           as PropertyReal,
                        tea.property_float          as PropertyFloat,
                        tea.property_decimal        as PropertyDecimal,
                        tea.property_varchar        as PropertyVarChar,
                        tea.property_datetime       as PropertyDateTime,
                        tea.property_enum           as PropertyEnum

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
                .SetParameter("@property_enum", testEntity.PropertyEnum);

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
                        property_enum
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
                    @property_enum
                )";

            using var session = CreateSession();
            using var command = session.CreateCommand(sql);

            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_bigint", Value = testEntity.PropertyBigInt, DbType = DbType.Int64 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_int", Value = testEntity.PropertyInt, DbType = DbType.Int32 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_smallint", Value = testEntity.PropertySmallInt, DbType = DbType.Int16 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_tinyint", Value = testEntity.PropertyTinyInt, DbType = DbType.Byte });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_varchar", Value = testEntity.PropertyVarChar, DbType = DbType.AnsiString });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_real", Value = testEntity.PropertyReal, DbType = DbType.Single });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_float", Value = testEntity.PropertyFloat, DbType = DbType.Double });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_decimal", Value = testEntity.PropertyDecimal, DbType = DbType.Decimal });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_datetime", Value = testEntity.PropertyDateTime, DbType = DbType.DateTime2 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_enum", Value = (int)testEntity.PropertyEnum, DbType = DbType.Int32 });

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
                .SetParameter("@property_enum", testEntity.PropertyEnum);
                
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

                entity.PropertyBigInt = (long)reader[nameof(entity.PropertyBigInt)];
                entity.PropertyInt = (int)reader[nameof(entity.PropertyInt)];
                entity.PropertySmallInt = (short)reader[nameof(entity.PropertySmallInt)];
                entity.PropertyTinyInt = (byte)reader[nameof(entity.PropertyTinyInt)];
                entity.PropertyReal = (float)reader[nameof(entity.PropertyReal)];
                entity.PropertyFloat = (double)reader[nameof(entity.PropertyFloat)];
                entity.PropertyDecimal = (decimal)reader[nameof(entity.PropertyDecimal)];
                entity.PropertyVarChar = (string)reader[nameof(entity.PropertyVarChar)];
                entity.PropertyDateTime = (DateTime)reader[nameof(entity.PropertyDateTime)];
                entity.PropertyEnum = (TestEnumA)(int)reader[nameof(entity.PropertyEnum)];

                entities.Add(entity);
            }

            return entities;
        }

        #endregion
    }
}
