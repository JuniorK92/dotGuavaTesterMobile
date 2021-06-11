using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace dotGuava.Essential
{
    /// <summary>
    /// This class provides an easy way to get device mac addresses.
    /// </summary>
    public static class Mac
    {
        /// <summary>
        /// Gets device main mac NetworkInterface object.
        /// </summary>
        /// <returns>Main mac NetworkInterface object.</returns>
        public static NetworkInterface GetMainNetworkInterface()
        {
            var macAddresses = NetworkInterface.GetAllNetworkInterfaces();
            if (macAddresses == null || macAddresses.Length <= 0)
            {
                return null;
            }

            return macAddresses.FirstOrDefault(o => o.OperationalStatus == OperationalStatus.Up);
        }
        /// <summary>
        /// Gets device main mac address.
        /// </summary>
        /// <returns>Formatted string showing the mac address.</returns>
        public static string GetMainMacAddress()
        {
            var mainMacAddress = GetMainNetworkInterface().GetPhysicalAddress().ToString();

            string processed = "";

            for (int i = 0; i <= (mainMacAddress.Length - 2); i += 2)
            {
                processed += mainMacAddress.Substring(i, 2);

                if (i != (mainMacAddress.Length - 2))
                {
                    processed += ":";
                }
            }

            return processed;
        }
        /// <summary>
        /// Gets device NetworkInterface objects.
        /// </summary>
        /// <returns>Device NetworkInterface objects.</returns>
        public static IEnumerable<NetworkInterface> GetDeviceNetworkInterfaces()
        {
            var macAddresses = NetworkInterface.GetAllNetworkInterfaces();
            if (macAddresses == null || macAddresses.Length <= 0)
            {
                return null;
            }

            return macAddresses;
        }
        /// <summary>
        /// Gets device macs addresses.
        /// </summary>
        /// <returns>Device mac addresses.</returns>
        public static IEnumerable<string> GetDeviceMacAddresses()
        {
            var processedMacs = new List<string>();

            var macList = GetDeviceNetworkInterfaces().Select(o => o.GetPhysicalAddress().ToString());

            foreach (var x in macList)
            {
                string processed = "";

                for (int i = 0; i <= (x.Length - 2); i += 2)
                {
                    processed += x.Substring(i, 2);

                    if (i != (x.Length - 2))
                        processed += ":";
                }

                processedMacs.Add(processed);
            }

            return processedMacs;
        }
        public static string ParseAddress(string address)
        {
            string processed = "";

            for (int i = 0; i <= (address.Length - 2); i += 2)
            {
                processed += address.Substring(i, 2);

                if (i != (address.Length - 2))
                    processed += ":";
            }

            return processed;
        }
    }
}
