/* Copyright 2015-2016 Andrew Lyu and Uriel Van
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Newq
{
    using System;

    /// <summary>
    /// The HAVING clause was added to SQL 
    /// because the WHERE clause could not be used with aggregate functions.
    /// </summary>
    public class HavingClause : WhereClause
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HavingClause"/> class.
        /// </summary>
        /// <param name="statement"></param>
        public HavingClause(Statement statement)
            : base(statement)
        {

        }

        /// <summary>
        /// Returns a SQL-string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToSql()
        {
            var sql = filter.GetCustomization();

            if (sql.Length > 0)
            {
                sql = string.Format("HAVING {0} ", sql);
            }

            return sql;
        }

        /// <summary>
        /// SELECT column_name, aggregate_function(column_name)
        /// FROM table_name
        /// WHERE column_name operator value
        /// GROUP BY column_name
        /// HAVING aggregate_function(column_name) operator value
        /// ORDER BY column_name [ASC|DESC]
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

