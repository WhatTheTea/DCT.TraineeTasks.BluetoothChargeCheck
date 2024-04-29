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
    private void ToggleTaskbarIcon(IBluetoothDevice device)
    {
        var viewModel = this.TrayIconViewModels
            .FirstOrDefault(x => x.BluetoothDevice == device);

        if (viewModel is not null)
        {
            this.RemoveDevice(viewModel);
        }
        else
        {
            this.TrayIconViewModels.Add(new TestTrayIconViewModel(device));
        }
    }

    private void RemoveDevice(TrayIconViewModel viewModel)
    {
        this.TrayIconViewModels.Remove(viewModel);
        viewModel.Dispose();
    }
}