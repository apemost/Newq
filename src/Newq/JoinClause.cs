﻿/* Copyright 2015-2016 Andrew Lyu and Uriel Van
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
    /// The JOIN clause is used to
    /// combine rows from two or more tables,
    /// based on a common field between them.
    /// </summary>
    public class JoinClause : Clause
    {
        /// <summary>
        /// 
        /// </summary>
        protected Filter filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinClause"/> class.
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="table"></param>
        /// <param name="type"></param>
        public JoinClause(Statement statement, Table table, JoinType type = JoinType.InnerJoin)
            : base(statement)
        {
            filter = new Filter(statement.Context);
            JoinTable = table;
            JoinType = type;
        }

        /// <summary>
        /// 
        /// </summary>
        public ICustomizable<Action<Filter, Context>> Filter
        {
            get { return filter; }
        }

        /// <summary>
        /// Gets <see cref="JoinTable"/>.
        /// </summary>
        public Table JoinTable { get; }

        /// <summary>
        /// Gets <see cref="JoinType"/>.
        /// </summary>
        public JoinType JoinType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string GetJoinType()
        {
            return JoinType == JoinType.InnerJoin ? "JOIN"
                : JoinType == JoinType.LeftJoin ? "LEFT JOIN"
                : JoinType == JoinType.RightJoin ? "RIGHT JOIN"
                : JoinType == JoinType.FullJoin ? "FULL JOIN"
                : JoinType == JoinType.CrossJoin ? "CROSS JOIN"
                : string.Empty;
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
                sql = string.Format("{0} {1} ON {2} ", GetJoinType(), JoinTable, sql);
            }

            return sql;
        }
    }
}
