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
        private readonly IList<IDbDataParameter> _parameters;
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        private readonly string _sql;

        internal DbQuery(IDbConnection connection, IDbTransaction transaction, string sql)
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                throw new ArgumentException("Cannot create a Query on a closed connection.");
            }

            _parameters = new List<IDbDataParameter>();
            _connection = connection;
            _transaction = transaction;

            _sql = sql;
        }

        public T Result()
        {
            return Results().First();
        }

        public IList<T> Results()
        {
            var results = new List<T>();

            using var command = _connection.CreateCommand();

            command.Transaction = _transaction;
            command.CommandText = _sql;
            command.CommandType = CommandType.Text;

            foreach (var parameter in _parameters)
            {
                command.Parameters.Add(parameter);
            }

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

        public IDbQuery<T> SetParameter(IDbDataParameter parameter)
        {
            _parameters.Add(parameter);

            return this;
        }
    }
}
