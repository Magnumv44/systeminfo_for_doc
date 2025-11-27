using System.Runtime.InteropServices;
using SystemInfo.Properties;

namespace SystemInfo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string osInformations = string.Empty;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine(PcOrLaptop.GetPcOrLaptop());
                Console.WriteLine(KeyDecoder.GetWindowsProductKeyFromRegistry());
                osInformations = $"{PcOrLaptop.GetPcOrLaptop()}\n{KeyDecoder.GetWindowsProductKeyFromRegistry()}";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Console.WriteLine("Використовується операційна система Linux. Ключ непотрібен.");
                osInformations = "Використовується операційна система Linux. Ключ непотрібен.";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Console.WriteLine("Використовується операційна система macOS. Ключ непотрібен.");
                osInformations = "Використовується операційна система macOS. Ключ непотрібен.";
            }
            else
            {
                Console.WriteLine("Невідомо (не Windows)");
                osInformations = "Невідомо (не Windows)";
            }

            Console.WriteLine("MAC-адреса:" + GetMac.GetMacAddress());
            Console.WriteLine("Iм'я пристрою:" + Environment.MachineName);

            osInformations += $"\nMAC-адреса: {GetMac.GetMacAddress()}\nIм'я пристрою: {Environment.MachineName}";

            CreateWriteTextFile.CreateAndWriteFile(osInformations);

        }
    }
}