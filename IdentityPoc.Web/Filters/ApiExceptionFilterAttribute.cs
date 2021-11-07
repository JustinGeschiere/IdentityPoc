using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityPoc.Web.Filters
{
	public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
	{
		private static Dictionary<int, Type[]> exceptionStatusCodeMap = new()
		{
			
			{ 400, new[] { typeof(InvalidOperationException), typeof(NotSupportedException) } },
			{ 403, new[] { typeof(UnauthorizedAccessException) } },
			{ 404, new[] { typeof(IndexOutOfRangeException), typeof(KeyNotFoundException) } }
		};

		private readonly ILogger _logger;

		public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
		{
			_logger = logger;
		}

		private int GetStatusCode(Type type)
		{
			foreach(var entry in exceptionStatusCodeMap)
			{
				if (entry.Value.Contains(type))
				{
					return entry.Key;
				}
			}

			return 500;
		}

		private void HandleResponse(ExceptionContext context)
		{
			var statusCode = GetStatusCode(context.Exception.GetType());
			context.HttpContext.Response.StatusCode = statusCode;

			var result = new 
			{ 
				Path = context.HttpContext.Request.Path.Value ?? string.Empty, 
				Error = context.Exception.Message,
#if DEBUG
				StackTrace = context.Exception.StackTrace
#endif
			};

			context.Result = new JsonResult(result);

			_logger.LogError($"Exception at '{result.Path}': {result.Error} \n{context.Exception.StackTrace}");
		}

		public override void OnException(ExceptionContext context)
		{
			HandleResponse(context);
			context.ExceptionHandled = true;

			base.OnException(context);
		}
	}
}
