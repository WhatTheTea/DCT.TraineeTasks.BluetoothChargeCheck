// <copyright file = "BluetoothDevice.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using InTheHand.Bluetooth;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

public partial class BluetoothDevice : ObservableObject, IBluetoothDevice
{
    [ObservableProperty]
    private string name;
    [ObservableProperty]
    private double charge;
    [ObservableProperty]
    private bool connected;

    private InTheHand.Bluetooth.BluetoothDevice device;

    public BluetoothDevice(InTheHand.Bluetooth.BluetoothDevice device)
    {
        Task.Run(async () =>
        {
            this.Name = device.Name;
            this.Connected = device.Gatt.IsConnected;
            var gatt = device.Gatt;
            await gatt.ConnectAsync();

            var batteryService = await gatt.GetPrimaryServiceAsync(GattServiceUuids.Battery);

            var battery = await batteryService?.GetCharacteristicAsync(BluetoothUuid.GetCharacteristic("battery_level"));

            this.Charge = battery?.Value[0] ?? 0;

        });
    }
}