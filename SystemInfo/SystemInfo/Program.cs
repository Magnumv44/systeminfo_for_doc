using System;
using System.Runtime.InteropServices;

namespace SystemInfo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(GetMac.GetMacAddress());

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Console.WriteLine(PcOrLaptop.GetPcOrLaptop());
            else
                Console.WriteLine("Невідомо (не Windows)");
        }
    }
}