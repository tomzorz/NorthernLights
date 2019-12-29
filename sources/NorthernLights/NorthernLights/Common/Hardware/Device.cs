using Newtonsoft.Json;

namespace NorthernLights.Common.Hardware
{
    public class Device
    {
        public string Ip { get; }

        public string Port { get; }

        public string Id { get; }

        public string Firmware { get; }

        public string Model { get; }

        public string DisplayName { get; }

        [JsonConstructor]
        internal Device(string ip, string port, string id, string firmware, string model, string displayName)
        {
            Ip = ip;
            Port = port;
            Id = id;
            Firmware = firmware;
            Model = model;
            DisplayName = displayName;
        }

        public override string ToString() => $"{DisplayName} {Ip}:{Port} {Id} - {Model} {Firmware}";
    }
}
