// <copyright file = "MainViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;

using DCT.BluetoothChargeCheck.ViewModels.Device;

namespace DCT.BluetoothChargeCheck.ViewModels;

public partial class MainViewModel : ObservableObject, IViewModelWithIdentity
{
    // https://github.com/HavenDV/H.NotifyIcon/issues/103#issuecomment-1705989614
    // TLDR: GUID-Path pairs of Taskbar Icons are stored in Registry, if executable is moved - TryCreate fails
    public Guid Id { get; } = Guid.NewGuid();

    public DeviceCollectionViewModel DevicesViewModel { get; } = new();
}