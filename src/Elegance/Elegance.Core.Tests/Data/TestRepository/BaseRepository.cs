using Elegance.Core.Interface;
using Elegance.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
