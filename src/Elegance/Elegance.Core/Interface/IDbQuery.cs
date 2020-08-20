using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Interface
{
    public interface IDbQuery<T> where T : new()
    {
        public IDbQuery<T> SetParameter<TParam>(string name, TParam value) where TParam : IConvertible;

        public IDbQuery<T> SetParameter<TParam>(string name, TParam value, DbType? dbTypeOverride) where TParam : IConvertible;

        public T Result();

        public IList<T> Results();
    }
}
