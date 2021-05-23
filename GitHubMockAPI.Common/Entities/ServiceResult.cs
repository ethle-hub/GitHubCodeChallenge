namespace GitHubMockAPI.Common.Entities
{
    using System;

    /// <summary>
    /// This <see cref="ServiceResult{T}" /> class will be used throughout the solution as the general outcome of an operation with an output data.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class ServiceResult<T>
    {
        /// <summary>
        /// The result of an operation. To be used when the Outcome is successful.
        /// </summary>
        public T Data { get; private set; }

        /// <summary>
        /// Gets the Outcome. See <see cref="ServiceOutcome" />.
        /// </summary>
        public ServiceOutcome Outcome { get; private set; }

        #region private constructors
        /// <summary>
        /// Use implicit operators to return this instance of the <see cref="ServiceResult{T}"/> class.
        /// </summary>
        /// <param name="outcome">The outcome<see cref="ServiceOutcome"/>.</param>
        /// <param name="value">The value<see cref="T"/>.</param>
        private ServiceResult(ServiceOutcome outcome, T value)
        {
            Outcome = outcome;
            Data = value;
        }

        #endregion


        /// <summary>
        /// Cast from the payload. The common use case is "return value;" to auto-cast to ServiceResult{T}. This is used for success results.
        /// </summary>
        public static implicit operator ServiceResult<T>(T data)
        {
            return new(ServiceOutcome.Success(), data);
        }

        /// <summary>
        /// Implicit cast from a (negative) ServiceOutcome. Use this when the operation has failed. The Value property will not be set.
        /// </summary>
        public static implicit operator ServiceResult<T>(ServiceOutcome failure)
        {
            if (failure.IsSuccess) throw new Exception("Invalid call to create a negative service result using a successful outcome. If the result is successful, you should have a payload <T>.");

            return new ServiceResult<T>(failure, default);
        }
    }
}
