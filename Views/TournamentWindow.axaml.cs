using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using MySql.Data.MySqlClient;

namespace whottadota;

public partial class TournamentWindow : Window
{
    private string _con = "server=localhost;database=cursach;user=root;password=root";
    private MySqlConnection _connection;
    private MySqlConnection _connectionСountTournament;
    private MySqlConnection _connectionCountPrizePool;
    private MySqlConnection _connectionLocationStats;
    private AvaloniaList<Location> _locations;
    private AvaloniaList<Tournament> _locationStats;
    private AvaloniaList<Tournament> _prizePoolTournamentStats;
    private AvaloniaList<Tournament> _tournaments;
    private AvaloniaList<Tournament> _tournamentsPreSearch;
    private AvaloniaList<Tournament> _yearTournamentStats;

    public TournamentWindow()
    {
        InitializeComponent();
        ShowTournamentTable();
        ShowYearTournamentStatsTable();
        ShowYearPrizePoolStatsTable();
        ShowRatingLocationStatsTable();
    }

    public AvaloniaList<Tournament> TournamentsPreSearch
    {
        get { return _tournamentsPreSearch; }
        set { _tournamentsPreSearch = value; }
    }

    public AvaloniaList<Tournament> Tournaments
    {
        get { return _tournaments; }
        set { _tournaments = value; }
    }

    public void ShowTournamentTable()
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

