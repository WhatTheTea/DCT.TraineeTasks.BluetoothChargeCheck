// <copyright file = "TrayIconViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;
using H.NotifyIcon;
using Wpf.Ui.Appearance;
using Wpf.Ui.Markup;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;

public partial class TrayIconViewModel : ObservableObject
{
    private string[] chargeStrings = [
        "\uEBA0", "\uEBA1","\uEBA2","\uEBA3","\uEBA4","\uEBA5",
        "\uEBA6","\uEBA7","\uEBA8","\uEBA9","\uEBAA"];

    [RelayCommand]
    private void ExitApplication() =>
        Application.Current.Shutdown();
    
    public IBluetoothDevice BluetoothDevice;

    [ObservableProperty] private Brush? accent = new BrushConverter().ConvertFrom("White") as Brush;
    [ObservableProperty] private string glyph;

    public TrayIconViewModel()
    {
        this.Glyph = this.chargeStrings[0];
        var testDevice = new TestBluetoothDevice();
        this.BluetoothDevice = testDevice;
        Task.Run(() => testDevice.ChargeCyclingAsync(CancellationToken.None));
        this.BluetoothDevice.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(this.BluetoothDevice.Charge))
            {
                this.Glyph = this.chargeStrings[(int)(this.BluetoothDevice.Charge / 10)];
            }
        };
    }
}