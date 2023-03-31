using OnaxTools.Enums;

namespace OnaxTools.Dto
{
    public class GenResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Result { get; set; }
        public string Message { get; set; } = null;
        public string Error { get; set; } = null;
        public int StatCode { get; set; } = (int)StatusCodeEnum.OK;

        public static GenResponse<T> Success(T result, StatusCodeEnum statusCode = StatusCodeEnum.OK, string message = null) => new GenResponse<T> { IsSuccess = true, Result = result, Message = message, StatCode = (int)statusCode };
        public static GenResponse<T> Failed(string error, StatusCodeEnum statusCode = StatusCodeEnum.BadRequest) => new GenResponse<T> { IsSuccess = false, Error = error, StatCode = (int)statusCode };
    }
}
