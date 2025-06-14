using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject mainMenu;
    public AudioSource backgroundMusic;
    public Slider volumeSlider;
    private bool isInitializing = true;

    void Awake()
    {
        if (backgroundMusic == null) return;

        backgroundMusic.playOnAwake = true;
        backgroundMusic.loop = true;
        backgroundMusic.mute = false;
        backgroundMusic.volume = 0.5f;
        backgroundMusic.Play();
    }

    void Start()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = 0.5f;
            backgroundMusic.mute = false;
            
            if (!backgroundMusic.isPlaying)
            {
                backgroundMusic.Play();
            }
        }

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.onValueChanged.RemoveAllListeners();
            
            isInitializing = true;
            volumeSlider.value = 0.5f;
            isInitializing = false;
            
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float volume)
    {
        if (isInitializing || backgroundMusic == null) return;

        float clampedVolume = Mathf.Clamp(volume, 0f, 1f);
        backgroundMusic.volume = clampedVolume;
        backgroundMusic.mute = false;
    }

    void OnDestroy()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(SetVolume);
        }
    }

    public void OpenOptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void OpenMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Prueba");
    }
}
