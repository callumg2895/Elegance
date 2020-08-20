using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Interface
{
    public interface IDbQueryParameter
    {
        public void AddToCommand(IDbCommand command);
    }
}
