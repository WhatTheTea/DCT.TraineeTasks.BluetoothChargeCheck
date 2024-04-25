// <copyright file = "TestBluetoothService.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Services;

public class TestBluetoothService : ObservableObject, IBluetoothService
{
    public ReadOnlyObservableCollection<IBluetoothDevice> Devices { get; private set; }
    public ReadOnlyObservableCollection<IBluetoothDevice> Connected { get; private set; }
}