        _tournaments = new AvaloniaList<Tournament>();
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
            _tournaments.Add(curTournament);
        }

        _connection.Close();

        TournamentDG.ItemsSource = _tournaments;
    }

    private void YearTournamentCheckBox_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (YearTournamentCheckBox.IsChecked == true)
        {
            YearTournamentStatsDG.IsVisible = true;
            CountYearStatsTextBlock.IsVisible = true;
            CountYearStatsTextBox.IsVisible = true;
            SaveYearStatsBTN.IsVisible = true;
        }
        else if (YearTournamentCheckBox.IsChecked == false)
        {
            YearTournamentStatsDG.IsVisible = false;
            CountYearStatsTextBlock.IsVisible = false;
            CountYearStatsTextBox.IsVisible = false;
            SaveYearStatsBTN.IsVisible = false;
        }
    }

    private void YearPrizePoolStatsCheckBox_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (YearPrizePoolStatsCheckBox.IsChecked == true)
        {
            YearPrizePoolStatsDG.IsVisible = true;
            CountYearPrizePoolStatsTextBlock.IsVisible = true;
            CountYearPrizePoolStatsTextBox.IsVisible = true;
            SavePrizePoolStatsBTN.IsVisible = true;
        }
        else if (YearPrizePoolStatsCheckBox.IsChecked == false)
        {
            YearPrizePoolStatsDG.IsVisible = false;
            CountYearPrizePoolStatsTextBlock.IsVisible = false;
            CountYearPrizePoolStatsTextBox.IsVisible = false;
            SavePrizePoolStatsBTN.IsVisible = false;
        }
    }

    private void RatingLocationStatsCheckBox_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (RatingLocationStatsCheckBox.IsChecked == true)
        {
            RatingLocationStatsDG.IsVisible = true;
            MostPopularLocationStatsTextBox.IsVisible = true;
            MostPopularLocationStatsTextBlock.IsVisible = true;
            SaveLocationStatsBTN.IsVisible = true;
        }
        else if (RatingLocationStatsCheckBox.IsChecked == false)
        {
            RatingLocationStatsDG.IsVisible = false;
            MostPopularLocationStatsTextBox.IsVisible = false;
            MostPopularLocationStatsTextBlock.IsVisible = false;
            SaveLocationStatsBTN.IsVisible = false;
        }
    }

    public void ShowYearTournamentStatsTable()
    {
        string sql =
            "SELECT Tournament.ID, Tournament.Name, Tournament.StartDate, Tournament.EndDate, " +
            "L.Name as Location, Tournament.PrizePool" +
            " FROM Tournament " +
            "  join cursach.Location L on Tournament.Location = L.ID " +
            " WHERE YEAR(EndDate) = YEAR(CURRENT_DATE) ";
        _yearTournamentStats = new AvaloniaList<Tournament>();
        _connection = new MySqlConnection(_con);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read() && reader.HasRows)
        {
            var curYearTournamentStat = new Tournament()
            {
                ID = reader.GetInt32("ID"),
                Name = reader.GetString("Name"),
                StartDate = reader.GetDateTime("StartDate"),
                EndDate = reader.GetDateTime("EndDate"),
                Location = reader.GetString("Location"),
                PrizePool = reader.GetDecimal("PrizePool")
            };
            _yearTournamentStats.Add(curYearTournamentStat);
        }

        _connection.Close();

        YearTournamentStatsDG.ItemsSource = _yearTournamentStats;

        string sqlCount =
            "SELECT Count(*) as Count" +
            " FROM Tournament " +
            " WHERE YEAR(EndDate) = YEAR(CURRENT_DATE); ";
        _connectionСountTournament = new MySqlConnection(_con);
        _connectionСountTournament.Open();
        MySqlCommand commandCount = new MySqlCommand(sqlCount, _connectionСountTournament);
        int totalCount;
        totalCount = Convert.ToInt32(commandCount.ExecuteScalar());
        _connectionСountTournament.Close();
        CountYearStatsTextBox.Text = totalCount.ToString();
    }


    public void ShowYearPrizePoolStatsTable()
    {
        string sql =
            "select Tournament.ID, " +
            "Tournament.PrizePool, " +
            "Tournament.Name " +
            "from cursach.Tournament " +
            " WHERE YEAR(EndDate) = YEAR(CURRENT_DATE) ";
        ;
        _prizePoolTournamentStats = new AvaloniaList<Tournament>();
        _connection = new MySqlConnection(_con);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read() && reader.HasRows)
        {
            var curPrizePoolTournamentStat = new Tournament()
            {
                ID = reader.GetInt32("ID"),
                Name = reader.GetString("Name"),
                PrizePool = reader.GetDecimal("PrizePool")
            };
            _prizePoolTournamentStats.Add(curPrizePoolTournamentStat);
        }

        _connection.Close();
        YearPrizePoolStatsDG.ItemsSource = _prizePoolTournamentStats;

        string sqlSum =
            " select Sum(PrizePool) AS Sum" +
            " from Tournament" +
            " WHERE YEAR(EndDate) = YEAR(CURRENT_DATE) ";
        _connectionCountPrizePool = new MySqlConnection(_con);
        _connectionCountPrizePool.Open();
        MySqlCommand commandSum = new MySqlCommand(sqlSum, _connectionCountPrizePool);
        int totalSum;
        totalSum = Convert.ToInt32(commandSum.ExecuteScalar());
        _connectionCountPrizePool.Close();
        CountYearPrizePoolStatsTextBox.Text = totalSum.ToString();
    }

    public void ShowRatingLocationStatsTable()
    {
        string sql =
            "  SELECT Tournament.ID, L.Name as Location , Count(Tournament.Location) as CountLocation " +
            " from Tournament " +
            "  join cursach.Location L on Tournament.Location = L.ID " +
            " GROUP BY L.Name" +
            "  ORDER BY COUNT(Location) desc  ";


        _locationStats = new AvaloniaList<Tournament>();
        _connection = new MySqlConnection(_con);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read() && reader.HasRows)
        {
            var curLocationStats = new Tournament()
            {
                ID = reader.GetInt32("ID"),
                CountLocation = reader.GetInt32("CountLocation"),
                Location = reader.GetString("Location")
            };
            _locationStats.Add(curLocationStats);
        }

        _connection.Close();

        RatingLocationStatsDG.ItemsSource = _locationStats;

        string sqlCountLocation =
            "  SELECT Location " +
            "    FROM ( " +
            "         SELECT l.Name as Location, COUNT(Location) AS LocationCount " +
            "   FROM tournament " +
            "   join cursach.location l on l.ID = tournament.Location " +
            "  GROUP BY Location" +
            " ORDER BY LocationCount DESC " +
            " LIMIT 1 " +
            "   ) AS SubQuery; ";
        _connectionLocationStats = new MySqlConnection(_con);
        _connectionLocationStats.Open();
        MySqlCommand commandCountLocation = new MySqlCommand(sqlCountLocation, _connectionLocationStats);
        string totalCountLocation;
        totalCountLocation = commandCountLocation.ExecuteScalar().ToString();
        ;
        _connectionLocationStats.Close();
        MostPopularLocationStatsTextBox.Text = totalCountLocation;
    }

    private void BackBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        AuthWindow mainWindow = new AuthWindow();
        mainWindow.Show();
        this.Close();
    }

    private void SearchTextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_tournamentsPreSearch is null)
        {
            TournamentsPreSearch = Tournaments;
        }

        if (SearchTextBox.Text is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
        {
            TournamentDG.ItemsSource = TournamentsPreSearch;
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
                var filtered = TournamentsPreSearch.Where(
                    it => it.Name.Contains(SearchTextBox.Text)
                          || it.StartDate.ToString().Contains(SearchTextBox.Text)
                          || it.PrizePool.ToString().Contains(SearchTextBox.Text)
                          || it.Location.ToString().Contains(SearchTextBox.Text)).ToList();
                filtered = filtered.OrderBy(name => name.Name).ToList();
                TournamentDG.ItemsSource = filtered;
            }
            else if (SortCB.SelectedIndex == 1)
            {
                var filtered = TournamentsPreSearch
                    .Where(it => it.Name.Contains(SearchTextBox.Text)).ToList();
                filtered = filtered.OrderBy(name => name.Name).ToList();
                TournamentDG.ItemsSource = filtered;
            }
            else if (SortCB.SelectedIndex == 2)
            {
                var filtered = TournamentsPreSearch
                    .Where(it => it.PrizePool.ToString().Contains(SearchTextBox.Text)).ToList();
                filtered = filtered.OrderBy(prizepool => prizepool.PrizePool).ToList();
                TournamentDG.ItemsSource = filtered;
            }
            else if (SortCB.SelectedIndex == 3)
            {
                var filtered = TournamentsPreSearch
                    .Where(it => it.Location.ToString().Contains(SearchTextBox.Text)).ToList();
                filtered = filtered.OrderBy(location => location.Location).ToList();
                TournamentDG.ItemsSource = filtered;
            }
        }
    }

    private void SortCB_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) => TournamentFilter();

    private void AddTournamentBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        CreateTournament createTournament = new CreateTournament();
        createTournament.Show();
        this.Close();
    }

    private void EditTournamentBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        Tournament selectedTournament = TournamentDG.SelectedItem as Tournament;
        if (selectedTournament is null)
        {
            return;
        }

        EditTournamentWindow edit = new EditTournamentWindow(selectedTournament);
        edit.ShowDialog(this);
        edit.Closed += delegate
        {
            if (edit.Result)
            {
                ShowTournamentTable();
                TournamentDG.ItemsSource = _tournaments;
                TournamentDG.SelectedItem = null;
            }
        };
    }

    private void DeleteTournamentBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        var myValue = TournamentDG.SelectedItem as Tournament;
        if (myValue is null)
        {
            return;
        }

        DeleteTournamentWindow del = new DeleteTournamentWindow(myValue);
        del.ShowDialog(this);
        del.Closed += (o, args) =>
        {
            if (del.Result)
            {
                ShowTournamentTable();
            }
        };
    }

    private void TournamentGridBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        Tournament selectedTournament = TournamentDG.SelectedItem as Tournament;
        TournamentGridWindow grid = new TournamentGridWindow(selectedTournament);
        grid.ShowDialog(this);
        grid.Closed += delegate
        {
            TournamentDG.ItemsSource = _tournaments;
            TournamentDG.SelectedItem = null;
        };
    }


    private void AuthBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        AuthWindow authWindow = new AuthWindow();
        authWindow.Show();
        this.Close();
    }

    string ToCsvYearTournament(List<Tournament> list)
    {
        string description = "Отчет по мероприятиям за год";
        string result1 = "ID, Name, Location, PrizePool, StartDate, EndDate";

        foreach (var item in list)
        {
            result1 += $"\n{item.ID},{item.Name},{item.Location},{item.PrizePool}, {item.StartDate}, {item.EndDate}";
        }

        return $"{description}\n{result1}";
    }

    private async void SaveYearTournamentStatsBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Отчетность по мероприятиям за год"
        });
        if (file is not null)
        {
            await using var stream = await file.OpenWriteAsync();
            using var streamWriter = new StreamWriter(stream);

            await streamWriter.WriteLineAsync(ToCsvYearTournament(_yearTournamentStats.ToList()));
        }
    }

    string ToCsvPrizePool(List<Tournament> listPrizePool)
    {
        string description = "Отчет по призовому фонду";
        string result2 = "ID, Name, PrizePool";

        foreach (var itemPrizePool in listPrizePool)
        {
            result2 += $"\n {itemPrizePool.ID},{itemPrizePool.Name},{itemPrizePool.PrizePool}";
        }

        return $"{description}\n{result2}";
    }

    private async void SavePrizePoolStatsBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Отчетность по призовому фонду за год"
        });
        if (file is not null)
        {
            await using var stream = await file.OpenWriteAsync();
            using var streamWriter = new StreamWriter(stream);
            await streamWriter.WriteLineAsync(ToCsvPrizePool(_prizePoolTournamentStats.ToList()));
        }
    }

    string ToCsvLocation(List<Tournament> listLocation)
    {
        string description = "Отчет по популярным локациям";
        string result3 = "ID, Location, CountLocation";

        foreach (var itemLocation in listLocation)
        {
            result3 += $"\n{itemLocation.ID},{itemLocation.Location},{itemLocation.CountLocation}";
        }

        return $"{description}\n{result3}";
    }

    private async void SaveLocationStatsBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Отчетность по призовому фонду за год"
        });
        if (file is not null)
        {
            // Open writing stream from the file.
            await using var stream = await file.OpenWriteAsync();
            using var streamWriter = new StreamWriter(stream);
            // Write some content to the file.
            await streamWriter.WriteLineAsync(ToCsvLocation(_locationStats.ToList()));
        }
    }
}