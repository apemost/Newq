namespace Newq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///
    /// </summary>
    public class Syntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Syntax"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameter"></param>
        /// <param name="withinBrackets"></param>
        public Syntax(string name, object parameter, bool withinBrackets = true)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(nameof(name) + " can't be null or empty");
            }

            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            Name = name.ToUpper();
            Parameters = new List<object> { parameter };
            WithinBrackets = withinBrackets;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Syntax"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="withinBrackets"></param>
        public Syntax(string name, IEnumerable<object> parameters, bool withinBrackets = true)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            Name = name.ToUpper();
            Parameters = parameters.ToList();
            WithinBrackets = withinBrackets;
        }

        /// <summary>
        /// Gets <see cref="Name"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///
        /// </summary>
        public bool WithinBrackets { get; }

        /// <summary>
        ///
        /// </summary>
        public List<object> Parameters { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return WithinBrackets
                ? string.Format("{0}({1})", Name, GetParameters())
                : string.Format("{0} {1}", Name, GetParameters());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string GetParameters()
        {
            var parameters = string.Empty;

            if (Parameters != null && Parameters.Count > 0)
            {
                Parameters.ForEach(p => parameters += string.Format(",{0}", p));
            }

            return parameters.Length > 0 ? parameters.Substring(1) : string.Empty;
        }
    }
}
