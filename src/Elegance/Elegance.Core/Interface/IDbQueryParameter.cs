using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Interface
{
    /// <summary>
    /// An interface representing a parameter for a SQL command. Defines requires properties for
    /// adding a parameter value to a command.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDbQueryParameter<T>
    {
        /// <summary>
        /// The name of the parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The value of the parameter.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// The 'DbType' value of the parameter.
        /// </summary>
        public DbType DbType { get; }

        /// <summary>
        /// The 'ParameterDirection' value of the parameter.
        /// </summary>
        public ParameterDirection Direction { get; }

        /// <summary>
        /// The size value to use for the parameter.
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Get the value used for the IDbDataParameter used in the IDbCommand.
        /// </summary>
        /// <returns>The value used for the IDbDataParameter</returns>
        public object GetValueForParameter();
    }
}
