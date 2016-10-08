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
    /// The DELETE statement is used to
    /// delete rows in a table.
    /// </summary>
    public class DeleteStatement : Statement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteStatement"/> class.
        /// </summary>
        /// <param name="table"></param>
        public DeleteStatement(Table table)
            : base(table)
        {
        }

        /// <summary>
        /// Gets or sets <see cref="TopRows"/>.
        /// </summary>
        public int TopRows { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IsPercent"/>.
        /// </summary>
        public bool IsPercent { get; set; }

        /// <summary>
        /// Returns a SQL-string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToSql()
        {
            var sql = string.Format("DELETE {0}FROM {1} ", GetParameters(), Context[0]);

            Clauses.ForEach(clause => sql += clause.ToSql());

            return sql;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string GetParameters()
        {
            var topClause = string.Empty;

            if (TopRows > 0)
            {
                topClause = IsPercent
                    ? string.Format("TOP({0}) PERCENT ", TopRows)
                    : string.Format("TOP({0}) ", TopRows);
            }

            return topClause;
        }

        /// <summary>
        /// DELETE
        /// FROM table_name1
        /// INNER JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public DeleteStatement Join<T>(Action<Filter, Context> customization) where T : class, new()
        {
            return Provider.Join<T>(this, JoinType.InnerJoin, customization) as DeleteStatement;
        }

        /// <summary>
        /// DELETE
        /// FROM table_name1
        /// LEFT JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public DeleteStatement LeftJoin<T>(Action<Filter, Context> customization) where T : class, new()
        {
            return Provider.Join<T>(this, JoinType.LeftJoin, customization) as DeleteStatement;
        }

        /// <summary>
        /// DELETE
        /// FROM table_name1
        /// RIGHT JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public DeleteStatement RightJoin<T>(Action<Filter, Context> customization) where T : class, new()
        {
            return Provider.Join<T>(this, JoinType.RightJoin, customization) as DeleteStatement;
        }

        /// <summary>
        /// DELETE
        /// FROM table_name1
        /// FULL JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public DeleteStatement FullJoin<T>(Action<Filter, Context> customization) where T : class, new()
        {
            return Provider.Join<T>(this, JoinType.FullJoin, customization) as DeleteStatement;
        }

        /// <summary>
        /// DELETE
        /// FROM table_name1
        /// CROSS JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public DeleteStatement CrossJoin<T>(Action<Filter, Context> customization) where T : class, new()
        {
            return Provider.Join<T>(this, JoinType.CrossJoin, customization) as DeleteStatement;
        }

        /// <summary>
        /// DELETE
        /// FROM table_name
        /// WHERE column_name operator value
        /// </summary>
        /// <param name="customization"></param>
        /// <returns></returns>
        public WhereClause Where(Action<Filter, Context> customization)
        {
            return Provider.Filtrate(new WhereClause(this), customization);
        }
    }
}
