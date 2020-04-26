using System;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Server
{
    public static class Extensions
    {
        public static byte[] GetSecretKey(this IConfiguration configuration)
        {
            var key = configuration["TRAINS_JWT_SECRET"];
            return Encoding.UTF8.GetBytes(string.IsNullOrEmpty(key) ? new Guid().ToString() : key.Trim());
        }
    }
}