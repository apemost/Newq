namespace Newq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Used to build SQL statement.
    /// </summary>
    public class QueryBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        public QueryBuilder()
        {

        }

        /// <summary>
        /// Gets or sets <see cref="Statement"/>.
        /// </summary>
        public Statement Statement { get; protected set; }

        /// <summary>
        /// Gets or sets <see cref="Paginator"/>.
        /// </summary>
        public Paginator Paginator { get; set; }

        /// <summary>
        /// Returns a SQL-string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Statement == null)
            {
                return string.Empty;
            }

            var sql = Statement.ToSql();

            if (Paginator != null && Statement is SelectStatement)
            {
                var orderByIndex = sql.IndexOf(" ORDER BY ");
                var orderByClause = string.Empty;
                var originalTarget = sql.Substring(7, sql.IndexOf(" FROM ") - 7);
                var newTarget = originalTarget;
                var targetAlias = string.Empty;
                var column = string.Empty;
                var alias = string.Empty;
                string[] targetColumns = originalTarget.Split(',');

                foreach (var col in targetColumns)
                {
                    alias = col.IndexOf(" AS ") > -1 ? col.Substring(col.IndexOf(" AS ") + 4) : col;
                    targetAlias += string.Format(",{0}", alias.Trim());
                }

                if (orderByIndex > -1)
                {
                    orderByClause = sql.Substring(orderByIndex + 10).Trim();
                    sql = sql.Remove(orderByIndex);

                    var orderByColumns = orderByClause.Split(',');

                    foreach (var col in orderByColumns)
                    {
                        column = col.Replace(" ASC", "").Replace(" DESC", "");
                        alias = column.Trim().Replace("].[", ".");

                        if (originalTarget.IndexOf(alias) == -1)
                        {
                            newTarget += string.Format(",{0} AS {1}", column, alias);
                            targetAlias += string.Format(",{0}", alias);
                        }
                    }

                    orderByClause = orderByClause.Replace("].[", ".");
                    sql = sql.Replace(originalTarget, newTarget);
                }
                else
                {
                    var firstTargetEndIndex = originalTarget.IndexOf(",");
                    var aliasIndex = originalTarget.IndexOf(" AS ") + 4;

                    orderByClause = firstTargetEndIndex == -1 ? originalTarget.Substring(aliasIndex)
                                  : originalTarget.Substring(aliasIndex, firstTargetEndIndex - (aliasIndex));
                }

                if (targetAlias.Length > 2)
                {
                    targetAlias = targetAlias.Substring(1);
                }

                sql = string.Format(
                    "SELECT {0} FROM (" +
                        "SELECT ROW_NUMBER() OVER(ORDER BY {1}) AS [$ROW_NUMBER],{0} FROM ({2}) AS [$ORIGINAL_QUERY]" +
                    ") AS [$PAGINATOR] " +
                    "WHERE [$PAGINATOR].[$ROW_NUMBER] BETWEEN {3} AND {4} ",
                    targetAlias, orderByClause, sql.Trim(), Paginator.BeginRowNumber, Paginator.EndRowNumber);
            }

            return sql;
        }

        /// <summary>
        /// Define whether the statement is paginable.
        /// </summary>
        /// <param name="paginator"></param>
        /// <returns>true when the statement is select statement, false if not</returns>
        public bool Paginate(Paginator paginator)
        {
            var isPaginable = false;

            if (Statement is SelectStatement)
            {
                Paginator = paginator;
                isPaginable = true;
            }

            return isPaginable;
        }

        /// <summary>
        /// SELECT column_name(s)
        /// FROM table_name
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <returns>
        /// SelectStatement to select all records
        /// from the certain table
        /// without the keyword distinct
        /// </returns>
        public SelectStatement Select<T>() where T : class, new()
        {
            return Select<T>(false, 0, false, null);
        }

        /// <summary>
        /// SELECT TOP number|percent column_name(s)
        /// FROM table_name
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="topRows">Number of the top records to select</param>
        /// <param name="isPercent">
        /// Determine whether the topRows represent the percentage,
        /// default is false
        /// </param>
        /// <returns>
        /// SelectStatement to select certain quantity(percentage if isPercent is true)
        /// of the top records from the table
        /// </returns>
        public SelectStatement Select<T>(int topRows, bool isPercent = false) where T : class, new()
        {
            return Select<T>(false, topRows, false, null);
        }

        /// <summary>
        /// SELECT column_name(s)
        /// FROM table_name
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="customization">Determine which column(s) to be selected from the table</param>
        /// <returns>SelectStatement to select all records within certain columns from the table without the keyword distinct</returns>
        public SelectStatement Select<T>(Action<Target, Context> customization) where T : class, new()
        {
            return Select<T>(false, 0, false, customization);
        }

        /// <summary>
        /// SELECT DISTINCT TOP number column_name(s)
        /// FROM table_name
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="topRows"></param>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectStatement Select<T>(int topRows, Action<Target, Context> customization) where T : class, new()
        {
            return Select<T>(false, topRows, false, customization);
        }

        /// <summary>
        /// SELECT TOP number|percent column_name(s)
        /// FROM table_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topRows"></param>
        /// <param name="isPercent"></param>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectStatement Select<T>(int topRows, bool isPercent, Action<Target, Context> customization) where T : class, new()
        {
            return Select<T>(false, topRows, isPercent, customization);
        }

        /// <summary>
        /// SELECT DISTINCT TOP number|percent column_name(s)
        /// FROM table_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isDistinct"></param>
        /// <param name="topRows"></param>
        /// <param name="isPercent"></param>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectStatement Select<T>(bool isDistinct, int topRows, bool isPercent, Action<Target, Context> customization) where T : class, new()
        {
            var statement = new SelectStatement(new Table<T>());

            Statement = statement;
            statement.IsDistinct = isDistinct;
            statement.TopRows = topRows;
            statement.IsPercent = isPercent;
            statement.Target.Customize(customization);

            return statement;
        }

        /// <summary>
        /// SELECT DISTINCT column_name(s)
        /// FROM table_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public SelectStatement SelectDistinct<T>() where T : class, new()
        {
            return Select<T>(true, 0, false, null);
        }

        /// <summary>
        /// SELECT DISTINCT TOP number|percent column_name(s)
        /// FROM table_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topRows"></param>
        /// <param name="isPercent"></param>
        /// <returns></returns>
        public SelectStatement SelectDistinct<T>(int topRows, bool isPercent = false) where T : class, new()
        {
            return Select<T>(true, topRows, isPercent, null);
        }

        /// <summary>
        /// SELECT DISTINCT column_name(s)
        /// FROM table_name
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="customization">Determine which column(s) to be selected from the table</param>
        /// <returns>SelectStatement to select all records within certain columns from the table without the keyword distinct</returns>
        public SelectStatement SelectDistinct<T>(Action<Target, Context> customization) where T : class, new()
        {
            return Select<T>(true, 0, false, customization);
        }

        /// <summary>
        /// SELECT DISTINCT TOP number column_name(s)
        /// FROM table_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topRows"></param>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectStatement SelectDistinct<T>(int topRows, Action<Target, Context> customization) where T : class, new()
        {
            return Select<T>(true, topRows, false, customization);
        }

        /// <summary>
        /// SELECT DISTINCT TOP number|percent column_name(s)
        /// FROM table_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topRows"></param>
        /// <param name="isPercent"></param>
        /// <param name="customization"></param>
        /// <returns></returns>
        public SelectStatement SelectDistinct<T>(int topRows, bool isPercent, Action<Target, Context> customization) where T : class, new()
        {
            return Select<T>(true, topRows, isPercent, customization);
        }

        /// <summary>
        /// UPDATE table_name
        /// SET column1 = value, column2 = value,...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="customization"></param>
        /// <returns></returns>
        public UpdateStatement Update<T>(T obj, Action<Target, Context> customization = null) where T : class, new()
        {
            var statement = new UpdateStatement(new Table<T>());

            statement.ObjectList.Add(obj);
            statement.Target.Customize(customization);
            Statement = statement;

            return statement;
        }

        /// <summary>
        /// UPDATE table_name
        /// SET column1 = value, column2 = value,...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <param name="customization"></param>
        /// <returns></returns>
        public UpdateStatement UpdateMany<T>(IEnumerable<T> objects, Action<Target, Context> customization = null) where T : class, new()
        {
            var statement = new UpdateStatement(new Table<T>());
            var enumerator = ((IEnumerable)objects).GetEnumerator();

            while (enumerator.MoveNext())
            {
                statement.ObjectList.Add(enumerator.Current);
            }

            statement.Target.Customize(customization);
            Statement = statement;

            return statement;
        }

        /// <summary>
        /// INSERT INTO table_name
        /// (column1, column2, column3,...)
        /// VALUES(value1, value2, value3,...)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public InsertStatement Insert<T>(T obj) where T : class, new()
        {
            var statement = new InsertStatement(new Table<T>());

            statement.ObjectList.Add(obj);
            Statement = statement;

            return statement;
        }

        /// <summary>
        /// INSERT INTO table_name
        /// (column1, column2, column3,...)
        /// VALUES(value1, value2, value3,...)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <returns></returns>
        public InsertStatement InsertMany<T>(IEnumerable<T> objects) where T : class, new()
        {
            var statement = new InsertStatement(new Table<T>());
            var enumerator = objects.GetEnumerator();

            while (enumerator.MoveNext())
            {
                statement.ObjectList.Add(enumerator.Current);
            }

            Statement = statement;

            return statement;
        }

        /// <summary>
        /// DELETE FROM table_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DeleteStatement Delete<T>() where T : class, new()
        {
            return Delete<T>(0, false);
        }

        /// <summary>
        /// DELETE TOP number|percent FROM table_name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topRows"></param>
        /// <param name="isPercent"></param>
        /// <returns></returns>
        public DeleteStatement Delete<T>(int topRows, bool isPercent = false) where T : class, new()
        {
            var statement = new DeleteStatement(new Table<T>());

            Statement = statement;
            statement.TopRows = topRows;
            statement.IsPercent = isPercent;

            return statement;
        }
    }
}
