﻿// <copyright file = "TrayIconViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Appearance;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;

public partial class TrayIconViewModel : ObservableObject
{
    [RelayCommand]
    private void ExitApplication() =>
        Application.Current.Shutdown();

    [ObservableProperty] private ImageSource iconSource;
    [ObservableProperty] private int charge = 0;

    public TrayIconViewModel()
    {
        this.iconSource = this.ChargeToIcon(this.charge);
    }

    private ImageSource FindIcon(int iconNumber)
    {
        if (iconNumber <= 10 && iconNumber >= 0)
        {
            var appTheme = Enum.GetName(ApplicationThemeManager.GetAppTheme());
            return new BitmapImage(new Uri(
                $"pack://application:,,,/BluetoothChargeCheck.UI;component/Resources/Images/Battery-{appTheme}/Battery{iconNumber}.ico"));
        }

        throw new ArgumentOutOfRangeException(nameof(iconNumber));
    }

    private ImageSource ChargeToIcon(int charge) => this.FindIcon(charge / 10);
}