// <copyright file = "TrayIconViewModel.cs" company = "Digital Cloud Technologies">
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
    /// <summary>
    /// TODO: Remove
    /// </summary>
    [RelayCommand]
    private void AddCharge() => this.Charge += 10;

    /// <summary>
    /// TODO: Remove
    /// </summary>
    [RelayCommand]
    private void DecreaseCharge() => this.Charge -= 10;

    public ImageSource IconSource => this.ChargeToIcon(this.Charge);

    public int Charge
    {
        get => this.charge;
        set
        {
            if (value >= 0 && value <= 100)
            {
                this.SetProperty(ref this.charge, value);
                this.OnPropertyChanged(nameof(this.IconSource));
            }
        }
    }

    private int charge = 0;

    public TrayIconViewModel()
    {
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