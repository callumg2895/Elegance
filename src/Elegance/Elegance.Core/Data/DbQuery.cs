using Elegance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Elegance.Core.Data
{
    internal abstract class DbQuery<T> : IDbQuery<T>
    {
        private readonly IList<IDbQueryParameter> _parameters;
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        private readonly string _sql;

        internal DbQuery(IDbConnection connection, IDbTransaction transaction, string sql)
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                throw new ArgumentException("Cannot create a Query on a closed connection.");
            }

            _parameters = new List<IDbQueryParameter>();
            _connection = connection;
            _transaction = transaction;

            _sql = sql;
        }

        public virtual T Result()
        {
            return Results().FirstOrDefault();
        }

        public virtual IList<T> Results()
        {
            using var command = CreateCommand();
            using var reader = command.ExecuteReader();

            return GetResultFromReader(reader);
        }

        public IDbQuery<T> SetParameter<TParam>(string name, TParam value) where TParam : IConvertible
        {
            return SetParameter(name, value, null);
        }

        public IDbQuery<T> SetParameter<TParam>(string name, TParam value, DbType? dbTypeOverride) where TParam : IConvertible
        {
            var parameter = new DbQueryParameter<TParam>(name, value, dbTypeOverride);

            _parameters.Add(parameter);

            return this;
        }

        protected IDbCommand CreateCommand()
        {
            var command = _connection.CreateCommand();

            command.Transaction = _transaction;
            command.CommandText = _sql;
            command.CommandType = CommandType.Text;

            foreach (var parameter in _parameters)
            {
                parameter.AddToCommand(command);
            }

            _parameters.Clear();

            return command;
        }

        protected abstract IList<T> GetResultFromReader(IDataReader reader);
    }
}
