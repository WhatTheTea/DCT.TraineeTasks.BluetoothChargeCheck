// <copyright file = "TaskbarIconManager.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Diagnostics;
using System.Windows;

using DCT.BluetoothChargeCheck.ViewModels;

using H.NotifyIcon;

namespace DCT.BluetoothChargeCheck;

/// <summary>
/// Class to contain and manage lifetime of taskbar icons
/// </summary>
public class TaskbarIconManager
{
    public IReadOnlyDictionary<Guid, TaskbarIcon> Instances => this.instances;

    private readonly Dictionary<Guid, TaskbarIcon> instances = [];
    // App specific
    private readonly DataTemplate deviceIconDataTemplate = Application.Current.FindResource("BatteryTrayIcon") as DataTemplate
                                                     ?? throw new InvalidOperationException("Can't load BatteryTrayIcon resource");
    private readonly DataTemplate appIconTemplate = Application.Current.FindResource("AppTrayIcon") as DataTemplate
                           ?? throw new InvalidOperationException("Can't load AppTrayIcon resource");

    public void CreateFromTemplate(DataTemplate template, IViewModelWithIdentity viewModel)
    {
        if (template.LoadContent() is TaskbarIcon taskbarIcon)
        {
            taskbarIcon.DataContext = viewModel;
            taskbarIcon.Id = viewModel.Id;
            taskbarIcon.ForceCreate();

            this.instances[viewModel.Id] = taskbarIcon;
        }
        else
        {
            Debug.WriteLine("Can't create taskbar icon: template does not contain TaskbarIcon");
        }
    }

    public void Remove(Guid id)
    {
        _ = this.instances.TryGetValue(id, out var iconToRemove);
        if (iconToRemove is not null)
        {
            iconToRemove.Dispose();
            this.instances.Remove(id);
        }
    }

    // App specific
    public void CreateAppIcon(IViewModelWithIdentity viewModel) =>
        this.CreateFromTemplate(this.appIconTemplate, viewModel);

    public void CreateDeviceIcon(IViewModelWithIdentity viewModel) =>
        this.CreateFromTemplate(this.deviceIconDataTemplate, viewModel);
}
