using Elegance.Core.Data;
using Elegance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elegance.Core.Configuration
{
    public class Configuration
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public Configuration(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public IDbSessionFactory CreateSessionFactory()
        {
            if (_dbConnectionFactory == null)
            {
                throw new Exception("No connection factory was provided");
            }

            return new DbSessionFactory(_dbConnectionFactory);
        }
    }
}
