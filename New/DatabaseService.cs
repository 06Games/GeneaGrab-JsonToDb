﻿using GeneaGrab_JsonToDb.New.Models;
using GeneaGrab_JsonToDb.New.Models.Dates;
using GeneaGrab_JsonToDb.New.Models.Indexing;
using GeneaGrab_JsonToDb.Old;
using GeneaGrab.Models.Indexing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Registry = GeneaGrab_JsonToDb.New.Models.Registry;

namespace GeneaGrab_JsonToDb.New;

public class DatabaseContext : DbContext
{
    private sealed class JsonConverter<T>() : ValueConverter<T, string>(v => JsonConvert.SerializeObject(v), v => JsonConvert.DeserializeObject<T>(v)!);

    public DbSet<Registry> Registries { get; set; } = null!;
    public DbSet<Frame> Frames { get; set; } = null!;
    public DbSet<Record> Records { get; set; } = null!;
    public DbSet<Person> Persons { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={LocalData.AppData}/data.db");
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Enum>().HaveConversion<string>(); // Store enums as string
        configurationBuilder.Properties<Date>().HaveConversion<JsonConverter<Date>>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Record>(e =>
            {
                e.Property(b => b.Position).HasConversion<JsonConverter<Rect>>();
                e.HasOne(r => r.Frame).WithMany().HasForeignKey(r => new { r.ProviderId, r.RegistryId, r.FrameNumber });
            })
            .Entity<Registry>(e =>
            {
                e.HasMany(r => r.Frames).WithOne(f => f.Registry).HasForeignKey(r => new { r.ProviderId, r.RegistryId });
                e.Property(r => r.Extra).HasConversion<JsonConverter<object>>();
                e.Property(r => r.Types).HasConversion(
                    v => JsonConvert.SerializeObject(v, Formatting.None, new StringEnumConverter()),
                    v => JsonConvert.DeserializeObject<List<RegistryType>>(v)!);
            })
            .Entity<Frame>(e =>
            {
                e.Property(f => f.Extra).HasConversion<JsonConverter<object>>();
            });
    }
}
