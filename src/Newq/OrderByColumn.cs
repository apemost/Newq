namespace Newq
{
    /// <summary>
    ///
    /// </summary>
    public class OrderByColumn : ICustomItem<Target>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderByColumn"/> class.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="order"></param>
        public OrderByColumn(Column column, SortOrder order)
        {
            Column = column;
            Order = order;
        }

        /// <summary>
        ///
        /// </summary>
        public Column Column { get; }

        /// <summary>
        ///
        /// </summary>
        public SortOrder Order { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="customization"></param>
        public void AppendTo(Target customization)
        {
            customization.Items.Add(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string GetIdentifier()
        {
            return ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", Column, GetOrder());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected string GetOrder()
        {
            return Order == SortOrder.Desc ? "DESC" : "ASC";
        }
    }
}
