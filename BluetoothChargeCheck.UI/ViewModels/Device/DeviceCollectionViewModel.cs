// <copyright file = "DeviceCollectionViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels.Device;
/// <summary>
/// Class to contain all information about bluetooth devices and its view models<br/>
/// On instantiating starts fething devices on dispatcher.
/// </summary>
public class DeviceCollectionViewModel
{
    public ObservableCollection<DeviceViewModel> Devices { get; } = [];
    /// <summary>
    /// The key here is bluetooth device ID
    /// </summary>
    private readonly Dictionary<string, DeviceViewModel> viewModels = [];
    private readonly BluetoothService deviceService;

    public DeviceCollectionViewModel()
    {
        var deviceFetcherComposite = () => GattBluetoothDataProvider.FetchDevicesAsync()
            .Concat(HfpBluetoothDataProvider.FetchDevicesAsync());

        this.deviceService = new BluetoothService(deviceFetcherComposite);

        App.Current.Dispatcher.BeginInvoke(this.FetchDevices, System.Windows.Threading.DispatcherPriority.Background);
    }

    private async Task FetchDevices()
    {
        // never ends, async enumerable returns new lists of devices in specified interval
        await foreach (IEnumerable<BluetoothDeviceData> newDevices in this.deviceService.GetDevicesAsync())
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
                var viewModelToRemove = viewModels[device.Id];
                // Remove taskbar icon
                viewModelToRemove.Dispose();
                // Remove device from collections
                this.Devices.Remove(viewModelToRemove);
                this.viewModels.Remove(device.Id);
            }
        }
        // unreachable!
    }
}
