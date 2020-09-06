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
        private readonly List<IDbCommand> _dbCommands;
        private readonly IDbConnection _dbConnection;

        private IDbTransaction _dbTransaction;

        internal DbSession(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _dbCommands = new List<IDbCommand>();
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

            _dbCommands.Add(command);

            return command;
        }

        public IDbQuery<T> CreateObjectQuery<T>(string sql) where T : new()
        {
            return new ObjectDbQuery<T>(_dbConnection, _dbTransaction, sql);
        }

        public IDbQuery<T> CreateScalarQuery<T>(string sql) where T : IConvertible
        {
            return new ScalarDbQuery<T>(_dbConnection, _dbTransaction, sql);
        }

        public void Dispose()
        {
            foreach (var command in _dbCommands)
            {
                command.Dispose();
            }

            _dbTransaction?.Dispose();
            _dbConnection?.Dispose();
        }
    }
}
