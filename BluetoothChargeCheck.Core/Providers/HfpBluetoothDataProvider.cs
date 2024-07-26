// <copyright file = "HfpBluetoothDataProvider.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Diagnostics;

using DCT.BluetoothChargeCheck.Models;
using DCT.BluetoothChargeCheck.Core.Extensions;

using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using System.Runtime.InteropServices;

namespace DCT.BluetoothChargeCheck.Core.Providers;
// NOTE: https://stackoverflow.com/questions/17067971/invoking-powershell-cmdlets-from-c-sharp

/// <summary>
/// Provides bluetooth handsfree device data using RFCOMM and AT commands by retrieving open sockets in Windows.
/// </summary>
public class HfpBluetoothDataProvider : IBluetoothDataProvider
{
    private const int HandsFreeShortServiceId = 0x111e;
    private static readonly string ConnectedDeviceSelector = BluetoothDevice.GetDeviceSelectorFromPairingState(true);

    private static async IAsyncEnumerable<BluetoothDeviceData> fetchDevicesAsync()
    {
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
            when (ex is COMException || ex is IOException)
            {
                Debug.WriteLine($"Communication went wrong with {device.Name}");
            }

            var bluetoothData = new BluetoothDeviceData()
            {
                Id = bluetoothDevice.DeviceId,
                Name = bluetoothDevice.Name,
                Connected = bluetoothDevice.ConnectionStatus == BluetoothConnectionStatus.Connected,
                Charge = charge,
            };

            // NOTE: Maybe extract validation rules
            if (bluetoothData.Charge > 0)
            {
                yield return bluetoothData;
            }
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

    public IEnumerable<BluetoothDeviceData> FetchDevices() =>
        fetchDevicesAsync().ToArrayAsync()
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    public IAsyncEnumerable<BluetoothDeviceData> FetchDevicesAsync() => fetchDevicesAsync();
}
