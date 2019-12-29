using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NorthernLights.Authentication;
using NorthernLights.Common;
using NorthernLights.Common.Hardware;
using NorthernLights.Control;
using NorthernLights.Discovery;

namespace NorthernLights.CommandLineTester
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string fn = "auth.json";

            AuthenticatedDevice cDevice;

            // discover and auth if there's no file
            if (!File.Exists(fn))
            {
                Console.WriteLine("Discovery starting...");
                Console.WriteLine();

                var devices = await DeviceDiscovery.Discover();
                for (var index = 0; index < devices.Count; index++)
                {
                    var device = devices[index];
                    Console.WriteLine($"[{index}] {device}");
                }

                Console.Write("Pick device to auth:");
                var line = Console.ReadLine();
                // ReSharper disable once AssignNullToNotNullAttribute
                var pick = int.Parse(line);

                var authDev = devices[pick];
                Console.WriteLine($"picked {authDev}");
                Console.WriteLine("hold power for 5-7 seconds on device until light starts blinking then press enter to continue");
                Console.ReadLine();

                var asession = AuthenticationSession.CreateFromDiscovery(devices);
                var authedDev = await asession.Authenticate(authDev, "pc");
                Console.WriteLine(authedDev);
                var content = asession.SerializeAuthenticatedDevices();
                File.WriteAllText(fn, content);

                cDevice = authedDev;
            }
            else
            {
                Console.WriteLine("Loading pre-authed devices from file...");
                Console.WriteLine();

                var s = File.ReadAllText(fn);
                var psession = AuthenticationSession.RestoreFromFile(s);

                cDevice = psession.AuthenticatedDevices[0];
            }

            Console.WriteLine("controlling device...");
            Console.WriteLine();

            Console.WriteLine(cDevice);
            Console.WriteLine();

            var loop = true;
            var cs = new ControlSession(cDevice);
            while (loop)
            {
                Console.Write($"Q - exit; A - query on/off; W - turn on; S - turn off: ");
                var line = Console.ReadLine();

                switch (line.ToUpper())
                {
                    case "Q":
                        loop = false;
                        break;
                    case "A":
                        Console.WriteLine(await cs.QueryOnOff());
                        break;
                    case "W":
                        await cs.SetOnOff(true);
                        break;
                    case "S":
                        await cs.SetOnOff(false);
                        break;
                }
            }

            Console.ReadLine();
        }
    }
}
