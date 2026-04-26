using UnityEngine;
using UnityEngine.UI;

public class ThemeSelector : MonoBehaviour
{
    [SerializeField] Transform[] Themes;

    public void SelectTheme(int themeIndex)
    {
        if (themeIndex < 0 || themeIndex >= Themes.Length)
        {
            Debug.LogError("Invalid theme index");
            return;
        }
        for (int i = 0; i < Themes.Length; i++)
        {
            Themes[i].GetComponent<Image>().enabled = (i == themeIndex);
        }
    }
}
