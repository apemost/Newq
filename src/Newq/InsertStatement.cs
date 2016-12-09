namespace Newq
{
    using System;
    using System.Collections.Generic;
    using Extensions;

    /// <summary>
    /// The INSERT INTO statement is used to
    /// insert new records in a table.
    /// </summary>
    public class InsertStatement : Statement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertStatement"/> class.
        /// </summary>
        /// <param name="table"></param>
        public InsertStatement(Table table)
            : base(table)
        {
            ObjectList = new List<object>();
        }

        /// <summary>
        /// Gets <see cref="ObjectList"/>.
        /// </summary>
        public List<object> ObjectList { get; }

        /// <summary>
        /// Returns a SQL-string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToSql()
        {
            var columns = string.Empty;
            var values = string.Empty;
            var valueClause = string.Empty;
            Type type = null;

            foreach (var col in Context[0])
            {
                columns += string.Format(",{0}", col);
            }

            ObjectList.ForEach(obj => {
                type = obj.GetType();

                foreach (var col in Context[0])
                {
                    values += string.Format(",{0}", type.GetProperty(col.Name).GetValue(obj).ToSqlValue());
                }

                valueClause += string.Format(",({0})", values.Substring(1));
                values = string.Empty;
            });

            valueClause = string.Format("({0}) VALUES {1}", columns.Substring(1), valueClause.Substring(1));

            return string.Format("INSERT INTO {0} {1} ", Context[0], valueClause);
        }
    }
}
