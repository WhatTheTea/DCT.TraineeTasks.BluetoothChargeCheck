using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;

using DCT.BluetoothChargeCheck.Core.Providers;
using DCT.BluetoothChargeCheck.Core.Services;
using DCT.BluetoothChargeCheck.Models;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing;
using FluentAssertions;

namespace DCT.BluetoothChargeCheck.Tests.Core;

public class BluetoothServiceTests : IDisposable
{

    private const int DeviceDataCount = 5;

    private BluetoothService BluetoothService { get; }

    private IBluetoothDataProvider DataProvider { get; }

    private TestScheduler Scheduler { get; } = new();

    public BluetoothServiceTests()
    {
        var providerMock = new Mock<IBluetoothDataProvider>();
        providerMock.Setup(x => x.FetchDevicesAsync())
                    .Returns(() =>
                        AsyncEnumerable.Repeat(Mock.Of<BluetoothDeviceData>(x =>
                            x.Id == "Valid ID" &&
                            x.Name == "Valid Name" &&
                            x.Charge == 50 &&
                            x.Connected == true),
                        DeviceDataCount));

        this.DataProvider = providerMock.Object;
        this.BluetoothService = new BluetoothService(this.DataProvider);
    }

    [Fact]
    public void ServiceFiresOnMethodCall()
    {
        var data = Array.Empty<BluetoothDeviceData>();

        var subscription = this.BluetoothService.GetDevicesObservable(TimeSpan.FromSeconds(2), this.Scheduler)
            .Subscribe(x => data = x.ToArray());

        this.Scheduler.Schedule(subscription.Dispose);
        this.Scheduler.Start();
        data.Should().HaveCount(DeviceDataCount);
    }

    [Fact]
    public void ServiceFiresOnSpecifiedInterval()
    {
        int observableCount = 0;
 
        var subscription = this.BluetoothService.GetDevicesObservable(TimeSpan.FromSeconds(2), this.Scheduler)
            .Subscribe(x => observableCount++);

        this.Scheduler.Schedule(TimeSpan.FromSeconds(2.1), x => subscription.Dispose());
        this.Scheduler.Start();

        observableCount.Should().Be(2);
    }

    [Theory]
    [InlineData("",null, null, null)]
    [InlineData(null, "", null, null)]
    [InlineData(null, null, -1.0, null)]
    [InlineData(null, null, 101.0, null)]
    [InlineData(null, null, null, false)]
    public void DoNotReturnInvalidData(
        string? id,
        string? name,
        double? charge,
        bool? connected)
    {
        var data = new BluetoothDeviceData
        {
            Id = id ?? "Valid ID",
            Name = name ?? "Valid Name",
            Charge = charge ?? 50,
            Connected = connected ?? true,
        };
        var providerMock = new Mock<IBluetoothDataProvider>();
        providerMock.Setup(x => x.FetchDevicesAsync())
                    .Returns(() => AsyncEnumerable.Repeat(data, DeviceDataCount));
        var service = new BluetoothService(providerMock.Object);
        int returnedDataCount = 0;

        var subscription = service.GetDevicesObservable(TimeSpan.FromSeconds(2), this.Scheduler)
            .Subscribe(x => returnedDataCount = x.Count());
        this.Scheduler.Schedule(subscription.Dispose);
        this.Scheduler.Start();

        returnedDataCount.Should().Be(0);
    }

    public void Dispose() => this.Scheduler.Stop();
}
