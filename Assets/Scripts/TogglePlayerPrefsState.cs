using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class TogglePlayerPrefsState : MonoBehaviour
{
    [SerializeField] private Toggle Toggle;
    [SerializeField] private string PlayerPrefsKey;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Toggle == null)
        {
            Toggle = GetComponent<Toggle>();
        }
        Toggle.isOn = PlayerPrefs.GetInt(PlayerPrefsKey, 1) == 1;
    }
}
