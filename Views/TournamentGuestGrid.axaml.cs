using System.Linq;
using Avalonia.Collections;
using Avalonia.Controls;
using MySql.Data.MySqlClient;

namespace whottadota;

public partial class TournamentGuestGrid : Window
{
    private readonly string _con = "server=localhost;database=cursach;user=root;password=root";
    private readonly MySqlConnection _connection;
    private readonly GridData _gridData;
    private readonly Tournament _selectedTournament;
    public AvaloniaList<GridData> _GridDatas;

    public TournamentGuestGrid(Tournament selectedTournament, GridData data)
    {
        //  TournamentGridWindow tournamentGridWindow = new TournamentGridWindow(_selectedTournament);
        // tournamentGridWindow.InputData();
        _gridData = data;
        _selectedTournament = selectedTournament;
        InitializeComponent();
        FillGridData();
    }

    public void FillGridData()
    {
        MySqlConnection _connection;
        string sql = "Select ID, FirstPlace, FirstTeam, SecondPlace, SecondTeam, ThirdPlace, ThirdTeam," +
                     " FourthPlace, FourthTeam, FifthPlace, FifthTeam, SixthPlace, SixthTeam, " +
                     "SeventhPlace, SeventhTeam, Tournament from cursach.GridData " +
                     " where Tournament = @tournamentId";
        _GridDatas = new AvaloniaList<GridData>();
        _connection = new MySqlConnection(_con);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        command.Parameters.Add("tournamentId", MySqlDbType.Int32);
        command.Parameters["@tournamentId"].Value = _selectedTournament.ID;
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var curGridData = new GridData()
            {
                ID = reader.GetInt32("ID"),
                FirstPlace = reader.GetString("FirstPlace"),
                FirstTeam = reader.GetString("FirstTeam"),
                SecondPlace = reader.GetString("SecondPlace"),
                SecondTeam = reader.GetString("SecondTeam"),
                ThirdPlace = reader.GetString("ThirdPlace"),
                ThirdTeam = reader.GetString("ThirdTeam"),
                FourthPlace = reader.GetString("FourthPlace"),
                FourthTeam = reader.GetString("FourthTeam"),
                FifthPlace = reader.GetString("FifthPlace"),
                FifthTeam = reader.GetString("FifthTeam"),
                SixthPlace = reader.GetString("SixthPlace"),
                SixthTeam = reader.GetString("SixthTeam"),
                SeventhPlace = reader.GetString("SeventhPlace"),
                SeventhTeam = reader.GetString("SeventhTeam"),
                Tournament = reader.GetInt32("Tournament")
            };
            _GridDatas.Add(curGridData);
        }

        _connection.Close();

        var FirstPlace = this.Find<TextBox>("FirstPlaceTextBox");
        FirstPlace.Text = _GridDatas.First().FirstPlace;

        var FirstTeam = this.Find<TextBox>("FirstTeamTextBox");
        FirstTeam.Text = _GridDatas.First().FirstTeam;

        var SecondPlace = this.Find<TextBox>("SecondPlaceTextBox");
        SecondPlace.Text = _GridDatas.First().SecondPlace;

        var SecondTeam = this.Find<TextBox>("SecondTeamTextBox");
        SecondTeam.Text = _GridDatas.First().SecondTeam;

        var ThirdPlace = this.Find<TextBox>("ThirdPlaceTextBox");
        ThirdPlace.Text = _GridDatas.First().ThirdPlace;

        var ThirdTeam = this.Find<TextBox>("ThirdTeamTextBox");
        ThirdTeam.Text = _GridDatas.First().ThirdTeam;

        var FourthPlace = this.Find<TextBox>("FourthPlaceTextBox");
        FourthPlace.Text = _GridDatas.First().FourthPlace;

        var FourthTeam = this.Find<TextBox>("FourthTeamTextBox");
        FourthTeam.Text = _GridDatas.First().FourthTeam;

        var FifthPlace = this.Find<TextBox>("FifthPlaceTextBox");
        FifthPlace.Text = _GridDatas.First().FifthPlace;

        var FifthTeam = this.Find<TextBox>("FifthTeamTextBox");
        FifthTeam.Text = _GridDatas.First().FifthTeam;

        var SixthPlace = this.Find<TextBox>("SixthPlaceTextBox");
        SixthPlace.Text = _GridDatas.First().SixthPlace;

        var SixthTeam = this.Find<TextBox>("SixthTeamTextBox");
        SixthTeam.Text = _GridDatas.First().SixthTeam;

        var SeventhPlace = this.Find<TextBox>("SeventhPlaceTextBox");
        SeventhPlace.Text = _GridDatas.First().SeventhPlace;

        var SeventhTeam = this.Find<TextBox>("SeventhTeamTextBox");
        SeventhTeam.Text = _GridDatas.First().SeventhTeam;
    }

    public void InitialGridData()
    {
        if (_selectedTournament == null)
        {
            this.Close();
        }
        else if (_selectedTournament != null && _gridData != null)
        {
            FirstPlaceTextBox.Text = _gridData.FirstPlace;
            FirstPlaceTextBox.IsReadOnly = true;
            FirstTeamTextBox.Text = _gridData.FirstTeam;
            FirstTeamTextBox.IsReadOnly = true;
            SecondPlaceTextBox.Text = _gridData.SecondPlace;
            SecondPlaceTextBox.IsReadOnly = true;
            SecondTeamTextBox.Text = _gridData.SecondTeam;
            SecondPlaceTextBox.IsReadOnly = true;
            ThirdPlaceTextBox.Text = _gridData.ThirdPlace;
            ThirdPlaceTextBox.IsReadOnly = true;
            ThirdTeamTextBox.Text = _gridData.ThirdTeam;
            ThirdTeamTextBox.IsReadOnly = true;
            FourthPlaceTextBox.Text = _gridData.FourthPlace;
            FourthPlaceTextBox.IsReadOnly = true;
            FourthTeamTextBox.Text = _gridData.FourthTeam;
            FourthTeamTextBox.IsReadOnly = true;
            FifthPlaceTextBox.Text = _gridData.FifthPlace;
            FifthPlaceTextBox.IsReadOnly = true;
            FifthTeamTextBox.Text = _gridData.FifthTeam;
            FifthTeamTextBox.IsReadOnly = true;
            SixthPlaceTextBox.Text = _gridData.SixthPlace;
            SixthPlaceTextBox.IsReadOnly = true;
            SixthTeamTextBox.Text = _gridData.SixthTeam;
            SixthTeamTextBox.IsReadOnly = true;
            SeventhPlaceTextBox.Text = _gridData.SeventhPlace;
            SeventhPlaceTextBox.IsReadOnly = true;
            SeventhTeamTextBox.Text = _gridData.SeventhTeam;
            SeventhTeamTextBox.IsReadOnly = true;
        }
    }
}