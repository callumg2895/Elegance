using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Elegance.Core.Interface
{
    public interface IDbQuery<T>
    {
        public IDbQuery<T> SetParameter<TParam>(string name, TParam value) where TParam : IConvertible;

        public IDbQuery<T> SetParameter<TParam>(string name, TParam value, DbType? dbTypeOverride) where TParam : IConvertible;

        public IDbQuery<T> SetParameter<TParam>(string name, TParam value, ParameterDirection? directionOverride) where TParam : IConvertible;

        public IDbQuery<T> SetParameter<TParam>(string name, TParam value, DbType? dbTypeOverride, ParameterDirection? directionOverride) where TParam : IConvertible;

        public IDbQuery<T> SetParameter<TParam>(string name, TParam value, IDbQueryParameterOptions options) where TParam : IConvertible;

        public IDbQuery<T> SetParameter<TParam>(IDbQueryParameter<TParam> parameter);

        public TParam GetParameter<TParam>(string name) where TParam : IConvertible;

        public T Result();

        public IList<T> Results();
    }
}
