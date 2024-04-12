using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace whottadota;

public partial class ExitWindow : Window
{
    public ExitWindow()
    {
        InitializeComponent();
    }

    private void NoBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    private void YesBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}