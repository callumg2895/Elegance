using Elegance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elegance.Core.Data
{
    public class DbSessionFactory : IDbSessionFactory
    {
        IDbConnectionFactory _dbConnectionFactory;

        internal DbSessionFactory(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public IDbSession CreateSession()
        {
            return new DbSession(_dbConnectionFactory);
        }
    }
}
