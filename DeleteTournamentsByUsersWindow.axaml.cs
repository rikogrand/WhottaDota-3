using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

namespace whottadota;

public partial class DeleteTournamentsByUsersWindow : Window
{
    private readonly TournamentByUser _tournamentByUser;
    private string _con = "server=localhost;database=cursach;user=root;password=root";
    public DeleteTournamentsByUsersWindow(TournamentByUser tournamentByUser)
    {
        InitializeComponent();
        _tournamentByUser = tournamentByUser;
    }

    private void No_Btn_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void Yes_Btn_OnClick(object? sender, RoutedEventArgs e)
    {
        MySqlConnection _connection;
        string sql = "SET FOREIGN_KEY_CHECKS=0;" + "Delete from TournamentByUser where ID = @ID LIMIT 1";
        _connection = new MySqlConnection(_con);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        command.Parameters.Add("@ID", MySqlDbType.Int32);
        command.Parameters["@ID"].Value = _tournamentByUser.ID;
        command.ExecuteNonQuery();
        _connection.Close();
        Close(true);
    }
    public void Close(bool result) {
        Result = result;
        base.Close(result);
    }
    public bool Result { get; private set; }
}