using Elegance.Core.Tests.Data.TestEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Tests.Data.TestRepository
{
    public class TestEntityDRepository : BaseRepository
    {
        #region Insert

        public void InsertTestEntity_Standard(TestEntityD testEntity)
        {
            var sql = @"
                
                insert into test_entity_d
                (
                    property_bigint,
                    property_varchar
                )
                values
                (
                    @property_bigint,
                    @property_varchar
                )";

            using var session = CreateSession();
            using var command = session.CreateCommand(sql);

            command.Parameters.Add(GetParameter("@property_bigint", testEntity.PropertyBigInt, DbType.Int64));
            command.Parameters.Add(GetParameter("@property_varchar", testEntity.PropertyVarChar, DbType.String));

            command.ExecuteNonQuery();
        }

        #endregion

        #region Delete

        public void DeleteTestEntities()
        {
            using var session = CreateSession();

            var sql = @"delete from test_entity_d";

            session
                .CreateCommand(sql)
                .ExecuteNonQuery();
        }

        #endregion
    }
}
