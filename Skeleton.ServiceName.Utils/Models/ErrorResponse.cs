using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using Skeleton.ServiceName.Utils.Resources;

namespace Skeleton.ServiceName.Utils.Models
{
    public class ErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string[] Details { get; set; }
        public ErrorResponse InnerError { get; set; }

        public static ErrorResponse From(System.Exception e)
        {
            if (e == null)
            {
                return null;
            }
            return new ErrorResponse
            {
                Code = e.HResult,
                Message = e.Message,
                InnerError = ErrorResponse.From(e.InnerException)
            };
        }

        public static ErrorResponse FromModelStateError(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(v => v.Errors);
            return new ErrorResponse
            {
                Code = 100,
                Message = Global.RequestValidationError,
                Details = errors.Select(e => e.ErrorMessage).ToArray(),
            };
        }
    }
}
