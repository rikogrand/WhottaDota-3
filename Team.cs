using System;
using System.IO;
using Avalonia.Media.Imaging;

namespace whottadota;

public class Team
{
    public int ID { get; set; }
    public byte[] Image { get; set; }
    public Uri Logo => new Uri($"avares://whottadota/Image/{Image}");

    public Bitmap bitmap
    {
        get
        {
            using var memoryStream = new MemoryStream(Image);
            return new Bitmap(memoryStream);
        }
    }

    public string PlayerList { get; set; }
    public string Location { get; set; }
    public decimal WinMoney { get; set; }
    public string Link { get; set; }

}