using SimbaShoppe.Data.Databases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleAPIProject.Data.Database
{
   public partial class SimbaShoppeEntities : DbContext,IDbContext
    {
        public System.Data.Entity.Database Db
        {
            get
            {
                return this.Database;
            }
        }

        /// <summary>
        /// Gets the configuration-val.
        /// </summary>
        /// <value>
        /// The configuration-val.
        /// </value>
        public DbContextConfiguration Configurationval
        {
            get
            {
                return this.Configuration;
            }
        }

        /// <summary>
        /// Get DBSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>return DBSet</returns>
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">The Parameters</param>
        /// <returns>return ENTITIES</returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : class
        {
            ////add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                    {
                        throw new Exception("Not support parameter type");
                    }

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        ////output parameter
                        commandText += " output";
                    }
                }
            }

            return this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();
        }


        /// <summary>
        /// Nilesh Prajapati
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<TElement> ExecuteStoredProcedure<TElement>(string commandText, params object[] parameters)
        {
            ////StringBuilder sp = new StringBuilder();
            ////sp.Append(commandText);
            ////foreach (var item in parameters)
            ////{
            ////    sp.Append(" @" + );
            ////}
            this.Database.CommandTimeout = 300;
            return this.Database.SqlQuery<TElement>(commandText, parameters);
        }
        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">THE PARAMETERS TO APPLY TO THE SQL QUERY STRING.</param>
        /// <returns>return RESULT</returns>
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }
    }
}
