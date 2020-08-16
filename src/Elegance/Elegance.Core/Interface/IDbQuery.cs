using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Interface
{
    public interface IDbQuery<T> where T : new()
    {
        public IDbQuery<T> SetParameter(IDbDataParameter parameter);

        public IList<T> Results();

        public T Result();
    }
}
