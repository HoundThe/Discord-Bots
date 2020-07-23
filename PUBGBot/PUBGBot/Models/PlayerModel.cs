using System;
using Newtonsoft.Json;

namespace PUBGBot.Models
{
    public partial class PlayerModel
    {
        [JsonProperty("data")] public PlayerModelData Data { get; set; }

        [JsonProperty("links")] public Links Links { get; set; }

        [JsonProperty("meta")] public Meta Meta { get; set; }
    }

    public partial class PlayerModelData
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("attributes")] public Attributes Attributes { get; set; }

        [JsonProperty("relationships")] public Relationships Relationships { get; set; }
    }

    public partial class Attributes
    {
        [JsonProperty("gameModeStats")] public GameModeStats GameModeStats { get; set; }

        [JsonProperty("bestRankPoint")] public double BestRankPoint { get; set; }
    }

    public partial class GameModeStats
    {
        [JsonProperty("duo")] public Duo Duo { get; set; }

        [JsonProperty("duo-fpp")] public Duo DuoFpp { get; set; }

        [JsonProperty("solo")] public Duo Solo { get; set; }

        [JsonProperty("solo-fpp")] public Duo SoloFpp { get; set; }

        [JsonProperty("squad")] public Duo Squad { get; set; }

        [JsonProperty("squad-fpp")] public Duo SquadFpp { get; set; }
    }

    public partial class Duo
    {
        [JsonProperty("assists")] public long Assists { get; set; }

        [JsonProperty("boosts")] public long Boosts { get; set; }

        [JsonProperty("dBNOs")] public long DBnOs { get; set; }

        [JsonProperty("dailyKills")] public long DailyKills { get; set; }

        [JsonProperty("dailyWins")] public long DailyWins { get; set; }

        [JsonProperty("damageDealt")] public double DamageDealt { get; set; }

        [JsonProperty("days")] public long Days { get; set; }

        [JsonProperty("headshotKills")] public long HeadshotKills { get; set; }

        [JsonProperty("heals")] public long Heals { get; set; }

        [JsonProperty("killPoints")] public long KillPoints { get; set; }

        [JsonProperty("kills")] public long Kills { get; set; }

        [JsonProperty("longestKill")] public double LongestKill { get; set; }

        [JsonProperty("longestTimeSurvived")] public double LongestTimeSurvived { get; set; }

        [JsonProperty("losses")] public long Losses { get; set; }

        [JsonProperty("maxKillStreaks")] public long MaxKillStreaks { get; set; }

        [JsonProperty("mostSurvivalTime")] public double MostSurvivalTime { get; set; }

        [JsonProperty("rankPoints")] public double RankPoints { get; set; }

        [JsonProperty("rankPointsTitle")] public string RankPointsTitle { get; set; }

        [JsonProperty("revives")] public long Revives { get; set; }

        [JsonProperty("rideDistance")] public double RideDistance { get; set; }

        [JsonProperty("roadKills")] public long RoadKills { get; set; }

        [JsonProperty("roundMostKills")] public long RoundMostKills { get; set; }

        [JsonProperty("roundsPlayed")] public long RoundsPlayed { get; set; }

        [JsonProperty("suicides")] public long Suicides { get; set; }

        [JsonProperty("swimDistance")] public double SwimDistance { get; set; }

        [JsonProperty("teamKills")] public long TeamKills { get; set; }

        [JsonProperty("timeSurvived")] public double TimeSurvived { get; set; }

        [JsonProperty("top10s")] public long Top10S { get; set; }

        [JsonProperty("vehicleDestroys")] public long VehicleDestroys { get; set; }

        [JsonProperty("walkDistance")] public double WalkDistance { get; set; }

        [JsonProperty("weaponsAcquired")] public long WeaponsAcquired { get; set; }

        [JsonProperty("weeklyKills")] public long WeeklyKills { get; set; }

        [JsonProperty("weeklyWins")] public long WeeklyWins { get; set; }

        [JsonProperty("winPoints")] public long WinPoints { get; set; }

        [JsonProperty("wins")] public long Wins { get; set; }
    }

    public partial class Relationships
    {
        [JsonProperty("matchesDuoFPP")] public Matches MatchesDuoFpp { get; set; }

        [JsonProperty("matchesSquad")] public Matches MatchesSquad { get; set; }

        [JsonProperty("matchesSquadFPP")] public Matches MatchesSquadFpp { get; set; }

        [JsonProperty("season")] public Player Season { get; set; }

        [JsonProperty("player")] public Player Player { get; set; }

        [JsonProperty("matchesSolo")] public Matches MatchesSolo { get; set; }

        [JsonProperty("matchesSoloFPP")] public Matches MatchesSoloFpp { get; set; }

        [JsonProperty("matchesDuo")] public Matches MatchesDuo { get; set; }
    }

    public partial class Matches
    {
        [JsonProperty("data")] public object[] Data { get; set; }
    }

    public partial class Player
    {
        [JsonProperty("data")] public PlayerData Data { get; set; }
    }

    public partial class PlayerData
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("id")] public string Id { get; set; }
    }

    public partial class Links
    {
        [JsonProperty("self")] public Uri Self { get; set; }
    }

    public partial class Meta
    {
    }
}