using UnityEngine;

public class PersonajeManager : MonoBehaviour
{
    public GameObject[] personajes; // Array de los 4 personajes en la escena

    void Start()
    {
        // Obtener el índice del personaje seleccionado
        int personajeSeleccionado = PlayerPrefs.GetInt("PersonajeSeleccionado", 0);

        // Desactivar todos los personajes
        foreach (GameObject personaje in personajes)
        {
            if (personaje != null)
            {
                personaje.SetActive(false);
            }
        }

        // Activar el personaje seleccionado
        if (personajeSeleccionado >= 0 && personajeSeleccionado < personajes.Length)
        {
            personajes[personajeSeleccionado].SetActive(true);
        }
    }
} 