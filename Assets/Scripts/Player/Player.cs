using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float sprintSpeed = 7f;

    [Space(20)]
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 5f;

    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravityScale = 2f;
    [SerializeField] private float turnSpeed = 60f;

    [Header("Component References ")]
    [SerializeField] CharacterController characterController;
    [SerializeField] Animator animator;

    [Header("Unity Events ")]
    public UnityEvent jumped;
    public UnityEvent coinCollected;
    public UnityEvent landed;
    public UnityEvent sprintStarted;
    public UnityEvent sprintFinished;


    public Vector3 CharacterControllerVelocity { get; private set; }
    public float CharacterControllerSpeed { get; private set; }


    private PlayerHeath playerHeath;
    private Vector2 MoveInput;
    private bool jumpInput;
    private bool sprintInput;
    private Vector3 lastNonZeroMoveInput3D;


    bool wasGrounded = false;
    bool wasSprinting = false;
    private float TargetMoveSpeed => sprintInput ? sprintSpeed : speed;

    private float CurrentSpeed = 0f;
    /// <summary>
    /// Kiểm tra xem người chơi có đang ở trên mặt đất hay không
    /// </summary>
    public bool Grounded
    {
        get { return characterController.isGrounded; }
    }

    public bool Sprinting { get;private set; }=false;
    

    /// <summary>
    /// Vận tốc theo trục Y (chiều dọc) của người chơi
    /// </summary>
    private float verticalVelocity = 0f;
    private bool InputEnable = true;

    #region Input Handing Melthods
    /// <summary>
    /// Xử lý input di chuyển từ người chơi
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (InputEnable == false)
        {
            MoveInput = Vector2.zero;
            return;
        }
        MoveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Xử lý input tấn công từ người chơi
    /// </summary>
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (InputEnable == false)
        {
            MoveInput = Vector2.zero;

            return;
        }

        //TODO: Xử lý tấn công

    }

    /// <summary>
    /// Xử lý input nhảy từ người chơi
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (InputEnable == false)
        {
            jumpInput = false;
            return;
        }
        if (context.performed)
        {
            jumpInput = true;
        }
        jumpInput = context.performed;
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UIManager.Instance.TogglePause();
        }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (InputEnable == false)
        {
            sprintInput = false;
            return;
        }
        if (context.performed)
        {
            sprintInput = true;
        }
        else if (context.canceled)
        {
            sprintInput = false;
        }

    }
    #endregion

    #region Unity Callback Methods (start, update, etc)

    private void Start()
    {
        playerHeath = GetComponent<PlayerHeath>();
    }

    private void Update()
    {
        if (playerHeath != null && !playerHeath.Alive)
        {
            return;
        }
        UpdateSprinting();
        UpdateMovement();
        UpdateAnimator();
        UpdateCurrentSpeed();
    }

    private void OnTriggerEnter(Collider other)
    {
        var levelExit = other.GetComponent<LevelExit>();

        if (levelExit != null)
        {
            levelExit.ExitLevel();
        }
    }
    #endregion

    #region Charactor Control Method
    void UpdateSprinting()
    {
        wasSprinting = Sprinting;

        Sprinting = Grounded && sprintInput && CharacterControllerSpeed > speed;
        
        if (!wasSprinting && Sprinting)
        {
            sprintStarted.Invoke();
        }

        if(wasSprinting && !Sprinting)
        {
            sprintFinished.Invoke();
        }
    }
    void UpdateCurrentSpeed()
    {
        CharacterControllerVelocity = characterController.velocity;
        Vector3 horizontalVelocity = CharacterControllerVelocity;
        horizontalVelocity.y = 0f;
        CharacterControllerSpeed = horizontalVelocity.magnitude; 
    }

    /// <summary>
    /// Cập nhật di chuyển và nhảy của nhân vật
    /// </summary>
    void UpdateMovement()
    {
        // Chuyển đổi input 2D thành vector 3D
        Vector3 moveInput3D = new Vector3(MoveInput.x, 0f, MoveInput.y);
        if (moveInput3D.magnitude > 0.001f)
        {
            lastNonZeroMoveInput3D = moveInput3D;
            CurrentSpeed = Mathf.MoveTowards(CurrentSpeed, TargetMoveSpeed, acceleration * Time.deltaTime);

        }
        else
        {
            CurrentSpeed = Mathf.MoveTowards(CurrentSpeed, 0f, deceleration * Time.deltaTime);

        }

        Vector3 motion = lastNonZeroMoveInput3D * CurrentSpeed * Time.deltaTime;

        // Cập nhật hướng quay của nhân vật
        UpdatePlayerRotation(moveInput3D);

        // Cập nhật vận tốc dọc (trọng lực)
        if (Grounded)
        {
            verticalVelocity = -3f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime * gravityScale;
        }

        // Xử lý nhảy
        if (jumpInput && Grounded)
        {
            verticalVelocity = Mathf.Sqrt(2f * jumpHeight * Mathf.Abs(Physics.gravity.y * gravityScale));
            jumpInput = false;
            jumped.Invoke();
        }

        // Áp dụng vận tốc dọc vào chuyển động
        motion.y = verticalVelocity * Time.deltaTime;
        characterController.Move(motion);

        // Phát sự kiện khi nhân vật tiếp đất
        if (!wasGrounded && Grounded)
        {
            landed.Invoke();
        }

        wasGrounded = Grounded;
    }

    /// <summary>
    /// Cập nhật hướng quay của nhân vật dựa trên input di chuyển
    /// </summary>
    void UpdatePlayerRotation(Vector3 moveInput)
    {
        // Nếu không có input di chuyển, không quay
        if (moveInput.sqrMagnitude < 0.01f) return;

        // Tính toán góc quay mục tiêu
        Vector3 playerRotation = transform.rotation.eulerAngles;
        playerRotation.y = GetAngleFromVector(moveInput);
        Quaternion targetRotation = Quaternion.Euler(playerRotation);
        float maxDegreesDelta = turnSpeed * Time.deltaTime;

        // Quay nhân vật mượt mà hướng tới mục tiêu
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegreesDelta);
    }

    /// <summary>
    /// Tính toán góc quay từ một vector hướng
    /// </summary>
    float GetAngleFromVector(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        return rotation.eulerAngles.y;
    }
    #endregion

    #region Other Methods
    /// <summary>
    /// Cập nhật các tham số animation dựa trên trạng thái hiện tại
    /// </summary>
    void UpdateAnimator()
    {
        bool jump = false;
        bool fall = false;

        // Xác định trạng thái nhảy hoặc rơi
        if (characterController.isGrounded)
        {
            jump = false;
            fall = false;
        }
        else
        {
            if (verticalVelocity >= 0)
            {
                jump = true;
            }
            else
            {
                fall = true;
            }
        }

        // Tính tốc độ ngang của nhân vật
        Vector3 horizontalVelocity = characterController.velocity;
        horizontalVelocity.y = 0f;
        float speed = horizontalVelocity.magnitude;

        // Cập nhật animator
        animator.SetFloat("Speed", speed);
        animator.SetBool("Jump", jump);
        animator.SetBool("Fall", fall);
    }

    public void OnDeath()
    {

        animator.SetBool("Alive", false);
        InputEnable = false;
    }
    #endregion
}
