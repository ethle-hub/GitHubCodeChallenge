namespace GitHubMockAPI.Services
{
    using Contracts;

    /// <summary>
    /// Defines the <see cref="Extentions" />.
    /// </summary>
    public static class Extentions
    {
        /// <summary>
        /// The ConvertToApiResponse.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="dto">The dto<see cref="T"/>.</param>
        /// <param name="errorMessage">The errorMessage<see cref="string"/>.</param>
        /// <returns>The <see cref="ApiResponse{T}"/>.</returns>
        public static ApiResponse<T> ConvertToApiResponse<T>(this T dto, string errorMessage = "") where T : class
        {
            if (dto != null)
            {
                return new ApiResponse<T>
                {
                    IsSuccess = true,
                    Data = dto,
                };
            }

            return new ApiResponse<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }

        /// <summary>
        /// The ConvertToApiRequest.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="dto">The dto<see cref="T"/>.</param>
        /// <returns>The <see cref="ApiRequest{T}"/>.</returns>
        public static ApiRequest<T> ConvertToApiRequest<T>(this T dto) where T : class
        {
            return new ApiRequest<T>
            {
                Data = dto
            };
        }
    }
}
