namespace Newq
{
    /// <summary>
    ///
    /// </summary>
    public interface ICustomItem<in T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="customization"></param>
        void AppendTo(T customization);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        string GetIdentifier();
    }
}
