using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Interface
{
    public interface IDbQueryParameterOptions
    {
        public DbType? DbTypeOverride { get; set; }

        public ParameterDirection? DirectionOverride { get; set; }

        public int? SizeOverride { get; set; }
    }
}
