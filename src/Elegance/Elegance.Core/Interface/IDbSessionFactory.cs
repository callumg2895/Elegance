using System;
using System.Collections.Generic;
using System.Text;

namespace Elegance.Core.Interface
{
    /// <summary>
    /// An interface used for creating connection sessions.
    /// </summary>
    public interface IDbSessionFactory
    {
        /// <summary>
        /// Creates a new database connection session.
        /// </summary>
        /// <returns>The created 'IDbSession' instance.</returns>
        public IDbSession CreateSession();
    }
}
