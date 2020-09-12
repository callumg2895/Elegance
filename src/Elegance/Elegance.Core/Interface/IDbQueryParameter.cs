using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Interface
{
    public interface IDbQueryParameter<T>
    {
        public string Name { get; }
        public T Value { get; }
        public DbType DbType { get; }
        public ParameterDirection Direction { get; }
        public int Size { get; }
    }
}
