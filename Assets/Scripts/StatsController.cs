using System;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    public Stats CurrentStats = new Stats();
    private readonly string statsKey = "stats";
    
    public delegate void OnStatsUpdated();
    public event OnStatsUpdated StatsUpdated;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadStats();
    }

    private void LoadStats()
    {
        if (PlayerPrefs.HasKey(statsKey))
        {
            CurrentStats = JsonUtility.FromJson<Stats>(PlayerPrefs.GetString(statsKey));
        }
        else
        {
            CurrentStats = new Stats();
        }

        StatsUpdated?.Invoke();
    }

    public void SaveStats()
    {
        PlayerPrefs.SetString(statsKey, JsonUtility.ToJson(CurrentStats));
        PlayerPrefs.Save();
    }

    public void UpdateStats(GridController.CellType currentPlayer, int duration)
    {
        switch (currentPlayer)
        {
            case GridController.CellType.Blank:
                CurrentStats.Draws++; // use Blank for draws
                break;
            case GridController.CellType.X:
                CurrentStats.XWins++;
                break;
            case GridController.CellType.O:
                CurrentStats.OWins++;
                break;
            default:
                break;
        }

        HandleCommonStats(duration);
        SaveStats();
        StatsUpdated?.Invoke();
    }

    private void HandleCommonStats(int duration)
    {
        CurrentStats.TotalGames++;
        CurrentStats.AverageDuration = (CurrentStats.AverageDuration * (CurrentStats.TotalGames - 1) + duration) / CurrentStats.TotalGames;
    }

    public class Stats
    {
        public int TotalGames;
        public int XWins;
        public int OWins;
        public int Draws;
        public int AverageDuration;
    }
}
