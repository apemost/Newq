namespace Newq
{
    using System;
    using System.Collections.Generic;
    using Extensions;

    /// <summary>
    ///
    /// </summary>
    public class Column : ICustomItem<Target>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Column"/> class.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Column(Table table, string name, object value = null)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(nameof(name) + " can't be null or empty");
            }

            Table = table;
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Table"/>.
        /// </summary>
        public Table Table { get; protected set; }

        /// <summary>
        /// Gets or sets <see cref="Name"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets <see cref="Value"/>.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ExcludePattern"/>.
        /// </summary>
        public Exclude? ExcludePattern { get; set; }

        /// <summary>
        /// Gets <see cref="Alias"/>.
        /// </summary>
        public string Alias
        {
            get { return string.Format("[{0}.{1}]", Table.Name, Name); }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="customization"></param>
        public void AppendTo(Target customization)
        {
            customization.Items.Add(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string GetIdentifier()
        {
            return ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[{0}].[{1}]", Table.Name, Name);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool IsExclude(object value)
        {
            var isExclude = false;

            if (ExcludePattern.HasValue)
            {
                var sqlValue = value.ToSqlValue();

                switch (ExcludePattern)
                {
                    case Exclude.Empty:
                        isExclude = sqlValue == "''";
                        break;
                    case Exclude.WhiteSpace:
                        isExclude = sqlValue == "''" || string.IsNullOrWhiteSpace(sqlValue.Replace("'", ""));
                        break;
                    case Exclude.Zero:
                        var tryZero = sqlValue.Replace("'", "").Split('.')[0];

                        if (!string.IsNullOrEmpty(tryZero) && tryZero.Length == 1)
                        {
                            isExclude = tryZero == "0";
                        }
                        break;
                    case Exclude.MaxDateTime:
                        var tryMaxDateTime = DateTime.MinValue;

                        if (DateTime.TryParse(sqlValue.Replace("'", ""), out tryMaxDateTime))
                        {
                            isExclude = tryMaxDateTime == DateTime.MaxValue;
                        }
                        break;
                    case Exclude.MinDateTime:
                        var tryMinDateTime = DateTime.MaxValue;

                        if (DateTime.TryParse(sqlValue.Replace("'", ""), out tryMinDateTime))
                        {
                            isExclude = tryMinDateTime == DateTime.MinValue;
                        }
                        break;
                    default:
                        break;
                }
            }

            return isExclude;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Function Count()
        {
            return new Function("COUNT", this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Function Max()
        {
            return new Function("MAX", this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Function Min()
        {
            return new Function("MIN", this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Function Avg()
        {
            return new Function("Avg", this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Function Sum()
        {
            return new Function("SUM", this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Condition IsNull()
        {
            var comparison = new Comparison(this, ComparisonOperator.IsNull);

            return new Condition(comparison, comparison);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Condition IsNotNull()
        {
            var comparison = new Comparison(this, ComparisonOperator.IsNotNull);

            return new Condition(comparison, comparison);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="comparisonOperator"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected Condition Compare(ComparisonOperator comparisonOperator, object value)
        {
            if (IsExclude(value))
            {
                return new Condition
                {
                    Source = null,
                    Target = null,
                };
            }

            var comparison = new Comparison(this, comparisonOperator, value);

            return new Condition(comparison, comparison);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="comparisonOperator"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        protected Condition Compare(ComparisonOperator comparisonOperator, object[] values)
        {
            var comparison = new Comparison(this, comparisonOperator, values);

            return new Condition(comparison, comparison);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="comparisonOperator"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        protected Condition Compare(ComparisonOperator comparisonOperator, List<object> values)
        {
            var comparison = new Comparison(this, comparisonOperator, values);

            return new Condition(comparison, comparison);
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that greater than a specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition GreaterThan(object value)
        {
            return Compare(ComparisonOperator.GreaterThan, value);
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that greater than or equal to a specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition GreaterThanOrEqualTo(object value)
        {
            return Compare(ComparisonOperator.GreaterThanOrEqualTo, value);
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that less than a specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition LessThan(object value)
        {
            return Compare(ComparisonOperator.LessThan, value);
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that less than or equal to a specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition LessThanOrEqualTo(object value)
        {
            return Compare(ComparisonOperator.LessThanOrEqualTo, value);
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that equal to a specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition EqualTo(object value)
        {
            return Compare(ComparisonOperator.EqualTo, value);
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that not equal to a specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Condition NotEqualTo(object value)
        {
            return Compare(ComparisonOperator.NotEqualTo, value);
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that search for a specified pattern.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="escape"></param>
        /// <returns></returns>
        public Condition Like(object value, Pattern type = Pattern.Fuzzy, char escape = ' ')
        {
            if (IsExclude(value))
            {
                return new Condition
                {
                    Source = null,
                    Target = null,
                };
            }

            return Compare(ComparisonOperator.Like, new LikePattern(value, type, escape));
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that search for not match a specified pattern.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="escape"></param>
        /// <returns></returns>
        public Condition NotLike(object value, Pattern type = Pattern.Fuzzy, char escape = ' ')
        {
            if (IsExclude(value))
            {
                return new Condition
                {
                    Source = null,
                    Target = null,
                };
            }

            return Compare(ComparisonOperator.NotLike, new LikePattern(value, type, escape));
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that specify multiple values.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public Condition In(object[] values)
        {
            return Compare(ComparisonOperator.In, values);
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that specify multiple values.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public Condition In(List<object> values)
        {
            return Compare(ComparisonOperator.In, values);
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that specify multiple values.
        /// </summary>
        /// <param name="subQuery"></param>
        /// <returns></returns>
        public Condition In(QueryBuilder subQuery)
        {
            return Compare(ComparisonOperator.In, subQuery);
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that specify multiple exceptive values.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public Condition NotIn(object[] values)
        {
            return Compare(ComparisonOperator.NotIn, values);
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that specify multiple exceptive values.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public Condition NotIn(List<object> values)
        {
            return Compare(ComparisonOperator.NotIn, values);
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that specify multiple exceptive values.
        /// </summary>
        /// <param name="subQuery"></param>
        /// <returns></returns>
        public Condition NotIn(QueryBuilder subQuery)
        {
            return Compare(ComparisonOperator.NotIn, subQuery);
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that selects values within a range.
        /// The values can be numbers, text, or dates.
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public Condition Between(object value1, object value2)
        {
            if (IsExclude(value1) || IsExclude(value2))
            {
                return new Condition
                {
                    Source = null,
                    Target = null,
                };
            }

            return Compare(ComparisonOperator.Between, new object[] { value1, value2 });
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that selects values not within a range.
        /// The values can be numbers, text, or dates.
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public Condition NotBetween(object value1, object value2)
        {
            if (IsExclude(value1) || IsExclude(value2))
            {
                return new Condition
                {
                    Source = null,
                    Target = null,
                };
            }

            return Compare(ComparisonOperator.NotBetween, new object[] { value1, value2 });
        }

        /// <summary>
        /// <see cref="EqualTo(object)"/>.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Condition operator ==(Column column, object value)
        {
            return column.EqualTo(value);
        }

        /// <summary>
        /// <see cref="NotEqualTo(object)"/>.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Condition operator !=(Column column, object value)
        {
            return column.NotEqualTo(value);
        }

        /// <summary>
        /// <see cref="GreaterThan(object)"/>.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Condition operator >(Column column, object value)
        {
            return column.GreaterThan(value);
        }

        /// <summary>
        /// <see cref="LessThan(object)"/>.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Condition operator <(Column column, object value)
        {
            return column.LessThan(value);
        }

        /// <summary>
        /// <see cref="GreaterThanOrEqualTo(object)"/>.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Condition operator >=(Column column, object value)
        {
            return column.GreaterThanOrEqualTo(value);
        }

        /// <summary>
        /// <see cref="LessThanOrEqualTo(object)"/>.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Condition operator <=(Column column, object value)
        {
            return column.LessThanOrEqualTo(value);
        }
    }
}
