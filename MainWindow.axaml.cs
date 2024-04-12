using Avalonia.Controls;
using Avalonia.Interactivity;

namespace whottadota;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }


    private void TournamentsBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        TournamentWindow tournamentWindow = new TournamentWindow();
        tournamentWindow.Show();
        this.Close();
    }
    

    private void ExitBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        ExitWindow exitWindow = new ExitWindow();
        exitWindow.Show();
        this.Close();
    }

    private void SettingsBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        SettingsWindow teamWindow = new SettingsWindow();
        this.Hide();
        teamWindow.Show();
    }
}