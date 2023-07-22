namespace StoreApp.Infrastructure.Extensions
{
    public static class HttpRequestExtansion
    {
        public static string PathAndQuery(this HttpRequest request)
        {
            return request.QueryString.HasValue 
            ? $"{request.Path}{request.QueryString}" 
            : request.Path.ToString();
        } 
    }
}