//-----------------------------------------------------------------------
// <copyright file="IDbContext.cs" company="Premiere Digital Services">
//     Copyright Premiere Digital Services. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SimbaShoppe.Data.Databases
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    /// <summary>
    /// Interface IDB Context
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Gets the configuration-val.
        /// </summary>
        /// <value>The configuration-val.</value>
        DbContextConfiguration Configurationval { get; }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>The database.</value>
        Database Db { get; }

        /// <summary>
        /// Get Database Set
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>Database Set</returns>
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns>return INT</returns>
        int SaveChanges();

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Array of Parameters</param>
        /// <returns>Return Entities</returns>
        IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : class;

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Return Result</returns>
        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);
        
        /// <summary>
        /// Database entity Entry
        /// </summary>
        /// <param name="entity">entity object</param>
        /// <returns>Database entity entry</returns>
        DbEntityEntry Entry(object entity);
    }
}
