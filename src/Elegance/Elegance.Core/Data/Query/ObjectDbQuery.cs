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
        private readonly ISet<ObjectMap> _resultSetHash;
        private readonly ObjectMetadataFactory _metadataFactory;

        internal ObjectDbQuery( IDbSession session, 
                                IDbConnection connection, 
                                IDbTransaction transaction, 
                                string commandText, 
                                CommandType commandType)
            : base(session, connection, transaction, commandText, commandType)
        {
            _resultSet = new List<ObjectMap>();
            _resultSetHash = new HashSet<ObjectMap>();
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
            var currentObjectMap = new ObjectMap(_metadataFactory, reader, typeof(T));
            
            if (_resultSetHash.Contains(currentObjectMap))
            {
                var index = _resultSet.IndexOf(currentObjectMap);
                var originalObjectMap = _resultSet[index];

                originalObjectMap.Merge(currentObjectMap);
            }
            else
            {
                _resultSet.Add(currentObjectMap);
                _resultSetHash.Add(currentObjectMap);
            }
        }
    }
}
