using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{   

    private float mueveX;
    private float mueveY;
    private float speed = 7;
    private float speedGiro = 80;
    private Animator controlNino;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controlNino = GetComponent<Animator>();
    }

    // Update is called once per frame 
    void Update()
    {
        mueveX = Input.GetAxis("Horizontal");
        mueveY = Input.GetAxis("Vertical");
        transform.Translate(0, 0, mueveY * Time.deltaTime * speed);
        transform.Rotate(0, mueveX * Time.deltaTime * speedGiro, 0);

        controlNino.SetFloat("ValorX", mueveX);
        controlNino.SetFloat("ValorY", mueveY);

    }
}
