namespace ReportingSystem.Enum
{
    public enum Status
    {
        Continue = 100,
        Ok = 200,
        NoContent = 204,
        MovedPermanently = 301,
        NotModyfied = 304,
        PermanentRedirect = 308,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        URITooLong = 414,
        InternalServerError = 500,
        UpdatesAvailable = 600
    }
}
