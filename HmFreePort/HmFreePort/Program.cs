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
                return GetFreePort();
            }
        }

        static private int GetFreePort()
        {
            try
            {
                var ipGP = IPGlobalProperties.GetIPGlobalProperties();
                if (ipGP == null)
                {
                    // Console.WriteLine("IPGlobalPropertiesの取得に失敗しました。");
                    return 0;
                }

                var usedPorts = new HashSet<int>(ipGP.GetActiveTcpListeners()
                                                  .Concat(ipGP.GetActiveUdpListeners())
                                                  .Select(endpoint => endpoint.Port));

                for (int port = 49152; port <= 65535; port++)
                {
                    if (!usedPorts.Contains(port))
                    {
                        // Console.WriteLine($"利用可能なポートが見つかりました: {port}");
                        return port;
                    }
                }

                // Console.WriteLine("利用可能なポートが見つかりませんでした。");
                return 0;
            }
            catch (NetworkInformationException ex)
            {
                // Console.WriteLine("ネットワーク接続中エラー");
                return 0;
            }
            catch (Exception ex)
            {
                // Console.WriteLine("なんか知らんがエラー");
                return 0;
            }

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
