﻿namespace Newq
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The SELECT statement is used to select data from a database.
    /// </summary>
    public class SelectStatement : Statement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectStatement"/> class.
        /// </summary>
        /// <param name="table"></param>
        public SelectStatement(Table table) : base(table)
        {
            Target = new Target(Context);
        }

        /// <summary>
        /// Gets <see cref="Target"/>.
        /// </summary>
        public ICustomizable<Action<Target, Context>> Target { get; }

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

            var target = string.Empty;
            var items = (Target as Target).Items;

            if (items.Count == 0)
            {
                foreach (var tab in Context.Tables)
                {
                    foreach (var col in tab.Columns)
                    {
                        target += string.Format("{0} AS {1}, ", col, col.Alias);
                    }
                }
            }
            else
            {
                foreach (var item in items)
                {
                    if (item is Column)
                    {
                        target += string.Format("{0} AS {1}, ", item, (item as Column).Alias);
                    }
                    else if (item is Function)
                    {
                        target += string.Format("{0} AS {1}, ", item, (item as Function).Alias);
                    }
                }
            }

            return target.Remove(target.Length - 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string GetTargetAlias()
        {
            Target.Perform();

            var target = string.Empty;
            var alias = string.Empty;
            var items = (Target as Target).Items;

            if (items.Count == 0)
            {
                foreach (var tab in Context.Tables)
                {
                    foreach (var col in tab.Columns)
                    {
                        alias = col.Alias;
                        target += string.Format("{0}, ", alias);
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

                    target += string.Format("{0}, ", alias);
                }
            }

            return target.Remove(target.Length - 2);
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
        public SelectStatement Join<T>(Action<Filter, Context> customization)
        {
            return Provider.Join<T>(this, JoinType.InnerJoin, customization) as SelectStatement;
        }

        /// <summary>
        /// SELECT column_name(s)
        /// FROM table_name1
        /// JOIN table_name2
        /// ON table_name1.column_name=table_name2.column_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectStatement Join<T>(JoinType type, Action<Filter, Context> customization)
        {
            return Provider.Join<T>(this, type, customization) as SelectStatement;
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
        public SelectStatement Union<T>(Action<Target, Context> customization)
        {
            return Provider.Union<T>(this, customization);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectStatement UnionAll<T>(Action<Target, Context> customization)
        {
            return Provider.Union<T>(this, customization, UnionType.UnionAll);
        }
    }
}
