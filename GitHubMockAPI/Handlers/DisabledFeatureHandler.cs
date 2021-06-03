using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.FeatureManagement.Mvc;

namespace GitHubMockAPI.Handlers
{
	public class DisabledFeatureHandler : IDisabledFeaturesHandler
    {
        public Task HandleDisabledFeatures(IEnumerable<string> features, ActionExecutingContext context)
        {
            context.Result = new ObjectResult($"Action disallowed. Feature(s) are disabled: {string.Join(", ", features)}")
            {
                StatusCode = StatusCodes.Status404NotFound
            };
            return Task.CompletedTask;
        }
    }
}
