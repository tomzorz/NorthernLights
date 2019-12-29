using Newtonsoft.Json;

namespace NorthernLights.Common.Hardware
{
    public class AuthenticatedDevice : Device
    {
        public string UserAssignedName { get; }

        public string Token { get; }

        [JsonConstructor]
        internal AuthenticatedDevice(string ip, string port, string id, string firmware, string model, string displayName,
            string userAssignedName, string token) : base(ip, port, id, firmware, model, displayName)
        {
            UserAssignedName = userAssignedName;
            Token = token;
        }

        public override string ToString() => $"{base.ToString()} {UserAssignedName} {Token}";
    }
}
