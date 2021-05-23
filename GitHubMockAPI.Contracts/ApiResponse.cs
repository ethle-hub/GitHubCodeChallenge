namespace GitHubMockAPI.Contracts
{
    /// <summary>
    /// Defines the wrapper <see cref="ApiResponse{T}" /> class for all API response.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class ApiResponse<T> where T : class
    {
        /// <summary>
        /// `true` if the operation was successful, else the otherwise.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Will pe populated when the response had a payload. To be used when the Outcome is successful.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Gets the ErrorMessage
        /// The error message should be populated when IsSuccess is `false`.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
