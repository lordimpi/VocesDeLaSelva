using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player Stats Bars")]
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private Image playerHungerBar;
    [SerializeField] private Image playerThirstBar;
    [SerializeField] private Image playerSanityBar;

    [Header("Panels")]
    [SerializeField] private GameObject deathPanel;

    private void Start(){
        PlayerEvents.HealthUpdate += replyPlayerHealthUpdate;
        PlayerEvents.Death += replyPlayerDeath;
        PlayerEvents.Revive += replyPlayerRevive;
        PlayerEvents.StatsUpdate += replyPlayerStatsUpdate;
    }

    public void RevivePlayer(){
        PlayerEvents.Revive?.Invoke();
    }

    #region EVENTS
    private void replyPlayerHealthUpdate(float actual, float max){
        playerHealthBar.fillAmount = actual / max;
    }

    private void replyPlayerDeath(){
        deathPanel.SetActive(true);
    }

    private void replyPlayerRevive(){
        deathPanel.SetActive(false);
    }

    private void replyPlayerStatsUpdate(float hunger, float thirst, float sanity){
        playerHungerBar.fillAmount = hunger / 100;
        playerThirstBar.fillAmount = thirst / 100;
        playerSanityBar.fillAmount = sanity / 100;
    }

    private void OnDisable(){
        PlayerEvents.HealthUpdate -= replyPlayerHealthUpdate;
        PlayerEvents.Death -= replyPlayerDeath;
        PlayerEvents.Revive -= replyPlayerRevive;
        PlayerEvents.StatsUpdate -= replyPlayerStatsUpdate;
    }
    
    #endregion
}
