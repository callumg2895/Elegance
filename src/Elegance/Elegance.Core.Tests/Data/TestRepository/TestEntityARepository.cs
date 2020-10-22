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
        private const string _testEntitySelectTextNonAliased = @"
                select  tea.property_bigint,
                        tea.property_int,
                        tea.property_smallint,
                        tea.property_tinyint,
                        tea.property_real,
                        tea.property_float,
                        tea.property_decimal,
                        tea.property_varchar,
                        tea.property_datetime,
                        tea.property_enum,
                        tea.property_nullable_bigint,
                        tea.property_nullable_int,
                        tea.property_nullable_smallint,
                        tea.property_nullable_tinyint,
                        tea.property_nullable_real,
                        tea.property_nullable_float,
                        tea.property_nullable_decimal,
                        tea.property_nullable_datetime,
                        tea.property_nullable_enum

                from    test_entity_a tea (nolock)";

        private const string _testEntitySelectTextAliased = @"
                select      tea.property_bigint                     AS PropertyBigInt,
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
                            teb.property_bigint                     AS TestEntityB_PropertyBigInt

                from        test_entity_a tea (nolock)
                left join   test_entity_b teb (nolock)              on tea.property_bigint = teb.property_bigint";

        private const string _testEntitySelectCountText = @"
                select  count(*)
                from    test_entity_a tea (nolock)";

        public TestEntityARepository()
            : base()
        {

        }

        #region GetAll

        public IList<TestEntityA> GetAll_Query_Aliased()
        {
            using var session = CreateSession();

            return session
                .CreateObjectQuery<TestEntityA>(_testEntitySelectTextAliased, CommandType.Text)
                .Results();
        }

        public IList<TestEntityA> GetAll_Query_NonAliased()
        {
            using var session = CreateSession();

            return session
                .CreateObjectQuery<TestEntityA>(_testEntitySelectTextNonAliased, CommandType.Text)
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

            var query = session.CreateObjectQuery<TestEntityA>(_testEntitySelectTextAliased, CommandType.Text);
            var reader = query.Reader();

            return ReadTestEntitiesAliased(reader);
        }

        public IList<TestEntityA> GetAll_Standard_Aliased()
        {
            using var session = CreateSession();
            using var reader = session
                .CreateCommand(_testEntitySelectTextAliased)
                .ExecuteReader();

            return ReadTestEntitiesAliased(reader);
        }

        public IList<TestEntityA> GetAll_Standard_NonAliased()
        {
            using var session = CreateSession();
            using var reader = session
                .CreateCommand(_testEntitySelectTextNonAliased)
                .ExecuteReader();

            return ReadTestEntitiesNonAliased(reader);
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
                .AppendLine(_testEntitySelectTextAliased)
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
                .AppendLine(_testEntitySelectTextAliased)
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

            command.Parameters.Add(GetParameter("@property_bigint", testEntity.PropertyBigInt, DbType.Int64));
            command.Parameters.Add(GetParameter("@property_int", testEntity.PropertyInt, DbType.Int32));
            command.Parameters.Add(GetParameter("@property_smallint", testEntity.PropertySmallInt, DbType.Int16));
            command.Parameters.Add(GetParameter("@property_tinyint", testEntity.PropertyTinyInt, DbType.Byte));
            command.Parameters.Add(GetParameter("@property_real", testEntity.PropertyReal, DbType.Single));
            command.Parameters.Add(GetParameter("@property_float", testEntity.PropertyFloat, DbType.Double));
            command.Parameters.Add(GetParameter("@property_decimal", testEntity.PropertyDecimal, DbType.Decimal));
            command.Parameters.Add(GetParameter("@property_varchar", testEntity.PropertyVarChar, DbType.AnsiString));
            command.Parameters.Add(GetParameter("@property_datetime", testEntity.PropertyDateTime, DbType.DateTime2));
            command.Parameters.Add(GetParameter("@property_enum", (int)testEntity.PropertyEnum, DbType.Int32));
            command.Parameters.Add(GetParameter("@property_nullable_bigint", testEntity.PropertyNullableBigInt, DbType.Int64));
            command.Parameters.Add(GetParameter("@property_nullable_int", testEntity.PropertyNullableInt, DbType.Int32));
            command.Parameters.Add(GetParameter("@property_nullable_smallint", testEntity.PropertyNullableSmallInt, DbType.Int16));
            command.Parameters.Add(GetParameter("@property_nullable_tinyint", testEntity.PropertyNullableTinyInt, DbType.Byte));
            command.Parameters.Add(GetParameter("@property_nullable_real", testEntity.PropertyNullableReal, DbType.Single));
            command.Parameters.Add(GetParameter("@property_nullable_float", testEntity.PropertyNullableFloat, DbType.Double));
            command.Parameters.Add(GetParameter("@property_nullable_decimal", testEntity.PropertyNullableDecimal, DbType.Decimal));
            command.Parameters.Add(GetParameter("@property_nullable_datetime", testEntity.PropertyNullableDateTime, DbType.DateTime2));
            command.Parameters.Add(GetParameter("@property_nullable_enum", testEntity.PropertyNullableEnum, DbType.Int32));

            using var reader = command.ExecuteReader();

            return ReadTestEntitiesAliased(reader).FirstOrDefault();
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

            command.Parameters.Add(GetParameter("@property_bigint", testEntity.PropertyBigInt, DbType.Int64));
            command.Parameters.Add(GetParameter("@property_int", testEntity.PropertyInt, DbType.Int32));
            command.Parameters.Add(GetParameter("@property_smallint", testEntity.PropertySmallInt, DbType.Int16));
            command.Parameters.Add(GetParameter("@property_tinyint", testEntity.PropertyTinyInt, DbType.Byte));
            command.Parameters.Add(GetParameter("@property_real", testEntity.PropertyReal, DbType.Single));
            command.Parameters.Add(GetParameter("@property_float", testEntity.PropertyFloat, DbType.Double));
            command.Parameters.Add(GetParameter("@property_decimal", testEntity.PropertyDecimal, DbType.Decimal));
            command.Parameters.Add(GetParameter("@property_varchar", testEntity.PropertyVarChar, DbType.AnsiString));
            command.Parameters.Add(GetParameter("@property_datetime", testEntity.PropertyDateTime, DbType.DateTime2));
            command.Parameters.Add(GetParameter("@property_enum", (int)testEntity.PropertyEnum, DbType.Int32));
            command.Parameters.Add(GetParameter("@property_nullable_bigint", testEntity.PropertyNullableBigInt, DbType.Int64));
            command.Parameters.Add(GetParameter("@property_nullable_int", testEntity.PropertyNullableInt, DbType.Int32));
            command.Parameters.Add(GetParameter("@property_nullable_smallint", testEntity.PropertyNullableSmallInt, DbType.Int16));
            command.Parameters.Add(GetParameter("@property_nullable_tinyint", testEntity.PropertyNullableTinyInt, DbType.Byte));
            command.Parameters.Add(GetParameter("@property_nullable_real", testEntity.PropertyNullableReal, DbType.Single));
            command.Parameters.Add(GetParameter("@property_nullable_float", testEntity.PropertyNullableFloat, DbType.Double));
            command.Parameters.Add(GetParameter("@property_nullable_decimal", testEntity.PropertyNullableDecimal, DbType.Decimal));
            command.Parameters.Add(GetParameter("@property_nullable_datetime", testEntity.PropertyNullableDateTime, DbType.DateTime2));
            command.Parameters.Add(GetParameter("@property_nullable_enum", testEntity.PropertyNullableEnum, DbType.Int32));

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

        private IList<TestEntityA> ReadTestEntitiesAliased(IDataReader reader)
        {
            var entityALookup = new Dictionary<long, TestEntityA>();
            var entityBLookup = new Dictionary<long, TestEntityB>();
            var entities = new List<TestEntityA>();

            while (reader.Read())
            {
                var entityABigInt = (long)GetReaderValue(reader, nameof(TestEntityA.PropertyBigInt));
                var entityBBigInt = (long?)GetReaderValue(reader, $"{nameof(TestEntityB)}_{nameof(TestEntityB.PropertyBigInt)}");

                if (!entityALookup.TryGetValue(entityABigInt, out TestEntityA entityA))
                {
                    entityA = new TestEntityA();

                    entityA.PropertyBigInt = entityABigInt;
                    entityA.PropertyInt = (int)GetReaderValue(reader, nameof(TestEntityA.PropertyInt));
                    entityA.PropertySmallInt = (short)GetReaderValue(reader, nameof(TestEntityA.PropertySmallInt));
                    entityA.PropertyTinyInt = (byte)GetReaderValue(reader, nameof(TestEntityA.PropertyTinyInt));
                    entityA.PropertyReal = (float)GetReaderValue(reader, nameof(TestEntityA.PropertyReal));
                    entityA.PropertyFloat = (double)GetReaderValue(reader, nameof(TestEntityA.PropertyFloat));
                    entityA.PropertyDecimal = (decimal)GetReaderValue(reader, nameof(TestEntityA.PropertyDecimal));
                    entityA.PropertyVarChar = (string)GetReaderValue(reader, nameof(TestEntityA.PropertyVarChar));
                    entityA.PropertyDateTime = (DateTime)GetReaderValue(reader, nameof(TestEntityA.PropertyDateTime));
                    entityA.PropertyEnum = (TestEnumA)(int)GetReaderValue(reader, nameof(TestEntityA.PropertyEnum));

                    entityA.PropertyNullableBigInt = (long?)GetReaderValue(reader, nameof(TestEntityA.PropertyNullableBigInt));
                    entityA.PropertyNullableInt = (int?)GetReaderValue(reader, nameof(TestEntityA.PropertyNullableInt));
                    entityA.PropertyNullableSmallInt = (short?)GetReaderValue(reader, nameof(TestEntityA.PropertyNullableSmallInt));
                    entityA.PropertyNullableTinyInt = (byte?)GetReaderValue(reader, nameof(TestEntityA.PropertyNullableTinyInt));
                    entityA.PropertyNullableReal = (float?)GetReaderValue(reader, nameof(TestEntityA.PropertyNullableReal));
                    entityA.PropertyNullableFloat = (double?)GetReaderValue(reader, nameof(TestEntityA.PropertyNullableFloat));
                    entityA.PropertyNullableDecimal = (decimal?)GetReaderValue(reader, nameof(TestEntityA.PropertyNullableDecimal));
                    entityA.PropertyNullableDateTime = (DateTime?)GetReaderValue(reader, nameof(TestEntityA.PropertyNullableDateTime));
                    entityA.PropertyNullableEnum = (TestEnumA?)GetReaderValue(reader, nameof(TestEntityA.PropertyNullableEnum));

                    entityALookup.Add(entityABigInt, entityA);
                    entities.Add(entityA);
                }

                if (entityBBigInt.HasValue && !entityBLookup.TryGetValue(entityBBigInt.Value, out TestEntityB entityB))
                {
                    entityB = new TestEntityB();

                    entityB.PropertyBigInt = entityBBigInt.Value;

                    entityA.TestEntityB = entityB;
                    entityBLookup.Add(entityBBigInt.Value, entityB);
                }
                
            }

            return entities;
        }

        private IList<TestEntityA> ReadTestEntitiesNonAliased(IDataReader reader)
        {
            var entities = new List<TestEntityA>();

            while (reader.Read())
            {
                var entityA = new TestEntityA();

                entityA.PropertyBigInt = (long)GetReaderValue(reader, "property_bigint");
                entityA.PropertyInt = (int)GetReaderValue(reader, "property_int");
                entityA.PropertySmallInt = (short)GetReaderValue(reader, "property_smallint");
                entityA.PropertyTinyInt = (byte)GetReaderValue(reader, "property_tinyint");
                entityA.PropertyReal = (float)GetReaderValue(reader, "property_real");
                entityA.PropertyFloat = (double)GetReaderValue(reader, "property_float");
                entityA.PropertyDecimal = (decimal)GetReaderValue(reader, "property_decimal");
                entityA.PropertyVarChar = (string)GetReaderValue(reader, "property_varchar");
                entityA.PropertyDateTime = (DateTime)GetReaderValue(reader, "property_datetime");
                entityA.PropertyEnum = (TestEnumA)(int)GetReaderValue(reader, "property_enum");

                entityA.PropertyNullableBigInt = (long?)GetReaderValue(reader, "property_nullable_bigint");
                entityA.PropertyNullableInt = (int?)GetReaderValue(reader, "property_nullable_int");
                entityA.PropertyNullableSmallInt = (short?)GetReaderValue(reader, "property_nullable_smallint");
                entityA.PropertyNullableTinyInt = (byte?)GetReaderValue(reader, "property_nullable_tinyint");
                entityA.PropertyNullableReal = (float?)GetReaderValue(reader, "property_nullable_real");
                entityA.PropertyNullableFloat = (double?)GetReaderValue(reader, "property_nullable_float");
                entityA.PropertyNullableDecimal = (decimal?)GetReaderValue(reader, "property_nullable_decimal");
                entityA.PropertyNullableDateTime = (DateTime?)GetReaderValue(reader, "property_nullable_datetime");
                entityA.PropertyNullableEnum = (TestEnumA?)GetReaderValue(reader, "property_nullable_enum");

                entities.Add(entityA);
            }

            return entities;
        }


        #endregion
    }
}
