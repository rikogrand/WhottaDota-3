using System.Collections.Generic;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;

namespace whottadota;

public partial class TournamentGridWindow : Window
{
    private readonly string _con = "server=localhost;database=cursach;user=root;password=root";
    private readonly Tournament _selectedTournament;
    private MySqlConnection _connection;
    private GridData _gridData;
    private AvaloniaList<Placement> _placements;
    private AvaloniaList<Team> _teams;

    public TournamentGridWindow(Tournament selectedTournament)
    {
        InitializeComponent();
        _selectedTournament = selectedTournament;
        FillTeam1();
        FillPlacement1();
        //  InputData();
    }

    public bool Result { get; private set; }

    private void SaveBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        _connection = new MySqlConnection(_con);
        _connection.Open();

        if (_selectedTournament == null)
        {
            this.Close();
        }
        else if (_selectedTournament != null)
        {
            List<ComboBox> comboBoxes = new List<ComboBox>
            {
                firstTeamCB, firstPlaceCB,
                secondTeamCB, secondPlaceCB,
                thirdTeamCB, thirdPlaceCB,
                fourthTeamCB, fourthPlaceCB,
                fifthTeamCB, fifthPlaceCB,
                sixthTeamCB, sixthPlaceCB,
                seventhTeamCB, seventhPlaceCB
            };
            string sql1 =
                "insert into GridData (FirstPlace, FirstTeam, SecondPlace, " +
                "SecondTeam, ThirdPlace, ThirdTeam, FourthPlace, FourthTeam, " +
                "FifthPlace, FifthTeam, SixthPlace, SixthTeam, SeventhPlace, SeventhTeam, Tournament) " +
                "values (@FirstPlace, @FirstTeam, @SecondPlace, @SecondTeam, @ThirdPlace, @ThirdTeam, @FourthPlace, " +
                "@FourthTeam, @FifthPlace, @FifthTeam, @SixthPlace, @SixthTeam, @SeventhPlace, @SeventhTeam, @Tournament)";

            MySqlCommand command1 = new MySqlCommand(sql1, _connection);

            GridData gridData = new GridData
            {
                FirstPlace = (firstPlaceCB.SelectedItem as Placement)?.Name,
                FirstTeam = (firstTeamCB.SelectedItem as Team)?.Name,
                SecondPlace = (secondPlaceCB.SelectedItem as Placement)?.Name,
                SecondTeam = (secondTeamCB.SelectedItem as Team)?.Name,
                ThirdPlace = (thirdPlaceCB.SelectedItem as Placement)?.Name,
                ThirdTeam = (thirdTeamCB.SelectedItem as Team)?.Name,
                FourthPlace = (fourthPlaceCB.SelectedItem as Placement)?.Name,
                FourthTeam = (fourthTeamCB.SelectedItem as Team)?.Name,
                FifthPlace = (fifthPlaceCB.SelectedItem as Placement)?.Name,
                FifthTeam = (fifthTeamCB.SelectedItem as Team)?.Name,
                SixthPlace = (sixthPlaceCB.SelectedItem as Placement)?.Name,
                SixthTeam = (sixthTeamCB.SelectedItem as Team)?.Name,
                SeventhPlace = (seventhPlaceCB.SelectedItem as Placement)?.Name,
                SeventhTeam = (seventhTeamCB.SelectedItem as Team)?.Name,
                Tournament = _selectedTournament.ID
            };

            foreach (var comboBox in comboBoxes)
            {
                if (comboBox.SelectedItem is whottadota.Team team && comboBox.Name.EndsWith("TeamCB"))
                {
                    var property = gridData.GetType().GetProperty(comboBox.Name.Replace("CB", ""));
                    property?.SetValue(gridData, team.ID);
                }
                else if (comboBox.SelectedItem is whottadota.Placement placement && comboBox.Name.EndsWith("PlaceCB"))
                {
                    var property = gridData.GetType().GetProperty(comboBox.Name.Replace("CB", ""));
                    property?.SetValue(gridData, placement.ID);
                }
            }

            foreach (var property in gridData.GetType().GetProperties())
            {
                if (property.Name != "ID")
                {
                    command1.Parameters.AddWithValue($"@{property.Name}", property.GetValue(gridData));
                }
            }

            command1.ExecuteNonQuery();

            TournamentGuestGrid tournament = new TournamentGuestGrid(_selectedTournament, gridData);
            tournament.InitialGridData();
            _connection.Close();
            this.Close();
            Close(true);
        }
    }

    public void Close(bool result)
    {
        Result = result;
        base.Close(result);
    }

    public void FillTeam1()
    {
        MySqlConnection _connection;
        string sql = "Select ID, Name from cursach.Team";
        _teams = new AvaloniaList<Team>();
        _connection = new MySqlConnection(_con);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var curTeam1 = new Team()
            {
                ID = reader.GetInt32("ID"),
                Name = reader.GetString("Name")
            };
            _teams.Add(curTeam1);
        }

        _connection.Close();

        var Team1 = this.Find<ComboBox>("firstTeamCB");
        Team1.ItemsSource = _teams;
        var Team2 = this.Find<ComboBox>("secondTeamCB");
        Team2.ItemsSource = _teams;
        var Team3 = this.Find<ComboBox>("thirdTeamCB");
        Team3.ItemsSource = _teams;
        var Team4 = this.Find<ComboBox>("fourthTeamCB");
        Team4.ItemsSource = _teams;
        var Team5 = this.Find<ComboBox>("fifthTeamCB");
        Team5.ItemsSource = _teams;
        var Team6 = this.Find<ComboBox>("sixthTeamCB");
        Team6.ItemsSource = _teams;
        var Team7 = this.Find<ComboBox>("seventhTeamCB");
        Team7.ItemsSource = _teams;
    }

    public void FillPlacement1()
    {
        MySqlConnection _connection;
        string sql222 = "Select ID, Name from cursach.Placement";
        _placements = new AvaloniaList<Placement>();
        _connection = new MySqlConnection(_con);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql222, _connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var curPlacement = new Placement()
            {
                ID = reader.GetInt32("ID"),
                Name = reader.GetString("Name")
            };
            _placements.Add(curPlacement);
        }

        _connection.Close();

        var Placement1 = this.Find<ComboBox>("firstPlaceCB");
        Placement1.ItemsSource = _placements;
        var Placement2 = this.Find<ComboBox>("secondPlaceCB");
        Placement2.ItemsSource = _placements;
        var Placement3 = this.Find<ComboBox>("thirdPlaceCB");
        Placement3.ItemsSource = _placements;
        var Placement4 = this.Find<ComboBox>("fourthPlaceCB");
        Placement4.ItemsSource = _placements;
        var Placement5 = this.Find<ComboBox>("fifthPlaceCB");
        Placement5.ItemsSource = _placements;
        var Placement6 = this.Find<ComboBox>("sixthPlaceCB");
        Placement6.ItemsSource = _placements;
        var Placement7 = this.Find<ComboBox>("seventhPlaceCB");
        Placement7.ItemsSource = _placements;
    }
}