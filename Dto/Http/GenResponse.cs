using OnaxTools.Enums.Http;

namespace OnaxTools.Dto.Http
{
    public class GenResponse<T>
    {
        public virtual bool IsSuccess { get; set; }
        public T Result { get; set; }
        public virtual string Message { get; set; } = null;
        public virtual string Error { get; set; } = null;
        public virtual int StatCode { get; set; } = (int)StatusCodeEnum.OK;

        public static GenResponse<T> Success(T result, StatusCodeEnum statusCode = StatusCodeEnum.OK, string message = null) => new GenResponse<T> { IsSuccess = true, Result = result, Message = message, StatCode = (int)statusCode };
        public static GenResponse<T> Failed(string error, StatusCodeEnum statusCode = StatusCodeEnum.BadRequest) => new GenResponse<T> { IsSuccess = false, Error = error, StatCode = (int)statusCode };
    }
}
