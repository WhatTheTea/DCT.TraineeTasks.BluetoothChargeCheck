// <copyright file = "MainWindow.xaml.cs" company = "Digital Cloud Technologies">
// Copyright (c) Digital Cloud Technologies.All rights reserved.
// </copyright>

using Wpf.Ui.Controls;

namespace DCT.TraineeTasks.BluetoothChargeCheck.UI.Views;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        this.Loaded += (_, _) => Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this,
                                                                            WindowBackdropType.Auto);
    }


}
