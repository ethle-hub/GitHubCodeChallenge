namespace GitHubMockAPI.Common.Entities
{
    /// <summary>
    /// Defines the ErrorType.
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// Defines the None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Defines the BusinessError.
        /// </summary>
        BusinessError,

        /// <summary>
        /// Defines the DependencyError.
        /// </summary>
        DependencyError,

        /// <summary>
        /// Defines the Forbidden.
        /// </summary>
        Forbidden
    }
}
