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
    /// The GROUP BY clause is used in conjunction with
    /// the aggregate functions to group the result-set by one or more columns.
    /// </summary>
    public class GroupByClause : Clause
    {
        /// <summary>
        /// 
        /// </summary>
        protected Target target;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupByClause"/> class.
        /// </summary>
        /// <param name="statement"></param>
        public GroupByClause(Statement statement)
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
                sql = string.Format("GROUP BY {0} ", sql);
            }

            return sql;
        }

        /// <summary>
        /// SELECT column_name, aggregate_function(column_name)
        /// FROM table_name
        /// WHERE column_name operator value
        /// GROUP BY column_name
        /// HAVING aggregate_function(column_name) operator value
        /// </summary>
        /// <param name="customization"></param>
        /// <returns></returns>
        public HavingClause Having(Action<Filter, Context> customization)
        {
            return Provider.Filtrate(new HavingClause(Statement), customization) as HavingClause;
        }

        /// <summary>
        /// SELECT column_name, aggregate_function(column_name)
        /// FROM table_name
        /// WHERE column_name operator value
        /// GROUP BY column_name
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
