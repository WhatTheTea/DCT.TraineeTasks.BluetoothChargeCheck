// <copyright file = "HfpDataExtensions.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

namespace DCT.BluetoothChargeCheck.Core.Extensions;
internal static class HfpDataExtensions
{
    /// <summary>
    /// Writes a correctly formatted response to the Handsfree device.
    /// </summary>
    public static void WriteAtResponse(this Stream stream, string command)
    {
        byte[] converted = System.Text.Encoding.ASCII.GetBytes($"\r\n{command}\r\n");
        stream.Write(converted, 0, converted.Length);
        stream.Flush();
    }

    /// <summary>
    /// Returns array of commands from stream
    /// </summary>
    public static string[] GetAtCommands(this Stream inputStream)
    {
        var buffer = new byte[1024];
        var readBytes = inputStream.Read(buffer, 0, 80);
        var commandsRaw = System.Text.Encoding.ASCII.GetString(buffer, 0, readBytes);

        return commandsRaw.Split('\r')
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();
    }

    public static int ParseAppleBatteryPercentage(this string iphoneAccev)
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
    /// <param name="rawValue">integer from 0 to 9</param>
    private static int AppleBatteryLevelToPercentage(int rawValue)
        => (rawValue + 1) * 10;
}
