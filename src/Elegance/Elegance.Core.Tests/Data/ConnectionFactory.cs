using Elegance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Elegance.Core.Tests.Data
{
    public sealed class ConnectionFactory : IDbConnectionFactory
    {
        #region Singleton Implementation

        static ConnectionFactory()
        {
            _instance = new ConnectionFactory();
        }

        private static readonly ConnectionFactory _instance;

        public static ConnectionFactory Instance 
        { 
            get
            {
                return _instance;
            } 
        }

        #endregion

        private readonly string _connectionString;

        private ConnectionFactory()
        {
            _connectionString = ConfigurationManager.AppSettings["connectionString"];
        }

        public IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);

            connection.Open();

            return connection;
        }
    }
}
