using Elegance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Elegance.Core.Data.Query
{
    internal class ObjectDbQuery<T> : DbQuery<T> where T : new()
    {
        internal ObjectDbQuery(IDbConnection connection, IDbTransaction transaction, string sql)
            : base(connection, transaction, sql)
        {

        }

        protected override IList<T> GetResultFromReader(IDataReader reader)
        {
            var results = new List<T>();

            while (reader.Read())
            {
                var result = new T();

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
    }
}
