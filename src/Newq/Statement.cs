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
            Clauses = new List<Statement>();
        }

        /// <summary>
        /// Database context of statement.
        /// </summary>
        public Context Context { get; protected set; }

        /// <summary>
        /// Gets or sets <see cref="Clauses"/>.
        /// </summary>
        public List<Statement> Clauses { get; protected set; }

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
            public static Statement Join<T>(Statement statement, JoinType type, Action<Filter, Context> customization) where T : class, new()
            {
                var objType = typeof(T);
                JoinClause clause = null;

                statement.Context.Add(new Table<T>());
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

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="statement"></param>
            /// <param name="customization"></param>
            /// <param name="type"></param>
            /// <returns></returns>
            public static SelectStatement Union<T>(SelectStatement statement, Action<Target, Context> customization, UnionType type = UnionType.Union) where T : class, new()
            {
                if (statement == null)
                {
                    throw new ArgumentNullException(nameof(statement));
                }

                var table = new Table<T>();
                var unionStatement = new SelectStatement(table);

                foreach (var tbl in statement.Context)
                {
                    if (tbl.Name != table.Name)
                    {
                        unionStatement.Context.Add(tbl);
                    }
                }

                statement.Clauses.Add(unionStatement);
                unionStatement.HasUnion = type;
                unionStatement.Target.Customize(customization);

                return unionStatement;
            }
        }
    }
}
