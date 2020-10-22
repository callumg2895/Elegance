using Elegance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Data
{
    public abstract class DbExecutable : IDisposable
    {
        protected readonly IDictionary<string, IDbDataParameter> _parametersLookup;

        protected readonly IDbSession _session;
        protected readonly IDbConnection _connection;
        protected readonly IDbTransaction _transaction;
        protected readonly IDbCommand _command;

        internal DbExecutable(  IDbSession session,
                                IDbConnection connection,
                                IDbTransaction transaction,
                                string commandText,
                                CommandType commandType)
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                throw new ArgumentException("Cannot create a database executable on a closed connection.");
            }

            _parametersLookup = new Dictionary<string, IDbDataParameter>();

            _session = session;
            _connection = connection;
            _transaction = transaction;

            _command = _connection.CreateCommand();

            _command.Transaction = _transaction;
            _command.CommandText = commandText;
            _command.CommandType = commandType;
        }

        public void Dispose()
        {
            _command?.Dispose();
        }
    }
}
