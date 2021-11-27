using Application.Extentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Application.Models
{
    public class ApiResult
    {
        public ApiResultStatusCode StatusCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Message { get; set; }

        public ApiResult(ApiResultStatusCode statusCode, List<string> message = null)
        {
            StatusCode = statusCode;
            Message = message ?? statusCode.ToDisplays();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        #region Implicit Operators
        public static implicit operator ApiResult(OkResult result)
        {
            return new ApiResult(ApiResultStatusCode.Success);
        }

        public static implicit operator ApiResult(BadRequestResult result)
        {
            return new ApiResult(ApiResultStatusCode.BadRequest);
        }

        public static implicit operator ApiResult(BadRequestObjectResult result)
        {
            List<string> message = new List<string>();
            if (result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message.AddRange(errorMessages);
            }

            else if (result.Value is IEnumerable<IdentityError> identityErrors)
            {
                var errorMessages = identityErrors.Select(p => p.Description).Distinct();
                message.AddRange(errorMessages);
            }

            else
            {
                message.Add(result.Value.ToString());
            }

            return new ApiResult(ApiResultStatusCode.BadRequest, message);
        }

        public static implicit operator ApiResult(ContentResult result)
        {
            List<string> Message = new List<string>() { result.Content };
            return new ApiResult(ApiResultStatusCode.Success, Message);
        }

        public static implicit operator ApiResult(NotFoundResult result)
        {
            return new ApiResult(ApiResultStatusCode.NotFound);
        }
        #endregion
    }

    public class ApiResult<TData> : ApiResult
        where TData : class
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TData data { get; set; }

        public ApiResult(ApiResultStatusCode statusCode, TData _data, List<string> message = null)
            : base(statusCode, message)
        {
            data = _data;
        }

        #region Implicit Operators
        public static implicit operator ApiResult<TData>(TData data)
        {
            return new ApiResult<TData>(ApiResultStatusCode.Success, data);
        }

        public static implicit operator ApiResult<TData>(OkResult result)
        {
            return new ApiResult<TData>(ApiResultStatusCode.Success, null);
        }

        public static implicit operator ApiResult<TData>(OkObjectResult result)
        {
            return new ApiResult<TData>(ApiResultStatusCode.Success, (TData)result.Value);
        }

        public static implicit operator ApiResult<TData>(BadRequestResult result)
        {
            return new ApiResult<TData>(ApiResultStatusCode.BadRequest, null);
        }

        public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
        {

            List<string> message = new List<string>();
            if (result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message.AddRange(errorMessages);
            }

            else if (result.Value is IEnumerable<IdentityError> identityErrors)
            {
                var errorMessages = identityErrors.Select(p => p.Description).Distinct();
                message.AddRange(errorMessages);
            }

            else
            {
                if (result.Value is string[])
                    foreach (string error in result.Value as string[])
                        message.Add(error);
                else if (result.Value is string)
                    if (!string.IsNullOrEmpty(result.Value as string))
                        message.Add(result.Value as string);

            }

            return new ApiResult<TData>(ApiResultStatusCode.BadRequest, null, message);
        }

        public static implicit operator ApiResult<TData>(ContentResult result)
        {
            List<string> Message = new List<string>() { result.Content };
            return new ApiResult<TData>(ApiResultStatusCode.Success, null, Message);
        }

        public static implicit operator ApiResult<TData>(NotFoundResult result)
        {
            return new ApiResult<TData>(ApiResultStatusCode.NotFound, null);
        }

        public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
        {
            return new ApiResult<TData>(ApiResultStatusCode.NotFound, (TData)result.Value);
        }

        public static implicit operator ApiResult<TData>(UnauthorizedResult result)
        {
            return new ApiResult<TData>(ApiResultStatusCode.UnAuthorized, null);
        }

        public static implicit operator ApiResult<TData>(UnauthorizedObjectResult result)
        {
            return new ApiResult<TData>(ApiResultStatusCode.UnAuthorized, (TData)result.Value);
        }

        public static implicit operator ApiResult<TData>(ForbidResult result)
        {
            return new ApiResult<TData>(ApiResultStatusCode.Forbiden, null);
        }
        #endregion
    }

    public enum ApiResultStatusCode
    {
        [Display(Name = "Invalid parameters.")]
        BadRequest = 400,
        [Display(Name = "Success")]
        Success = 200,
        [Display(Name = "Internal server error.")]
        ServerError = 500,
        [Display(Name = "Not found")]
        NotFound = 404,
        [Display(Name = "Empty list.")]
        ListEmpty = 444,
        [Display(Name = "دسترسی شما محدود شده است.")]
        Forbiden = 403,
        [Display(Name = "Unauthorized")]
        UnAuthorized = 401
    }
}