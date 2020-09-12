using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Interface
{
    /// <summary>
    /// An interface representing a set of override values for an IDbQueryParameter implementation. Defines
    /// overrides for 'DbType', 'ParameterDirection' and 'Size'.
    /// </summary>
    public interface IDbQueryParameterOptions
    {
        /// <summary>
        /// The override value for the 'DbType' field.
        /// </summary>
        public DbType? DbTypeOverride { get; set; }

        /// <summary>
        /// The override value for the 'ParameterDirection' field.
        /// </summary>
        public ParameterDirection? DirectionOverride { get; set; }

        /// <summary>
        /// The override value for the 'Size' field.
        /// </summary>
        public int? SizeOverride { get; set; }
    }
}
