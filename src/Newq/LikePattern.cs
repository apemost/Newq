namespace Newq
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public class LikePattern
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="escape"></param>
        public LikePattern(object value, Pattern type, char escape = ' ')
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            Type = type;
            Escape = escape;
        }

        /// <summary>
        /// Gets or sets <see cref="Value"/>.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Type"/>.
        /// </summary>
        public Pattern Type { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Escape"/>.
        /// </summary>
        public char Escape { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Value == null || string.IsNullOrEmpty(Value.ToString()))
            {
                throw new Exception(nameof(Value) + " can't be null or empty");
            }

            var esc = char.IsWhiteSpace(Escape)
                    ? string.Empty
                    : string.Format(" ESCAPE '{0}'", Escape);

            switch (Type)
            {
                case Pattern.Fuzzy:
                    return string.Format("'%{0}%'{1}", Value, esc);
                case Pattern.BeginWith:
                    return string.Format("'{0}%'{1}", Value, esc);
                case Pattern.EndWith:
                    return string.Format("'%{0}'{1}", Value, esc);
                case Pattern.Regular:
                    return string.Format("'{0}'{1}", Value, esc);
                default:
                    return string.Empty;
            }
        }
    }
}
