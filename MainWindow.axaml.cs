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

    private void TeamsBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        TeamWindow teamWindow = new TeamWindow();
        this.Hide();
        teamWindow.Show();
    }

    private void NewsBTN_OnClick(object? sender, RoutedEventArgs e)
    {
     
    }
}