using System;

namespace whottadota;

public class TournamentByUser
{
    public int ID { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; }
    public decimal PrizePool { get; set; }
}