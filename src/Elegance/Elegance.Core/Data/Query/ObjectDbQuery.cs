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
        internal ObjectDbQuery( IDbSession session, 
                                IDbConnection connection, 
                                IDbTransaction transaction, 
                                string commandText, 
                                CommandType commandType)
            : base(session, connection, transaction, commandText, commandType)
        {

        }

        protected override IList<T> GetResultFromReader(IDbDataReader reader)
        {
            var results = new List<T>();

            while (reader.Read())
            {
                results.Add(GetResult(reader));
            }

            return results;
        }

        private T GetResult(IDbDataReader reader)
        {
            var objectMap = new ObjectMap(reader, typeof(T));

            return (T)objectMap.Value;
        }
    }
}
