// <copyright file = "HfpBluetoothDataProvider.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Diagnostics;
using System.Runtime.InteropServices;

using DCT.BluetoothChargeCheck.Core.Extensions;
using DCT.BluetoothChargeCheck.Models;

using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;

namespace DCT.BluetoothChargeCheck.Core.Providers;
/// <summary>
/// Provides bluetooth handsfree device data using RFCOMM and AT commands by retrieving open sockets in Windows.
/// </summary>
public sealed class HfpBluetoothDataProvider : BluetoothDataProviderBase
{
    private const int HandsFreeShortServiceId = 0x111e;
    private static readonly string ConnectedDeviceSelector = BluetoothDevice.GetDeviceSelectorFromPairingState(true);

    private static async IAsyncEnumerable<BluetoothDeviceData> fetchDevicesAsync()
    {
        if (!await CheckBluetoothAvailability())
        {
            yield break;
        }

        var devices = await DeviceInformation.FindAllAsync(ConnectedDeviceSelector);
        foreach (var device in devices)
        {
            var bluetoothDevice = await BluetoothDevice.FromIdAsync(device.Id);
            double charge = 0;

            try
            {
                charge = await GetChargeFor(bluetoothDevice);
            }
            catch (Exception ex)
            when (ex is COMException or IOException)
            {
                Debug.WriteLine($"Communication went wrong with {device.Name}");
            }

            var bluetoothData = new BluetoothDeviceData
            {
                Id = bluetoothDevice.DeviceId,
                Name = bluetoothDevice.Name,
                Connected = bluetoothDevice.ConnectionStatus == BluetoothConnectionStatus.Connected,
                Charge = charge
            };

            yield return bluetoothData;
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

            await using var inputStream = socket.InputStream.AsStreamForRead();
            await using var outputStream = socket.OutputStream.AsStreamForWrite();

            // data may not present yet - retry until it is present
            bool isChargeReceived = false;
            for (int i = 0; i < 100 && !isChargeReceived; i++)
            {
                string[] commands = inputStream.GetAtCommands();

                if (commands.Length > 0)
                {
                    foreach (var command in commands)
                    {
                        // parse charge if given any, else - send "ok"
                        if (command.StartsWith("AT+IPHONEACCEV"))
                        {
                            charge = command.ParseAppleBatteryPercentage();
                            isChargeReceived = true;
                        }

                        outputStream.WriteAtResponse("OK");
                    }
                }
            }
        }
        return charge;
    }

    public override IEnumerable<BluetoothDeviceData> FetchDevices() =>
        fetchDevicesAsync().ToArrayAsync()
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

    public override IAsyncEnumerable<BluetoothDeviceData> FetchDevicesAsync() => fetchDevicesAsync();
}
