using System;
using System.Net.Http;

namespace OctopusSubscriptionHandler.Exceptions
{
    public class BadRequestException : HttpRequestException
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
