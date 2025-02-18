using System.Collections.Generic;
using System.Linq;
using System;
using System.Net.NetworkInformation;

namespace HmAvailablePort
{
    internal class Program
    {
        static List<int> portsInUse;

        public static int AvailablePort()
        {
            var ipGP = IPGlobalProperties.GetIPGlobalProperties();
            var tcpEPs = ipGP.GetActiveTcpListeners();
            var udpEPs = ipGP.GetActiveUdpListeners();
            portsInUse = tcpEPs.Concat(udpEPs).Select(p => p.Port).ToList();

            for (int port = 49152; port <= 65535; port++)
            {
                if (!portsInUse.Contains(port))
                {
                    return port;
                }
            }

            return 0; // 空きポートが見つからない場合
        }


        static int Main(string[] args)
        {
            int port = AvailablePort();
            Console.WriteLine("PORT:" + port);
            if (port > 0) { return 0; } else { return 1; }
        }
    }
}
