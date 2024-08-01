// <copyright file = "DeviceCollectionViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using DCT.BluetoothChargeCheck.Core;
using DCT.BluetoothChargeCheck.Models;

namespace DCT.BluetoothChargeCheck.ViewModels.Device;
/// <summary>
/// Class to contain all information about bluetooth devices and its view models<br/>
/// On instantiating starts fething devices on dispatcher.
/// </summary>
public class DeviceCollectionViewModel
{
    // Collection of devices used for binding
    public ObservableCollection<DeviceViewModel> Devices { get; } = [];

    // The key here is bluetooth device ID
    // It is used to keep device viewmodels alive until there is no data about device
    private readonly Dictionary<string, DeviceViewModel> viewModels = [];

    private readonly BluetoothService deviceService;

    public DeviceCollectionViewModel(BluetoothService bluetoothService)
    {
        this.deviceService = bluetoothService;

        this.deviceService.GetDevicesObservable(TimeSpan.FromSeconds(60))
            .ObserveOn(SynchronizationContext.Current ?? new SynchronizationContext())
            .Subscribe(this.UpdateDevices);
    }

    private void UpdateDevices(IEnumerable<BluetoothDeviceData> newDevices)
    {
        // Update or add devices
        foreach (var device in newDevices)
        {
            if (this.viewModels.TryGetValue(device.Id, out var viewModel))
            {
                if (viewModel.BluetoothDevice != device)
                {
                    viewModel.BluetoothDevice = device;
                }
            }
            else
            {
                var newViewModel = new DeviceViewModel(device);
                this.Devices.Add(newViewModel);
                this.viewModels.Add(device.Id, newViewModel);
            }
        }

        // Remove disconnected
        var devicesToRemove = this.Devices.Select(x => x.BluetoothDevice).Except(newDevices).ToArray();
        foreach (var device in devicesToRemove)
        {
            var viewModelToRemove = this.viewModels[device.Id];
            // Remove taskbar icon
            viewModelToRemove.Dispose();
            // Remove device from collections
            this.Devices.Remove(viewModelToRemove);
            this.viewModels.Remove(device.Id);
        }
    }
}
