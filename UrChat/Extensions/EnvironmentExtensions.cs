using System;
using Microsoft.AspNetCore.Hosting;

namespace UrChat.Extensions
{
    public static class EnvironmentExtensions
    {
        public static bool IsDevelopment()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return environment == EnvironmentName.Development;
        }
    }
}