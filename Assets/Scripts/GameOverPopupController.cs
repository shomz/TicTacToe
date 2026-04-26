using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopupController : PopupController
{
    public Button RetryButton;
    public Button ExitButton;
    [SerializeField] private TMP_Text Title;
    [SerializeField] private TMP_Text Duration;

    public void ShowPopup(GridController.CellType currentPlayer, int duration, Action retryButtonAction, Action exitButtonAction)
    {
        switch (currentPlayer)
        {
            case GridController.CellType.Blank:
                Title.text = "It's a draw!";
                break;
            case GridController.CellType.X:
                Title.text = "Player 1 wins!";
                break;
            case GridController.CellType.O:
                Title.text = "Player 2 wins!";
                break;
            default:
                break;
        }

        Duration.text = $"Match Duration: {duration}s";

        RetryButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();

        RetryButton.onClick.AddListener(() =>
        {
            retryButtonAction();
            ClosePopup();
        });
        ExitButton.onClick.AddListener(() =>
        {
            CurrentPopupCloseCallback = exitButtonAction;
            CloseCurrentPopup();
        });

        OpenPopup(exitButtonAction);
    }
}
