namespace Newq
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public class Condition : ICustomItem<Filter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> class.
        /// </summary>
        public Condition()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> class.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="logicalOperator"></param>
        public Condition(Comparison source, Comparison target, LogicalOperator logicalOperator = LogicalOperator.And)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            Source = source;
            Target = target;
            Operator = logicalOperator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> class.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="logicalOperator"></param>
        public Condition(Condition source, Condition target, LogicalOperator logicalOperator = LogicalOperator.And)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            Source = source;
            Target = target;
            Operator = logicalOperator;
        }

        /// <summary>
        /// Gets or sets <see cref="Source"/>.
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Target"/>.
        /// </summary>
        public object Target { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Operator"/>.
        /// </summary>
        public LogicalOperator Operator { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="customization"></param>
        public void AppendTo(Filter customization)
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
        /// <param name="logicalOperator"></param>
        /// <returns></returns>
        protected string GetOperator(LogicalOperator logicalOperator)
        {
            return logicalOperator == LogicalOperator.And ? "AND" : "OR";
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var condition = string.Empty;

            if (Source == null || Target == null)
            {
                return string.Empty;
            }

            var source = Source.ToString();
            var target = Target.ToString();

            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target))
            {
                return source + target;
            }

            if (ReferenceEquals(Source, Target))
            {
                condition = Source.ToString();
            }
            else
            {
                condition = string.Format("({0} {1} {2})", Source, GetOperator(Operator), Target);
            }

            return condition;
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that 'and' with a comparison.
        /// </summary>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public Condition And(Comparison comparison)
        {
            return And(new Condition(comparison, comparison));
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that 'and' with another condition.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Condition And(Condition condition)
        {
            return new Condition(this, condition);
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that 'or' with a comparison.
        /// </summary>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public Condition Or(Comparison comparison)
        {
            return Or(new Condition(comparison, comparison));
        }

        /// <summary>
        /// Returns a <see cref="Condition"/> that 'or' with another condition.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Condition Or(Condition condition)
        {
            return new Condition(this, condition, LogicalOperator.Or);
        }

        /// <summary>
        /// <see cref="And(Comparison)"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Condition operator &(Condition source, Comparison target)
        {
            return source.And(target);
        }

        /// <summary>
        /// <see cref="And(Condition)"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Condition operator &(Condition source, Condition target)
        {
            return source.And(target);
        }

        /// <summary>
        /// <see cref="Or(Comparison)"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Condition operator |(Condition source, Comparison target)
        {
            return source.Or(target);
        }

        /// <summary>
        /// <see cref="Or(Condition)"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Condition operator |(Condition source, Condition target)
        {
            return source.Or(target);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="subquery"></param>
        /// <returns></returns>
        public static Condition Exists(QueryBuilder subquery)
        {
            var condition = new Syntax("EXISTS", subquery, false);

            return new Condition
            {
                Source = condition,
                Target = condition,
                Operator = LogicalOperator.And,
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="subquery"></param>
        /// <returns></returns>
        public static Condition NotExists(QueryBuilder subquery)
        {
            var condition = new Syntax("NOT EXISTS", subquery, false);

            return new Condition
            {
                Source = condition,
                Target = condition,
                Operator = LogicalOperator.And,
            };
        }
    }

    /// <summary>
    ///
    /// </summary>
    public enum LogicalOperator
    {
        /// <summary>
        /// The AND operator displays a record
        /// if both the first condition AND the second condition are true.
        /// </summary>
        And,

        /// <summary>
        /// The OR operator displays a record
        /// if either the first condition OR the second condition is true.
        /// </summary>
        Or,
    }
}
