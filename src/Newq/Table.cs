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
    using Attributes;

    /// <summary>
    /// Simulate tables in Sql.
    /// </summary>
    public class Table<T> : Table where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        public Table() : base(typeof(T))
        {
        }
    }

    /// <summary>
    /// Simulate tables in Sql.
    /// </summary>
    public class Table : IEnumerable<Column>
    {
        /// <summary>
        /// An dictionary used to get columns in the table.
        /// </summary>
        protected List<Column> columns;

        /// <summary>
        /// The primary key of current table.
        /// </summary>
        protected List<Column> primaryKey = new List<Column>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        protected Table()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="type">Type of the corresponding class</param>
        protected Table(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            columns = new List<Column>();

            var tableAttributes = type.GetCustomAttributes(typeof(TableAttribute), false);

            if (tableAttributes.Length > 0)
            {
                Name = ((TableAttribute)tableAttributes[0]).TableName;
            }
            else
            {
                Name = type.Name;
            }

            var defaultPK = "Id";
            Column column = null;
            object[] columnAttributes = null;

            var keyProperties = type.GetProperties()
                .Where(prop => prop.CanRead &&
                               prop.CanWrite &&
                               prop.PropertyType.Namespace == "System" &&
                               prop.CustomAttributes.Any(attr => attr.AttributeType == typeof(KeyAttribute)) &&
                               !prop.CustomAttributes.Any(attr => attr.AttributeType == typeof(ColumnIgnoreAttribute)));

            foreach (var prop in keyProperties)
            {
                columnAttributes = prop.GetCustomAttributes(typeof(ColumnAttribute), false);

                if (columnAttributes.Length > 0)
                {
                    column = new Column(this, ((ColumnAttribute)columnAttributes[0]).ColumnName);
                }
                else
                {
                    column = new Column(this, prop.Name);
                }

                columns.Add(column);
                primaryKey.Add(column);
            }

            var properties = type.GetProperties()
                .Where(prop => prop.CanRead &&
                               prop.CanWrite &&
                               prop.PropertyType.Namespace == "System" &&
                               !prop.CustomAttributes.Any(attr => attr.AttributeType == typeof(ColumnIgnoreAttribute)));

            if (primaryKey.Count == 0 && properties.Any(p => p.Name == defaultPK))
            {
                column = new Column(this, defaultPK);
                columns.Add(column);
            }

            properties = properties.Where(prop => {
                var attrs = prop.GetCustomAttributes(typeof(ColumnAttribute), false);

                if (attrs.Length > 0)
                {
                    column = new Column(this, ((ColumnAttribute)columnAttributes[0]).ColumnName);
                }
                else
                {
                    column = new Column(this, prop.Name);
                }

                return prop.Name != defaultPK &&
                    (attrs.Length == 0
                        ? !primaryKey.Any(pk => pk.Name == prop.Name)
                        : !primaryKey.Any(pk => pk.Name == ((ColumnAttribute)columnAttributes[0]).ColumnName));
            });

            foreach (var prop in properties)
            {
                column = new Column(this, prop.Name);
                columns.Add(column);
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Name"/>.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets primary key of current table.
        /// </summary>
        public IReadOnlyList<Column> PrimaryKey
        {
            get { return primaryKey; }
        }

        /// <summary>
        /// Gets <see cref="Column"/> by column name.
        /// </summary>
        /// <param name="columnName">name of the column</param>
        /// <returns>an entity of column</returns>
        public Column this[string columnName]
        {
            get { return columns.First(c => c.Name == columnName); }
        }

        /// <summary>
        /// Gets <see cref="Column"/> by column name.
        /// </summary>
        /// <param name="columnName">name of the column</param>
        /// <param name="exclude"></param>
        /// <returns>an entity of column</returns>
        public Column this[string columnName, Exclude exclude]
        {
            get
            {
                var column = this[columnName];

                column.ExcludePattern = exclude;

                return column;
            }
        }

        /// <summary>
        /// Gets <see cref="OrderByColumn"/> by column name and sort order.
        /// </summary>
        /// <param name="columnName">name of the column</param>
        /// <param name="order"></param>
        /// <returns>an entity of column</returns>
        public OrderByColumn this[string columnName, SortOrder order]
        {
            get { return new OrderByColumn(this[columnName], order); }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>Name of this with a pair of square brackets</returns>
        public override string ToString()
        {
            return string.Format("[{0}]", Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Column> GetEnumerator()
        {
            return columns.GetEnumerator();
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
