using UnityEngine;
using System.Collections;

public class WeatherRandomizer : MonoBehaviour
{
    [Header("Clima")]
    public GameObject rainSystem;
    public VFXTools.VFXController rainController;
    public float rainIntensity = 2f;

    [Header("Tiempos (en segundos)")]
    public float waitTime = 180f; // 3 minutos
    public float rainDuration = 60f; // 1 minuto

    void Start()
    {
        StartCoroutine(RandomWeatherRoutine());
    }

    IEnumerator RandomWeatherRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            if (rainSystem) rainSystem.SetActive(true);
            if (rainController) rainController.SetIntensity(rainIntensity);

            Debug.Log("Lluvia activada");

            yield return new WaitForSeconds(rainDuration);

            if (rainSystem) rainSystem.SetActive(false);
            Debug.Log("Fin de la lluvia");

        }
    }
}