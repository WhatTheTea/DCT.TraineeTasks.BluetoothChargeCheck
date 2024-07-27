// <copyright file = "DeviceTests.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using DCT.BluetoothChargeCheck.Models;
using DCT.BluetoothChargeCheck.ViewModels.Device;

using FluentAssertions;

using Moq;

namespace DCT.BluetoothChargeCheck.Tests.ViewModels;
public class DeviceTests
{
    private DeviceViewModel ViewModel { get; } = new DeviceViewModel();

    [Fact]
    public void UpdatesOnNewData()
    {
        BluetoothDeviceData dummyData = Mock.Of<BluetoothDeviceData>();
        var viewModelMonitor = this.ViewModel.Monitor();

        this.ViewModel.BluetoothDevice = dummyData;

        viewModelMonitor.OccurredEvents.Should().NotBeEmpty();
    }

    [Fact]
    public void GlyphsAreSyncedWithCharge()
    {
        string[] expectedGlyphs =
        [
        "\uEBA0", "\uEBA1", "\uEBA2", "\uEBA3", "\uEBA4", "\uEBA5",
        "\uEBA6", "\uEBA7", "\uEBA8", "\uEBA9", "\uEBAA"
        ];

        var realGlyphs = Enumerable.Range(0, 100+1)
            .Where(x => x % 10 == 0)
            .Select(x =>
            {
                this.ViewModel.BluetoothDevice = this.ViewModel.BluetoothDevice with { Charge = x };
                return this.ViewModel.Glyph;
            })
            .ToArray();

        realGlyphs.Should().BeEquivalentTo(expectedGlyphs); 
    }
}
