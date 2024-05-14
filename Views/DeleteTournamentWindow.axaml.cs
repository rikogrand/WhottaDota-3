using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;

namespace whottadota;

public partial class DeleteTournamentWindow : Window
{
    private readonly Tournament _tournament;
    private string _con = "server=localhost;database=cursach;user=root;password=root";

    public DeleteTournamentWindow(Tournament tournament)
    {
        InitializeComponent();
        _tournament = tournament;
    }

    public bool Result { get; private set; }

    private void No_Btn_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void Yes_Btn_OnClick(object? sender, RoutedEventArgs e)
    {
        MySqlConnection _connection;
        string sql = "SET FOREIGN_KEY_CHECKS=0;" + "Delete from Tournament where ID = @ID LIMIT 1";
        _connection = new MySqlConnection(_con);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        command.Parameters.Add("@ID", MySqlDbType.Int32);
        command.Parameters["@ID"].Value = _tournament.ID;
        command.ExecuteNonQuery();
        _connection.Close();
        Close(true);
    }

    public void Close(bool result)
    {
        Result = result;
        base.Close(result);
    }
}