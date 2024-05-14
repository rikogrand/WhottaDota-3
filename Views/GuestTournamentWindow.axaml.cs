using System.Linq;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;

namespace whottadota;

public partial class GuestTournamentWindow : Window
{
    private readonly GridData _gridData;

    // private string _con = "server=10.10.1.24;database=pro1_11;user=user_01;password=user01pro";
    private string _con = "server=localhost;database=cursach;user=root;password=root";
    private MySqlConnection _connection;
    private AvaloniaList<Tournament> _guestTournaments;
    private AvaloniaList<Tournament> _guestTournamentsPreSearch;
    private AvaloniaList<Location> _locations;

    public GuestTournamentWindow()
    {
        InitializeComponent();
        ShowGuestTournamentTable();
    }

    public AvaloniaList<Tournament> GuestTournamentsPreSearch
    {
        get { return _guestTournamentsPreSearch; }
        set { _guestTournamentsPreSearch = value; }
    }

    public AvaloniaList<Tournament> GuestTournaments
    {
        get { return _guestTournaments; }
        set { _guestTournaments = value; }
    }

    public void ShowGuestTournamentTable()
    {
        string sql =
                "select Tournament.ID, " +
                " Tournament.StartDate, " +
                "Tournament.EndDate, " +
                "Tournament.PrizePool, " +
                "Tournament.Name, " +
                "L.Name as Location " +
                "from cursach.Tournament " +
                "join cursach.Location L on Tournament.Location = L.ID "
            ;

        _guestTournaments = new AvaloniaList<Tournament>();
        _connection = new MySqlConnection(_con);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read() && reader.HasRows)
        {
            var curTournament = new Tournament()
            {
                ID = reader.GetInt32("ID"),
                Name = reader.GetString("Name"),
                StartDate = reader.GetDateTime("StartDate"),
                EndDate = reader.GetDateTime("EndDate"),
                Location = reader.GetString("Location"),
                PrizePool = reader.GetDecimal("PrizePool"),
            };
            _guestTournaments.Add(curTournament);
        }

        _connection.Close();

        GuestTournamentDG.ItemsSource = _guestTournaments;
    }

    private void BackBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        AuthWindow authWindow = new AuthWindow();
        authWindow.Show();
        this.Close();
    }

    private void SearchTextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_guestTournamentsPreSearch is null)
        {
            GuestTournamentsPreSearch = GuestTournaments;
        }

        if (SearchTextBox.Text is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
        {
            GuestTournamentDG.ItemsSource = GuestTournamentsPreSearch;
            searchTextBlock.IsVisible = false;
            return;
        }

        TournamentFilter();
    }

    private void TournamentFilter()
    {
        if (SearchTextBox.Text is null)
        {
            return;
        }
        else
        {
            if (SortCB.SelectedIndex == 0)
            {
                var filtered = GuestTournamentsPreSearch.Where(
                    it => it.Name.Contains(SearchTextBox.Text)
                          || it.StartDate.ToString().Contains(SearchTextBox.Text)
                          || it.PrizePool.ToString().Contains(SearchTextBox.Text)
                          || it.Location.ToString().Contains(SearchTextBox.Text)).ToList();
                filtered = filtered.OrderBy(name => name.Name).ToList();
                GuestTournamentDG.ItemsSource = filtered;
            }
            else if (SortCB.SelectedIndex == 1)
            {
                var filtered = GuestTournamentsPreSearch
                    .Where(it => it.Name.Contains(SearchTextBox.Text)).ToList();
                filtered = filtered.OrderBy(name => name.Name).ToList();
                GuestTournamentDG.ItemsSource = filtered;
            }
            else if (SortCB.SelectedIndex == 2)
            {
                var filtered = GuestTournamentsPreSearch
                    .Where(it => it.PrizePool.ToString().Contains(SearchTextBox.Text)).ToList();
                filtered = filtered.OrderBy(prizepool => prizepool.PrizePool).ToList();
                GuestTournamentDG.ItemsSource = filtered;
            }
            else if (SortCB.SelectedIndex == 3)
            {
                var filtered = GuestTournamentsPreSearch
                    .Where(it => it.Location.ToString().Contains(SearchTextBox.Text)).ToList();
                filtered = filtered.OrderBy(location => location.Location).ToList();
                GuestTournamentDG.ItemsSource = filtered;
            }
        }
    }

    private void SortCB_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) => TournamentFilter();


    private void TournamentGridBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        Tournament selectedTournament = GuestTournamentDG.SelectedItem as Tournament;
        TournamentGuestGrid grid = new TournamentGuestGrid(selectedTournament, _gridData);
        grid.ShowDialog(this);
        grid.Closed += delegate
        {
            GuestTournamentDG.ItemsSource = _guestTournaments;
            GuestTournamentDG.SelectedItem = null;
        };
    }
}