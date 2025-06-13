using UnityEngine;

public class SanitySystem : MonoBehaviour
{
    public float maxSanity = 100f;
    public float currentSanity;
    public float decayPerSecond = 2f;

    void Start()
    {
        currentSanity = maxSanity;
    }

    void Update()
    {
        currentSanity -= decayPerSecond * Time.deltaTime;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);

        if (currentSanity < 25f)
        {
            Debug.Log("Cordura crítica: aplicar efectos de locura aquí.");
        }
    }

    public void IncreaseSanity(float amount)
    {
        currentSanity = Mathf.Clamp(currentSanity + amount, 0, maxSanity);
    }

    public void DecreaseSanity(float amount)
    {
        currentSanity = Mathf.Clamp(currentSanity - amount, 0, maxSanity);
    }
}