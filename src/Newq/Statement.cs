namespace Newq
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// SQL Statement
    /// </summary>
    public abstract class Statement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Statement"/> class.
        /// </summary>
        protected Statement()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Statement"/> class.
        /// </summary>
        /// <param name="table"></param>
        protected Statement(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            Context = new Context(table);
            Clauses = new List<Clause>();
        }

        /// <summary>
        /// Database context of statement.
        /// </summary>
        public Context Context { get; protected set; }

        /// <summary>
        /// Gets or sets <see cref="Clauses"/>.
        /// </summary>
        public List<Clause> Clauses { get; protected set; }

        /// <summary>
        /// Returns a SQL-string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public abstract string ToSql();

        /// <summary>
        /// Provide sql methods.
        /// </summary>
        protected static class Provider
        {
            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="statement"></param>
            /// <param name="type"></param>
            /// <param name="customization"></param>
            /// <returns></returns>
            public static Statement Join<T>(Statement statement, JoinType type, Action<Filter, Context> customization)
            {
                var objType = typeof(T);
                JoinClause clause = null;

                if (!statement.Context.Contains(objType.Name))
                {
                    statement.Context.Add(new Table(objType));
                }

                clause = new JoinClause(statement, statement.Context[objType.Name], type);
                statement.Clauses.Add(clause);
                clause.Filter.Customize(customization);

                return statement;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="clause"></param>
            /// <param name="customization"></param>
            /// <returns></returns>
            public static WhereClause Filtrate(WhereClause clause, Action<Filter, Context> customization)
            {
                clause.Statement.Clauses.Add(clause);
                clause.Filter.Customize(customization);

                return clause;
            }
        }
    }
}
