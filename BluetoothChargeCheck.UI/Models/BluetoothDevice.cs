// <copyright file = "BluetoothDevice.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using InTheHand.Bluetooth;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

public partial class BluetoothDevice : ObservableObject, IBluetoothDevice
{
    [ObservableProperty]
    private string name = string.Empty;
    [ObservableProperty]
    private double charge;
    [ObservableProperty]
    private bool connected;

    private readonly InTheHand.Bluetooth.BluetoothDevice device;

    public BluetoothDevice(InTheHand.Bluetooth.BluetoothDevice device)
    {
        this.device = device;
        Task.Run(this.Initialize);
    }

    private async Task Initialize()
    {
        this.Name = this.device.Name;
        var gatt = this.device.Gatt;

        await gatt.ConnectAsync();

        this.StartListeningConnection(gatt);
        await this.StartListeningBatteryUpdates(gatt);
    }

    private void StartListeningConnection(RemoteGattServer gatt)
    {
        this.Connected = this.device.Gatt.IsConnected;
        gatt.Device.GattServerDisconnected += (_, _)
            => this.Connected = false;
    }

    private async Task StartListeningBatteryUpdates(RemoteGattServer gatt)
    {
        var batteryService = await gatt.GetPrimaryServiceAsync(GattServiceUuids.Battery);
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (batteryService is not null)
        {
            var battery = await batteryService.GetCharacteristicAsync(BluetoothUuid.GetCharacteristic("battery_level"));

            battery.CharacteristicValueChanged += (_, args)
                => this.Charge = args.Value?[0] ?? 0;

            this.Charge = battery.Value[0];
        }
    }
}