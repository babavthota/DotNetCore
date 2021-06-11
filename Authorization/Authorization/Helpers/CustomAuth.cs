using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Authorization.Helpers.Global;

namespace Authorization.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(Type type) : base(typeof(ClaimRequirementFilter))
        {
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly string _x;
        string bodyContent = "";

        public ClaimRequirementFilter(string x)
        {
            _x = x;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.HttpContext.Request.EnableBuffering();

            //Extract request from httpcontext
            var httpRequest = context.HttpContext.Request;

            if (httpRequest != null)
            {
                var contentType = httpRequest.ContentType;

                //GET user information if the request type is multipart
                if (contentType.Contains(RequestType.JsonType.ToString()))
                {
                    var userInfo = httpRequest.Form.Where(k => k.Key == "UserCode").FirstOrDefault();
                }
                else if (contentType.Contains(RequestType.FileWithForm.ToString()))
                {
                    using(StreamReader reader = new StreamReader(httpRequest.Body, Encoding.UTF8, true,1024, true))
                    {
                        bodyContent = reader.ReadToEnd();
                    }

                    JObject json = JObject.Parse(bodyContent);
                }
                
            }

            context.Result = new UnauthorizedResult();
        }
    }
}
