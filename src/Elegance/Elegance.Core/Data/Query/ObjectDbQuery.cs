using Elegance.Core.Attributes;
using Elegance.Core.Interface;
using Elegance.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Elegance.Core.Data.Query
{
    internal class ObjectDbQuery<T> : DbQuery<T> where T : new()
    {
        private readonly IList<ObjectMap> _resultSet;
        private readonly ObjectMetadataFactory _metadataFactory;

        internal ObjectDbQuery( IDbSession session, 
                                IDbConnection connection, 
                                IDbTransaction transaction, 
                                string commandText, 
                                CommandType commandType)
            : base(session, connection, transaction, commandText, commandType)
        {
            _resultSet = new List<ObjectMap>();
            _metadataFactory = new ObjectMetadataFactory();
        }

        protected override IList<T> GetResultFromReader(IDbDataReader reader)
        {
            while (reader.Read())
            {
                ReadResult(reader);
            }

            return _resultSet
                .Select(r => (T)r.Value)
                .ToList();
        }

        private void ReadResult(IDbDataReader reader)
        {
            var type = typeof(T);
            var currentObjectMap = new ObjectMap(_metadataFactory, reader, type);
            var originalObjectMap = _resultSet.Contains(currentObjectMap)
                ? _resultSet[_resultSet.IndexOf(currentObjectMap)]
                : null;

            if (originalObjectMap == null)
            {
                _resultSet.Add(currentObjectMap);
            }
            else
            {
                originalObjectMap.Merge(currentObjectMap);
            }
        }
    }
}
