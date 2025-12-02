using SystemInfo.Properties;
using System.Net.NetworkInformation;

namespace SystemInfo
{
    public static class GetMac
    {
        /// <summary>
        /// Retrieves the MAC address of the first available network interface on the local machine.
        /// </summary>
        /// <remarks>This method excludes loopback and tunnel interfaces when searching for a MAC address.
        /// If no suitable network interface is found, the returned string indicates that the MAC address could not be
        /// determined.</remarks>
        /// <returns>A string containing the MAC address in hexadecimal format separated by hyphens, or a default message if no
        /// MAC address is found.</returns>
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