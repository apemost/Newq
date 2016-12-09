namespace Newq
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public abstract class Clause : Statement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Clause"/> class.
        /// </summary>
        /// <param name="statement"></param>
        protected Clause(Statement statement)
        {
            if (statement == null)
            {
                throw new ArgumentNullException(nameof(statement));
            }

            Statement = statement;
            Context = statement.Context;
        }

        /// <summary>
        /// Gets <see cref="Statement"/>.
        /// </summary>
        public Statement Statement { get; }
    }
}
