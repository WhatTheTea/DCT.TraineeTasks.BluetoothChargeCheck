// <copyright file = "GattBluetoothDataProvider.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>


using InTheHand.Bluetooth;

using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace DCT.BluetoothChargeCheck.Models;
/// <summary>
/// Provides bluetooth low energy devices data using 32feet library.
/// Provider gets connected devices, battery GATT service from them and reads data.
/// </summary>
internal class GattBluetoothDataProvider
{
    static readonly string ConnectedDeviceSelector = BluetoothLEDevice.GetDeviceSelectorFromConnectionStatus(BluetoothConnectionStatus.Connected);

    public static async IAsyncEnumerable<BluetoothDeviceData> FetchDevicesAsync()
    {
        var foundDevices = await DeviceInformation.FindAllAsync(ConnectedDeviceSelector);
        // Transform found device information to IBluetoothDevice
        foreach (var device in foundDevices)
        {
            var leDevice = await BluetoothLEDevice.FromIdAsync(device.Id);

            if (leDevice is not null)
            {
                yield return await ToBluetoothDevice(leDevice);
            }
        }
    }
    /// <summary>
    /// Converts windows bluetooth LE device to <see cref="BluetoothDeviceData"/> and tries to retrieve charge status from GATT server.
    /// </summary>
    private static async Task<BluetoothDeviceData> ToBluetoothDevice(BluetoothLEDevice device) => new BluetoothDeviceData()
    {
        Name = device.Name,
        Id = device.DeviceId,
        Charge = await GetCharge(device),
        Connected = device.ConnectionStatus == BluetoothConnectionStatus.Connected
    };

    private static async Task<int> GetCharge(InTheHand.Bluetooth.BluetoothDevice device)
    {
        RemoteGattServer gatt = device.Gatt;
        GattService? batteryService = await gatt.GetPrimaryServiceAsync(GattServiceUuids.Battery);
        int charge = 0;

        if (batteryService is not null)
        {
            var characteristicUuid = BluetoothUuid.GetCharacteristic("battery_level");
            var characteristic = await batteryService.GetCharacteristicAsync(characteristicUuid);
            charge = characteristic?.Value[0] ?? 0;
        }

        return charge;
    }
}
