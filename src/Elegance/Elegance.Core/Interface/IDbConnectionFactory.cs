using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Interface
{
    public interface IDbConnectionFactory
    {
        public IDbConnection CreateConnection();
    }
}
