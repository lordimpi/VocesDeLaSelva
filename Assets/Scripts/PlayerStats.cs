using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerStats : MonoBehaviour
{
    [Header("Estadísticas Base")]
    [SerializeField] private float thirst = 100;
    [SerializeField] private float hunger = 100;
    [SerializeField] private float sanity = 100;

    [Header("Tasas de Consumo Base (por 300 segundos = 5 minutos)")]
    [SerializeField] private float baseThirstConsumeRate = 100f;    // 100% en 5 minutos
    [SerializeField] private float baseHungerConsumeRate = 80f;     // 80% en 5 minutos
    [SerializeField] private float baseSanityConsumeRate = 60f;     // 60% en 5 minutos

    [Header("Efectos de Correr")]
    [SerializeField] private float runThirstMultiplier = 2f;      // 2x más sed al correr
    [SerializeField] private float runHungerMultiplier = 1.5f;    // 1.5x más hambre al correr
    [SerializeField] private float runSanityMultiplier = 1.2f;    // 1.2x más cansancio al correr

    [Header("Efectos de Estadísticas Bajas")]
    [SerializeField] private float lowStatThreshold = 20f;        // Umbral para considerar una estadística baja
    [SerializeField] private float criticalStatThreshold = 10f;   // Umbral para considerar una estadística crítica
    [SerializeField] private float damagePerSecond = 10f;         // Daño por segundo cuando las estadísticas están críticas

    [Header("Efectos de Sed")]
    [SerializeField] private float noRunThirstThreshold = 15f;    // No puede correr si la sed está por debajo de este valor
    [SerializeField] private float slowWalkThirstThreshold = 30f; // Camina más lento si la sed está por debajo de este valor

    private float actualThirstConsumeRate;
    private float actualHungerConsumeRate;
    private float actualSanityConsumeRate;
    private bool isRunning = false;
    private bool isExhausted = false;

    // Propiedades públicas para acceder a los valores
    public float Thirst => thirst;
    public float Hunger => hunger;
    public float Sanity => sanity;
    public bool CanRun => thirst > noRunThirstThreshold;
    public bool IsExhausted => isExhausted;
    public float WalkSpeedMultiplier => thirst < slowWalkThirstThreshold ? 0.7f : 1f;

    private void Start()
    {
        // Inicializar tasas de consumo
        actualThirstConsumeRate = baseThirstConsumeRate;
        actualHungerConsumeRate = baseHungerConsumeRate;
        actualSanityConsumeRate = baseSanityConsumeRate;
        isRunning = false;

        // Suscribirse a eventos
        PlayerEvents.Revive += replyPlayerRevive;
        PlayerEvents.Run += replyPlayerRun;
        PlayerEvents.StopRun += replyStopRun;
        PlayerEvents.Drink += replyDrink;
        PlayerEvents.Eat += replyEat;
        PlayerEvents.Rest += replyRest;
        PlayerEvents.HealthUpdate += replyHealthUpdate;

        StartCoroutine(UpdateStats());
    }

    private IEnumerator UpdateStats()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            consumeStats();
            checkStatEffects();
            UpdateUI();
        }
    }

    private void consumeStats()
    {
        // Consumir estadísticas (dividido por 300 para que dure 5 minutos)
        thirst -= actualThirstConsumeRate / 300f;
        hunger -= actualHungerConsumeRate / 300f;
        sanity -= actualSanityConsumeRate / 300f;

        // Asegurar que los valores no sean negativos
        thirst = Mathf.Max(0, thirst);
        hunger = Mathf.Max(0, hunger);
        sanity = Mathf.Max(0, sanity);
    }

    private void checkStatEffects()
    {
        // Verificar efectos de sed
        if (thirst <= noRunThirstThreshold)
        {
            if (isRunning)
            {
                PlayerEvents.StopRun?.Invoke();
            }
        }

        // Verificar daño por estadísticas críticas
        if (thirst <= 0 || hunger <= 0 || sanity <= 0)
        {
            // Aplicar daño directamente al sistema de vida
            PlayerEvents.TakeDamage?.Invoke(damagePerSecond);
        }

        // Verificar agotamiento
        if (sanity <= criticalStatThreshold)
        {
            isExhausted = true;
            PlayerEvents.Exhausted?.Invoke();
        }
        else
        {
            isExhausted = false;
        }
    }

    private void UpdateUI()
    {
        PlayerEvents.StatsUpdate?.Invoke(hunger, thirst, sanity);
    }

    #region EVENTS
    private void replyPlayerRevive()
    {
        thirst = 100;
        hunger = 100;
        sanity = 100;
        isExhausted = false;
        isRunning = false;
        UpdateUI();
    }

    private void replyPlayerRun()
    {
        isRunning = true;
        actualThirstConsumeRate = baseThirstConsumeRate * runThirstMultiplier;
        actualHungerConsumeRate = baseHungerConsumeRate * runHungerMultiplier;
        actualSanityConsumeRate = baseSanityConsumeRate * runSanityMultiplier;
        UpdateUI();
    }

    private void replyStopRun()
    {
        isRunning = false;
        actualThirstConsumeRate = baseThirstConsumeRate;
        actualHungerConsumeRate = baseHungerConsumeRate;
        actualSanityConsumeRate = baseSanityConsumeRate;
        UpdateUI();
    }

    private void replyDrink(float amount)
    {
        thirst = Mathf.Min(100, thirst + amount);
        UpdateUI();
    }

    private void replyEat(float amount)
    {
        hunger = Mathf.Min(100, hunger + amount);
        UpdateUI();
    }

    private void replyRest(float amount)
    {
        sanity = Mathf.Min(100, sanity + amount);
        if (sanity > criticalStatThreshold)
        {
            isExhausted = false;
        }
        UpdateUI();
    }

    private void replyHealthUpdate(float health, float max)
    {
        // La regeneración de vida solo ocurre si las estadísticas están en buen estado
        if (health < max && hunger > lowStatThreshold && thirst > lowStatThreshold)
        {
            float healAmount = 1f; // 1 de vida por segundo si las estadísticas están bien
            PlayerEvents.Heal?.Invoke(healAmount);
        }
    }

    private void OnDisable()
    {
        PlayerEvents.Revive -= replyPlayerRevive;
        PlayerEvents.Run -= replyPlayerRun;
        PlayerEvents.StopRun -= replyStopRun;
        PlayerEvents.Drink -= replyDrink;
        PlayerEvents.Eat -= replyEat;
        PlayerEvents.Rest -= replyRest;
        PlayerEvents.HealthUpdate -= replyHealthUpdate;
    }
    #endregion
}
