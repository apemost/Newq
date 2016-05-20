﻿/* Copyright 2015-2016 Andrew Lyu & Uriel Van
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
