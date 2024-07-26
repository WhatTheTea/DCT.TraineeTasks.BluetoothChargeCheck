// <copyright file = "PowershellBluetoothDataProvider.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using DCT.BluetoothChargeCheck.Models;

using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace DCT.BluetoothChargeCheck.Core.Providers;

public enum BluetoothKind
{
    Classic,
    LowEnergy
}

/// <summary>
/// Retrieves data about bluetooth devices from Windows by powershell using PnpDevice module
/// </summary>
public class PowershellBluetoothDataProvider : IBluetoothDataProvider
{
    private const string BluetoothClassicHardwareId = "BTHENUM";
    private const string BluetoothLeHardwareId = "BTHLE";
    /// <summary>
    /// Should be formatted to use <see cref="BluetoothClassicHardwareId"/> or <see cref="BluetoothLeHardwareId"/>
    /// </summary>
    private string DeviceDataScript =>
        "Get-PnpDevice -Class Bluetooth " +
        @$"| where HardwareID -like '{this.GetKindAsString()}\Dev*' " + // Bluetooth tech
        "| Get-PnpDeviceProperty -KeyName " +
                "DEVPKEY_Device_InstanceId, " +                  // Id
                "DEVPKEY_Device_FriendlyName, " +                // Name
                "'{104EA319-6EE2-4701-BD47-8DDBF425BBE5} 2', " + // Charge
                "'{83DA6326-97A6-4088-9453-A1923F573B29} 15' " + // Connected
        "| group -Property InstanceId";


    private InitialSessionState sessionState = InitialSessionState.CreateDefault();

    public BluetoothKind BluetoothKind { get; set; }

    public PowershellBluetoothDataProvider()
    {
        this.sessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Bypass;
    }

    public PowershellBluetoothDataProvider(BluetoothKind bluetoothKind) : this()
    {
        this.BluetoothKind = bluetoothKind;
    }
    /// <summary>
    /// Starts shell script to return data about bluetooth devices <br/>
    /// Method fetches Bluetooth Classic devices by default
    /// </summary>
    public async IAsyncEnumerable<BluetoothDeviceData> FetchDevicesAsync()
    {
        using var shell = PowerShell.Create(this.sessionState);
        shell.AddScript(this.DeviceDataScript);

        var objects = await shell.InvokeAsync();

        if (shell.HadErrors)
        {
            throw new InvalidPowerShellStateException("Script had errors:", shell.Streams.Error.FirstOrDefault()?.Exception);
        }

        foreach (PSObject? obj in objects)
        {
            var group = obj?.Members["Group"]?.Value as Collection<PSObject?> ?? [];
            var data = new BluetoothDeviceData()
            {
                Id = GetPowershellValue(group[0]) as string ?? string.Empty,
                Name = GetPowershellValue(group[1]) as string ?? string.Empty,
                Charge = GetPowershellValue(group[2]) as byte? ?? 0,
                Connected = GetPowershellValue(group[3]) as bool? ?? false,
            };
            // TODO: Move to validation
            if (data.Connected && data.Charge > 0)
            {
                yield return data;
            }
        }
    }

    public IEnumerable<BluetoothDeviceData> FetchDevices() =>
        this.FetchDevicesAsync()
            .ToArrayAsync()
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

    private string GetKindAsString() => this.BluetoothKind switch
    {
        BluetoothKind.Classic => BluetoothClassicHardwareId,
        BluetoothKind.LowEnergy => BluetoothLeHardwareId,
        _ => throw new ArgumentOutOfRangeException(),
    };

    private static object? GetPowershellValue(PSObject? value) =>
        value?.Members["Data"]?.Value;

}
