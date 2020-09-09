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
        internal ObjectDbQuery(IDbConnection connection, IDbTransaction transaction, string commandText, CommandType commandType)
            : base(connection, transaction, commandText, commandType)
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
                    var type = property.PropertyType;

                    if (type.IsEnum)
                    {
                        var underlyingType = type.GetEnumUnderlyingType();
                        var convertedUnderlyingValue = Convert.ChangeType(value, underlyingType);
                        var convertedValue = Enum.Parse(type, convertedUnderlyingValue.ToString());

                        property.SetValue(result, convertedValue);
                    }
                    else
                    {
                        var convertedValue = Convert.ChangeType(value, type);

                        property.SetValue(result, convertedValue);
                    }
                }

                results.Add(result);
            }

            return results;
        }
    }
}
