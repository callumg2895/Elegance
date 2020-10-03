using Elegance.Core.Interface;
using Elegance.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Elegance.Core.Tests.Data.TestRepository
{
    public class BaseRepository
    {
        private static IDbSessionFactory _dbSessionFactory;

        static BaseRepository()
        {
            var configuration = new Configuration.Configuration(ConnectionFactory.Instance);

            _dbSessionFactory = configuration.CreateSessionFactory();
        }

        public BaseRepository()
        {

        }

        protected IDbSession CreateSession()
        {
            return _dbSessionFactory.CreateSession();
        }

        protected object GetReaderValue(IDataReader reader, string name)
        {
            return reader[name] == DBNull.Value
                ? null
                : reader[name];
        }

        protected SqlParameter GetParameter<T>(string name, T value, DbType dbType)
        {


            return new SqlParameter
            {
                ParameterName = name,
                Value = value == null 
                    ? DBNull.Value
                    : (object)value,
                DbType = dbType
            };
        }
    }
}
