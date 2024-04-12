using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace whottadota;

public partial class SettingsWindow : Window
{
    private string _con = "server=localhost;database=cursach;user=root;password=root";
    private AvaloniaList<Settings> _settings;
    private AvaloniaList<Format> _formats;
    private AvaloniaList<ThirdPlace> _thirdPlaces;
    private AvaloniaList<Language> _languages;
    private MySqlConnection _connection;

    public SettingsWindow()
    {
        InitializeComponent();
        FillFormat();
        FillThirdPlace();
        FillLanguage();
    }

    private void SaveSettings_OnClick(object? sender, RoutedEventArgs e)
    {

        string sql =
                "insert into Settings (FormatTournament, ThirdPlace, Language)" +
                " values (@FormatTournament, @ThirdPlace, @Language ) "
            ;

        _connection = new MySqlConnection(_con);
        _connection.Open();

        MySqlCommand command = new MySqlCommand(sql, _connection);
        command.Parameters.Add("@FormatTournament", MySqlDbType.Int32);
        command.Parameters["@FormatTournament"].Value = (formatCB.SelectedItem as whottadota.Format).ID;

        command.Parameters.Add("@ThirdPlace", MySqlDbType.Int32);
        command.Parameters["@ThirdPlace"].Value = (thirdplaceCB.SelectedItem as whottadota.ThirdPlace).ID;

        command.Parameters.Add("@Language", MySqlDbType.Int32);
        command.Parameters["@Language"].Value = (languageCB.SelectedItem as whottadota.Language).ID;
        
        command.ExecuteNonQuery();
        _connection.Close();
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
        Close(true);
    }

    public void Close(bool result)
    {
        Result = result;
        base.Close(result);
    }

    public bool Result { get; private set; }

    public void FillFormat()
    {
        string sql = "select ID, Name from cursach.Format";
        _formats = new AvaloniaList<Format>();
        _connection = new MySqlConnection(_con);
        _connection.Open();

        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var curFormat = new Format()
            {
                ID = reader.GetInt32("ID"),
                Name = reader.GetString("Name")
            };
            _formats.Add(curFormat);
        }

        _connection.Close();
        var formatCB = this.Find<ComboBox>("formatCB");
        formatCB.ItemsSource = _formats;

    }
    public void FillThirdPlace()
    {
        string sql = "select ID, YesOrNo from cursach.ThirdPlace";
        _thirdPlaces = new AvaloniaList<ThirdPlace>();
        _connection = new MySqlConnection(_con);
        _connection.Open();

        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var curThirdPlace = new ThirdPlace()
            {
                ID = reader.GetInt32("ID"),
                YesOrNo = reader.GetString("YesOrNo")
            };
            _thirdPlaces.Add(curThirdPlace);
        }

        _connection.Close();
        var thirdplaceCB = this.Find<ComboBox>("thirdplaceCB");
        thirdplaceCB.ItemsSource = _thirdPlaces;

    }
    public void FillLanguage()
    {
        string sql = "select ID, Name from cursach.Language";
        _languages = new AvaloniaList<Language>();
        _connection = new MySqlConnection(_con);
        _connection.Open();

        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var curLanguage = new Language()
            {
                ID = reader.GetInt32("ID"),
                Name = reader.GetString("Name")
            };
            _languages.Add(curLanguage);
        }

        _connection.Close();
        var languageCB = this.Find<ComboBox>("languageCB");
        languageCB.ItemsSource = _languages;

    }

    private void BackBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }
}
    

