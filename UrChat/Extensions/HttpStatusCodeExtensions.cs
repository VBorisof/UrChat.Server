using System.Net;

namespace UrChat.Extensions
{
    public static class HttpStatusCodeExtensions
    {
        public static bool IsSuccessful(this HttpStatusCode statusCode)
        {
            return ((int) statusCode >= 200) && ((int) statusCode <= 299);
        }
    }
}