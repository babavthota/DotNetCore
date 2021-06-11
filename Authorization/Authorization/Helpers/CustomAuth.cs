using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Authorization.Helpers.Global;

namespace Authorization.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomAuthorizeAttribute(string claimType, string accessLevel = "") : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, accessLevel) };
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly Claim _claim;
        string bodyContent = "";
        

        public ClaimRequirementFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.HttpContext.Request.EnableBuffering();

            //Extract request from httpcontext
            var httpRequest = context.HttpContext.Request;

            if (httpRequest != null)
            {
                var contentType = httpRequest.ContentType;
                JObject json;
                //GET user information if the request type is multipart
                if (contentType.Contains(RequestType.JsonType.ToString()))
                {
                    var userInfo = httpRequest.Form.Where(k => k.Key == "UserCode").FirstOrDefault();
                    JObject jsonObj = JObject.Parse(userInfo.Value.ToString());
                    json = new JObject(new JProperty("prop", jsonObj));
                }
                else if (contentType.Contains(RequestType.FileWithForm.ToString()))
                {
                    using (StreamReader reader = new StreamReader(httpRequest.Body, Encoding.UTF8, true, 1024, true))
                    {
                        bodyContent = reader.ReadToEnd();
                    }

                    json = JObject.Parse(bodyContent);
                }
                //validate json for validity of user;

                //if valid user, allow access
                context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            }

            context.Result = new ForbidResult();
        }
    }
}
