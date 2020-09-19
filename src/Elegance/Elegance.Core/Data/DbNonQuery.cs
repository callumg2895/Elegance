using Elegance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Data
{
    public class DbNonQuery : IDbNonQuery, IDisposable
    {
        private readonly IDictionary<string, IDbDataParameter> _parametersLookup;

        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        private readonly IDbCommand _command;

        internal DbNonQuery(IDbConnection connection, IDbTransaction transaction, string commandText, CommandType commandType)
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                throw new ArgumentException("Cannot create a database command on a closed connection.");
            }

            _parametersLookup = new Dictionary<string, IDbDataParameter>();
            _connection = connection;
            _transaction = transaction;

            _command = _connection.CreateCommand();

            _command.Transaction = _transaction;
            _command.CommandText = commandText;
            _command.CommandType = commandType;
        }

        public IDbNonQuery SetParameter<TParam>(string name, TParam value) where TParam : IConvertible
        {
            return SetParameter(name, value, null, null);
        }

        public IDbNonQuery SetParameter<TParam>(string name, TParam value, DbType? dbTypeOverride) where TParam : IConvertible
        {
            return SetParameter(name, value, dbTypeOverride, null);
        }

        public IDbNonQuery SetParameter<TParam>(string name, TParam value, ParameterDirection? directionOverride) where TParam : IConvertible
        {
            return SetParameter(name, value, null, directionOverride);
        }

        public IDbNonQuery SetParameter<TParam>(string name, TParam value, DbType? dbTypeOverride, ParameterDirection? directionOverride) where TParam : IConvertible
        {
            var options = new DbQueryParameterOptions()
            {
                DbTypeOverride = dbTypeOverride,
                DirectionOverride = directionOverride,
                SizeOverride = null,
            };

            return SetParameter(name, value, options);
        }

        public IDbNonQuery SetParameter<TParam>(string name, TParam value, IDbQueryParameterOptions options) where TParam : IConvertible
        {
            return SetParameter(new DbQueryParameter<TParam>(name, value, options));
        }

        public IDbNonQuery SetParameter<TParam>(IDbQueryParameter<TParam> parameter)
        {
            return UpdateCommandParameters(parameter);
        }

        public TParam GetParameter<TParam>(string name) where TParam : IConvertible
        {
            return _parametersLookup.TryGetValue(name, out IDbDataParameter dbDataParameter)
                ? (TParam)dbDataParameter.Value
                : default;
        }

        private IDbNonQuery UpdateCommandParameters<TParam>(IDbQueryParameter<TParam> dbQueryParameter)
        {
            var dbDataParameter = _command.CreateParameter();
            var dbDataParameterName = dbQueryParameter.Name;

            dbDataParameter.ParameterName = dbQueryParameter.Name;
            dbDataParameter.Value = dbQueryParameter.Value;
            dbDataParameter.DbType = dbQueryParameter.DbType;
            dbDataParameter.Direction = dbQueryParameter.Direction;
            dbDataParameter.Size = dbQueryParameter.Size;

            _command.Parameters.Add(dbDataParameter);
            _parametersLookup.Add(dbDataParameterName, dbDataParameter);

            return this;
        }

        public void Execute()
        {
            _command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            _command?.Dispose();
        }
    }
}
