using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TMP_Text DurationText;
    [SerializeField] private TMP_Text MoveText;
    private int moves = 0;
    private int startTime;

    public void Init(int timeStarted, GameObject settingsButton)
    {
        moves = 0;
        startTime = timeStarted;

        UpdateText();

        Instantiate(settingsButton, transform); // not too proud of this one
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    public void MovePlayed()
    {
        moves++;
    }

    private void UpdateText()
    {
        DurationText.text = $"Match Duration: {(int)Time.realtimeSinceStartup - startTime}s";
        MoveText.text = $"Move Count: {moves}";
    }
}
