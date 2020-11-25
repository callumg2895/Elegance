using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Interface
{
    /// <summary>
    /// An interface that extends <see cref="IDataReader"/> with extra functionality.
    /// <para/>
    /// From <see cref="IDataReader"/>:
    /// <para/>
    /// <inheritdoc/>
    /// </summary>
    public interface IDbDataReader : IDataReader
    {
        /// <summary>
        /// Gets the object represented by the provided alias, provided that there is an object to be retrieved
        /// and it has not been retrieved previously.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns>The object represented by the provided alias, or null if no such object exists.</returns>
        public object GetValue(string alias);

        /// <summary>
        /// Checks to see if there is an object represented by the provided alias, and that object has been
        /// retrieved previously.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns>A boolean representing whether or not a value exists for this alias.</returns>
        public bool HasValue(string alias);
    }
}
