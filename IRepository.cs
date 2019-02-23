//-----------------------------------------------------------------------
// <copyright file="IRepository.cs" company="Premiere Digital Services">
//     Copyright Premiere Digital Services. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SampleAPIProject.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// I Repository
    /// </summary>
    /// <typeparam name="T">Interface Repository</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets no tracking when u read data for not edit
        /// </summary>
        IQueryable<T> AsNoTracking { get; }

        /// <summary>
        /// Gets whole table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets record by id
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>return entity</returns>
        T GetById(object id);

        /// <summary>
        /// insert record
        /// </summary>
        /// <param name="entity">return entity</param>
        void Insert(T entity);

        /// <summary>
        /// update record
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="changeState">if set to <c>true</c> [change state].</param>
        /// <exception cref="System.ArgumentNullException">return entity</exception>
        void Update(T entity, bool changeState = true);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// delete record
        /// </summary>
        /// <param name="entity">return entity</param>
        void Delete(T entity);

        /// <summary>
        /// Gets all lazy load.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="children">The children.</param>
        /// <returns>Query able List</returns>
        IQueryable<T> GetAllLazyLoad(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] children);
        ////IQueryable<T> GetAllLazyLoad(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] children);

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">The Parameters</param>
        /// <returns>return Entities</returns>
        IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : class;

        /// <summary>
        /// Executes the stored procedure.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>return List</returns>
        IEnumerable<TElement> ExecuteStoredProcedure<TElement>(string commandText, params object[] parameters);

        /// <summary>
        /// Executes the stored procedure list temporary.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>return list</returns>
        IList<TEntity> ExecuteStoredProcedureListTmp<TEntity>(string commandText, params object[] parameters) where TEntity : class;

        /// <summary>
        /// Executes the stored procedure.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>return list</returns>
        IEnumerable<TElement> ExecuteStoredProceduretmp<TElement>(string commandText, params object[] parameters);
        /////// <summary>
        /////// Gets the mutliple tables.
        /////// </summary>
        /////// <typeparam name="TEntity">The type of the entity.</typeparam>
        /////// <param name="tableCount">The table count.</param>
        /////// <param name="commandText">The command text.</param>
        /////// <param name="parameters">The parameters.</param>
        /////// <returns></returns>
        ////TEntity GetMutlipleTables<TEntity>(int tableCount, string commandText, params object[] parameters);
    }
}
