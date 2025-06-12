using UnityEngine;

public class PlayerDebug : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private LifePlayer playerLife;

    [Header("Valores de Prueba")]
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float healAmount = 10f;
    [SerializeField] private float foodAmount = 20f;
    [SerializeField] private float waterAmount = 20f;
    [SerializeField] private float restAmount = 20f;

    private void Update()
    {
        // Pruebas de Vida
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Aplicando daño: " + damageAmount);
            PlayerEvents.TakeDamage?.Invoke(damageAmount);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Curando: " + healAmount);
            PlayerEvents.Heal?.Invoke(healAmount);
        }

        // Pruebas de Estadísticas
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Comiendo: " + foodAmount);
            PlayerEvents.Eat?.Invoke(foodAmount);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Bebiendo: " + waterAmount);
            PlayerEvents.Drink?.Invoke(waterAmount);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Descansando: " + restAmount);
            PlayerEvents.Rest?.Invoke(restAmount);
        }

        // Revivir jugador
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Reviviendo jugador");
            PlayerEvents.Revive?.Invoke();
        }
    }
} 