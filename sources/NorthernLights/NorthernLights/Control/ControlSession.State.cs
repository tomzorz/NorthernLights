using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NorthernLights.Common.Networking;
using NorthernLights.Control.Models;

namespace NorthernLights.Control
{
    public partial class ControlSession
    {
        private const string ApiSetOnOff = "state";
        private const string ModelSetOnOffKey = "on";

        public async Task<bool> QueryOnOff()
        {
            var result = await ApiCalls.HttpClient.GetStringAsync($"{_device.GetUrl()}/{ApiSetOnOff}/{ModelSetOnOffKey}");
            var response = JsonConvert.DeserializeObject<BooleanValueResponse>(result);
            return response.value;
        }

        public async Task SetOnOff(bool on)
        {
            var result = await ApiCalls.HttpClient.PutAsync($"{_device.GetUrl()}/{ApiSetOnOff}", new StringContent(
                JsonConvert.SerializeObject(new Dictionary<string, object>
                {
                    [ModelSetOnOffKey] = new BooleanValueResponse
                    {
                        value = on
                    }
                })));
            if (!result.IsSuccessStatusCode) throw new Exception("Failed to set state on/off!");
        }
    }
}
