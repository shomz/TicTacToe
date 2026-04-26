using UnityEngine;

public class AudioController : Singleton<AudioController>
{
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioSource AudioSourceBG;
    [SerializeField] private AudioClip ClickSound;
    [SerializeField] private AudioClip MoveSound;
    [SerializeField] private AudioClip PopupSound;
    [SerializeField] private AudioClip WinSound;
    [SerializeField] private AudioClip ThemeMusic;
    private readonly string BGKey = "bg";
    private readonly string SFXKey = "sfx";

    private void Start()
    {
        AudioSourceBG.clip = ThemeMusic;
        AudioSourceBG.Play();
        AudioSourceBG.loop = true;

        var bg = PlayerPrefs.GetInt(BGKey, 1);
        AudioSourceBG.mute = bg == 0;

        var sfx = PlayerPrefs.GetInt(SFXKey, 1);
        AudioSource.mute = sfx == 0;
    }

    public void PlayClick()
    {
        AudioSource.PlayOneShot(ClickSound);
    }

    public void PlayMove()
    {
        AudioSource.PlayOneShot(MoveSound);
    }

    public void PlayPopup()
    {
        AudioSource.PlayOneShot(PopupSound);
    }

    public void PlayWin()
    {
        AudioSource.PlayOneShot(WinSound);
    }

    public void ToggleBGMusic(bool on)
    {
        PlayerPrefs.SetInt(BGKey, on ? 1 : 0);
        AudioSourceBG.mute = !on;
    }

    public void ToggleSFX(bool on)
    {
        PlayerPrefs.SetInt(SFXKey, on ? 1 : 0);
        AudioSource.mute = !on;
    }
}
