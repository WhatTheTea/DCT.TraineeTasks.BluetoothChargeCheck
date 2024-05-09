﻿// <copyright file = "MainViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Messages;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels.Device;
using H.NotifyIcon;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public IBluetoothService BluetoothService { get; } = new SampleBluetoothService();

    [ObservableProperty]
    private ObservableCollection<DeviceViewModel> deviceViewModels = [];

    private Dictionary<Guid, TaskbarIcon?> taskbarIcons = [];

    public MainViewModel()
    {
        var viewModels = this.BluetoothService.Connected.Select(x => new SampleDeviceViewModel(x));
        this.DeviceViewModels = new ObservableCollection<DeviceViewModel>(viewModels);

        WeakReferenceMessenger.Default.Register<TrayIconVisibilityChanged>(this, (r, m) =>
        {
            var viewModel = m.Value;
            if (viewModel.IsTrayIconVisible)
            {
                this.taskbarIcons[viewModel.Id] = this.CreateTaskbarIcon(viewModel);
            }
            else
            {
                this.taskbarIcons[viewModel.Id]!.Dispose();
                this.taskbarIcons.Remove(viewModel.Id);
                
            }
        });
    }

    private readonly DataTemplate iconDataTemplate = (Application.Current.Resources["BatteryTrayIcon"] as DataTemplate)!;

    private TaskbarIcon CreateTaskbarIcon(DeviceViewModel device)
    {
        var trayIcon = this.iconDataTemplate.LoadContent() as TaskbarIcon ??
                        throw new InvalidOperationException("Can't load tray icon from template");
        trayIcon.DataContext = device;
        trayIcon.ForceCreate(false);
        return trayIcon;
    }
}