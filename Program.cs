// See https://aka.ms/new-console-template for more information

using GeneaGrab_JsonToDb.New;
using GeneaGrab_JsonToDb.New.Models;
using GeneaGrab_JsonToDb.New.Models.Dates;
using GeneaGrab_JsonToDb.Old.Dates;
using Microsoft.EntityFrameworkCore;
using Date = GeneaGrab_JsonToDb.New.Models.Dates.Date;
var floatingScale = new[] { "AD06", "AD17", "AD79-86", "Antenati" };

Console.WriteLine("Reading JSON files");
var registries = await GeneaGrab_JsonToDb.Old.LocalData.LoadDataAsync();

Console.WriteLine("Creating db");
await using var db = new DatabaseContext();
db.Database.Migrate();

Console.WriteLine("Generating new entries");
await db.Registries.AddRangeAsync(registries.Select(ToNewReg));
Console.WriteLine("Saving entries");
await db.SaveChangesAsync();
Console.WriteLine("Done");
return;

Registry ToNewReg(GeneaGrab_JsonToDb.Old.Registry oldReg)
{
    Console.WriteLine("Processing " + oldReg.ID + " (" + oldReg.ProviderID + ")");
    return new Registry(oldReg.ProviderID, oldReg.ID)
    {
        CallNumber = oldReg.CallNumber,
        URL = oldReg.URL,
        ArkURL = oldReg.ArkURL,
        Types = oldReg.Types.ToList(),

        Title = oldReg.Title,
        Subtitle = oldReg.Subtitle,
        Author = oldReg.Author,
        Location = ToNewLocation(oldReg.LocationDetails, oldReg.Location, oldReg.District),
        Notes = oldReg.Notes,


        Extra = oldReg.Extra,
        From = OldToNewDate(oldReg.From),
        To = OldToNewDate(oldReg.To),
        Frames = oldReg.Pages.Select(oldPage => new Frame
        {
            ProviderId = oldReg.ProviderID,
            RegistryId = oldReg.ID,
            FrameNumber = oldPage.Number,
            ArkUrl = oldPage.URL,
            DownloadUrl = oldPage.DownloadURL ?? oldPage.URL,
            ImageSize = GetScale(oldPage.Zoom, oldPage.MaxZoom, oldReg.ProviderID),
            Width = oldPage.Width,
            Height = oldPage.Height,
            TileSize = oldPage.TileSize,
            Notes = oldPage.Notes,
            Extra = oldPage.Extra
        }).ToArray()
    };
}

Scale GetScale(int inZoom, int? maxZoom, string providerId)
{
    double zoom = inZoom;
    if (floatingScale.Contains(providerId)) zoom /= 100;
    
    if (zoom < 0) return Scale.Unavailable;
    if (zoom < 0.5) return Scale.Thumbnail;
    if (inZoom == maxZoom) return Scale.Full;
    if (providerId == "AMNice") return  inZoom == 2 ? Scale.Full : Scale.Navigation;
    if (providerId != "Geneanet" && Math.Abs(zoom - 1.0) < .01) return Scale.Full;
    return Scale.Navigation;
}

List<string> ToNewLocation(string[]? details, string? city, string? district)
{
    var list = new List<string>();
    if (details != null) list.AddRange(details);
    if (city != null)
    {
        var item = list.Find(l => string.Equals(l, city, StringComparison.CurrentCultureIgnoreCase));
        var locationInDetails = item is null ? -1 : list.IndexOf(item);
        if (locationInDetails >= 0) list[locationInDetails] = city;
        else list.Add(city);
    }
    if (district != null) list.Add(district);
    return list;
}

Date? OldToNewDate(GeneaGrab_JsonToDb.Old.Dates.Date? old)
{
    if (old is null) return null;
    var dt = new DateTime(old.Year.Value, old.Month?.Value ?? 1, old.Day?.Value ?? 1, old.Hour?.Value ?? 0, old.Minute?.Value ?? 0, old.Second?.Value ?? 0, DateTimeKind.Utc);
    return old.Calendar switch
    {
        Calendar.Gregorian => new GregorianDate(dt, old.Precision),
        Calendar.Julian => new JulianDate(dt),
        Calendar.FrenchRepublican => new FrenchRepublicanDate(dt),
        _ => null
    };
}
