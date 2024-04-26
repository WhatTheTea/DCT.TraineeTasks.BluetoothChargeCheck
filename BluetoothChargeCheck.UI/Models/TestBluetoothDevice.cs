// <copyright file = "TestBluetoothDevice.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

internal partial class TestBluetoothDevice : ObservableObject, IBluetoothDevice
{
    [ObservableProperty] private string name = "Bluetooth Device";
    [ObservableProperty] private double charge;
    [ObservableProperty] private bool connected;
    [ObservableProperty] private bool isInTray;

    public async Task ChargeCyclingAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            NextCharge();

            await Task.Delay(TimeSpan.FromSeconds(1), token);
        }
    }

    internal void NextCharge()
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