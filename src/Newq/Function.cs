namespace Newq
{
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public class Function : Syntax
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Function"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameter"></param>
        public Function(string name, object parameter)
            : base(name, parameter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Function"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        public Function(string name, IEnumerable<object> parameters)
            : base(name, parameters)
        {
        }

        /// <summary>
        ///
        /// </summary>
        public string Alias
        {
            get
            {
                var alias = string.Empty;

                if (Parameters.Count > 0 && Parameters[0] is Column)
                {
                    var column = Parameters[0] as Column;

                    alias = string.Format("[{0}.{1}.{2}]", Name, column.Table.Name, column.Name);
                }
                else
                {
                    alias = string.Format("[{0}]", Name);
                }

                return alias;
            }
        }
    }
}
