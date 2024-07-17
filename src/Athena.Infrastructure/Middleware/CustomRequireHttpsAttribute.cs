﻿using System.Net;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Athena.Infrastructure.Middleware
{
    public class CustomRequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //both the request is not https
            if (actionContext.Request.RequestUri!.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Found);
                actionContext.Response.Content = new StringContent
                    ("<p>Use https instead of http</p>", Encoding.UTF8, "text/html");

                //Create the request URI
                UriBuilder uriBuilder = new(actionContext.Request.RequestUri)
                {
                    //Set the Request scheme as HTTPS
                    Scheme = Uri.UriSchemeHttps,

                    //Set the HTTPS Port number as 44300
                    //In the project properties window you can find the port number
                    //for SSL URL
                    Port = 443
                };
                actionContext.Response.Headers.Location = uriBuilder.Uri;
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }
    }
}

