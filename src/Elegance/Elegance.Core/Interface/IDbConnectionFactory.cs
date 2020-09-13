using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Interface
{
    /// <summary>
    /// In interface used for creating database connections.
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Creates a database connection.
        /// </summary>
        /// <returns>The created 'IDbConnection' instance.</returns>
        public IDbConnection CreateConnection();
    }
}
