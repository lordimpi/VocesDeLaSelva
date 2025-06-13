using System;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 100f;
    private Rigidbody rb;
    private Animator animator;


    public float minX = 1f, maxX = 499f;
    public float minZ = 1f, maxZ = 599f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true; // Evita volcaduras
    }

    void Update()
    {
        // Animaciones
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        animator.SetFloat("ValorX", moveX);
        animator.SetFloat("ValorY", moveY);
    }

    void FixedUpdate()
    {
        // Movimiento y rotación
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.forward * moveY * speed;
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z); // Usamos linearVelocity

        transform.Rotate(0, moveX * rotationSpeed * Time.fixedDeltaTime, 0);

        Vector3 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Math.Clamp(pos.z, minZ, maxZ);
        rb.position = pos;
    }
}