namespace Newq.Attributes
{
    using System;

    /// <summary>
    /// Specify the table name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class TableAttribute : Attribute
    {
        readonly string tableName;

        public TableAttribute(string tableName)
        {
            this.tableName = tableName;
        }

        public string TableName
        {
            get { return tableName; }
        }
    }
}
