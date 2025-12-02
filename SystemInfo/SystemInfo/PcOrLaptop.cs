using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using SystemInfo.Properties;

namespace SystemInfo
{
    public static class PcOrLaptop
    {
        public static string GetPcOrLaptop()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return GetPcOrLaptopWindows();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return GetPcOrLaptopLinux();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return GetPcOrLaptopMacOS();
            }

            return Resources.Unknown;
        }

        private static string GetPcOrLaptopWindows()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_SystemEnclosure");
                using var collection = searcher.Get();
                foreach (ManagementObject obj in collection)
                {
                    using (obj)
                    {
                        var chassisObj = obj["ChassisTypes"];
                        ushort[] chassisTypes = chassisObj as ushort[] ??
                                                (chassisObj as object[])?.Select(o => Convert.ToUInt16(o)).ToArray();

                        if (chassisTypes != null)
                        {
                            var laptopTypes = new ushort[] { 8, 9, 10, 14, 30 };
                            if (chassisTypes.Any(t => laptopTypes.Contains(t)))
                            {
                                return Resources.Notebook;
                            }
                            else
                            {
                                return Resources.PC;
                            }
                        }
                    }
                }
            }
            catch (ManagementException)
            {
                return Resources.Management;
            }
            catch (UnauthorizedAccessException)
            {
                return Resources.UnauthorizedAccess;
            }

            return Resources.Unknown;
        }

        private static string GetPcOrLaptopLinux()
        {
            try
            {
                const string chassisTypePath = "/sys/class/dmi/id/chassis_type";
                if (File.Exists(chassisTypePath))
                {
                    var chassisType = File.ReadAllText(chassisTypePath).Trim();
                    var laptopTypes = new[] { "8", "9", "10", "14", "30" }; // Типи для ноутбуків
                    if (laptopTypes.Contains(chassisType))
                    {
                        return Resources.Notebook;
                    }
                    else
                    {
                        return Resources.PC;
                    }
                }
            }
            catch
            {
                // Ігноруємо помилки і повертаємо Unknown
            }

            return Resources.Unknown;
        }

        private static string GetPcOrLaptopMacOS()
        {
            try
            {
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "sysctl",
                        Arguments = "hw.model",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (output.Contains("MacBook", StringComparison.OrdinalIgnoreCase))
                {
                    return Resources.Notebook;
                }
                else
                {
                    return Resources.PC;
                }
            }
            catch
            {
                // Ігноруємо помилки і повертаємо Unknown
            }

            return Resources.Unknown;
        }
    }
}