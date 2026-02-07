using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 7f;
    public float groundDrag = 5f;
    public CharacterController controller;
    
    [Header("Ground Check")]
    public float groundDist = 0.4f;
    public LayerMask groundLayer;
    
    [Header("Camera & Look")]
    public Camera playerCamera;
    public float mouseSensitivity = 2f;
    private float xRotation = 0f;
    
    [Header("Weapons")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Animator animator;
    
    [Header("Systems")]
    public HealthSystem healthSystem;
    public WeaponSystem weaponSystem;
    public UIManager uiManager;

    private Vector3 velocity;
    private float gravity = -9.8f;
    private bool isGrounded;
    private bool isSprinting = false;
    private bool isProne = false;
    private bool isScoped = false;
    private float currentSpeed;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleCamera();
        HandleActions();
    }

    void HandleInput()
    {
        // Sprint
        isSprinting = Input.GetKey(KeyCode.LeftShift) && !isProne;
        
        // Prone
        if (Input.GetKeyDown(KeyCode.C))
        {
            isProne = !isProne;
            animator.SetBool("Prone", isProne);
        }
        
        // Scope/Aim
        if (Input.GetMouseButtonDown(1))
        {
            isScoped = !isScoped;
            animator.SetBool("Aim", isScoped);
            
            if (isScoped)
                playerCamera.fieldOfView = 30f;
            else
                playerCamera.fieldOfView = 60f;
        }
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(transform.position - Vector3.up * (controller.height / 2), groundDist, groundLayer);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        if (isProne) currentSpeed *= 0.5f;

        controller.Move(move * currentSpeed * Time.deltaTime);

        animator.SetFloat("Speed", move.magnitude * currentSpeed);
        animator.SetBool("Sprint", isSprinting);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isProne)
        {
            velocity.y = jumpForce;
            animator.SetTrigger("Jump");
        }
    }

    void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleActions()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weaponSystem.Shoot(firePoint, playerCamera.transform.forward);
            animator.SetTrigger("Shoot");
        }

        // Reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            weaponSystem.Reload();
            animator.SetTrigger("Reload");
        }

        // Switch Weapon
        if (Input.GetKeyDown(KeyCode.E))
        {
            weaponSystem.SwitchWeapon();
        }
    }

    public void TakeDamage(float damage)
    {
        healthSystem.TakeDamage(damage);
        animator.SetTrigger("Hit");
    }

    public bool IsAlive()
    {
        return healthSystem.IsAlive();
    }
}