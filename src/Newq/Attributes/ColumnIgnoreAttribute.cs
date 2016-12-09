namespace Newq.Attributes
{
    using System;

    /// <summary>
    /// Specify a property not a column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ColumnIgnoreAttribute : Attribute
    {
    }
}
