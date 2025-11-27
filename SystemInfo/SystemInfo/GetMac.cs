using SystemInfo.Properties;
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
                    n.GetPhysicalAddress().GetAddressBytes().Length > 0)
                .Select(n => n.GetPhysicalAddress().GetAddressBytes())
                .FirstOrDefault();

            if (bytes != null && bytes.Length > 0)
            {
                return string.Join("-", bytes.Select(b => b.ToString("X2")));
            }

            return Resources.MacAddressNotFound;
        }
    }
}