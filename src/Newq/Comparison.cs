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
    using Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class Comparison
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Comparison"/> class.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="comparisonOperator"></param>
        public Comparison(Column column, ComparisonOperator comparisonOperator)
        {
            if ((object)column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            Column = column;
            Operator = comparisonOperator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Comparison"/> class.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="comparisonOperator"></param>
        /// <param name="value"></param>
        public Comparison(Column column, ComparisonOperator comparisonOperator, object value)
            : this(column, comparisonOperator)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Values = new List<object> { value };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Comparison"/> class.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="comparisonOperator"></param>
        /// <param name="values"></param>
        public Comparison(Column column, ComparisonOperator comparisonOperator, IEnumerable<object> values)
            : this(column, comparisonOperator)
        {
            if (values == null || values.Count() == 0)
            {
                throw new ArgumentException(nameof(values) + " can't be null or empty");
            }

            Values = values.ToList();
        }

        /// <summary>
        /// Gets <see cref="Column"/>.
        /// </summary>
        public Column Column { get; }

        /// <summary>
        /// Gets or sets <see cref="Operator"/>.
        /// </summary>
        public ComparisonOperator Operator { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Values"/>.
        /// </summary>
        public List<object> Values { get; set; }

        /// <summary>
        /// Gets first condition parameter.
        /// </summary>
        /// <returns></returns>
        protected string GetFirstValue()
        {
            return Values[0].ToSqlValue();
        }

        /// <summary>
        /// Gets IN condition parameters.
        /// </summary>
        /// <returns></returns>
        protected string GetInValues()
        {
            var values = string.Empty;

            Values.ForEach(val => values += "," + val.ToSqlValue());

            if (values.Length < 4)
            {
                throw new Exception(nameof(values) + " can't be null or empty");
            }

            return values.Substring(1);
        }

        /// <summary>
        /// Gets BETWEEN condition parameters.
        /// </summary>
        /// <returns></returns>
        protected string GetBetweenValues()
        {
            var value1 = Values[0].ToSqlValue();
            var value2 = Values[1].ToSqlValue();

            if (value1.Length == 0)
            {
                throw new Exception(nameof(value1) + " can't be null or empty");
            }

            if (value2.Length == 0)
            {
                throw new Exception(nameof(value2) + " can't be null or empty");
            }

            return string.Format("{0} AND {1}", value1, value2);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Operator == ComparisonOperator.GreaterThan ? string.Format("{0} > {1}", Column, GetFirstValue())
                : Operator == ComparisonOperator.GreaterThanOrEqualTo ? string.Format("{0} >= {1}", Column, GetFirstValue())
                : Operator == ComparisonOperator.LessThan ? string.Format("{0} < {1}", Column, GetFirstValue())
                : Operator == ComparisonOperator.LessThanOrEqualTo ? string.Format("{0} <= {1}", Column, GetFirstValue())
                : Operator == ComparisonOperator.EqualTo ? string.Format("{0} = {1}", Column, GetFirstValue())
                : Operator == ComparisonOperator.NotEqualTo ? string.Format("{0} != {1}", Column, GetFirstValue())
                : Operator == ComparisonOperator.Like ? string.Format("{0} LIKE {1}", Column, GetFirstValue())
                : Operator == ComparisonOperator.NotLike ? string.Format("{0} NOT LIKE {1}", Column, GetFirstValue())
                : Operator == ComparisonOperator.In ? string.Format("{0} IN ({1})", Column, GetInValues())
                : Operator == ComparisonOperator.NotIn ? string.Format("{0} NOT IN ({1})", Column, GetInValues())
                : Operator == ComparisonOperator.Between ? string.Format("{0} BETWEEN {1}", Column, GetBetweenValues())
                : Operator == ComparisonOperator.NotBetween ? string.Format("{0} NOT BETWEEN {1}", Column, GetBetweenValues())
                : Operator == ComparisonOperator.IsNull ? string.Format("{0} IS NULL", Column)
                : Operator == ComparisonOperator.IsNotNull ? string.Format("{0} IS NOT NULL", Column)
                : string.Empty;
        }
    }
}
