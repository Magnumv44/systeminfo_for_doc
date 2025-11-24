using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace SystemInfo
{
    public static class GetMac
    {
        public static string GetMacAddress()
        {
            var bytes = NetworkInterface.GetAllNetworkInterfaces()
                .Where(n =>
                    n.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    n.NetworkInterfaceType != NetworkInterfaceType.Tunnel &&
                    n.OperationalStatus == OperationalStatus.Up)
                .Select(n => n.GetPhysicalAddress().GetAddressBytes())
                .FirstOrDefault(b => b != null && b.Length > 0);

            if (bytes != null && bytes.Length > 0)
            {
                return string.Join("-", bytes.Select(b => b.ToString("X2")));
            }

            return "MAC-адресу не знайдено";
        }
    }
}