using Elegance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Elegance.Core.Data
{
    internal class DbQuery<T> : IDbQuery<T> where T : new()
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

        public T Result()
        {
            return Results().FirstOrDefault();
        }

        public IList<T> Results()
        {
            var results = new List<T>();

            using var command = CreateCommand();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                T result = new T();

                foreach (PropertyInfo property in typeof(T).GetProperties())
                {
                    var value = reader[property.Name];
                    var convertedValue = Convert.ChangeType(value, property.PropertyType);

                    property.SetValue(result, convertedValue);
                }

                results.Add(result);
            }

            return results;
        }

        private IDbCommand CreateCommand()
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
    }
}
