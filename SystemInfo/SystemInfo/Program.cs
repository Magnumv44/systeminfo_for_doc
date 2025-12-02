using System;
using System.Runtime.InteropServices;
using System.Text;
using SystemInfo.Properties;

namespace SystemInfo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var osInformations = new StringBuilder();

            // Визначення ОС
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var pcOrLaptop = PcOrLaptop.GetPcOrLaptop();
                var productKey = KeyDecoder.GetWindowsProductKeyFromRegistry();

                osInformations.AppendLine(pcOrLaptop)
                              .AppendLine(productKey);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                osInformations.AppendLine(Resources.LinuxOS);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                osInformations.AppendLine(Resources.macOS);
            }
            else
            {
                osInformations.AppendLine(Resources.UnknownOS);
            }

            // Отримання MAC-адреси та імені пристрою
            var macAddress = GetMac.GetMacAddress();
            var machineName = Environment.MachineName;

            osInformations.AppendLine(Resources.MAC + macAddress)
                          .AppendLine(Resources.PcName + machineName);

            Console.WriteLine(osInformations);

            // Запис у файл
            CreateWriteTextFile.CreateAndWriteFile(osInformations.ToString());

            Console.WriteLine(Resources.Exit);
            Console.ReadLine();
        }
    }
}