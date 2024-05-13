// <copyright file = "BluetoothService.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
using InTheHand.Bluetooth;
using BluetoothDevice = DCT.TraineeTasks.BluetoothChargeCheck.UI.Models.BluetoothDevice;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;

public partial class BluetoothService : ObservableObject, IBluetoothService
{
    [ObservableProperty]
    private ObservableCollection<IBluetoothDevice> devices = [];
    [ObservableProperty]
    private ObservableCollection<IBluetoothDevice> connected = [];

    public BluetoothService()
    {
        this.ScanDevices();
    }

    private void ScanDevices() => App.Current.Dispatcher.BeginInvoke(async () =>
    {
        var devices = await Bluetooth.GetPairedDevicesAsync();
        this.Devices = new ObservableCollection<IBluetoothDevice>(devices.Select(x =>
            new BluetoothDevice(x.Name, 0, x.IsPaired)));
        this.Connected = this.Devices;
    });
}