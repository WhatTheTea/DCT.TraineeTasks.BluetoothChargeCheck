// <copyright file = "MainViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Messages;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels.Device;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Views;
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

    // Main tray icon

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(this.HideCommand))]
    private bool canExecuteHideCommand = true;

    [RelayCommand(CanExecute = nameof(this.CanExecuteHideCommand))]
    private void Hide()
    {
        Application.Current.MainWindow?.Hide(true);
        this.CanExecuteHideCommand = false;
        this.CanExecuteShowCommand = true;
    }
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(this.ShowCommand))]
    private bool canExecuteShowCommand = true;

    [RelayCommand(CanExecute = nameof(this.CanExecuteShowCommand))]
    private void Show()
    {
        Application.Current.MainWindow = new MainWindow
        {
            DataContext = this
        };
        Application.Current.MainWindow?.Show(true);
        this.CanExecuteHideCommand = true;
        this.CanExecuteShowCommand = false;
    }
}