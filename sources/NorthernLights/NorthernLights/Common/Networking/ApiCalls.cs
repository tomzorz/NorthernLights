using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NorthernLights.Common.Hardware;

namespace NorthernLights.Common.Networking
{
    internal static class ApiCalls
    {
        public static readonly HttpClient HttpClient = new HttpClient();

        public const string ApiPrefix = "api/v1";

        public static string GetUrl(this AuthenticatedDevice device) => $"http://{device.Ip}:{device.Port}/{ApiPrefix}/{device.Token}";
    }
}
