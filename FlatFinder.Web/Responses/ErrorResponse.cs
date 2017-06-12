using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace FlatFinder.Web.Responses
{
    public class ValidationError
    {
        public ValidationError(string field, string message)
        {
            Field = field;
            Message = message;
        }

        public string Field { get; }
        public string Message { get; }
    }

    public class ErrorResponse
    {
        public ErrorResponse(string message, ModelStateDictionary modelState = null)
        {
            Message = message;

            if (modelState != null)
                Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
        }

        public string Message { get; }
        public List<ValidationError> Errors { get; }
    }
}