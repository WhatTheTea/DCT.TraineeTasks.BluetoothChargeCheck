// <copyright file = "MainViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using DCT.BluetoothChargeCheck.Messages;
using DCT.BluetoothChargeCheck.ViewModels.Device;

using H.NotifyIcon;

namespace DCT.BluetoothChargeCheck.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly DataTemplate iconDataTemplate = Application.Current.FindResource("BatteryTrayIcon") as DataTemplate
                                                     ?? throw new InvalidOperationException("Can't load BatteryTrayIcon resource");

    /// <summary>
    /// The key here is Device viewmodel GUID
    /// </summary>
    private readonly Dictionary<Guid, TaskbarIcon> taskbarIcons = [];

    // https://github.com/HavenDV/H.NotifyIcon/issues/103#issuecomment-1705989614
    // TLDR: GUID-Path pairs of Taskbar Icons are stored in Registry, if executable is moved - TryCreate fails
    public Guid Id { get; } = Guid.NewGuid();

    public DeviceCollectionViewModel DevicesViewModel { get; } = new();

    public MainViewModel()
    {
        WeakReferenceMessenger.Default.Register(this, this.TaskbarIconIsVisibleChangedHandler);
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