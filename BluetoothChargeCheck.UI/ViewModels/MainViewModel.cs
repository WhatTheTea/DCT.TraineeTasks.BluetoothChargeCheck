// <copyright file = "MainViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Messages;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Views;
using H.NotifyIcon;

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

    [RelayCommand]
    private void Hide() =>
        Application.Current.MainWindow?.Hide(true);

    [RelayCommand]
    private void Show()
    {
        Application.Current.MainWindow = new MainWindow
        {
            DataContext = this
        };
        Application.Current.MainWindow?.Show(true);
    }

    public MainViewModel()
    {
        WeakReferenceMessenger.Default.Register<RemoveTrayIconMessage>(this,
            (r,m) => this.RemoveDevice(m.Value));
    }

    private void RemoveDevice(TrayIconViewModel viewModel)
    {
        this.TrayIconViewModels.Remove(viewModel);
        viewModel.Dispose();
    }
}