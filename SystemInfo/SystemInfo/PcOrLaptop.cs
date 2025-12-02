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
        #region GetPcOrLaptop
        /// <summary>
        /// Determines whether the current device is identified as a PC or a laptop based on the operating system
        /// platform.
        /// </summary>
        /// <remarks>The method uses the operating system platform to infer the device type. The returned
        /// value may vary depending on platform-specific detection logic and may not be accurate for all hardware
        /// configurations.</remarks>
        /// <returns>A string indicating whether the device is a PC or a laptop. Returns a platform-specific value, or "Unknown"
        /// if the platform is not recognized.</returns>
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
        #endregion

        #region GetPcOrLaptopWindows
        /// <summary>
        /// Determines whether the current Windows device is identified as a laptop or a desktop PC based on system
        /// enclosure information.
        /// </summary>
        /// <remarks>This method queries the system's enclosure information using Windows Management
        /// Instrumentation (WMI) to classify the device. If access to WMI is denied or a management error occurs, a
        /// corresponding error message is returned. The method is intended for use on Windows platforms and may not
        /// provide meaningful results on other operating systems.</remarks>
        /// <returns>A string indicating the device type: a value representing "Notebook" if the device is a laptop, "PC" if it
        /// is a desktop, or an appropriate error message if the device type cannot be determined.</returns>
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
        #endregion

        #region GetPcOrLaptopLinux
        /// <summary>
        /// Determines whether the current Linux device is identified as a laptop or a desktop PC based on system
        /// chassis information.
        /// </summary>
        /// <remarks>This method reads the chassis type from the Linux system file
        /// "/sys/class/dmi/id/chassis_type". If the file is unavailable or an error occurs during reading, the method
        /// returns a value representing "Unknown". The returned strings are typically sourced from application
        /// resources and may vary depending on localization.</remarks>
        /// <returns>A string indicating the device type: a value representing "Notebook" if the chassis type matches known
        /// laptop identifiers, "PC" if it does not, or "Unknown" if the device type cannot be determined.</returns>
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
        #endregion

        #region GetPcOrLaptopMacOS
        /// <summary>
        /// Determines whether the current macOS device is a notebook (MacBook) or a desktop (PC) based on system
        /// information.
        /// </summary>
        /// <remarks>This method inspects the hardware model using the 'sysctl hw.model' command. If the
        /// model name contains 'MacBook', the device is considered a notebook; otherwise, it is considered a PC. If an
        /// error occurs while retrieving system information, the method returns an unknown device type.</remarks>
        /// <returns>A string indicating the device type: notebook, PC, or unknown if the type cannot be determined.</returns>
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
        #endregion
    }
}