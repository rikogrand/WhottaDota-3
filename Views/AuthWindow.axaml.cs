using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;

namespace whottadota;

public partial class AuthWindow : Window
{
    private string _con = "server=localhost;database=cursach;user=root;password=root";
    private MySqlConnection _connection;

    public AuthWindow()
    {
        InitializeComponent();
    }

    private void LoginButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MySqlConnection _connection;
        string sql = "SELECT Login, Password from cursach.AuthInfo WHERE Login = @login and Password = @password";
        _connection = new MySqlConnection(_con);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        command.Parameters.Add("@login", MySqlDbType.VarChar).Value = LoginTextBox.Text;
        var login = command.Parameters["@login"].Value;
        command.Parameters.Add("@password", MySqlDbType.VarChar).Value = PasswordTextBox.Text;
        var password = command.Parameters["@password"].Value;
        var reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            if (LoginTextBox.Text.Equals(login) && PasswordTextBox.Text.Equals(password))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }

    private void HidePasswordCheckBox_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (PasswordTextBox.PasswordChar == (char)0)
        {
            PasswordTextBox.PasswordChar = '•';
        }
        else
        {
            PasswordTextBox.PasswordChar = (char)0;
        }
    }

    private void GuestButton_OnClick(object? sender, RoutedEventArgs e)
    {
        GuestTournamentWindow guestTournamentWindow = new GuestTournamentWindow();
        guestTournamentWindow.Show();
        this.Close();
    }
}