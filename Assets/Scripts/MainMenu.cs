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

    [Header("Selección de Personaje")]
    public GameObject[] personajes;
    public GameObject contenedorPersonaje;
    public Button botonAnterior;
    public Button botonSiguiente;
    private int indicePersonajeActual = 0;

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

        // Configurar botones de cambio de personaje
        if (botonAnterior != null)
            botonAnterior.onClick.AddListener(CambiarPersonajeAnterior);
        if (botonSiguiente != null)
            botonSiguiente.onClick.AddListener(CambiarPersonajeSiguiente);

        // Mostrar el primer personaje
        MostrarPersonajeActual();
    }

    void MostrarPersonajeActual()
    {
        // Limpiar el contenedor
        foreach (Transform child in contenedorPersonaje.transform)
        {
            Destroy(child.gameObject);
        }

        // Instanciar el personaje actual
        if (personajes != null && personajes.Length > 0)
        {
            GameObject personaje = Instantiate(personajes[indicePersonajeActual], contenedorPersonaje.transform);
            personaje.transform.localPosition = Vector3.zero;
            personaje.transform.localRotation = Quaternion.identity;

            // Obtener el Animator y reproducir la animación
            Animator animator = personaje.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("Look Around");
            }
        }
    }

    public void CambiarPersonajeAnterior()
    {
        if (personajes == null || personajes.Length == 0) return;

        indicePersonajeActual--;
        if (indicePersonajeActual < 0)
            indicePersonajeActual = personajes.Length - 1;

        MostrarPersonajeActual();
    }

    public void CambiarPersonajeSiguiente()
    {
        if (personajes == null || personajes.Length == 0) return;

        indicePersonajeActual++;
        if (indicePersonajeActual >= personajes.Length)
            indicePersonajeActual = 0;

        MostrarPersonajeActual();
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
        // Guardar el índice del personaje seleccionado
        PlayerPrefs.SetInt("PersonajeSeleccionado", indicePersonajeActual);
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("Prueba");
    }
}
