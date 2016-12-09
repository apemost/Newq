namespace Newq
{
    /// <summary>
    ///
    /// </summary>
    public enum JoinType
    {
        /// <summary>
        /// The INNER JOIN selects all rows from both tables
        /// as long as there is a match between the columns in both tables.
        /// </summary>
        InnerJoin,

        /// <summary>
        /// The LEFT JOIN returns all rows from the left table (table1),
        /// with the matching rows in the right table (table2).
        /// The result is NULL in the right side when there is no match.
        /// </summary>
        LeftJoin,

        /// <summary>
        /// The RIGHT JOIN returns all rows from the right table (table2),
        /// with the matching rows in the left table (table1).
        /// The result is NULL in the left side when there is no match.
        /// </summary>
        RightJoin,

        /// <summary>
        /// The FULL JOIN returns all rows from the left table (table1)
        /// and from the right table (table2).
        /// </summary>
        FullJoin,

        /// <summary>
        /// The Cross JOIN returns all rows where each row from the left table (table1)
        /// is combined with each row from the right table (table2).
        /// </summary>
        CrossJoin,
    }
}
