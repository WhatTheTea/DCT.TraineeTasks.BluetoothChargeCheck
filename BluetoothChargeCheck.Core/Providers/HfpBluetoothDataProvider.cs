// <copyright file = "HfpBluetoothDataProvider.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Diagnostics;

using DCT.BluetoothChargeCheck.Models;
using DCT.BluetoothChargeCheck.Helpers;

using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;

namespace DCT.BluetoothChargeCheck.Core.Providers;
/// <summary>
/// Provides bluetooth handsfree device data using RFCOMM and AT commands by retrieving open sockets in Windows.
/// </summary>
internal class HfpBluetoothDataProvider
{
    private const int HandsFreeShortServiceId = 0x111e;
    private static readonly string ConnectedDeviceSelector = BluetoothDevice.GetDeviceSelectorFromConnectionStatus(BluetoothConnectionStatus.Connected);

    public static async IAsyncEnumerable<BluetoothDeviceData> FetchDevicesAsync()
    {
        var devices = await DeviceInformation.FindAllAsync(ConnectedDeviceSelector);
        foreach (var device in devices)
        {
            var bluetoothDevice = await BluetoothDevice.FromIdAsync(device.Id);
            yield return new BluetoothDeviceData()
            {
                Id = bluetoothDevice.DeviceId,
                Name = bluetoothDevice.Name,
                Connected = device.IsEnabled,
                Charge = await GetChargeFor(bluetoothDevice)
            };
        }
    }

    /// <summary>
    /// Retrieves service from Windows Runtime API in order to get streams for reading/writing AT commands. <br/>
    /// This method supports HFP only and expects AT+IPHONEACCEV command in order to read charge.
    /// </summary>
    private static async Task<double> GetChargeFor(BluetoothDevice device)
    {
        double charge = 0;
        StreamSocket socket = new();
        var deviceServices = await device.GetRfcommServicesAsync();
        var handsfreeService = deviceServices?.Services
            .FirstOrDefault(x => x.ServiceId.AsShortId() == HandsFreeShortServiceId);

        if (handsfreeService is null)
        {
            Debug.WriteLine($"Can't retrieve handsfree service from {device.Name}");
        }
        else
        {
            await socket.ConnectAsync(handsfreeService.ConnectionHostName, handsfreeService.ConnectionServiceName);

            using var inputStream = socket.InputStream.AsStreamForRead();
            using var outputStream = socket.OutputStream.AsStreamForWrite();

            // data may not present yet - retry until it is present
            bool isChargeReceived = false;
            for (int i = 0; i < 100 && !isChargeReceived; i++)
            {
                string[] commands = GetCommandsFromStream(inputStream);

                if (commands.Length > 0)
                {
                    foreach (var command in commands)
                    {
                        // parse charge if given any, else - send "ok"
                        if (command.StartsWith("AT+IPHONEACCEV"))
                        {
                            charge = AtStreamHelper.ParseAppleBatteryPercentage(command);
                            isChargeReceived = true;
                        }

                        outputStream.WriteString("OK");
                    }
                }
            }
        }
        return charge;
    }

    private static string[] GetCommandsFromStream(Stream inputStream)
    {
        var buffer = new byte[1024];
        var readBytes = inputStream.Read(buffer, 0, 80);
        var commandsRaw = System.Text.Encoding.ASCII.GetString(buffer, 0, readBytes);

        return commandsRaw.Split('\r')
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();
    }
}
