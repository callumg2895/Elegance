using Elegance.Core.Tests.Data.TestEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Tests.Data.TestRepository
{
    public class TestEntityBRepository : BaseRepository
    {
        #region Insert

        public void InsertTestEntity_Standard(TestEntityB testEntity)
        {
            var sql = @"
                
                insert into test_entity_b
                (
                    property_bigint
                )
                values
                (
                    @property_bigint
                )";

            using var session = CreateSession();
            using var command = session.CreateCommand(sql);

            command.Parameters.Add(GetParameter("@property_bigint", testEntity.PropertyBigInt, DbType.Int64));

            command.ExecuteNonQuery();
        }

        #endregion

        #region Delete

        public void DeleteTestEntities()
        {
            using var session = CreateSession();

            var sql = @"delete from test_entity_b";

            session
                .CreateCommand(sql)
                .ExecuteNonQuery();
        }

        #endregion
    }
}
