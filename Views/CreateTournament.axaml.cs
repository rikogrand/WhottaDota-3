using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;

namespace whottadota;

public partial class CreateTournament : Window

{
    private string _con = "server=localhost;database=cursach;user=root;password=root";
    private MySqlConnection _connection;
    private AvaloniaList<Location> _locations;
    private AvaloniaList<Tournament> _tournaments;

    public CreateTournament()
    {
        InitializeComponent();
        FillLocation();
    }

    public bool Result { get; private set; }

    private void AddUserTournamentBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        string sql =
                "insert into Tournament (Name, StartDate, EndDate, Location, PrizePool)" +
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

    public void FillLocation()
    {
        MySqlConnection _connection;
        string sql = "Select ID, Name from cursach.Location";
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

        var LocationCB = this.Find<ComboBox>("locationCB");
        LocationCB.ItemsSource = _locations;
    }
}