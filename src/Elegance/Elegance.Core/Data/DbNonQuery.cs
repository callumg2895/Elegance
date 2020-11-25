using Elegance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Data
{
    public class DbNonQuery : DbExecutable, IDbNonQuery
    {
        internal DbNonQuery(    IDbSession session,
                                IDbConnection connection,
                                IDbTransaction transaction,
                                string commandText,
                                CommandType commandType)
            : base(session, connection, transaction, commandText, commandType)
        {

        }

        public IDbNonQuery SetParameter<TParam>(string name, TParam value)
        {
            return SetParameter(name, value, null, null);
        }

        public IDbNonQuery SetParameter<TParam>(string name, TParam value, DbType? dbTypeOverride)
        {
            return SetParameter(name, value, dbTypeOverride, null);
        }

        public IDbNonQuery SetParameter<TParam>(string name, TParam value, ParameterDirection? directionOverride)
        {
            return SetParameter(name, value, null, directionOverride);
        }

        public IDbNonQuery SetParameter<TParam>(string name, TParam value, DbType? dbTypeOverride, ParameterDirection? directionOverride)
        {
            var options = new DbQueryParameterOptions()
            {
                DbTypeOverride = dbTypeOverride,
                DirectionOverride = directionOverride,
                SizeOverride = null,
            };

            return SetParameter(name, value, options);
        }

        public IDbNonQuery SetParameter<TParam>(string name, TParam value, IDbQueryParameterOptions options)
        {
            return SetParameter(new DbQueryParameter<TParam>(name, value, options));
        }

        public IDbNonQuery SetParameter<TParam>(IDbQueryParameter<TParam> parameter)
        {
            return UpdateCommandParameters(parameter);
        }

        public TParam GetParameter<TParam>(string name)
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
            dbDataParameter.Value = dbQueryParameter.GetValueForParameter();
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
    }
}
