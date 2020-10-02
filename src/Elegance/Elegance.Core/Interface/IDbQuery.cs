using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Elegance.Core.Interface
{
    /// <summary>
    /// An interface used for running a SQL query. Defines methods for getting/setting parameter values
    /// and retrieving result sets (singular or otherwise).
    /// </summary>
    /// <typeparam name="T">The type of the result(s) value.</typeparam>
    public interface IDbQuery<T>
    {
        /// <summary>
        /// Adds a new parameter to the internal database command, with the corresponding name and value.
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter value</typeparam>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The associated IDbQuery instance containing this parameter.</returns>
        public IDbQuery<T> SetParameter<TParam>(string name, TParam value) where TParam : IConvertible;

        /// <summary>
        /// Adds a new parameter to the internal database command, with the corresponding name and value. If 
        /// not null, the dbTypeOverride value will be used instead of the interpreted 'DbType' value.
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter value</typeparam>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="dbTypeOverride">The 'DbType' value to use for the parameter.</param>
        /// <returns>The associated IDbQuery instance containing this parameter.</returns>
        public IDbQuery<T> SetParameter<TParam>(string name, TParam value, DbType? dbTypeOverride) where TParam : IConvertible;

        /// <summary>
        /// Adds a new parameter to the internal database command, with the corresponding name and value. If
        /// not null, the directionOverride will be used instead of the default 'ParameterDirection' value.
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter value</typeparam>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="directionOverride">The 'ParameterDirection' value to use for the parameter.</param>
        /// <returns>The associated IDbQuery instance containing this parameter.</returns>
        public IDbQuery<T> SetParameter<TParam>(string name, TParam value, ParameterDirection? directionOverride) where TParam : IConvertible;

        /// <summary>
        /// Adds a new parameter to the internal database command, with the corresponding name and value. If 
        /// not null, the dbTypeOverride value will be used instead of the interpreted 'DbType' value. If not
        /// null, the directionOverride will be used instead of the default 'ParameterDirection' value.
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter value</typeparam>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="dbTypeOverride">The 'DbType' value to use for the parameter.</param>
        /// <param name="directionOverride">The 'ParameterDirection' value to use for the parameter.</param>
        /// <returns>The associated IDbQuery instance containing this parameter.</returns>
        public IDbQuery<T> SetParameter<TParam>(string name, TParam value, DbType? dbTypeOverride, ParameterDirection? directionOverride) where TParam : IConvertible;

        /// <summary>
        /// Adds a new parameter to the internal database command, with the corresponding name and value. If
        /// not null, the override values contained within the options will be used instead of the corresponding
        /// default/interpreted values
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter value</typeparam>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="options">The object containing override values for the parameter.</param>
        /// <returns>The associated IDbQuery instance containing this parameter.</returns>
        public IDbQuery<T> SetParameter<TParam>(string name, TParam value, IDbQueryParameterOptions options) where TParam : IConvertible;

        /// <summary>
        /// Adds a new parameter to the internal database command, defined by the provided 'IDbQueryParameter'
        /// implementation
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter value</typeparam>
        /// <param name="parameter"></param>
        /// <returns>The associated IDbQuery instance containing this parameter.</returns>
        public IDbQuery<T> SetParameter<TParam>(IDbQueryParameter<TParam> parameter);

        /// <summary>
        /// Retrieves the value associated with the parameter that has the provided name.
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter value</typeparam>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>The associated IDbQuery instance containing this parameter.</returns>
        /// <remarks>
        /// Typically this is used after the IDbQuery results have been retrieved, i.e the internal database
        /// command has been executed, in order to get the value of the out parameters. Eg: in the case where
        /// a stored procedure has been run.
        /// </remarks>
        public TParam GetParameter<TParam>(string name) where TParam : IConvertible;

        /// <summary>
        /// Retrieves the first result (or null if no results were found) for the query.
        /// </summary>
        /// <returns>The value of the result.</returns>
        public T Result();

        /// <summary>
        /// Retrieves the list of results (if any) for the query.
        /// </summary>
        /// <returns>The list of result values (if any).</returns>
        public IList<T> Results();

        /// <summary>
        /// Retrieves the internal data reader used for the query.
        /// </summary>
        /// <returns>The internal IDataReader instance.</returns>
        public IDataReader Reader();
    }
}
