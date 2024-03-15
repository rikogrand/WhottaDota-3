using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace whottadota;

public partial class TournamentWindow : Window
{
   private string _con = "server=10.10.1.24;database=pro1_11;user=user_01;password=user01pro";
    private AvaloniaList<Tournament> _tournaments;
    private AvaloniaList<Location> _locations;
    private AvaloniaList<TournamentByUser> _tournamentByUsers;
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
                "from pro1_11.Tournament " +
                 "join pro1_11.Location L on Tournament.Location = L.ID "
                + "join pro1_11.TierList TL on Tournament.Tier = TL.ID "
                + "WHERE Tournament.Tier = 1"
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
        TierOneDG.ItemsSource = _tournaments;
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
                "from pro1_11.Tournament " +
                "join pro1_11.Location L on Tournament.Location = L.ID "
                + "join pro1_11.TierList TL on Tournament.Tier = TL.ID "
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
                "from pro1_11.TournamentByUser " +
                "join pro1_11.Location L on TournamentByUser.Location = L.ID "
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

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        this.Close();
        mainWindow.Show();
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
        command.Parameters["@Location"].Value =(locationCB.SelectedItem as whottadota.Location).ID;
        
        command.Parameters.Add("@PrizePool", MySqlDbType.Decimal);
        command.Parameters["@PrizePool"].Value = prizePoolTB.Text;
        command.ExecuteNonQuery();
        _connection.Close();
        TournamentWindow tournamentWindow = new TournamentWindow();
        tournamentWindow.Show();
        this.Close();
        //   Close(true);
    }
    
   /* public void Close(bool result) {
        Result = result;
        base.Close(result);
    }
    public bool Result { get; private set; }
    */
    public void FillLocation()
    {
        string sql = "select ID, Name from pro1_11.Location";
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

    
    }
