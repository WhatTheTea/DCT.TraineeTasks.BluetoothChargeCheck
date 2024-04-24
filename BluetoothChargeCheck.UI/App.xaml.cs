using System.Drawing;
using System.Windows;
using System.Windows.Media;
using Hardcodet.Wpf.TaskbarNotification;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private TaskbarIcon trayIcon;
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        this.trayIcon = this.FindResource("TrayIconResource") as TaskbarIcon
            ?? throw new InvalidOperationException("Can't find resource");
    }

    protected override void OnExit(ExitEventArgs e)
    {
        this.trayIcon.Dispose();
        base.OnExit(e);
    }
}

