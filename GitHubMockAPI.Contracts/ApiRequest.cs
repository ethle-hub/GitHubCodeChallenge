namespace GitHubMockAPI.Contracts
{
    /// <summary>
    /// Defines the wrapper <see cref="ApiRequest{T}" /> class for all API request.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class ApiRequest<T> where T : class
    {
        /// <summary>
        /// Gets or sets the Data
        /// Indicates the payload of the request..
        /// </summary>
        public T Data { get; set; }
    }
}
