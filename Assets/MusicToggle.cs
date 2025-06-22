using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    public AudioSource musicSource;
    public Button toggleButton;
    public Sprite soundOnIcon;
    public Sprite soundOffIcon;

    private bool isMuted = false;

    void Start()
    {
        toggleButton.onClick.AddListener(ToggleMusic);
        UpdateIcon();
    }

    void ToggleMusic()
    {
        isMuted = !isMuted;
        musicSource.mute = isMuted;
        UpdateIcon();
    }

    void UpdateIcon()
    {
        toggleButton.image.sprite = isMuted ? soundOffIcon : soundOnIcon;
    }
}
