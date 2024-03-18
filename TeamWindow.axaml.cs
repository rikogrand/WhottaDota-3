using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace whottadota;

public partial class TeamWindow : Window
{
    // private string _con = "server=10.10.1.24;database=pro1_11;user=user_01;password=user01pro";
    private string _con = "server=localhost;database=cursach;user=root;password=root";
    private AvaloniaList<Team> _teams;
    private AvaloniaList<Team> _teamsPreSearch;
    private AvaloniaList<Location> _locations;
    private MySqlConnection _connection;

    public TeamWindow()
    {
        InitializeComponent();
        ShowTeamTable();

    }

    public void ShowTeamTable()
    {
        string sql =
                "select Team.ID, Team.Name, Team.Logo, " +
                " Team.TotalWinnings, " +
                "L.Name as Location " +
                "from cursach.Team " +
                "join cursach.Location L on Team.Location = L.ID "
            ;

        _teams = new AvaloniaList<Team>();
        _connection = new MySqlConnection(_con);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read() && reader.HasRows)
        {

            var curTeam = new Team()
            {
                ID = reader.GetInt32("ID"),
                Name = reader.GetString("Name"),
                Image = reader.GetFieldValue<byte[]>(reader.GetOrdinal("Logo")),
                TotalWinnings = reader.GetDecimal("TotalWinnings"),
                Location = reader.GetString("Location")
            };
            _teams.Add(curTeam);
        }

        _connection.Close();

        TeamDG.ItemsSource = _teams;

    }

    private void BackBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        this.Close();
        mainWindow.Show();
    }

    public AvaloniaList<Team> TeamPreSearch
    {
        get { return _teamsPreSearch; }
        set { _teamsPreSearch = value; }
    }

    public AvaloniaList<Team> Team
    {
        get { return _teams; }
        set { _teams = value; }
    }
    
    private void SearchTB_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_teamsPreSearch is null)
        {
            Team = TeamPreSearch;
        }

        if (SearchTB.Text is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(SearchTB.Text))
        {
            TeamDG.ItemsSource = TeamPreSearch;
            SearchBlock.IsVisible = false;
            return;
        }

        Filter();
    }

    private void Filter()
    {
        if (SearchTB.Text is null)
        {
            return;
        }
        else
        {
            if (SortCB.SelectedIndex == 0)
            {
                var filtered = TeamPreSearch.Where(
                    it => it.Name.Contains(SearchTB.Text)
                          || it.TotalWinnings.ToString().Contains(SearchTB.Text)
                          || it.Location.ToString().Contains(SearchTB.Text)).ToList();
                filtered = filtered.OrderBy(name => name.Name).ToList();
                TeamDG.ItemsSource = filtered;
            }
            else if (SortCB.SelectedIndex == 1)
            {
                var filtered = TeamPreSearch
                    .Where(it => it.Name.Contains(SearchTB.Text)).ToList();
                filtered = filtered.OrderBy(name => name.Name).ToList();
                TeamDG.ItemsSource = filtered;
            }
            else if (SortCB.SelectedIndex == 2)
            {
                var filtered = TeamPreSearch
                    .Where(it => it.TotalWinnings.ToString().Contains(SearchTB.Text)).ToList();
                filtered = filtered.OrderBy(totalwinnings => totalwinnings.TotalWinnings).ToList();
                TeamDG.ItemsSource = filtered;
            }
            else if (SortCB.SelectedIndex == 3)
            {
                var filtered = TeamPreSearch
                    .Where(it => it.Location.ToString().Contains(SearchTB.Text)).ToList();
                filtered = filtered.OrderBy(location => location.Location).ToList();
                TeamDG.ItemsSource = filtered;
            }

        }
    }
    private void SortCB_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) => Filter();
    }

