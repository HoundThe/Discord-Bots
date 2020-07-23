using System.Collections.Generic;

namespace ApexBot.Services.Model
{
    public class Metadata
{
    public string? LegendName { get; set; }
    public string? Icon { get; set; }
    public string? Bgimage { get; set; }
    public bool ? IsActive { get; set; }
}

public class Metadata2
{
    public string? Key { get; set; }
    public string? Name { get; set; }
    public string? CategoryKey { get; set; }
    public string? CategoryName { get; set; }
    public bool ?IsReversed { get; set; }
}

public class Stat
{
    public Metadata2? Metadata { get; set; }
    public double? Value { get; set; }
    public double? Percentile { get; set; }
    public string? DisplayValue { get; set; }
    public string? DisplayRank { get; set; }
    public int Rank { get; set; }
}

public class Child
{
    public string? Id { get; set; }
    public string? Type { get; set; }
    public Metadata? Metadata { get; set; }
    public List<Stat>? Stats { get; set; }
}

public class Metadata3
{
    public List<string>? StatsCategoryOrder { get; set; }
    public int ?PlatformId { get; set; }
    public string? PlatformUserHandle { get; set; }
    public string? AccountId { get; set; }
    public string? CacheExpireDate { get; set; }
    public int Level { get; set; }
    public string? AvatarUrl { get; set; }
    public string? CountryCode { get; set; }
    public int Collections { get; set; }
    public int ActiveLegend { get; set; }
}

public class Metadata4
{
    public string? Key { get; set; }
    public string? Name { get; set; }
    public string? CategoryKey { get; set; }
    public string? CategoryName { get; set; }
    public bool IsReversed { get; set; }
}

public class Stat2
{
    public Metadata4? Metadata { get; set; }
    public double Value { get; set; }
    public double Percentile { get; set; }
    public int Rank { get; set; }
    public string? DisplayValue { get; set; }
    public string? DisplayRank { get; set; }
}

public class Data
{
    public string? Id { get; set; }
    public string? Type { get; set; }
    public List<Child>? Children { get; set; }
    public Metadata3? Metadata { get; set; }
    public List<Stat2>? Stats { get; set; }
}

public class StatisticsModel
{
    public Data? Data { get; set; }
}
}