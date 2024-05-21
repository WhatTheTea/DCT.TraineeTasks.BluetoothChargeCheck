// <copyright file = "SampleBluetoothDevice.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

internal partial class SampleBluetoothDevice : ObservableObject, IBluetoothDevice
{
    [ObservableProperty]
    private double charge;

    [ObservableProperty]
    private bool connected;

    [ObservableProperty]
    private string name = "Bluetooth Device";

    public async Task ChargeCyclingAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            this.NextCharge();

            await Task.Delay(TimeSpan.FromSeconds(1), token);
        }
    }

    private void NextCharge()
    {
        if (this.Charge < 100)
        {
            this.Charge += 10;
        }
        else
        {
            this.Charge = 0;
        }
    }
}