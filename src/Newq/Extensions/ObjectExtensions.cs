namespace Newq.Extensions
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToSqlValue(this object obj)
        {
            var value = string.Empty;

            if (obj is string)
            {
                value = string.Format("'{0}'", obj.ToString().Replace("'", "''"));
            }
            else if (obj is DateTime)
            {
                var time = (DateTime)obj;

                value = time.Year < 1753 ? "'1753-01-01 00:00:00.000'"
                    : string.Format("'{0}'", time.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else if (obj == null)
            {
                value = "''";
            }
            else
            {
                value = obj.ToString();
            }

            return value;
        }
    }
}
