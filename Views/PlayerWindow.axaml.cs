using System.Linq;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;

namespace whottadota;

public partial class PlayerWindow : Window
{
    private string _con = "server=localhost;database=cursach;user=root;password=root";
    private MySqlConnection _connection;
    private AvaloniaList<Location> _locations;
    private AvaloniaList<Player> _players;
    private AvaloniaList<Player> _playersPreSearch;

    public PlayerWindow()
    {
        InitializeComponent();
        ShowPlayerTable();
    }

    public AvaloniaList<Player> PlayerPreSearch
    {
        get { return _playersPreSearch; }
        set { _playersPreSearch = value; }
    }

    public AvaloniaList<Player> Player
    {
        get { return _players; }
        set { _players = value; }
    }

    public void ShowPlayerTable()
    {
        string sql =
                "select Player.ID, " +
                " Player.Initials, " +
                "Player.Nickname, " +
                "Player.DateOfBirth, " +
                "Player.PhoneNumber, " +
                "Player.Email, " +
                "T.Name as Team " +
                "from cursach.Player " +
                "join cursach.Team T on Player.Team = T.ID "
            ;

        _players = new AvaloniaList<Player>();
        _connection = new MySqlConnection(_con);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read() && reader.HasRows)
        {
            var curPlayer = new Player()
            {
                ID = reader.GetInt32("ID"),
                Initials = reader.GetString("Initials"),
                Nickname = reader.GetString("Nickname"),
                DateOfBirth = reader.GetDateTime("DateOfBirth"),
                PhoneNumber = reader.GetString("PhoneNumber"),
                Email = reader.GetString("Email"),
                Team = reader.GetString("Team")
            };
            _players.Add(curPlayer);
        }

        _connection.Close();

        PlayerDG.ItemsSource = _players;
    }

    private void SearchTextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_playersPreSearch is null)
        {
            PlayerPreSearch = Player;
        }

        if (SearchTextBox.Text is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
        {
            PlayerDG.ItemsSource = PlayerPreSearch;
            searchTextBlock.IsVisible = false;
            return;
        }

        PlayerFilter();
    }

    private void PlayerFilter()
    {
        if (SearchTextBox.Text is null)
        {
            return;
        }
        else
        {
            if (SortCB.SelectedIndex == 0)
            {
                var filtered = PlayerPreSearch
                    .Where(it => it.Team.Contains(SearchTextBox.Text)).ToList();
                filtered = filtered.OrderBy(name => name.Team).ToList();
                PlayerDG.ItemsSource = filtered;
            }
        }
    }

    private void SortCB_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) => PlayerFilter();

    private void BackBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        AuthWindow authWindow = new AuthWindow();
        authWindow.Show();
        this.Close();
    }
}