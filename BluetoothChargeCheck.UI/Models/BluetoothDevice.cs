// <copyright file = "BluetoothDevice.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using InTheHand.Bluetooth;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

public partial class BluetoothDevice : ObservableObject, IBluetoothDevice, IDisposable
{
    public double Charge => this.DeviceCharge.Value[0];
    [ObservableProperty]
    private string name = string.Empty;
    [ObservableProperty]
    private bool connected;

    private readonly InTheHand.Bluetooth.BluetoothDevice device;
    private GattCharacteristic DeviceCharge { get; set; }

    public BluetoothDevice(InTheHand.Bluetooth.BluetoothDevice device)
    {
        this.device = device;
        this.DeviceCharge = null!;

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
        this.Connected = gatt.IsConnected;
        gatt.Device.GattServerDisconnected += this.OnDeviceDisconnected;
    }

    private void OnDeviceDisconnected(object? o, EventArgs eventArgs) => this.Connected = false;

    private async Task StartListeningBatteryUpdates(RemoteGattServer gatt)
    {
        var batteryService = await gatt.GetPrimaryServiceAsync(GattServiceUuids.Battery);
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (batteryService is not null)
        {
            var battery = await batteryService.GetCharacteristicAsync(BluetoothUuid.GetCharacteristic("battery_level"));
            battery.CharacteristicValueChanged += this.OnBatteryChargeChanged;
        }
    }

    private void OnBatteryChargeChanged(object? _, GattCharacteristicValueChangedEventArgs args)
        => this.OnPropertyChanged(nameof(this.Charge));

    public void Dispose()
    {
        this.device.Gatt.Device.GattServerDisconnected -= this.OnDeviceDisconnected;
        this.DeviceCharge.CharacteristicValueChanged -= this.OnBatteryChargeChanged;
    }
}