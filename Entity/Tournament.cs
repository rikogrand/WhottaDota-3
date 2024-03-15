using System;
using System.ComponentModel.Design;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace whottadota;

public class Tournament
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Tier { get; set; }
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

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal PrizePool { get; set; }
    public string Location { get; set; }
}