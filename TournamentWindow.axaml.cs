using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Math.EC.Rfc8032;

namespace whottadota;

public partial class TournamentWindow : Window
{
    // private string _con = "server=10.10.1.24;database=pro1_11;user=user_01;password=user01pro";
    private string _con = "server=localhost;database=cursach;user=root;password=root";
    private AvaloniaList<Tournament> _tournaments;
    private AvaloniaList<TournamentByUser> _tournamentsByUserPreSearch;
    private AvaloniaList<Location> _locations;
    private AvaloniaList<TournamentByUser> _tournamentByUsers;
    private AvaloniaList<Tournament> _tournamentsPreSearch;
    private AvaloniaList<Tournament> _tournamentsOne;
    private AvaloniaList<Tournament> _tournamentOnePreSearch;
    private MySqlConnection _connection;

    public TournamentWindow()
    {
        InitializeComponent();
        ShowTierOneTable();
        ShowTierTwoTable();
        ShowByUserTable();
        FillLocation();
    }

    public void ShowTierOneTable()
    {
        string sql =
                "select Tournament.ID, Tournament.Logo, " +
                " Tournament.StartDate, " +
                "Tournament.EndDate, " +
                "Tournament.PrizePool, " +
                "Tournament.Name, " +
                "L.Name as Location, " +
                "TL.Name as Tier " +
                "from cursach.Tournament " +
                "join cursach.Location L on Tournament.Location = L.ID "
                + "join TierList TL on Tournament.Tier = TL.ID "
                + "WHERE Tournament.Tier = 1"
            ;

        _tournamentsOne = new AvaloniaList<Tournament>();
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
                Image = reader.GetFieldValue<byte[]>(reader.GetOrdinal("Logo")),
                StartDate = reader.GetDateTime("StartDate"),
                EndDate = reader.GetDateTime("EndDate"),
                Location = reader.GetString("Location"),
                Tier = reader.GetString("Tier"),
                PrizePool = reader.GetDecimal("PrizePool")
            };
            _tournamentsOne.Add(curTournament);
        }

        _connection.Close();

        TierOneDG.ItemsSource = _tournamentsOne;

    }


    public void ShowTierTwoTable()
    {
        string sql =
                "select Tournament.ID, Tournament.Logo, " +
                " Tournament.StartDate, " +
                "Tournament.EndDate, " +
                "Tournament.PrizePool, " +
                "Tournament.Name, " +
                "L.Name as Location, " +
                "TL.Name as Tier " +
                "from cursach.Tournament " +
                "join cursach.Location L on Tournament.Location = L.ID "
                + "join cursach.TierList TL on Tournament.Tier = TL.ID "
                + "WHERE Tournament.Tier = 2"
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
                Image = reader.GetFieldValue<byte[]>(reader.GetOrdinal("Logo")),
                StartDate = reader.GetDateTime("StartDate"),
                EndDate = reader.GetDateTime("EndDate"),
                Location = reader.GetString("Location"),
                Tier = reader.GetString("Tier"),
                PrizePool = reader.GetDecimal("PrizePool")
            };
            _tournaments.Add(curTournament);
        }

        _connection.Close();
        TierTwoDG.ItemsSource = _tournaments;
    }

    public void ShowByUserTable()
    {
        string sql =
                "select TournamentByUser.ID, " +
                " TournamentByUser.StartDate, " +
                "TournamentByUser.EndDate, " +
                "TournamentByUser.Name, " +
                "TournamentByUser.PrizePool, " +
                "L.Name as Location " +
                "from cursach.TournamentByUser " +
                "join cursach.Location L on TournamentByUser.Location = L.ID "
            ;
        _tournamentByUsers = new AvaloniaList<TournamentByUser>();
        _connection = new MySqlConnection(_con);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {

            var curTournamentByUsers = new TournamentByUser()
            {
                ID = reader.GetInt32("ID"),
                Name = reader.GetString("Name"),
                StartDate = reader.GetDateTime("StartDate"),
                EndDate = reader.GetDateTime("EndDate"),
                Location = reader.GetString("Location"),
                PrizePool = reader.GetDecimal("PrizePool")
            };
            _tournamentByUsers.Add(curTournamentByUsers);
        }

        _connection.Close();
        byUserDG.ItemsSource = _tournamentByUsers;
    }

    private void AddUserTournamentBTN_OnClick(object? sender, RoutedEventArgs e)
    {

        string sql =
                "insert into TournamentByUser (Name, StartDate, EndDate, Location, PrizePool)" +
                " values (@Name, @StartDate, @EndDate, @Location, @PrizePool ) "
            ;

        _connection = new MySqlConnection(_con);
        _connection.Open();

        MySqlCommand command = new MySqlCommand(sql, _connection);
        command.Parameters.Add("@Name", MySqlDbType.String);
        command.Parameters["@Name"].Value = nameTournamentTB.Text;

        command.Parameters.Add("@StartDate", MySqlDbType.DateTime);
        command.Parameters["@StartDate"].Value = startDateDP.SelectedDate;

        command.Parameters.Add("@EndDate", MySqlDbType.DateTime);
        command.Parameters["@EndDate"].Value = endDateDP.SelectedDate;

        command.Parameters.Add("@Location", MySqlDbType.Int32);
        command.Parameters["@Location"].Value = (locationCB.SelectedItem as whottadota.Location).ID;

        command.Parameters.Add("@PrizePool", MySqlDbType.Decimal);
        command.Parameters["@PrizePool"].Value = prizePoolTB.Text;
        command.ExecuteNonQuery();
        _connection.Close();
        TournamentWindow tournamentWindow = new TournamentWindow();
        tournamentWindow.Show();
        this.Close();
        Close(true);
    }

    public void Close(bool result)
    {
        Result = result;
        base.Close(result);
    }

    public bool Result { get; private set; }

    public void FillLocation()
    {
        string sql = "select ID, Name from cursach.Location";
        _locations = new AvaloniaList<Location>();
        _connection = new MySqlConnection(_con);
        _connection.Open();

        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var curLocation = new Location()
            {
                ID = reader.GetInt32("ID"),
                Name = reader.GetString("Name")
            };
            _locations.Add(curLocation);
        }

        _connection.Close();
        var locationsCB = this.Find<ComboBox>("locationCB");
        locationsCB.ItemsSource = _locations;

    }
    
    private void BackBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    public AvaloniaList<TournamentByUser> TournamentsByUserPreSearch
    {
        get { return _tournamentsByUserPreSearch; }
        set { _tournamentsByUserPreSearch = value; }
    }

    public AvaloniaList<TournamentByUser> TournamentsByUsers
    {
        get { return _tournamentByUsers; }
        set { _tournamentByUsers = value; }
    }

    private void SearchTB_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_tournamentsByUserPreSearch is null)
        {
            TournamentsByUserPreSearch = TournamentsByUsers;
        }

        if (searchTB.Text is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(searchTB.Text))
        {
            byUserDG.ItemsSource = TournamentsByUserPreSearch;
            searchBlock.IsVisible = false;
            return;
        }

        FilterForUser();
    }

    private void FilterForUser()
    {
        if (searchTB.Text is null)
        {
            return;
        }
        else
        {
            if (SortCB.SelectedIndex == 0)
            {
                var filtered = TournamentsByUserPreSearch.Where(
                    it => it.Name.Contains(searchTB.Text)
                          || it.StartDate.ToString().Contains(searchTB.Text)
                          || it.PrizePool.ToString().Contains(searchTB.Text)
                          || it.Location.ToString().Contains(searchTB.Text)).ToList();
                filtered = filtered.OrderBy(name => name.Name).ToList();
                byUserDG.ItemsSource = filtered;
            }
            else if (SortCB.SelectedIndex == 1)
            {
                var filtered = TournamentsByUserPreSearch
                    .Where(it => it.Name.Contains(searchTB.Text)).ToList();
                filtered = filtered.OrderBy(name => name.Name).ToList();
                byUserDG.ItemsSource = filtered;
            }
            else if (SortCB.SelectedIndex == 2)
            {
                var filtered = TournamentsByUserPreSearch
                    .Where(it => it.PrizePool.ToString().Contains(searchTB.Text)).ToList();
                filtered = filtered.OrderBy(prizepool => prizepool.PrizePool).ToList();
                byUserDG.ItemsSource = filtered;
            }
            else if (SortCB.SelectedIndex == 3)
            {
                var filtered = TournamentsByUserPreSearch
                    .Where(it => it.Location.ToString().Contains(searchTB.Text)).ToList();
                filtered = filtered.OrderBy(location => location.Location).ToList();
                byUserDG.ItemsSource = filtered;
            }

        }
    }

    private void SortCB_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) => FilterForUser();

    private void DeleteBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        var myValue = byUserDG.SelectedItem as TournamentByUser;
        if (myValue is null)
        {
            return;
        }

        DeleteTournamentsByUsersWindow delete = new DeleteTournamentsByUsersWindow(myValue);
        delete.ShowDialog(this);
        delete.Closed += (o, args) =>
        {
            if (delete.Result)
            {
                ShowByUserTable();
            }
        };
    }

    private void EditBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        var MyValue = byUserDG.SelectedItem as TournamentByUser;
        if (MyValue is null) return;

        EditByUsersTournamentsWindow edit = new EditByUsersTournamentsWindow(MyValue);
        edit.ShowDialog(this);
        edit.Closed += (o, args) =>
        {
            if (edit.Result)
            {
                ShowByUserTable();
            }
        };
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

    private void SearchTiersTB_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_tournamentsPreSearch is null)
        {
            _tournamentsPreSearch = Tournaments;
        }

        if (searchTiersTB.Text is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(searchTiersTB.Text))
        {
            TierTwoDG.ItemsSource = TournamentsPreSearch;
            searchTiersBlock.IsVisible = false;
            return;
        }

        FilterTierTwo();
    }

    private void FilterTierTwo()
    {
        if (searchTiersTB.Text is null)
        {
            return;
        }
        else
        {
            if (SortTiersCB.SelectedIndex == 0)
            {
                var filtered = TournamentsPreSearch.Where(
                    it => it.Name.Contains(searchTiersTB.Text)
                          || it.StartDate.ToString().Contains(searchTiersTB.Text)
                          || it.PrizePool.ToString().Contains(searchTiersTB.Text)
                          || it.Location.ToString().Contains(searchTiersTB.Text)).ToList();
                filtered = filtered.OrderBy(name => name.Name).ToList();
                TierTwoDG.ItemsSource = filtered;
            }
            else if (SortTiersCB.SelectedIndex == 1)
            {
                var filtered = TournamentsPreSearch
                    .Where(it => it.Name.Contains(searchTiersTB.Text)).ToList();
                filtered = filtered.OrderBy(name => name.Name).ToList();
                TierTwoDG.ItemsSource = filtered;
            }
            else if (SortTiersCB.SelectedIndex == 2)
            {
                var filtered = TournamentsPreSearch
                    .Where(it => it.PrizePool.ToString().Contains(searchTiersTB.Text)).ToList();
                filtered = filtered.OrderBy(prizepool => prizepool.PrizePool).ToList();
                TierTwoDG.ItemsSource = filtered;
            }
            else if (SortTiersCB.SelectedIndex == 3)
            {
                var filtered = TournamentsPreSearch
                    .Where(it => it.Location.ToString().Contains(searchTiersTB.Text)).ToList();
                filtered = filtered.OrderBy(location => location.Location).ToList();
                TierTwoDG.ItemsSource = filtered;
            }

        }
    }

    private void SortTiersCB_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) => FilterTierTwo();

    public AvaloniaList<Tournament> TournamentOnePreSearch
    {
        get { return _tournamentOnePreSearch; }
        set { _tournamentOnePreSearch = value; }
    }

    public AvaloniaList<Tournament> TournamentOne
    {
        get { return _tournamentsOne; }
        set { _tournamentsOne = value; }
    }
