using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Interface
{
    /// <summary>
    /// An interface representing the context of a database connections. Provides methods for interacting
    /// with a database connection. 
    /// </summary>
    public interface IDbSession : IDisposable
    {
        /// <summary>
        /// Opens a new transaction (if one isn't already open) for current the database connection session.
        /// </summary>
        /// <param name="isolationLevel">The locking behaviour of the transaction</param>
        public void OpenTransaction(IsolationLevel? isolationLevel = null);

        /// <summary>
        /// Rolls back the current ongoing transaction (if any).
        /// </summary>
        public void RollbackTransaction();

        /// <summary>
        /// Commits the current ongoing transaction (if any).
        /// </summary>
        public void CommitTransaction();

        /// <summary>
        /// Creates an 'IDbCommand' using the database connection associated with this session.
        /// </summary>
        /// <param name="sql">The command text</param>
        /// <returns>The 'IDbCommand' instance created.</returns>
        public IDbCommand CreateCommand(string sql);

        /// <summary>
        /// Creates an 'IDbQuery' instance that retrieves complex object type results.
        /// </summary>
        /// <typeparam name="T">The type of the result set.</typeparam>
        /// <param name="commandText">The command text</param>
        /// <param name="commandType">The command type</param>
        /// <returns>The 'IDbQuery' instance created.</returns>
        public IDbQuery<T> CreateObjectQuery<T>(string commandText, CommandType commandType) where T : new();

        /// <summary>
        /// Creates an 'IDbQuery' instance that retrieves scalar type results.
        /// </summary>
        /// <typeparam name="T">The type of the result set.</typeparam>
        /// <param name="commandText">The command text</param>
        /// <param name="commandType">The command type</param>
        /// <returns>The 'IDbQuery' instance created.</returns>
        public IDbQuery<T> CreateScalarQuery<T>(string commandText, CommandType commandType) where T : IConvertible;
    }
}
