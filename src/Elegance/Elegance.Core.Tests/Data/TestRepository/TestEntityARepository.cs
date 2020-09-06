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
                select  tea.property_bigint      as PropertyBigInt,
                        tea.property_int         as PropertyInt,
                        tea.property_smallint    as PropertySmallInt,
                        tea.property_tinyint     as PropertyTinyInt,
                        tea.property_varchar     as PropertyVarChar,
                        tea.property_datetime    as PropertyDateTime

                from    test_entity_a tea (nolock)";

        private const string _testEntitySelectCountText = @"
                select  count(*)
                from    test_entity_a tea (nolock)";

        public TestEntityARepository()
            : base()
        {

        }

        public void CreateTable()
        {
            using var session = CreateSession();

            session.OpenTransaction();

            session
                .CreateCommand(@"
                    create table test_entity_a 
                    (
                        property_bigint     bigint,
                        property_int        int,
                        property_smallint   smallint,
                        property_tinyint    tinyint,
                        property_varchar    varchar(255),
                        property_datetime   datetime
                    )")
                .ExecuteNonQuery();

            session.CommitTransaction();
        }

        public void DropTable()
        {
            using var session = CreateSession();

            session
                .CreateCommand("drop table test_entity_a")
                .ExecuteNonQuery();
        }


        #region GetAll

        public IList<TestEntityA> GetAll_Query()
        {
            using var session = CreateSession();

            return session
                .CreateObjectQuery<TestEntityA>(_testEntitySelectText)
                .Results();
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
                .CreateScalarQuery<int>(_testEntitySelectCountText)
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
                .AppendLine("and tea.property_varchar = @property_varchar")
                .AppendLine("and tea.property_datetime = @property_datetime")
                .ToString();

            using var session = CreateSession();

            return session
                .CreateObjectQuery<TestEntityA>(sql)
                .SetParameter<Int64>("@property_bigint", testEntity.PropertyBigInt)
                .SetParameter<Int32>("@property_int", testEntity.PropertyInt)
                .SetParameter<Int16>("@property_smallint", testEntity.PropertySmallInt)
                .SetParameter<Byte>("@property_tinyint", testEntity.PropertyTinyInt)
                .SetParameter<String>("@property_varchar", testEntity.PropertyVarChar)
                .SetParameter<DateTime>("@property_datetime", testEntity.PropertyDateTime, DbType.DateTime)
                .Result();
        }

        public TestEntityA GetByAllProperties_Standard(TestEntityA testEntity)
        {
            var sql = new StringBuilder()
                .AppendLine(_testEntitySelectText)
                .AppendLine("where tea.property_bigint = @property_bigint")
                .AppendLine("and tea.property_int = @property_int")
                .AppendLine("and tea.property_smallint = @property_smallint")
                .AppendLine("and tea.property_tinyint = @property_tinyint")
                .AppendLine("and tea.property_varchar = @property_varchar")
                .AppendLine("and tea.property_datetime = @property_datetime")
                .ToString();

            using var session = CreateSession();
            using var command = session.CreateCommand(sql);

            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_bigint", Value = testEntity.PropertyBigInt, DbType = DbType.Int64 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_int", Value = testEntity.PropertyInt, DbType = DbType.Int32 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_smallint", Value = testEntity.PropertySmallInt, DbType = DbType.Int16 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_tinyint", Value = testEntity.PropertyTinyInt, DbType = DbType.Byte });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_varchar", Value = testEntity.PropertyVarChar, DbType = DbType.AnsiString });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_datetime", Value = testEntity.PropertyDateTime, DbType = DbType.DateTime });

            using var reader = command.ExecuteReader();

            return ReadTestEntities(reader).FirstOrDefault();
        }

        #endregion

        #region Insert

        public void InsertTestEntity(TestEntityA testEntity)
        {
            var sql = @"
                
                insert into test_entity_a
                (
                        property_bigint     ,
                        property_int        ,
                        property_smallint   ,
                        property_tinyint    ,
                        property_varchar    ,
                        property_datetime
                )
                values
                (
                    @property_bigint,
                    @property_int,
                    @property_smallint,
                    @property_tinyint,
                    @property_varchar,
                    @property_datetime
                )";

            using var session = CreateSession();
            using var command = session.CreateCommand(sql);

            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_bigint", Value = testEntity.PropertyBigInt, DbType = DbType.Int64 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_int", Value = testEntity.PropertyInt, DbType = DbType.Int32 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_smallint", Value = testEntity.PropertySmallInt, DbType = DbType.Int16 });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_tinyint", Value = testEntity.PropertyTinyInt, DbType = DbType.Byte });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_varchar", Value = testEntity.PropertyVarChar, DbType = DbType.AnsiString });
            command.Parameters.Add(new SqlParameter() { ParameterName = "@property_datetime", Value = testEntity.PropertyDateTime, DbType = DbType.DateTime });

            command.ExecuteNonQuery();
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
                entity.PropertyVarChar = (string)reader[nameof(entity.PropertyVarChar)];
                entity.PropertyDateTime = (DateTime)reader[nameof(entity.PropertyDateTime)];

                entities.Add(entity);
            }

            return entities;
        }

        #endregion
    }
}
