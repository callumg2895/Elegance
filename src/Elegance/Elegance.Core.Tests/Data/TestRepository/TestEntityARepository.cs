﻿using Elegance.Core.Tests.Data.TestEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Elegance.Core.Tests.Data.TestRepository
{
    public class TestEntityARepository : Repository
    {
        public TestEntityARepository()
            : base(ConnectionFactory.Instance)
        {

        }

        public void CreateTable()
        {
            using var session = CreateSession();

            session.OpenTransaction();

            session
                .CreateCommand("create table test_entity_a (id bigint, name varchar(255))")
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

            var sql = @"
                
                insert into test_entity_a
                (
                    id,
                    name
                )
                values
                (
                    1,
                    'name'
                )

                select  tea.id      as Id,
                        tea.name    as Name

                from    test_entity_a tea (nolock)

                delete from test_entity_a";

            return session
                .CreateQuery<TestEntityA>(sql)
                .Results();
        }

        public IList<TestEntityA> GetAll_Standard()
        {
            var sql = @"
                
                insert into test_entity_a
                (
                    id,
                    name
                )
                values
                (
                    1,
                    'name'
                )

                select  tea.id      as Id,
                        tea.name    as Name

                from    test_entity_a tea (nolock)

                delete from test_entity_a";

            using var session = CreateSession();
            using var reader = session
                .CreateCommand(sql)
                .ExecuteReader();

            var entities = new List<TestEntityA>();

            while (reader.Read())
            {
                var entity = new TestEntityA();

                entity.Id = (long)reader[nameof(entity.Id)];
                entity.Name = (string)reader[nameof(entity.Name)];

                entities.Add(entity);
            }

            return entities;
        }

        #endregion
    }
}
