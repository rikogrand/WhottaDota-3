using System;

namespace whottadota;

public class Player
{
    public int ID { get; set; }
    public string Initials { get; set; }
    public string Nickname { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Team { get; set; }
}