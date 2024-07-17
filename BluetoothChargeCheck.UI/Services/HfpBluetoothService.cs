// <copyright file = "BluetoothService.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;

public partial class HfpBluetoothService : ObservableObject, IBluetoothService
{
    [ObservableProperty]
    private ObservableCollection<IBluetoothDevice> devices = [];

    public HfpBluetoothService()
    {
        
    }

    private async Task ScanDevices()
    {
        DeviceInformationCollection PairedBluetoothDevices =
            await DeviceInformation.FindAllAsync(Windows.Devices.Bluetooth.BluetoothDevice.GetDeviceSelectorFromPairingState(true));
    }

}