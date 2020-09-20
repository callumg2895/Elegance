using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Elegance.Core.Interface
{
    /// <summary>
    /// An interface used for running a SQL command. Defines methods for getting/setting parameter values
    /// and executing internal commands.
    /// </summary>
    public interface IDbNonQuery
    {
        /// <summary>
        /// Adds a new parameter to the internal database command, with the corresponding name and value.
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter value</typeparam>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The associated IDbNonQuery instance containing this parameter.</returns>
        public IDbNonQuery SetParameter<TParam>(string name, TParam value) where TParam : IConvertible;

        /// <summary>
        /// Adds a new parameter to the internal database command, with the corresponding name and value. If 
        /// not null, the dbTypeOverride value will be used instead of the interpreted 'DbType' value.
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter value</typeparam>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="dbTypeOverride">The 'DbType' value to use for the parameter.</param>
        /// <returns>The associated IDbNonQuery instance containing this parameter.</returns>
        public IDbNonQuery SetParameter<TParam>(string name, TParam value, DbType? dbTypeOverride) where TParam : IConvertible;

        /// <summary>
        /// Adds a new parameter to the internal database command, with the corresponding name and value. If
        /// not null, the directionOverride will be used instead of the default 'ParameterDirection' value.
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter value</typeparam>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="directionOverride">The 'ParameterDirection' value to use for the parameter.</param>
        /// <returns>The associated IDbNonQuery instance containing this parameter.</returns>
        public IDbNonQuery SetParameter<TParam>(string name, TParam value, ParameterDirection? directionOverride) where TParam : IConvertible;

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
        /// <returns>The associated IDbNonQuery instance containing this parameter.</returns>
        public IDbNonQuery SetParameter<TParam>(string name, TParam value, DbType? dbTypeOverride, ParameterDirection? directionOverride) where TParam : IConvertible;

        /// <summary>
        /// Adds a new parameter to the internal database command, with the corresponding name and value. If
        /// not null, the override values contained within the options will be used instead of the corresponding
        /// default/interpreted values
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter value</typeparam>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="options">The object containing override values for the parameter.</param>
        /// <returns>The associated IDbNonQuery instance containing this parameter.</returns>
        public IDbNonQuery SetParameter<TParam>(string name, TParam value, IDbQueryParameterOptions options) where TParam : IConvertible;

        /// <summary>
        /// Adds a new parameter to the internal database command, defined by the provided 'IDbQueryParameter'
        /// implementation
        /// </summary>
        /// <typeparam name="TParam">The type of the parameter value</typeparam>
        /// <param name="parameter"></param>
        /// <returns>The associated IDbNonQuery instance containing this parameter.</returns>
        public IDbNonQuery SetParameter<TParam>(IDbQueryParameter<TParam> parameter);

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
        /// Executes the internal IDbCommand
        /// </summary>
        public void Execute();
    }
}
