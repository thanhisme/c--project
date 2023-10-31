using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication1.Constants.Strings;
using WebApplication1.Utils.HttpResponse;

namespace WebApplication1.Controllers
{
    public class BaseController : ControllerBase
    {
        public ActionResult<HttpResponse<T>> BaseResponse<T>(
            HttpStatusCode statusCode,
            string message,
            int? totalRecords = null,
            T? data = default
        )
        {
            return StatusCode(
                (int)statusCode,
                new HttpResponse<T>(statusCode, message, totalRecords, data)
            );
        }

        public ActionResult<HttpResponse<T>> SuccessResponse<T>(int totalRecords, T? data = default)
        {
            return BaseResponse(
                HttpStatusCode.OK,
                HttpSuccessfulMessages.BASE_SUCCESS,
                totalRecords,
                data
            );
        }

        public ActionResult<HttpResponse<T>> SuccessResponse<T>(T? data = default)
        {
            return BaseResponse(
                HttpStatusCode.OK,
                HttpSuccessfulMessages.BASE_SUCCESS,
                data: data
            );
        }

        public ActionResult<HttpResponse<T>> CreatedResponse<T>(T? data = default)
        {
            return BaseResponse(HttpStatusCode.Created, HttpSuccessfulMessages.CREATED, data: data);
        }
    }
}
