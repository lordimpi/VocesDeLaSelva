using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Configuración  de movimiento")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float jumpForce = 1.5f;

    [Header("Configuración de Cámara")]
    public Transform mainCamera;

    [Header("Configuración de física")]
    [SerializeField] private float gravityMultiplier = 2.5f;

    private CharacterController mycCharacterController;
    private Vector3 velocity;
    private bool isGrounded;
    private bool wasRunning = false;
    private Animator animator;
    private float currentSpeed;
    private float xRotation = 0f;

    private void Start()
    {
        mycCharacterController = GetComponent<CharacterController>();
        // Buscar el Animator en el modelo hijo
        animator = GetComponentInChildren<Animator>();

        // Ocultar el cursor al iniciar el juego
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        PlayerEvents.Death += replyPlayerDeath;
        PlayerEvents.Revive += replyPlayerRevive;

    }

    private void Update(){
        CheckGrounded();
        ProcessMovement();
        ProcessRotation();
        UpdateAnimations();
    }

    private void CheckGrounded(){
        isGrounded = mycCharacterController.isGrounded;
        if(isGrounded && velocity.y < 0){
            velocity.y = -2f;
        }
    }

    private void ProcessMovement(){
        // Obtener entrada horizaontal y vertical
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        // Calcular el vector de movimiento
        Vector3 movement = transform.right * horizontalMovement + transform.forward * verticalMovement;

        // Determinar la velocidad actual (caminar o correr)
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Manejar eventos de correr
        if (isRunning && !wasRunning)
        {
            PlayerEvents.Run?.Invoke();
        }
        else if (!isRunning && wasRunning)
        {
            PlayerEvents.StopRun?.Invoke();
        }
        wasRunning = isRunning;

        // Salto - Movido antes del movimiento para que sea inmediato    
        if(Input.GetButtonDown("Jump") && isGrounded){
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            if(animator != null) animator.SetTrigger("jump");
        }

        // Aplicar movimiento al Character Controller
        mycCharacterController.Move(movement * currentSpeed * Time.deltaTime);

        // Aplicar gravedad
        velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;

        // Mover el jugador
        mycCharacterController.Move(velocity * Time.deltaTime);
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        // Obtener entrada horizaontal y vertical
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        // Calcular la velocidad actual basada en el input
        float speed = Mathf.Abs(horizontalMovement) + Mathf.Abs(verticalMovement);
        speed = Mathf.Clamp01(speed); // Normalizar entre 0 y 1

        // Debug para verificar el valor de speed
        Debug.Log("Speed: " + speed);

        // Actualizar parámetros del animator
        animator.SetFloat("speed", speed);
        animator.SetBool("isRunning", wasRunning);
    }

    private void ProcessRotation(){
        // Obtener entrada de mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotar el jugador horizontalmente
        transform.Rotate(Vector3.up * mouseX);

        // Rotar la cámara verticalmente
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -65f, 65f); // Limitado a 65 grados arriba y abajo

        // Aplicar la rotación a la cámara
        if(mainCamera != null) {
            mainCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    #region EVENTS
    private void replyPlayerDeath(){
        this.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if(animator != null) animator.SetTrigger("death");
    }

    private void replyPlayerRevive(){
        Cursor.lockState = CursorLockMode.Locked;
        this.enabled = true;
        if(animator != null) {
            animator.Play("Idle");
        }
    }
    #endregion
}
