using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ebyteLearner.Helpers
{
    public class CustomAuthorization: AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("You are unauthorized to access this resource")
            };
        }
    }
}
