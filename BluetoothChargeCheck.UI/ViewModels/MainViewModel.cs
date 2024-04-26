// <copyright file = "MainViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public IBluetoothService BluetoothService { get; init; } = new TestBluetoothService();

    [ObservableProperty]
    private ObservableCollection<TrayIconViewModel> trayIconViewModels = [];

    [RelayCommand]
    private void TaskbarIcon(IBluetoothDevice device)
    {
        if (device.IsInTray)
        {
            var viewModel = this.TrayIconViewModels.First(x => x.BluetoothDevice == device);
            this.TrayIconViewModels.Remove(viewModel);
            viewModel.Dispose();
            device.IsInTray = false;
        }
        else
        {
            this.TrayIconViewModels.Add(new TestTrayIconViewModel(device));
            // feels dumb
            device.IsInTray = true;
        }
    }
}