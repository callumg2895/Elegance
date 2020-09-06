﻿using Elegance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Data.Query
{
    internal class ScalarDbQuery<T> : DbQuery<T> where T : IConvertible
    {
        internal ScalarDbQuery(IDbConnection connection, IDbTransaction transaction, string sql)
            : base(connection, transaction, sql)
        {

        }

        protected override IList<T> GetResultFromReader(IDataReader reader)
        {
            var results = new List<T>();

            while (reader.Read())
            {
                var value = reader.GetValue(0);
                var type = typeof(T);

                if (type.IsEnum)
                {
                    var underlyingType = type.GetEnumUnderlyingType();
                    var convertedUnderlyingValue = Convert.ChangeType(value, underlyingType);
                    var convertedValue = (T)Enum.Parse(type, convertedUnderlyingValue.ToString());

                    results.Add(convertedValue);
                }
                else
                {
                    var convertedValue = (T)Convert.ChangeType(value, type);

                    results.Add(convertedValue);
                }
            }

            return results;
        }
    }
}
