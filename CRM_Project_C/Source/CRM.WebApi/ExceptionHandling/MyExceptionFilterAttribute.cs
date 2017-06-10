﻿using CRM.WebApi.Infratructure;
using System;
using System.Data;
using System.Data.Entity.Core;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace CRM.WebApi.Filters
{
    public class MylExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ApplicationLoggerManager log = new ApplicationLoggerManager();

        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            log.LogError(actionExecutedContext.Exception, actionExecutedContext.Request.Method, actionExecutedContext.Request.RequestUri);

            if (actionExecutedContext.Exception is ArgumentNullException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format($"{actionExecutedContext.Exception.Message}\n{actionExecutedContext.Exception.InnerException?.Message}")),
                };
            }
            else if (actionExecutedContext.Exception is NullReferenceException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format($"{actionExecutedContext.Exception.Message}\n{actionExecutedContext.Exception.InnerException?.Message}")),
                };
            }
            else if (actionExecutedContext.Exception is DataException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.Conflict)
                { 
                    Content = new StringContent(string.Format($" Data Exception {actionExecutedContext.Exception.Message}\n{actionExecutedContext.Exception.InnerException?.Message}")),
                };
            }

            else if (actionExecutedContext.Exception is EntityException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent(string.Format($"Entity Exception{actionExecutedContext.Exception.Message}\n{actionExecutedContext.Exception.InnerException?.Message}")),
                };
            }
            else if (actionExecutedContext.Exception is NotImplementedException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented)
                {
                    Content = new StringContent(string.Format($"{actionExecutedContext.Exception.Message}\n{actionExecutedContext.Exception.InnerException?.Message}"))
                };
            }
            else
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(string.Format($"We apologize but an error occured within the application.\n{actionExecutedContext.Exception.Message}\n{actionExecutedContext.Exception.InnerException?.Message}"))
                };
            
            }

            return base.OnExceptionAsync(actionExecutedContext, cancellationToken);
        }
    }
}