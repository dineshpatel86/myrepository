//-----------------------------------------------------------------------
// <copyright file="EfRepository.cs" company="Premiere Digital Services">
//     Copyright Premiere Digital Services. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SampleAPIProject.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using SimbaShoppe.Data.Databases;

    /// <summary>
    /// class EFREPOSITORY
    /// </summary>
    /// <typeparam name="T">return type</typeparam>
    public class EfRepository<T> : IRepository<T> where T : class
    {
        #region Fields

        /// <summary>
        /// The _context
        /// </summary>
        private readonly IDbContext context;

        /// <summary>
        /// The _entities
        /// </summary>
        private IDbSet<T> entities;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public EfRepository(IDbContext context)
        {
            this.context = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets no tracking when u read data for not edit
        /// </summary>
        public virtual IQueryable<T> AsNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        /// <summary>
        /// Gets whole table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        protected virtual IDbSet<T> Entities
        {
            get
            {
                if (this.entities == null)
                {
                    this.entities = this.context.Set<T>();
                }

                return this.entities;
            }
        }

        /// <summary>
        /// get record by id
        /// </summary>
        /// <param name="id">the id</param>
        /// <returns>
        /// return entity
        /// </returns>
        public virtual T GetById(object id)
        {
            return this.Entities.Find(id);
        }

        /// <summary>
        /// insert record
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <exception cref="System.ArgumentNullException">return entity</exception>
        public virtual void Insert(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                this.Entities.Add(entity);

                ////this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbex)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }
                }

                var fail = new Exception(msg, dbex);
                throw fail;
            }
        }

        /// <summary>
        /// update record
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <param name="changeState">if set to <c>true</c> [change state].</param>
        /// <exception cref="System.ArgumentNullException">return entity</exception>
        public virtual void Update(T entity, bool changeState = true)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                if (changeState)
                {
                    this.context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                }

                ////this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbex)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                var fail = new Exception(msg, dbex);
                throw fail;
            }
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public virtual void SaveChanges()
        {
            this.context.SaveChanges();
        }

        /// <summary>
        /// delete record
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <exception cref="System.ArgumentNullException">the entity</exception>
        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                this.context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                this.Entities.Remove(entity);

                ////this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbex)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                var fail = new Exception(msg, dbex);
                throw fail;
            }
        }

        /// <summary>
        /// Gets all lazy load.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="children">The children.</param>
        /// <returns>return entity</returns>
        public virtual IQueryable<T> GetAllLazyLoad(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] children)
        {
            IQueryable<T> query = this.Entities;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = children.Aggregate(query, (current, include) => current.Include(include));
            ////children.ToList().ForEach(x => query.Include(x));
            return query;
        }

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">The Parameters</param>
        /// <returns>return entities</returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : class
        {
            this.context.Db.CommandTimeout = 300;
            return this.context.ExecuteStoredProcedureList<TEntity>(commandText, parameters);
        }

        /// <summary>
        /// Executes the stored procedure.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>return element</returns>
        public IEnumerable<TElement> ExecuteStoredProcedure<TElement>(string commandText, params object[] parameters)
        {
            ////StringBuilder sp = new StringBuilder();
            ////sp.Append(commandText);
            ////foreach (var item in parameters)
            ////{
            ////    sp.Append(" @" + );
            ////}
            this.context.Db.CommandTimeout = 300;
            return this.context.SqlQuery<TElement>(commandText, parameters);
        }

        /// <summary>
        /// Executes the stored procedure list temporary.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// return list
        /// </returns>
        public IList<TEntity> ExecuteStoredProcedureListTmp<TEntity>(string commandText, params object[] parameters) where TEntity : class
        {
            this.context.Db.CommandTimeout = 300;
            return this.context.ExecuteStoredProcedureList<TEntity>(commandText, parameters);
        }

        /// <summary>
        /// Executes the stored procedure.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// return List
        /// </returns>
        public IEnumerable<TElement> ExecuteStoredProceduretmp<TElement>(string commandText, params object[] parameters)
        {
            this.context.Db.CommandTimeout = 300;
            return this.context.SqlQuery<TElement>(commandText, parameters);
        }
        /////// <summary>
        /////// Gets the mutliple tables.
        /////// </summary>
        /////// <typeparam name="TEntity">The type of the entity.</typeparam>
        /////// <param name="tableCount">The table count.</param>
        /////// <param name="commandText">The command text.</param>
        /////// <param name="parameters">The parameters.</param>
        /////// <returns></returns>
        ////public TEntity GetMutlipleTables<TEntity>(int tableCount, string commandText, params object[] parameters)
        ////{
        ////    var data = new TEntity();

        ////    using (var db = newSimbaShoppe.DataDatabases.SimbaShoppeEntities())
        ////    {
        ////        // If using Code First we need to make sure the model is built before we open the connection 
        ////        // This isn't required for models created with the EF Designer 
        ////        db.Database.Initialize(force: false);

        ////        // Create a SQL command to execute the sproc 
        ////        var cmd = db.Database.Connection.CreateCommand();
        ////        cmd.CommandText = "[dbo].[AssetParameterValidateRule]";

        ////        try
        ////        {
        ////            db.Database.Connection.Open();
        ////            //// Run the sproc  
        ////            var reader = cmd.ExecuteReader();

        ////            //// Move to second result set and read Posts 

        ////            foreach (var prop in typeof(TEntity).GetProperties())
        ////            {
        ////                var posts_new = ((IObjectContextAdapter)db)
        ////                                 .ObjectContext
        ////                                 .Translate<prop>(reader);

        ////                reader.NextResult();

        ////                typeof(TEntity).GetProperty(prop.Name).SetValue(data, objValue, null);
        ////            }

        ////            //for (int i = 0; i < tableCount; i++)
        ////            //{
        ////            //    var posts_new = ((IObjectContextAdapter)db)
        ////            //                     .ObjectContext
        ////            //                     .Translate<PDS.Entities.AssetParameterValidateRule>(reader);

        ////            //    reader.NextResult();
        ////            //}

        ////            //var posts_new = ((IObjectContextAdapter)db)
        ////            //    .ObjectContext
        ////            //    .Translate<PDS.Entities.AssetParameterValidateRule>(reader);

        ////            //reader.NextResult();

        ////            ////// Read Blogs from the first result set 
        ////            //var blogs = ((IObjectContextAdapter)db)
        ////            //    .ObjectContext
        ////            //    .Translate<PDS.Entities.OrderServiceTerritory>(reader);

        ////            //int count = blogs.Count;

        ////            ////// Move to second result set and read Posts 
        ////            //reader.NextResult();
        ////            //var posts = ((IObjectContextAdapter)db)
        ////            //    .ObjectContext
        ////            //    .Translate<PDS.Entities.OrderService>(reader);

        ////            //int postcount = posts.Count;
        ////        }
        ////        catch (Exception ex)
        ////        {
        ////        }
        ////        finally
        ////        {
        ////            db.Database.Connection.Close();
        ////        }
        ////    }
        ////}
        #endregion
    }
}
