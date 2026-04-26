using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GridController;

public class GameManager : Singleton<GameManager>
{
    public CellType CurrentPlayer = CellType.X;
    [SerializeField] private Button CellPrefab;
    [SerializeField] private GridController GridController;
    [SerializeField] private StatsController StatsController;
    [SerializeField] private AudioController AudioController;
    [SerializeField] private ThemeSelector ThemeSelector;
    [SerializeField] private GameOverPopupController GameOverPopupController;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject SettingsButton;
    [SerializeField] private int TimeStart;
    [SerializeField] private Sprite[] AllThemes;
    [SerializeField] private Sprite[] Theme = new Sprite[2];
    private readonly string ThemeKey = "theme";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnHomeSceneLoaded();
    }

    private void OnHomeSceneLoaded()
    {
        if (ThemeSelector == null)
        {
            ThemeSelector = FindFirstObjectByType<ThemeSelector>(FindObjectsInactive.Include);
        }
        int savedThemeIndex = PlayerPrefs.GetInt(ThemeKey, 0);
        SetTheme(savedThemeIndex);

        MainMenu.SetActive(true);
    }

    public void SetTheme(int themeIndex)
    {
        if (themeIndex < 0 || themeIndex >= AllThemes.Length / 2)
        {
            Debug.LogError("Invalid theme index");
            return;
        }

        Theme[0] = AllThemes[themeIndex * 2];
        Theme[1] = AllThemes[themeIndex * 2 + 1];

        PlayerPrefs.SetInt(ThemeKey, themeIndex);

        AudioController.PlayClick();
        ThemeSelector.SelectTheme(themeIndex);
    }

    public void StartGame()
    {
        MainMenu.SetActive(false);
        PopupController.CloseCurrentPopup();

        var sceneLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        sceneLoad.completed += (AsyncOperation op) =>
        {
            //SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));

            GridController = FindFirstObjectByType<GridController>();
            GameOverPopupController = FindFirstObjectByType<GameOverPopupController>();

            InitGame();
        };
    }

    private void InitGame()
    {
        var hud = FindFirstObjectByType<HUDController>();

        CurrentPlayer = CellType.X;

        GridController.ResetGrid();
        GridController.SpawnCells((index) =>
        {
            Button cellButton = Instantiate(CellPrefab, GridController.transform);
            cellButton.onClick.AddListener(() =>
            {
                var valid = PlayMove(index);
                if (valid)
                {
                    cellButton.GetComponent<Image>().sprite = CurrentPlayer != CellType.X ? Theme[0] : Theme[1];
                    hud.MovePlayed();
                }
            });
        });

        TimeStart = (int)Time.realtimeSinceStartup;

        hud.Init(TimeStart, SettingsButton);
    }

    public bool PlayMove(int index)
    {
        var Grid = GridController.Grid;

        if (index < 0 || index >= Grid.Length)
        {
            Debug.LogError("Index out of bounds");
            return false;
        }

        if (Grid[index] != CellType.Blank)
        {
            Debug.LogError("Cell is already occupied");
            return false;
        }

        Grid[index] = CurrentPlayer;
        if (GridController.CheckVictory(CurrentPlayer))
        {
            Debug.Log($"{CurrentPlayer} wins!");
            int duration = (int)Time.realtimeSinceStartup - TimeStart;
            StatsController.UpdateStats(CurrentPlayer, duration);
            GameOverPopupController.ShowPopup(CurrentPlayer, duration, InitGame, GoMainMenu);
            AudioController.PlayWin();
        }
        else if (Grid.All(cell => cell != CellType.Blank))
        {
            Debug.Log("It's a draw!");
            int duration = (int)Time.realtimeSinceStartup - TimeStart;
            StatsController.UpdateStats(CellType.Blank, duration);
            GameOverPopupController.ShowPopup(CellType.Blank, duration, InitGame, GoMainMenu);
            AudioController.PlayWin();
        }
        else
        {
            AudioController.PlayMove();
        }

        CurrentPlayer = CurrentPlayer == CellType.X ?
            CellType.O :
            CellType.X;

        return true;
    }

    public void GoMainMenu()
    {
        GameOverPopupController = null;
        GridController = null;
        var sceneUnload = SceneManager.UnloadSceneAsync(1);
        sceneUnload.completed += (AsyncOperation op) =>
        {
            OnHomeSceneLoaded();
        };
    }

    public void Quit()
    {
        Application.Quit();
    }
}
