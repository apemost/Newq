namespace Newq
{
    using System;

    /// <summary>
    /// The WHERE clause is used to
    /// extract only those records that fulfill a specified criterion.
    /// </summary>
    public class SelectWhereClause : WhereClause
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectWhereClause"/> class.
        /// </summary>
        /// <param name="statement"></param>
        public SelectWhereClause(Statement statement)
            : base(statement)
        {
        }

        /// <summary>
        /// SELECT column_name, aggregate_function(column_name)
        /// FROM table_name
        /// WHERE column_name operator value
        /// GROUP BY column_name
        /// </summary>
        /// <param name="customization"></param>
        /// <returns></returns>
        public GroupByClause GroupBy(Action<Target, Context> customization)
        {
            var clause = new GroupByClause(Statement);

            Statement.Clauses.Add(clause);
            clause.Target.Customize(customization);

            return clause;
        }

        /// <summary>
        /// SELECT column_name, aggregate_function(column_name)
        /// FROM table_name
        /// WHERE column_name operator value
        /// ORDER BY column_name[ASC|DESC]
        /// </summary>
        /// <param name="customization"></param>
        /// <returns></returns>
        public OrderByClause OrderBy(Action<Target, Context> customization)
        {
            var clause = new OrderByClause(Statement);

            Statement.Clauses.Add(clause);
            clause.Target.Customize(customization);

            return clause;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectStatement Union<T>(Action<Target, Context> customization) where T : class, new()
        {
            return Provider.Union<T>(Statement as SelectStatement, customization);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectStatement UnionAll<T>(Action<Target, Context> customization) where T : class, new()
        {
            return Provider.Union<T>(Statement as SelectStatement, customization, UnionType.UnionAll);
        }
    }
}
