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

    private void Start() {
        mycCharacterController = GetComponent<CharacterController>();
                
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
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

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

        // Aplicar movimiento al Character Controller
        mycCharacterController.Move(movement * currentSpeed * Time.deltaTime);

        // Salto    
        if(Input.GetButtonDown("Jump") && isGrounded){
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }

        // Aplicar gravedad
        velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;

        // Mover el jugador
        mycCharacterController.Move(velocity * Time.deltaTime);
    }

    private void ProcessRotation(){
        // Obtener entrada de mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotar el jugador horizontalmente
        transform.Rotate(Vector3.up * mouseX);

        // Rotar la cámara verticalmente
        float xRotation = mainCamera.localEulerAngles.x - mouseY;

        // Limitar la rotación vertical para evitar que la camara gire completamente
        if(xRotation > 180f) xRotation -= 360f;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        mainCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    #region EVENTS
    private void replyPlayerDeath(){
        this.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void replyPlayerRevive(){
        Cursor.lockState = CursorLockMode.Locked;
        this.enabled = true;
    }
    #endregion
}
