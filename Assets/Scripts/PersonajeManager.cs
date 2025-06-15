using UnityEngine;

public class PersonajeManager : MonoBehaviour
{
    public GameObject[] personajes; // Array de los 4 personajes en la escena

    void Start()
    {
        // Obtener el índice del personaje seleccionado
        int personajeSeleccionado = PlayerPrefs.GetInt("PersonajeSeleccionado", 0);
        Debug.Log("Personaje seleccionado: " + personajeSeleccionado);

        // Verificar si hay personajes asignados
        if (personajes == null || personajes.Length == 0)
        {
            Debug.LogError("No hay personajes asignados en el PersonajeManager!");
            return;
        }

        // Desactivar todos los personajes primero
        foreach (GameObject personaje in personajes)
        {
            if (personaje != null)
            {
                personaje.SetActive(false);
                Debug.Log("Personaje desactivado: " + personaje.name);
            }
            else
            {
                Debug.LogError("Hay un slot vacío en el array de personajes!");
            }
        }

        // Activar solo el personaje seleccionado
        if (personajeSeleccionado >= 0 && personajeSeleccionado < personajes.Length)
        {
            personajes[personajeSeleccionado].SetActive(true);
            Debug.Log("Personaje activado: " + personajes[personajeSeleccionado].name);
        }
        else
        {
            Debug.LogError("Índice de personaje fuera de rango: " + personajeSeleccionado);
        }
    }
} 