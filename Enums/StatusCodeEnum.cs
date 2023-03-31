namespace OnaxTools.Enums
{
    public enum StatusCodeEnum
    {
        OK = 200,
        Created = 201,
        NoContent = 204,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        UnAvailableForLegalReasons = 451,
        ServerError = 500,
        NotImplemented = 501,
        ServiceNotAvailable = 503,
        GatewayTimeout = 504,
        InsufficientStorage = 507
    }
}
