// <copyright file = "TrayIconViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;

public partial class TrayIconViewModel : ObservableObject
{
    [RelayCommand]
    private void ExitApplication() =>
        Application.Current.Shutdown();
}