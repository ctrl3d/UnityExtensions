using System;
using System.Net;

namespace work.ctrl3d
{
    public static class IPAddressExtensions
    {
        public static IPAddress ToIPAddress(this string ipString)
        {
            if (IPAddress.TryParse(ipString, out var result))
            {
                return result;
            }

            return ipString.ToLower() switch
            {
                "localhost" or "local" => IPAddress.Loopback,
                "any" or "0.0.0.0" => IPAddress.Any,
                "broadcast" or "255.255.255.255" => IPAddress.Broadcast,
                _ => throw new ArgumentException($"Invalid IP address format: {ipString}")
            };
        }
        
        public static bool IsLocalAddress(this IPAddress ipAddress)
        {
            return IPAddress.IsLoopback(ipAddress) || 
                   ipAddress.Equals(IPAddress.Any) ||
                   ipAddress.ToString().StartsWith("192.168.") ||
                   ipAddress.ToString().StartsWith("10.") ||
                   ipAddress.ToString().StartsWith("172.");
        }
    }
    
    public enum IPAddressPreset
    {
        Localhost,
        Any,
        Broadcast,
        Custom
    }
    
    public static class IPAddressPresetExtensions
    {
        public static IPAddress ToIPAddress(this IPAddressPreset preset)
        {
            switch (preset)
            {
                case IPAddressPreset.Localhost:
                    return IPAddress.Loopback;
                case IPAddressPreset.Any:
                    return IPAddress.Any;
                case IPAddressPreset.Broadcast:
                    return IPAddress.Broadcast;
                default:
                    return IPAddress.Loopback;
            }
        }
    }
}