// <copyright file = "TestTrayIconViewModel.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using DCT.TraineeTasks.BluetoothChargeCheck.UI.Models;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.ViewModels;

public class TestTrayIconViewModel : TrayIconViewModel
{
    private readonly CancellationTokenSource source = new();
    public TestTrayIconViewModel(IBluetoothDevice device) : base(device)
    {
        Task.Run(async () => await (device as TestBluetoothDevice).ChargeCyclingAsync(this.source.Token));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.source.Cancel();
            this.source.Dispose();
        }
        base.Dispose(disposing);
    }
}