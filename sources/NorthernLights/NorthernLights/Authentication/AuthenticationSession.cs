using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NorthernLights.Authentication.Models;
using NorthernLights.Common.Hardware;
using NorthernLights.Common.Networking;

namespace NorthernLights.Authentication
{
    public class AuthenticationSession
    {
        private readonly List<Device> _devicesToAuthenticate;
        private readonly List<AuthenticatedDevice> _authenticatedDevices;

        public IReadOnlyList<Device> DevicesToAuthenticate => _devicesToAuthenticate;

        public IReadOnlyList<AuthenticatedDevice> AuthenticatedDevices => _authenticatedDevices;

        private AuthenticationSession()
        {
            _devicesToAuthenticate = new List<Device>();
            _authenticatedDevices = new List<AuthenticatedDevice>();
        }

        public static AuthenticationSession CreateFromDiscovery(IReadOnlyList<Device> devices)
        {
            var session = new AuthenticationSession();
            session._devicesToAuthenticate.AddRange(devices);
            return session;
        }

        public static AuthenticationSession RestoreFromFile(string s)
        {
            var session = new AuthenticationSession();
            var devices = JsonConvert.DeserializeObject<List<AuthenticatedDevice>>(s);
            session._authenticatedDevices.AddRange(devices);
            return session;
        }

        public string SerializeAuthenticatedDevices() => JsonConvert.SerializeObject(_authenticatedDevices);

        public async Task<AuthenticatedDevice> Authenticate(Device device, string userAssignedName)
        {
            if (!_devicesToAuthenticate.Contains(device)) throw new Exception("Can't authenticate device from a different session!");

            var post = await ApiCalls.HttpClient.PostAsync($"http://{device.Ip}:{device.Port}/{ApiCalls.ApiPrefix}/new", new StringContent(""));

            var responseBody = await post.Content.ReadAsStringAsync();

            if (!post.IsSuccessStatusCode) throw new Exception($"Authentication failed: {responseBody}");

            var response = JsonConvert.DeserializeObject<AddUserResponse>(responseBody);

            var authenticatedDevice = new AuthenticatedDevice(device.Ip, device.Port, device.Id, device.Firmware, device.Model,
                device.DisplayName, userAssignedName, response.auth_token);

            _authenticatedDevices.Add(authenticatedDevice);

            _devicesToAuthenticate.Remove(device);

            return authenticatedDevice;
        }
    }
}
