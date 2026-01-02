using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravityScale = 2f;
    [SerializeField] private float turnSpeed = 60f;
    private Vector2 MoveInput;
    private bool jumpInput;
    bool wasGrounded = false;

    [Header("Component References ")]
    [SerializeField] CharacterController characterController;
    [SerializeField] Animator animator;

    [Header("Unity Events ")]
    public UnityEvent jumped;
    public UnityEvent coinCollected;
    public UnityEvent landed;


    public bool Grounded
    {
        get { return characterController.isGrounded; }
    }



    private float verticalVelocity = 0f;

    #region Input Handing Melthods
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();

    }
    public void OnAttack(InputAction.CallbackContext context)
    {

    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpInput = true;
        }
        jumpInput = context.performed;
    }
    #endregion

    #region Unity Callback Methods (start, update, etc)
    private void Update()
    {
        UpdateMovement();
        UpdateAnimator();


    }
    #endregion

    #region Charactor Control Method
    void UpdateMovement()
    {
        Vector3 moveInput3D = new Vector3(MoveInput.x, 0f, MoveInput.y);
        Vector3 motion = moveInput3D * speed * Time.deltaTime;

        UpdatePlayerRotation(moveInput3D);
        if (Grounded)
        {
            verticalVelocity = -3f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime * gravityScale;

        }

        //nhay
        if (jumpInput && Grounded)
        {
            verticalVelocity = Mathf.Sqrt(2f * jumpHeight * Mathf.Abs(Physics.gravity.y * gravityScale));

            jumpInput = false;

            jumped.Invoke();
        }
        motion.y = verticalVelocity * Time.deltaTime;

        characterController.Move(motion);

        if (!wasGrounded && Grounded)
        {
            landed.Invoke();
        }

        wasGrounded = Grounded;
    }
    void UpdatePlayerRotation(Vector3 moveInput)
    {
        if (moveInput.sqrMagnitude < 0.01f) return;


        Vector3 playerRotation = transform.rotation.eulerAngles;
        playerRotation.y = GetAngleFromVector(moveInput);
        //transform.rotation = Quaternion.Euler(playerRotation);
        Quaternion targetRotation = Quaternion.Euler(playerRotation);
        float maxDegreesDelta = turnSpeed * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegreesDelta);
    }
    float GetAngleFromVector(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        return rotation.eulerAngles.y;
    }

    #endregion

    #region Other Methods
    void UpdateAnimator()
    {
        bool jump = false;
        bool fall = false;
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
        Vector3 horizontalVelocity = characterController.velocity;
        horizontalVelocity.y = 0f;
        float speed = horizontalVelocity.magnitude;

        animator.SetFloat("Speed", speed);


        animator.SetBool("Jump", jump);
        animator.SetBool("Fall", fall);

    }
    #endregion

}
