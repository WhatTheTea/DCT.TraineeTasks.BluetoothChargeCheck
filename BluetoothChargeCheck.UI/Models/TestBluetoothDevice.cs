// <copyright file = "TestBluetoothDevice.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

internal partial class TestBluetoothDevice : ObservableObject, IBluetoothDevice
{
    [ObservableProperty] private string name = string.Empty;
    [ObservableProperty] private double charge;
    [ObservableProperty] private bool connected;

    public async Task ChargeCyclingAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (this.Charge < 100)
            {
                this.Charge += 10;
            }
            else
            {
                this.Charge = 0;
            }

            await Task.Delay(TimeSpan.FromSeconds(0.5), token);
        }
    }
}