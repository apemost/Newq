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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Database context for statement or clause.
    /// </summary>
    public class Context : IEnumerable<Table>
    {
        /// <summary>
        /// 
        /// </summary>
        protected List<Table> tables;

        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        /// <param name="table"></param>
        public Context(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            tables = new List<Table> { table };
        }

        /// <summary>
        /// Gets <see cref="Newq.Table"/> by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Table this[int index]
        {
            get { return tables[index]; }
        }

        /// <summary>
        /// Gets <see cref="Newq.Table"/> by table name.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Table this[string tableName]
        {
            get { return tables.First(t => t.Name == tableName); }
        }

        /// <summary>
        /// Gets <see cref="Column"/> by table name and column name.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public Column this[string tableName, string columnName]
        {
            get { return this[tableName][columnName]; }
        }

        /// <summary>
        /// Gets <see cref="Column"/> by table name and column name.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        public Column this[string tableName, string columnName, Exclude exclude]
        {
            get { return this[tableName][columnName, exclude]; }
        }

        /// <summary>
        /// Gets <see cref="OrderByColumn"/> by table name, column name and sort order.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public OrderByColumn this[string tableName, string columnName, SortOrder order]
        {
            get { return this[tableName][columnName, order]; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Table Table<T>() where T : class, new()
        {
            return tables.First(t => t.Name == typeof(T).Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public Column Table<T>(Expression<Func<T, object>> expr) where T : class, new()
        {
            var split = expr.Body.ToString().Split("(.)".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var columnName = split[split.Length - 1];

            return tables.First(t => t.Name == typeof(T).Name).First(c => c.Name == columnName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public OrderByColumn Table<T>(Expression<Func<T, object>> expr, SortOrder order) where T : class, new()
        {
            var split = expr.Body.ToString().Split("(.)".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var columnName = split[split.Length - 1];

            return tables.First(t => t.Name == typeof(T).Name)[columnName, order];
        }

        /// <summary>
        /// Adds a <see cref="Newq.Table"/> to the end of the context.
        /// </summary>
        /// <param name="table"></param>
        public void Add(Table table)
        {
            if (tables.All(t => t.Name != table.Name))
            {
                tables.Add(table);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Table> GetEnumerator()
        {
            return tables.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
