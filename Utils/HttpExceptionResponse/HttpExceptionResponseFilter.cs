using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.RegularExpressions;
using WebApplication1.Constants.Enums;
using WebApplication1.Constants.Strings;

namespace WebApplication1.Utils.HttpExceptionResponse
{
    public class HttpExceptionResponseFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;
        private readonly IWebHostEnvironment _environment;

        public HttpExceptionResponseFilter(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = NormalizeValidationErrors(context.ModelState);

                context.Result = new ObjectResult(
                    new
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Validation Error",
                        Errors = errors
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                var exception = ExceptionFactory(context.Exception);

                if (_environment.IsDevelopment())
                {
                    context.Result = new ObjectResult(
                        new
                        {
                            exception.StatusCode,
                            exception.Message,
                            exception.Errors,
                            detail = context.Exception.StackTrace
                        }
                    )
                    {
                        StatusCode = (int)exception.StatusCode
                    };
                }
                else
                {
                    context.Result = new ObjectResult(
                        new { exception.StatusCode, exception.Errors }
                    )
                    {
                        StatusCode = (int)exception.StatusCode
                    };
                }
            }

            context.ExceptionHandled = true;
        }

        private static HttpExceptionResponse ExceptionFactory(Exception exception)
        {
            if (exception is HttpExceptionResponse httpExceptionResponse)
            {
                return httpExceptionResponse;
            }

            if (exception.InnerException is SqlException sqlException)
            {
                var errors = new Dictionary<string, List<string>>();

                foreach (SqlError error in sqlException.Errors)
                {
                    string duplicateConstraint = ExtractWithPattern(error.Message, @"unique index '([^']*)'");
                    string duplicateValue = ExtractWithPattern(error.Message, @"\(([^)]*)\)");

                    if (error.Number == 2601)
                    {
                        if (!errors.ContainsKey(duplicateConstraint))
                        {
                            errors[duplicateConstraint] = new List<string>();
                        }

                        errors[duplicateConstraint].Add($"Value {duplicateValue} already exists!");
                    }
                }

                return new HttpExceptionResponse(
                    HttpExceptionTypes.BadRequest,
                    HttpExceptionMessages.CONTRAINT_ERRORS,
                    errors
                );
            }


            return new HttpExceptionResponse(
                HttpExceptionTypes.InternalServerError,
                HttpExceptionMessages.INTERNAL_SERVER_ERROR
            );
        }

        private static Dictionary<string, List<string>> NormalizeValidationErrors(ModelStateDictionary state)
        {
            var errors = new Dictionary<string, List<string>>();

            // Iterate through ModelState errors to create the key-value mappings
            foreach (var key in state.Keys)
            {
                var errorMessages = state[key].Errors
                    .Select(error => error.ErrorMessage)
                    .ToList();

                errors[key] = errorMessages;
            }

            return errors;
        }

        private static string ExtractWithPattern(string srcString, string pattern)
        {
            Match match = Regex.Match(srcString, pattern);

            if (match.Success)
            {
                string value = match.Groups[1].Value;
                return value;
            }

            return string.Empty;
        }
    }
}
