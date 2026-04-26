using TMPro;
using UnityEngine;

public class StatsViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text Text;
    [SerializeField] private StatsController StatsController;

    private void Start()
    {
        StatsController.StatsUpdated += UpdateStatsDisplay;
        UpdateStatsDisplay();
    }

    private void UpdateStatsDisplay()
    {
        Text.text =
            $"Total Games: {StatsController.CurrentStats.TotalGames}\n" +
            $"Player 1 Wins: {StatsController.CurrentStats.XWins}\n" +
            $"Player 2 Wins: {StatsController.CurrentStats.OWins}\n" +
            $"Drawn Games: {StatsController.CurrentStats.Draws}\n" +
            $"Average Duration: {StatsController.CurrentStats.AverageDuration}s";
    }
}