private void SearchTierOneTB_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_tournamentOnePreSearch is null)
        {
            _tournamentOnePreSearch = TournamentOne;
        }

        if (searchTierOneTB.Text is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(searchTierOneTB.Text))
        {
            TierOneDG.ItemsSource = TournamentOnePreSearch;
            searchTierOneBlock.IsVisible = false;
            return;
        }

        FilterTierOne();
    }

    private void FilterTierOne()
    {
        if (searchTierOneTB.Text is null)
        {
            return;
        }
        else
        {
            if (SortTierOneCB.SelectedIndex == 0)
            {
                var filtered = TournamentOnePreSearch.Where(
                    it => it.Name.Contains(searchTierOneTB.Text)
                          || it.StartDate.ToString().Contains(searchTierOneTB.Text)
                          || it.PrizePool.ToString().Contains(searchTierOneTB.Text)
                          || it.Location.ToString().Contains(searchTierOneTB.Text)).ToList();
                filtered = filtered.OrderBy(name => name.Name).ToList();
                TierOneDG.ItemsSource = filtered;
            }
            else if (SortTierOneCB.SelectedIndex == 1)
            {
                var filtered = TournamentOnePreSearch
                    .Where(it => it.Name.Contains(searchTierOneTB.Text)).ToList();
                filtered = filtered.OrderBy(name => name.Name).ToList();
                TierOneDG.ItemsSource = filtered;
            }
            else if (SortTierOneCB.SelectedIndex == 2)
            {
                var filtered = TournamentOnePreSearch
                    .Where(it => it.PrizePool.ToString().Contains(searchTierOneTB.Text)).ToList();
                filtered = filtered.OrderBy(prizepool => prizepool.PrizePool).ToList();
                TierOneDG.ItemsSource = filtered;
            }
            else if (SortTierOneCB.SelectedIndex == 3)
            {
                var filtered = TournamentOnePreSearch
                    .Where(it => it.Location.ToString().Contains(searchTierOneTB.Text)).ToList();
                filtered = filtered.OrderBy(location => location.Location).ToList();
                TierOneDG.ItemsSource = filtered;
            }

        }
    }

    private void SortTierOneCB_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) => FilterTierOne();
}
    
 
