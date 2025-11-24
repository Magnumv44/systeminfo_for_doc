using System;
using System.Linq;
using System.Management;

namespace SystemInfo
{
    public static class PcOrLaptop
    {
        public static string GetPcOrLaptop()
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
                            // додано інші типи, які часто означають ноутбук (наприклад 14 — SubNotebook)
                            var laptopTypes = new ushort[] { 8, 9, 10, 14, 30 };
                            if (chassisTypes.Any(t => laptopTypes.Contains(t)))
                                return "Ноутбук";
                            else
                                return "ПК";
                        }
                    }
                }
            }
            catch (ManagementException)
            {
                return "Невідомо";
            }
            catch (UnauthorizedAccessException)
            {
                return "Невідомо";
            }

            return "Невідомо";
        }
    }
}