// <copyright file = "MainWindowViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;
using H.NotifyIcon;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public IBluetoothService BluetoothService { get; init; } = new TestBluetoothService();
    private List<TaskbarIcon> iconInstances = [];

    [RelayCommand]
    private void AddTaskbarIcon()
    {
        var app = Application.Current;
        var taskbarIcon = (TaskbarIcon)app.FindResource("TrayIconResource")!;
        taskbarIcon.ForceCreate(false);
        this.iconInstances.Add(taskbarIcon);
    }
}