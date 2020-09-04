using System;
using System.Collections.Generic;
using System.Text;

namespace Elegance.Core.Interface
{
    public interface IDbSessionFactory
    {
        public IDbSession CreateSession();
    }
}
