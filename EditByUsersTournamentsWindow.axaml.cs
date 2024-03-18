using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using MySql.Data.MySqlClient;

namespace whottadota;

public partial class EditByUsersTournamentsWindow : Window
{
    public TournamentByUser TournamentByUser { get; init; }
    private AvaloniaList<TournamentByUser> _tournamentsByUsers;
    private AvaloniaList<Location> _locations;
    private readonly TournamentByUser _tournamentByUser;
    private MySqlConnection _connection;
    private string _con = "server=localhost;database=cursach;user=root;password=root";
  //  private string _con = "server=10.10.1.24;database=pro1_11;user=user_01;password=user01pro";
    
    public EditByUsersTournamentsWindow(TournamentByUser tournamentByUser)
    {
        InitializeComponent();
        _tournamentByUser = tournamentByUser;
        FillLocation();
    }


    private void CnfrmEditTournamentBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        AvaloniaList<TournamentByUser> _tournamentByUsers;
        MySqlConnection _connection;
        string sql =
                "update TournamentByUser set   " +
                " Name = @Name, " +
                "StartDate = @StartDate, " +
                "EndDate = @EndDate, " +
                "Location = @Location, " +
                "PrizePool = @PrizePool" +
                "";
        _connection = new MySqlConnection(_con);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        command.Parameters.Add("@Name", MySqlDbType.VarChar);
        command.Parameters["@Name"].Value = nameTournamentTB.Text;
        
        command.Parameters.Add("@StartDate", MySqlDbType.DateTime);
        command.Parameters["@StartDate"].Value = startDateDP.SelectedDate;

        command.Parameters.Add("@EndDate", MySqlDbType.DateTime);
        command.Parameters["@EndDate"].Value = endDateDP.SelectedDate;

        command.Parameters.Add("@Location", MySqlDbType.Int32);
        command.Parameters["@Location"].Value = (locationCB.SelectedItem as whottadota.Location).ID;

        command.Parameters.Add("@PrizePool", MySqlDbType.Decimal);
        command.Parameters["@PrizePool"].Value = prizePoolTB.Text;
            command.ExecuteReader();
            _connection.Close();
            Close(true);
          
        }
    

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
    public void Close(bool result)
    {
        Result = result;
        base.Close(result);
    }
    public bool Result { get; private set; }
    
}