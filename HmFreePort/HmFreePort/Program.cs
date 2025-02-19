using System.Collections.Generic;
using System.Linq;
using System;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace HmFreePort
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)] // これは必須
    [Guid("B4280D3B-413D-4630-88A5-7AA68B562226")] // GUIDはそれぞれのclassで生成しなおすこと。

    public class HmFreePort
    {

        public int Port
        {
            get
            {
                try
                {
                    var ipGP = IPGlobalProperties.GetIPGlobalProperties();
                    var usedPorts = ipGP.GetActiveTcpListeners()
                        .Concat(ipGP.GetActiveUdpListeners())
                        .Select(endpoint => endpoint.Port)
                        .Distinct();

                    for (int port = 49152; port <= 65535; port++)
                    {
                        if (!usedPorts.Contains(port))
                        {
                            return port; // 空いているポートを見つけた場合
                        }
                    }
                }
                catch (Exception)
                {
                }

                return 0; // 空きポートが見つからない場合
            }
        }

        public int GetAvailablePort()
        {
            return Port;
        }


        static int Main(string[] args)
        {
            HmFreePort ap = new HmFreePort();
            int port = ap.Port;
            Console.WriteLine(port);
            if (port > 0) { return 0; } else { return 1; }
        }
    }
}
