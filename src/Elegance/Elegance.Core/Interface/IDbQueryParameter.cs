﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Interface
{
    public interface IDbQueryParameter
    {
        public string Name { get; }

        public IDbDataParameter AddToCommand(IDbCommand command);
    }
}
