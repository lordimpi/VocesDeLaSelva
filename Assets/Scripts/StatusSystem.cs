using UnityEngine;

public class StatusSystem : MonoBehaviour
{
    [Header("Referencias")]
    public LifePlayer lifePlayer;

    [Header("Estado: Hambre")]
    public float hunger = 0;
    public float maxHunger = 100;
    public float hungerDamage = 3;
    public float hungerTickTime = 5f;
    public float hungerIncreaseAmount = 5f;
    private float hungerTimer;

    [Header("Estado: Sed")]
    public float thirst = 0;
    public float maxThirst = 100;
    public float thirstDamage = 3;
    public float thirstTickTime = 10f;
    public float thirstIncreaseAmount = 5f;
    private float thirstTimer;

    [Header("Estado: Cordura")]
    public float sanity = 100;
    public float minSanity = 0;
    public float sanityDamage = 20;
    public float sanityTickTime = 30f;
    public float sanityDecreaseAmount = 5f;
    private float sanityTimer;

    [Header("Intervalo general de actualización")]
    public float statusIncreaseInterval = 10f;
    private float statusTimer;

    void Awake()
    {
        if (lifePlayer == null)
        {
            lifePlayer = GameObject.FindGameObjectWithTag("Player")?.GetComponent<LifePlayer>();
            if (lifePlayer == null)
            {
                Debug.LogWarning("No se encontró LifePlayer en el objeto con tag Player.");
            }
        }
    }

    void Update()
    {
        if (lifePlayer == null) return;

        // ====================
        // AUMENTO AUTOMÁTICO
        // ====================
        statusTimer += Time.deltaTime;
        if (statusTimer >= statusIncreaseInterval)
        {
            hunger += hungerIncreaseAmount;
            thirst += thirstIncreaseAmount;
            sanity -= sanityDecreaseAmount;

            hunger = Mathf.Clamp(hunger, 0, maxHunger);
            thirst = Mathf.Clamp(thirst, 0, maxThirst);
            sanity = Mathf.Clamp(sanity, minSanity, 100f);

            Debug.Log($"[Estado] +Hambre: {hunger}, +Sed: {thirst}, -Cordura: {sanity}");
            statusTimer = 0f;
        }

        // ====================
        // DAÑO POR HAMBRE
        // ====================
        if (hunger >= maxHunger)
        {
            hungerTimer += Time.deltaTime;
            if (hungerTimer >= hungerTickTime)
            {
                lifePlayer.takeDamage(hungerDamage);
                hungerTimer = 0f;
            }
        }

        // ====================
        // DAÑO POR SED
        // ====================
        if (thirst >= maxThirst)
        {
            thirstTimer += Time.deltaTime;
            if (thirstTimer >= thirstTickTime)
            {
                lifePlayer.takeDamage(thirstDamage);
                thirstTimer = 0f;
            }
        }

        // ====================
        // DAÑO POR CORDURA BAJA
        // ====================
        if (sanity <= minSanity)
        {
            sanityTimer += Time.deltaTime;
            if (sanityTimer >= sanityTickTime)
            {
                lifePlayer.takeDamage(sanityDamage);
                sanityTimer = 0f;
            }
        }

        // ===== Simulación manual =====
        if (Input.GetKeyDown(KeyCode.H)) hunger = maxHunger;
        if (Input.GetKeyDown(KeyCode.S)) thirst = maxThirst;
        if (Input.GetKeyDown(KeyCode.C)) sanity = minSanity;
    }
}