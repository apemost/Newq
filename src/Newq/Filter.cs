namespace Newq
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public class Filter : Customization, ICustomizable<Action<Filter, Context>>
    {
        /// <summary>
        ///
        /// </summary>
        protected Action<Filter, Context> customization;

        /// <summary>
        ///
        /// </summary>
        protected bool hasPerformed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="context"></param>
        public Filter(Context context)
            : base(context)
        {
            Items = new List<ICustomItem<Filter>>();
        }

        /// <summary>
        /// Gets or sets <see cref="Items"/>
        /// </summary>
        public List<ICustomItem<Filter>> Items { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="customization"></param>
        public void Customize(Action<Filter, Context> customization)
        {
            this.customization = customization;
            hasPerformed = false;
        }

        /// <summary>
        /// Returns a result after customization be performed.
        /// </summary>
        public bool Perform()
        {
            if (!hasPerformed && customization != null)
            {
                Items.Clear();
                customization(this, context);
                hasPerformed = true;
            }

            return hasPerformed;
        }

        /// <summary>
        /// Returns a string that represents the current customization.
        /// </summary>
        /// <returns></returns>
        public override string GetCustomization()
        {
            var filter = string.Empty;
            var filterItem = string.Empty;

            Perform();
            Items.ForEach(item => {
                if (item != null)
                {
                    filterItem = item.ToString();

                    if (!string.IsNullOrEmpty(filterItem))
                    {
                        filter += string.Format(" AND {0}", item);
                    }
                }
            });

            return filter.Length > 0 ? filter.Substring(5) : string.Empty;
        }

        /// <summary>
        /// Adds a <see cref="ICustomItem{T}"/> to the end of the filter.
        /// </summary>
        /// <param name="item"></param>
        public Filter Add(ICustomItem<Filter> item)
        {
            Items.Add(item);
            return this;
        }

        /// <summary>
        /// Removes the first occurrence of a specific <see cref="Condition"/> from the filter.
        /// </summary>
        /// <param name="item"></param>
        public Filter Remove(ICustomItem<Filter> item)
        {
            Items.Remove(item);
            return this;
        }

        /// <summary>
        /// <see cref="Add(ICustomItem{Filter})"/>.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Filter operator +(Filter filter, ICustomItem<Filter> item)
        {
            filter.Add(item);

            return filter;
        }

        /// <summary>
        /// <see cref="Remove(ICustomItem{Filter})"/>.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Filter operator -(Filter filter, ICustomItem<Filter> item)
        {
            filter.Remove(item);

            return filter;
        }
    }
}
