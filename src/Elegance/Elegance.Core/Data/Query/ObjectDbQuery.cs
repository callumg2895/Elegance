using Elegance.Core.Attributes;
using Elegance.Core.Interface;
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
        private readonly Dictionary<PropertyInfo, string> _propertyAliasLookup;

        private DataTable _schemaTable;

        internal ObjectDbQuery(IDbConnection connection, IDbTransaction transaction, string commandText, CommandType commandType)
            : base(connection, transaction, commandText, commandType)
        {
            _propertyAliasLookup = new Dictionary<PropertyInfo, string>();
        }

        protected override IList<T> GetResultFromReader(IDataReader reader)
        {
            var results = new List<T>();

            while (reader.Read())
            {
                var result = new T();

                foreach (PropertyInfo property in typeof(T).GetProperties())
                {
                    var value = GetReaderValue(reader, property);
                    var type = property.PropertyType;
                    var nullUnderlyingType = Nullable.GetUnderlyingType(type);

                    if (value == DBNull.Value)
                    {
                        property.SetValue(result, null);
                    }
                    else if (type.IsEnum)
                    {
                        var enumUnderlyingType = (nullUnderlyingType ?? type).GetEnumUnderlyingType();
                        var convertedUnderlyingValue = Convert.ChangeType(value, enumUnderlyingType);
                        var convertedValue = Enum.Parse(type, convertedUnderlyingValue.ToString());

                        property.SetValue(result, convertedValue);
                    }
                    else
                    {
                        var convertedValue = Convert.ChangeType(value, nullUnderlyingType ?? type);

                        property.SetValue(result, convertedValue);
                    }
                }

                results.Add(result);
            }

            return results;
        }

        private object GetReaderValue(IDataReader reader, PropertyInfo property)
        {
            if (_propertyAliasLookup.TryGetValue(property, out string alias))
            {
                return reader[alias];
            }
            else
            {
                alias = GetAliasOptions(property)
                    .Where(o => !string.IsNullOrEmpty(o))
                    .FirstOrDefault(o => HasColumn(reader, o));
            }

            if (string.IsNullOrEmpty(alias))
            {
                return null;
            }

            _propertyAliasLookup.Add(property, alias);

            return reader[alias];
        }

        private List<string> GetAliasOptions(PropertyInfo property)
        {
            var aliasOptions = new List<string>();
            var columnName = (property.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute)?.Name;
            var alternateAliases = property.GetCustomAttributes()
                .Where(p => p is AlternateAliasAttribute)
                .Select(p => (p as AlternateAliasAttribute).Name);

            aliasOptions.Add(property.Name);
            aliasOptions.Add(columnName);
            aliasOptions.AddRange(alternateAliases);

            return aliasOptions;
        }

        private bool HasColumn(IDataReader reader, string columnName)
        {
            if (_schemaTable == null)
            {
                _schemaTable = reader.GetSchemaTable();
            }

            foreach (DataRow row in _schemaTable.Rows)
            {
                if (row["ColumnName"].ToString() == columnName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
