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

        public void OpenTransaction()
        {
            if (_dbTransaction == null)
            {
                _dbTransaction = _dbConnection.BeginTransaction();
            }
        }

        public void RollbackTransaction()
        {
            if (_dbTransaction != null)
            {
                _dbTransaction.Rollback();
            }
        }

        public void CommitTransaction()
        {
            if (_dbTransaction != null)
            {
                _dbTransaction.Commit();
            }
        }

        public IDbCommand CreateCommand(string sql)
        {
            var command = _dbConnection.CreateCommand();

            command.Transaction = _dbTransaction;
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            _dbCommands.Add(command);

            return command;
        }

        public IDbQuery<T> CreateQuery<T>(string sql)
            where T : new()
        {
            return new DbQuery<T>(_dbConnection, _dbTransaction, sql);
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
