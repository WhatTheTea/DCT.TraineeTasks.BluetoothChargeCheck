// <copyright file = "MainWindowViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;
using H.NotifyIcon;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public IBluetoothService BluetoothService { get; init; } = new TestBluetoothService();

    [ObservableProperty]
    private ObservableCollection<TrayIconViewModel> trayIconViewModels = [];

    [RelayCommand]
    private void AddTaskbarIcon()
    {
        this.TrayIconViewModels.Add(new TrayIconViewModel());
    }
}