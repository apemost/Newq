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
    /// The SELECT statement is used to select data from a database.
    /// </summary>
    public class SelectStatement : Statement
    {
        /// <summary>
        /// 
        /// </summary>
        protected Target target;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectStatement"/> class.
        /// </summary>
        /// <param name="table"></param>
        public SelectStatement(Table table)
            : base(table)
        {
            target = new Target(Context);
        }

        /// <summary>
        /// Gets <see cref="Target"/>.
        /// </summary>
        public ICustomizable<Action<Target, Context>> Target
        {
            get { return target; }
        }

        /// <summary>
        /// The DISTINCT keyword can be used to
        /// return only distinct (different) values.
        /// </summary>
        public bool IsDistinct { get; set; }

        /// <summary>
        /// Gets or sets <see cref="TopRows"/>.
        /// </summary>
        public int TopRows { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPercent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UnionType? HasUnion { get; set; }

        /// <summary>
        /// Returns a SQL-string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToSql()
        {
            var union = HasUnion.HasValue
                      ? HasUnion == UnionType.Union ? "UNION " : "UNION ALL "
                      : string.Empty;

            var sql = string.Format("{0}SELECT {1}{2} FROM {3} ", union, GetParameters(), GetTarget(), Context[0]);

            Clauses.ForEach(clause => sql += clause.ToSql());

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
            var items = target.Items;

            if (items.Count == 0)
            {
                foreach (var col in Context[0])
                {
                    targetStr += string.Format(",{0} AS {1}", col, col.Alias);
                }
            }
            else
            {
                foreach (var item in items)
                {
                    if (item is Column)
                    {
                        targetStr += string.Format(",{0} AS {1}", item, (item as Column).Alias);
                    }
                    else if (item is Function)
                    {
                        targetStr += string.Format(",{0} AS {1}", item, (item as Function).Alias);
                    }
                }
            }

            return targetStr.Substring(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string GetTargetAlias()
        {
            Target.Perform();

            var targetStr = string.Empty;
            var alias = string.Empty;
            var items = target.Items;

            if (items.Count == 0)
            {
                foreach (var table in Context)
                {
                    foreach (var column in table)
                    {
                        alias = column.Alias;
                        targetStr += string.Format(",{0}", alias);
                    }
                }
            }
            else
            {
                foreach (var item in items)
                {
                    if (item is Column)
                    {
                        alias = (item as Column).Alias;
                    }
                    else if (item is Function)
                    {
                        alias = (item as Function).Alias;
                    }

                    targetStr += string.Format(",{0}", alias);
                }
            }

            return targetStr.Substring(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string GetParameters()
        {
            var distinct = IsDistinct ? "DISTINCT " : string.Empty;
            var topClause = string.Empty;

            if (TopRows > 0)
            {
                topClause = IsPercent
                          ? string.Format("TOP({0}) PERCENT ", TopRows)
                          : string.Format("TOP({0}) ", TopRows);
            }

            return distinct + topClause;
        }

        /// <summary>
        /// SELECT column_name(s)
        /// FROM table_name1
        /// INNER JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectStatement Join<T>(Action<Filter, Context> customization) where T : class, new()
        {
            return Provider.Join<T>(this, JoinType.InnerJoin, customization) as SelectStatement;
        }

        /// <summary>
        /// SELECT column_name(s)
        /// FROM table_name1
        /// LEFT JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectStatement LeftJoin<T>(Action<Filter, Context> customization) where T : class, new()
        {
            return Provider.Join<T>(this, JoinType.LeftJoin, customization) as SelectStatement;
        }

        /// <summary>
        /// SELECT column_name(s)
        /// FROM table_name1
        /// RIGHT JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectStatement RightJoin<T>(Action<Filter, Context> customization) where T : class, new()
        {
            return Provider.Join<T>(this, JoinType.RightJoin, customization) as SelectStatement;
        }

        /// <summary>
        /// SELECT column_name(s)
        /// FROM table_name1
        /// FULL JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectStatement FullJoin<T>(Action<Filter, Context> customization) where T : class, new()
        {
            return Provider.Join<T>(this, JoinType.FullJoin, customization) as SelectStatement;
        }

        /// <summary>
        /// SELECT column_name(s)
        /// FROM table_name1
        /// CROSS JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectStatement CrossJoin<T>(Action<Filter, Context> customization) where T : class, new()
        {
            return Provider.Join<T>(this, JoinType.CrossJoin, customization) as SelectStatement;
        }

        /// <summary>
        /// SELECT column_name(s)
        /// FROM table_name
        /// WHERE column_name operator value
        /// </summary>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectWhereClause Where(Action<Filter, Context> customization)
        {
            return Provider.Filtrate(new SelectWhereClause(this), customization) as SelectWhereClause;
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
            var clause = new GroupByClause(this);

            Clauses.Add(clause);
            clause.Target.Customize(customization);

            return clause;
        }

        /// <summary>
        /// SELECT column_name(s)
        /// FROM table_name
        /// ORDER BY column_name[ASC|DESC]
        /// </summary>
        /// <param name="customization"></param>
        /// <returns></returns>
        public OrderByClause OrderBy(Action<Target, Context> customization)
        {
            var clause = new OrderByClause(this);

            Clauses.Add(clause);
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
            return Provider.Union<T>(this, customization);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectStatement UnionAll<T>(Action<Target, Context> customization) where T : class, new()
        {
            return Provider.Union<T>(this, customization, UnionType.UnionAll);
        }
    }
}
