using Elegance.Core.Interface;
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
                var result = reader.GetValue(0);
                var value = (T)Convert.ChangeType(result, typeof(T));

                results.Add(value);
            }

            return results;
        }
    }
}
