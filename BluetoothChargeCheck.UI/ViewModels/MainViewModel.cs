// <copyright file = "MainViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Messages;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels.Device;
using H.NotifyIcon;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly DataTemplate iconDataTemplate = Application.Current.FindResource("BatteryTrayIcon") as DataTemplate
                                                     ?? throw new InvalidOperationException("Can't load BatteryTrayIcon resource");

    private readonly Dictionary<Guid, TaskbarIcon> taskbarIcons = [];

    [ObservableProperty]
    private ObservableCollection<DeviceViewModel> deviceViewModels = [];

    public MainViewModel()
    {
        this.BluetoothService = new GattBluetoothService();

        this.BluetoothService.Devices.CollectionChanged += this.UpdateDevices;
        WeakReferenceMessenger.Default.Register(this, this.TaskbarIconIsVisibleChangedHandler);
    }

    public Guid Id { get; } = Guid.NewGuid();

    public IBluetoothService BluetoothService { get; set; }
    private void UpdateDevices(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        var newDevices = e.NewItems?.Cast<BluetoothDeviceData>().ToArray() ?? [];
        var oldDevices = e.OldItems?.Cast<BluetoothDeviceData>().ToArray() ?? [];
        var devicesToRemove = oldDevices.Where(x => !this.BluetoothService.Devices.Contains(x)).ToArray() ?? [];
        var viewModelsToRemove = this.DeviceViewModels.Where(x => devicesToRemove.Contains(x.BluetoothDevice)).ToArray() ?? [];

        foreach (var device in newDevices)
        {
            this.DeviceViewModels.Add(new DeviceViewModel(device));
        }

        foreach (var viewModel in viewModelsToRemove)
        {
            this.DeviceViewModels.Remove(viewModel);
            viewModel.Dispose();
        }
    }

    private MessageHandler<object, TrayIconVisibilityChanged> TaskbarIconIsVisibleChangedHandler => (r, m) =>
    {
        DeviceViewModel viewModel = m.Value;
        this.ToggleTaskbarIconFor(viewModel);
    };

    private void ToggleTaskbarIconFor(DeviceViewModel viewModel)
    {
        if (viewModel.IsTrayIconVisible)
        {
            this.taskbarIcons[viewModel.Id] = this.CreateTaskbarIcon(viewModel);
        }
        else
        {
            this.taskbarIcons[viewModel.Id].Dispose();
            this.taskbarIcons.Remove(viewModel.Id);
        }
    }

    private TaskbarIcon CreateTaskbarIcon(DeviceViewModel device)
    {
        var trayIcon = (TaskbarIcon)this.iconDataTemplate.LoadContent();
        trayIcon.DataContext = device;
        trayIcon.ForceCreate(false);

        return trayIcon;
    }
}