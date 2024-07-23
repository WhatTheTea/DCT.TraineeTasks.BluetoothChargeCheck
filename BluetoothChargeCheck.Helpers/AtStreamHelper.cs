// <copyright file = "AtStreamHelper.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Diagnostics;
using System.IO;

namespace DCT.BluetoothChargeCheck.Helpers;
internal static class AtStreamHelper
{
    /// <summary>
    /// Writes a correctly formatted response to the Handsfree device.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="command"></param>
    public static void WriteString(this Stream stream, string command)
    {
        byte[] converted = System.Text.Encoding.ASCII.GetBytes($"\r\n{command}\r\n");
        stream.Write(converted, 0, converted.Length);
        stream.Flush();
    }

    public static int ParseAppleBatteryPercentage(string iphoneAccev)
    {
        var parts = iphoneAccev.Split('=')[1].Split(',');
        for (int i = 0; i < int.Parse(parts[0]); i++)
        {
            var key = int.Parse(parts[i * 2 + 1]);
            var value = int.Parse(parts[i * 2 + 2]);
            switch (key)
            {
                case 1:
                    return AppleBatteryLevelToPercentage(value);
            }
        }
        return 0;
    }

    /// <summary>
    /// Convert battery level returned from +IPHONEACCEV to a percentage
    /// </summary>
    /// <param name="rawValue"></param>
    /// <returns></returns>
    public static int AppleBatteryLevelToPercentage(int rawValue)
    {
        // rawValue is integer from 0 to 9. Round these up to the next full 10%
        return (rawValue + 1) * 10;
    }
}
