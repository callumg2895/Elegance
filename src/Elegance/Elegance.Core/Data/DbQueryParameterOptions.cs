using Elegance.Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Data
{
    internal class DbQueryParameterOptions : IDbQueryParameterOptions
    {
        public DbType? DbTypeOverride { get; set; }
        public ParameterDirection? DirectionOverride { get; set; }
        public int? SizeOverride { get; set; }
    }

}
