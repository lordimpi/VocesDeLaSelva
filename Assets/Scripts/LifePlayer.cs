using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LifePlayer : LifeBase { 
    public bool defeated {private set; get;}
    public bool canBeHealed => currentLife < maxLife && !defeated;

    // private Personaje personaje;
    private Collider _collider;

    private void Awake(){
        _collider = GetComponent<Collider>();
    }

    private void Start(){
        base.Start();
        updateHealthBar(currentLife, maxLife);
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.T)){
            takeDamage(10);
        }
        if(Input.GetKeyDown(KeyCode.Y)){
            Heal(10);
        }   
    }

    public void Heal(float amount){
        if(canBeHealed){
            currentLife += amount;
            if(currentLife > maxLife){
                currentLife = maxLife;
            }
            updateHealthBar(currentLife, maxLife);
        }
    }

    public void Revive(){
        PlayerEvents.Death?.Invoke();
    }

    protected override void updateHealthBar(float currentLife, float maxLife){
        PlayerEvents.HealthUpdate?.Invoke(currentLife, maxLife);
    }

    protected override void characterDefeated(){
        defeated = true;
        PlayerEvents.Death?.Invoke();
    }   
}