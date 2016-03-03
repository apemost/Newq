﻿namespace Newq
{
    using System;

    /// <summary>
    /// The HAVING clause was added to SQL 
    /// because the WHERE clause could not be used with aggregate functions.
    /// </summary>
    public class HavingClause : Clause
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HavingClause"/> class.
        /// </summary>
        /// <param name="statement"></param>
        public HavingClause(Statement statement) : base(statement)
        {

        }

        /// <summary>
        /// Returns a SQL-string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Target.ToString().Length > 0
                ? string.Format("Having {0} ", Filter)
                : string.Empty;
        }

        /// <summary>
        /// SELECT column_name, aggregate_function(column_name)
        /// FROM table_name
        /// WHERE column_name operator value
        /// GROUP BY column_name
        /// HAVING aggregate_function(column_name) operator value
        /// ORDER BY column_name [ASC|DESC]
        /// </summary>
        /// <param name="setTarget"></param>
        /// <returns></returns>
        public OrderByClause OrderBy(Action<Target> setTarget)
        {
            return Provider.SetTarget(new OrderByClause(Statement), setTarget) as OrderByClause;
        }
    }
}

