using BaseArchitect.Core.CustomException;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace BaseArchitect.API.Config
{
    public class JsonExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode code = CustomStatusCodeByException(context.Exception);

            var result = new ObjectResult(new
            {
                Code = code,
                Message = context.Exception.Message,                
            });

            context.HttpContext.Response.StatusCode = (int)code;
            context.Result = result;
        }

        private HttpStatusCode CustomStatusCodeByException(Exception ex)
        {
            // lỗi chưa đăng nhập
            if (ex is AuthenticationException)
            {
                return HttpStatusCode.Unauthorized; // 401
            }

            // lỗi truy cập vào link không được phép
            if (ex is UnauthorizedAccessException)
            {
                return HttpStatusCode.MethodNotAllowed; // 405
            }
            
            // lỗi hết hạn access token
            if (ex is TokenExpiredException)
            {
                return HttpStatusCode.Forbidden; // 403
            }

            // lỗi nghiệp vụ
            if (ex is Exception)
            {
                return HttpStatusCode.BadRequest; // 400
            }

            // các lỗi không kiểm soát được
            return HttpStatusCode.BadRequest;
        }
    }
}
