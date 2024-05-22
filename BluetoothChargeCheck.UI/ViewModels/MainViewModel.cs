// <copyright file = "MainViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.ComponentModel;
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
    private readonly DataTemplate iconDataTemplate = Application.Current.FindResource("BatteryTrayIcon") as DataTemplate
                                                     ?? throw new InvalidOperationException("Can't load BatteryTrayIcon resource");

    private readonly Dictionary<Guid, TaskbarIcon> taskbarIcons = [];

    [ObservableProperty]
    private ObservableCollection<DeviceViewModel> deviceViewModels = [];

    public MainViewModel()
    {
        this.BluetoothService = new BluetoothService();

        this.BluetoothService.PropertyChanged += this.UpdateDeviceViewModels;
        WeakReferenceMessenger.Default.Register(this, this.TaskbarIconIsVisibleChangedHandler);
    }

    public Guid Id { get; } = Guid.NewGuid();

    public IBluetoothService BluetoothService { get; set; }

    private MessageHandler<object, TrayIconVisibilityChanged> TaskbarIconIsVisibleChangedHandler => (r, m) =>
    {
        DeviceViewModel viewModel = m.Value;
        this.ToggleTaskbarIconFor(viewModel);
    };

    private void UpdateDeviceViewModels(object? sender, PropertyChangedEventArgs args)
    {
        IEnumerable<DeviceViewModel> viewModels = this.BluetoothService.Devices.Select(x => new DeviceViewModel(x));
        this.DisposeDeviceViewModels();
        this.DeviceViewModels = new ObservableCollection<DeviceViewModel>(viewModels);
    }

    private void DisposeDeviceViewModels()
    {
        foreach (DeviceViewModel device in this.DeviceViewModels)
        {
            device.Dispose();
        }
    }

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