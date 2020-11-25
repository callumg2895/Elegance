using Elegance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Elegance.Core.Data
{
    internal abstract class DbQuery<T> : DbExecutable, IDbQuery<T>
    {
        internal DbQuery(   IDbSession session, 
                            IDbConnection connection, 
                            IDbTransaction transaction, 
                            string commandText, 
                            CommandType commandType)
            : base(session, connection, transaction, commandText, commandType)
        {

        }

        public virtual T Result()
        {
            return Results().FirstOrDefault();
        }

        public virtual IList<T> Results()
        {
            using var reader = Reader();

            return GetResultFromReader(reader);
        }

        public virtual IDbDataReader Reader()
        {
            return new DbDataReader(_command.ExecuteReader());
        }

        public IDbQuery<T> SetParameter<TParam>(string name, TParam value)
        {
            return SetParameter(name, value, null, null);
        }

        public IDbQuery<T> SetParameter<TParam>(string name, TParam value, DbType? dbTypeOverride)
        {
            return SetParameter(name, value, dbTypeOverride, null);
        }

        public IDbQuery<T> SetParameter<TParam>(string name, TParam value, ParameterDirection? directionOverride)
        {
            return SetParameter(name, value, null, directionOverride);
        }

        public IDbQuery<T> SetParameter<TParam>(string name, TParam value, DbType? dbTypeOverride, ParameterDirection? directionOverride)
        {
            var options = new DbQueryParameterOptions()
            {
                DbTypeOverride = dbTypeOverride,
                DirectionOverride = directionOverride,
                SizeOverride = null,
            };

            return SetParameter(name, value, options);
        }

        public IDbQuery<T> SetParameter<TParam>(string name, TParam value, IDbQueryParameterOptions options)
        {
            return SetParameter(new DbQueryParameter<TParam>(name, value, options));
        }

        public IDbQuery<T> SetParameter<TParam>(IDbQueryParameter<TParam> parameter)
        {
            return UpdateCommandParameters(parameter);
        }

        public TParam GetParameter<TParam>(string name)
        {
            return _parametersLookup.TryGetValue(name, out IDbDataParameter dbDataParameter)
                ? (TParam)dbDataParameter.Value
                : default;
        }

        private IDbQuery<T> UpdateCommandParameters<TParam>(IDbQueryParameter<TParam> dbQueryParameter)
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

        protected abstract IList<T> GetResultFromReader(IDbDataReader reader);
    }
}
