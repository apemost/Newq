namespace Newq
{
    /// <summary>
    ///
    /// </summary>
    public interface ICustomizable<in T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="customization"></param>
        void Customize(T customization);

        /// <summary>
        /// Returns a result after customization be performed.
        /// </summary>
        bool Perform();
    }
}
