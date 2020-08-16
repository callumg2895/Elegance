using Elegance.Core.Interface;
using Elegance.Core.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;

namespace Elegance.Core
{
    public class Repository
    {
        static Repository()
        {

        }

        private readonly IDbConnectionFactory _dbConnectionFactory;

        public Repository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        protected IDbSession CreateSession()
        {
            return new DbSession(_dbConnectionFactory);
        }
    }
}
