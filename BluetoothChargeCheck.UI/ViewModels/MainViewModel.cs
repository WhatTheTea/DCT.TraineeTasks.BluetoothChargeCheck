// <copyright file = "MainViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.Immutable;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels.Device;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Views;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Views.AppTrayIcon;
using H.NotifyIcon;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public IBluetoothService BluetoothService { get; init; } = new SampleBluetoothService();

    [ObservableProperty]
    private ImmutableArray<DeviceViewModel> trayIconViewModels = [];

    public MainViewModel()
    {
        var viewModels = this.BluetoothService.Connected.Select(x => new SampleDeviceViewModel(x));
        this.TrayIconViewModels = viewModels.Cast<DeviceViewModel>()
                                            .ToImmutableArray();

        this.BluetoothService.Connected.CollectionChanged += (_, args) =>
        {
            var newItems = args.NewItems;
            if (newItems is not null)
            {
                this.TrayIconViewModels = newItems.Cast<DeviceViewModel>().ToImmutableArray();
            }
        };
    }
}