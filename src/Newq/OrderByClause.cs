namespace Newq
{
    using System;

    /// <summary>
    /// The ORDER BY clause is used to
    /// sort the result-set by one or more columns.
    /// </summary>
    public class OrderByClause : Clause
    {
        /// <summary>
        ///
        /// </summary>
        protected Target target;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderByClause"/> class.
        /// </summary>
        /// <param name="statement"></param>
        public OrderByClause(Statement statement)
            : base(statement)
        {
            target = new Target(statement.Context);
        }

        /// <summary>
        ///
        /// </summary>
        public ICustomizable<Action<Target, Context>> Target
        {
            get { return target; }
        }

        /// <summary>
        /// Returns a SQL-string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToSql()
        {
            var sql = target.GetCustomization();

            if (sql.Length > 0)
            {
                sql = string.Format("ORDER BY {0} ", sql);
            }

            return sql;
        }
    }
}
