using System;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Hora del día")]
    [Range(0.0f, 24f)]
    public float Hour = 12;
    public float DayTimeMinutes = 1;
    public Transform Sun;

    private float SunX;

    private void Update()
    {
        Hour += Time.deltaTime * (24 / (60 * DayTimeMinutes));
        if (Hour >= 24)
        {
            Hour = 0;
        }
        RotationSun();
    }

    private void RotationSun()
    {
        SunX = 15 * Hour;
        Sun.localEulerAngles = new Vector3(SunX, 0, 0);
        if (Hour < 6 || Hour > 18)
        {
            Sun.GetComponent<Light>().intensity = 0;
        }
        else
        {
            Sun.GetComponent<Light>().intensity = 1;
        }
    }
}