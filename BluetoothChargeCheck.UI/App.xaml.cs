// <copyright file = "App.xaml.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using System.Windows;

using DCT.BluetoothChargeCheck.Fonts;

using H.NotifyIcon;

using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private TaskbarIcon appTrayIcon = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        TryAddIconsFont();

        SystemThemeWatcher.Watch(null, WindowBackdropType.Auto);
        this.CreateAppTrayIcon();
    }

    /// H.NotifyIcons tries to convert <see cref="System.Windows.Media.FontFamily"/> to <see cref="System.Drawing.FontFamily"/>
    /// by searching for fonts in system. Thus I forced to install font for Win10.
    private static void TryAddIconsFont()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT
            && Environment.OSVersion.Version.Major < 11)
        {
            int result = FontManager.AddFontResource("Fonts\\SegoeFluentIcons.ttf");
            if (result == 0)
            {
                throw new InvalidOperationException("Failed to install Segoe Fluent Icons");
            }
        }
    }

    private void CreateAppTrayIcon()
    {
        this.appTrayIcon = this.FindResource("AppTrayIcon") as TaskbarIcon
                           ?? throw new InvalidOperationException("Can't load AppTrayIcon resource");

        this.appTrayIcon.ForceCreate();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        this.appTrayIcon.Dispose();
        base.OnExit(e);
    }
}