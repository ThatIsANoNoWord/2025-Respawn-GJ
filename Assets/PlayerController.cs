using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerController;

public class PlayerController : MonoBehaviour
{

    public enum ComboState
    {
        Idle,
        GroundAtt1,
        GroundAtt2,
        GroundAtt3
    }

    public float speed;
    public float jumpForce;
    public LayerMask groundLayer;
    public float upGrav;
    public float downGrav;
    public float jumpDelay;
    public float attackDelay;
    [SerializeField]
    Transform groundCheck;
    Vector2 movement;
    Rigidbody2D rb;
    bool isGrounded;
    float jumpTracker;
    float attackTracker;
    ComboState playerComboState;
    float timeSinceLastAttack;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGrounded = false;
        playerComboState = ComboState.Idle;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled)
        {
            movement = Vector2.zero;
            return;
        }
        movement = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            jumpTracker = jumpDelay;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            attackTracker = attackDelay;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.CircleCast(groundCheck.position, 0.1f, Vector2.zero, Mathf.Infinity, groundLayer.value);
        if (movement != Vector2.zero) print("PLEASE MOVE");
        rb.linearVelocityX = movement.x * Time.fixedDeltaTime * speed;

        if (jumpTracker > 0f && isGrounded)
        {
            jumpTracker = 0f;
            rb.AddForce(Vector2.up * jumpForce);
        }
        if (attackTracker > 0f)
        {
            switch (playerComboState)
            {
                case ComboState.Idle:
                    // Do first attack
                default:
                    return;
            }
        }

        attackTracker = Mathf.Clamp(attackTracker - Time.fixedDeltaTime, 0f, Mathf.Infinity);
        jumpTracker = Mathf.Clamp(jumpTracker - Time.fixedDeltaTime, 0f, Mathf.Infinity);

        rb.gravityScale = rb.linearVelocityY > 0 ? upGrav : downGrav;

    }

}

[System.Serializable]
class AttackStateInfo
{
    public ComboState comboState;
    public float attackDuration;
    public string animationName;

}
