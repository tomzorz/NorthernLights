using NorthernLights.Common.Hardware;

namespace NorthernLights.Control
{
    public partial class ControlSession
    {
        private readonly AuthenticatedDevice _device;

        public ControlSession(AuthenticatedDevice device)
        {
            _device = device;
        }


    }
}
