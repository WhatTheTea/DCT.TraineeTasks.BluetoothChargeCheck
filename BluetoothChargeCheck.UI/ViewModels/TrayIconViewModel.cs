// <copyright file = "TrayIconViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
using Wpf.Ui.Appearance;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;

public partial class TrayIconViewModel : ObservableObject
{
    [RelayCommand]
    private void ExitApplication() =>
        Application.Current.Shutdown();

    public ImageSource IconSource => this.ChargeToIcon(this.BluetoothDevice.Charge);
    public IBluetoothDevice BluetoothDevice;
    public TrayIconViewModel()
    {
        var testDevice = new TestBluetoothDevice();
        this.BluetoothDevice = testDevice;
        Task.Run(() => testDevice.ChargeCyclingAsync(CancellationToken.None));
        this.BluetoothDevice.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(this.BluetoothDevice.Charge))
            {
                this.OnPropertyChanged(nameof(this.IconSource));
            }
        };
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

    private ImageSource ChargeToIcon(double charge) => this.FindIcon((int)(charge / 10));
}