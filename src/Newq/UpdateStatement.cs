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
    using System.Collections.Generic;
    using Extensions;

    /// <summary>
    /// The UPDATE statement is used to
    /// update existing records in a table.
    /// </summary>
    public class UpdateStatement : Statement
    {
        /// <summary>
        /// 
        /// </summary>
        protected Target target;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateStatement"/> class.
        /// </summary>
        /// <param name="table"></param>
        public UpdateStatement(Table table)
            : base(table)
        {
            target = new Target(Context);
            ObjectList = new List<object>();
        }

        /// <summary>
        /// 
        /// </summary>
        public ICustomizable<Action<Target, Context>> Target
        {
            get { return target; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<object> ObjectList { get; }

        /// <summary>
        /// 
        /// </summary>
        public string JoinOnPrimaryKey
        {
            get
            {
                return Context[0].PrimaryKey.Count == 0 ? null
                    : Context[0].PrimaryKey[0].Name;
            }
        }

        /// <summary>
        /// Returns a SQL-string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToSql()
        {
            var targetStr = GetTarget();

            if (targetStr.Length == 0)
            {
                return string.Empty;
            }

            var sql = string.Format("UPDATE {0} SET {1} FROM {0} ", Context[0], targetStr);

            if (ObjectList.Count > 0 && !string.IsNullOrWhiteSpace(JoinOnPrimaryKey))
            {
                var type = ObjectList[0].GetType();
                var values = string.Empty;
                var val = string.Empty;
                var items = string.Empty;

                ObjectList.ForEach(obj => {
                    val += string.Format(",{0}", type.GetProperty(JoinOnPrimaryKey).GetValue(obj).ToSqlValue());

                    target.Items.ForEach(item => {
                        val += string.Format(",{0}", type.GetProperty((item as Column).Name).GetValue(obj).ToSqlValue());
                    });

                    values += string.Format(",({0})", val.Substring(1));
                    val = string.Empty;
                });

                target.Items.ForEach(item => {
                    if (item != null)
                    {
                        items += string.Format(",[{0}]", (item as Column).Name);
                    }
                });

                sql += string.Format("JOIN (VALUES {0}) AS [$UPDATE_SOURCE] ([{1}],{2}) ON {3} = [$UPDATE_SOURCE].[{1}] ",
                    values.Substring(1), JoinOnPrimaryKey, items.Substring(1), Context[0][JoinOnPrimaryKey]);
            }

            if (Clauses.Count > 0)
            {
                Clauses.ForEach(clause => sql += clause.ToSql());
            }

            return sql;
        }

        /// <summary>
        /// Returns a string that represents the current statement target.
        /// </summary>
        /// <returns></returns>
        protected string GetTarget()
        {
            Target.Perform();

            var targetStr = string.Empty;
            var type = ObjectList[0].GetType();
            object value = null;

            if (target.Items.Count == 0)
            {
                foreach (var col in Context[0])
                {
                    target.Items.Add(col);
                }
            }

            if (ObjectList.Count == 1)
            {
                target.Items.ForEach(col => {
                    if (col != null)
                    {
                        value = type.GetProperty((col as Column).Name).GetValue(ObjectList[0]);
                        targetStr += string.Format(",{0} = {1}", col.GetIdentifier(), value.ToSqlValue());
                    }
                });
            }
            else if (ObjectList.Count > 1)
            {
                target.Items.ForEach(col => {
                    if (col != null)
                    {
                        value = string.Format("[$UPDATE_SOURCE].[{0}]", (col as Column).Name);
                        targetStr += string.Format(",{0} = {1}", col.GetIdentifier(), value);
                    }
                });
            }

            return targetStr.Length > 0 ? targetStr.Substring(1) : string.Empty;
        }

        /// <summary>
        /// UPDATE table_name1
        /// SET column1 = value, column2 = value,...
        /// FROM table_name1
        /// INNER JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public UpdateStatement Join<T>(Action<Filter, Context> customization) where T : class, new()
        {
            return Provider.Join<T>(this, JoinType.InnerJoin, customization) as UpdateStatement;
        }

        /// <summary>
        /// UPDATE table_name1
        /// SET column1 = value, column2 = value,...
        /// FROM table_name1
        /// LEFT JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public UpdateStatement LeftJoin<T>(Action<Filter, Context> customization) where T : class, new()
        {
            return Provider.Join<T>(this, JoinType.LeftJoin, customization) as UpdateStatement;
        }

        /// <summary>
        /// UPDATE table_name1
        /// SET column1 = value, column2 = value,...
        /// FROM table_name1
        /// RIGHT JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public UpdateStatement RightJoin<T>(Action<Filter, Context> customization) where T : class, new()
        {
            return Provider.Join<T>(this, JoinType.RightJoin, customization) as UpdateStatement;
        }

        /// <summary>
        /// UPDATE table_name1
        /// SET column1 = value, column2 = value,...
        /// FROM table_name1
        /// FULL JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public UpdateStatement FullJoin<T>(Action<Filter, Context> customization) where T : class, new()
        {
            return Provider.Join<T>(this, JoinType.FullJoin, customization) as UpdateStatement;
        }

        /// <summary>
        /// UPDATE table_name1
        /// SET column1 = value, column2 = value,...
        /// FROM table_name1
        /// CROSS JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public UpdateStatement CrossJoin<T>(Action<Filter, Context> customization) where T : class, new()
        {
            return Provider.Join<T>(this, JoinType.CrossJoin, customization) as UpdateStatement;
        }

        /// <summary>
        /// UPDATE table_name
        /// SET column1 = value, column2 = value,...
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
