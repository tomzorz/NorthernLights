using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NorthernLights.Common;
using NorthernLights.Common.Hardware;
using Zeroconf;

namespace NorthernLights.Discovery
{
    public static class DeviceDiscovery
    {
        private const string ServiceId = "id";
        private const string ServiceModel = "md";
        private const string ServiceVersion = "srcvers";
        private const string DiscoveryId = "_nanoleafapi._tcp.local.";

        public static async Task<IReadOnlyList<Device>> Discover()
        {
            var l = new List<Device>();

            var results = await ZeroconfResolver.ResolveAsync(DiscoveryId);

            foreach (var host in results)
            {
                var infoDict = new Dictionary<string, string>();

                var service = host.Services[DiscoveryId];

                foreach (var z in host.Services) // should be just the one...
                {
                    foreach (var valueProperty in z.Value.Properties)
                    {
                        foreach (var keyValuePair in valueProperty)
                        {
                            infoDict[keyValuePair.Key] = keyValuePair.Value;
                        }
                    }
                }

                l.Add(new Device(host.IPAddress, service.Port.ToString(), infoDict[ServiceId], infoDict[ServiceVersion], infoDict[ServiceModel], host.DisplayName));
            }

            // default port: 16021, not recommended to hardcode
            // md: model name
            // srcvers: firmware version
            // id: device id randomly generated, changes if user resets the device + removes auth

            return l;
        }
    }
}
