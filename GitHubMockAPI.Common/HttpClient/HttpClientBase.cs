namespace GitHubMockAPI.Common.HttpClient
{
    using Entities;
    using System.Net;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="HttpClientBase" />.
    /// </summary>
    public class HttpClientBase
    {
        /// <summary>
        /// Helper method to converts a HttpResponseMessage to a ServiceResult{T}.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="httpResponse">The httpResponse<see cref="HttpResponseMessage"/>.</param>
        /// <returns>The <see cref="Task{ServiceResult{T}}"/>.</returns>
        protected virtual async Task<ServiceResult<T>> ConvertToServiceResult<T>(HttpResponseMessage httpResponse)
        {
            // errors
            if (httpResponse.StatusCode != HttpStatusCode.OK)
                return DependencyError(httpResponse.StatusCode, httpResponse.ReasonPhrase ?? "");

            // all good
            var content = await httpResponse.Content.ReadAsStringAsync();
            var payload = JsonSerializer.Deserialize<T>(content);

            if (payload == null)
                return BusinessError($"Unable to deserialize content `{content}`");
            return payload;
        }

        /// <summary>
        /// Return ServiceOutcome as a DependencyError.
        /// </summary>
        /// <param name="statusCode">The statusCode<see cref="HttpStatusCode"/>.</param>
        /// <param name="errorMessage">The errorMessage<see cref="string"/>.</param>
        /// <returns>The <see cref="ServiceOutcome"/>.</returns>
        public static ServiceOutcome DependencyError(HttpStatusCode statusCode, string errorMessage)
        {
            return ServiceOutcome.Error(ErrorType.DependencyError, $"({statusCode}) {errorMessage}");
        }

        /// <summary>
        /// Return ServiceOutcome as a BusinessError.
        /// </summary>
        /// <param name="errorMessage">The errorMessage<see cref="string"/>.</param>
        /// <returns>The <see cref="ServiceOutcome"/>.</returns>
        public static ServiceOutcome BusinessError(string errorMessage)
        {
            return ServiceOutcome.Error(ErrorType.BusinessError, errorMessage);
        }
    }
}
