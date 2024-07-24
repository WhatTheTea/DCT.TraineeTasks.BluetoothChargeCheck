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

public partial class MainViewModel : ObservableObject, IViewModelWithIdentity
{
    // https://github.com/HavenDV/H.NotifyIcon/issues/103#issuecomment-1705989614
    // TLDR: GUID-Path pairs of Taskbar Icons are stored in Registry, if executable is moved - TryCreate fails
    public Guid Id { get; } = Guid.NewGuid();

    public DeviceCollectionViewModel DevicesViewModel { get; } = new();

    public MainViewModel()
    {
        
    }
}