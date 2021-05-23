namespace GitHubMockAPI.Common.Entities
{
    /// <summary>
    /// This <see cref="ServiceOutcome" /> class will be used throughout the solution as the general outcome of an operation.
    /// </summary>
    public class ServiceOutcome
    {
        /// <summary>
        /// `true` if the operation was successful, else the otherwise.
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// Gets the ErrorMessage
        /// The error message should be populated when IsSuccess is `false`.
        /// </summary>
        public string? ErrorMessage { get; private set; }

        /// <summary>
        /// Gets the ErrorType. <see cref="ErrorType" />
        /// </summary>
        public ErrorType ErrorType { get; private set; }

        #region private constructor
        /// <summary>
        /// Use `Success()` or `Error(ErrorType errorType, string errorMessage)` to return this instance of the <see cref="ServiceOutcome"/> class.
        /// </summary>
        /// <param name="isSuccess">The isSuccess<see cref="bool"/>.</param>
        /// <param name="errorMessage">The errorMessage<see cref="string"/>.</param>
        /// <param name="errorType">The errorType<see cref="ErrorType"/>.</param>
        private ServiceOutcome(bool isSuccess, string errorMessage, ErrorType errorType)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }

        #endregion

        /// <summary>
        /// Returns a new successful result.
        /// </summary>
        /// <returns>The <see cref="ServiceOutcome"/>.</returns>
        public static ServiceOutcome Success()
        {
            return new(true, string.Empty, ErrorType.None);
        }

        /// <summary>
        /// Returns a new unsuccessful result with the error message.
        /// </summary>
        /// <param name="errorType">The errorType<see cref="ErrorType"/>.</param>
        /// <param name="errorMessage">The errorMessage<see cref="string"/>.</param>
        /// <returns>The <see cref="ServiceOutcome"/>.</returns>
        public static ServiceOutcome Error(ErrorType errorType, string errorMessage)
        {
            return new(false, errorMessage, errorType);
        }

        /// <summary>
        /// This allows us to treat a ServiceOutcome as if it were a boolean.
        /// e.g. usage `if (!theResultObject.Outcome) continue;`
        /// </summary>
        public static implicit operator bool(ServiceOutcome outcome)
        {
            return outcome.IsSuccess;
        }
    }
}
