namespace Newq.Attributes
{
    using System;

    /// <summary>
    /// Specify a property is a primary key.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class KeyAttribute : Attribute
    {
    }
}
