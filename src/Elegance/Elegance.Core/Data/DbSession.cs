using Elegance.Core.Data.Query;
using Elegance.Core.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Text;
using System.Threading;

namespace Elegance.Core.Data
{
    internal class DbSession : IDbSession
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IList<IDisposable> _disposables;
        private readonly IDbConnection _dbConnection;

        private IDbTransaction _dbTransaction;

        internal DbSession(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _disposables = new List<IDisposable>();
            _dbConnection = _dbConnectionFactory.CreateConnection();
        }

        public void OpenTransaction(IsolationLevel? isolationLevel = null) 
        {
            _dbTransaction ??= isolationLevel.HasValue
                ? _dbConnection.BeginTransaction(isolationLevel.Value)
                : _dbConnection.BeginTransaction();
        }

        public void RollbackTransaction() => _dbTransaction?.Rollback();

        public void CommitTransaction() => _dbTransaction?.Commit();

        public IDbCommand CreateCommand(string sql)
        {
            var command = _dbConnection.CreateCommand();

            command.Transaction = _dbTransaction;
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            _disposables.Add(command);

            return command;
        }

        public IDbQuery<T> CreateObjectQuery<T>(string commandText, CommandType commandType) where T : new()
        {
            var query = new ObjectDbQuery<T>(this, _dbConnection, _dbTransaction, commandText, commandType);

            _disposables.Add(query);

            return query;
        }

        public IDbQuery<T> CreateScalarQuery<T>(string commandText, CommandType commandType) where T : IConvertible
        {
            var query = new ScalarDbQuery<T>(this, _dbConnection, _dbTransaction, commandText, commandType);

            _disposables.Add(query);

            return query;
        }

        public IDbNonQuery CreateNonQuery(string commandText, CommandType commandType)
        {
            var nonQuery = new DbNonQuery(this, _dbConnection, _dbTransaction, commandText, commandType);

            _disposables.Add(nonQuery);

            return nonQuery;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable?.Dispose();
            }

            _dbTransaction?.Dispose();
            _dbConnection?.Dispose();
        }
    }
}
