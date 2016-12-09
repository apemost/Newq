namespace Newq.Attributes
{
    using System;

    /// <summary>
    /// Specify the column name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class ColumnAttribute : Attribute
    {
        readonly string columnName;

        public ColumnAttribute(string columnName)
        {
            this.columnName = columnName;
        }

        public string ColumnName
        {
            get { return columnName; }
        }
    }
}
