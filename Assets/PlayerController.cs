using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using static PlayerController;
using UnityEditor;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float jumpForce;
    public float dashForce;
    public LayerMask groundLayer;
    public float upGrav;
    public float downGrav;
    public float jumpDelay;
    public float attackDelay;
    public string idleAnimName;
    [SerializeField]
    List<AttackStateInfo> attackInfo;
    [SerializeField]
    AttackStateInfo airDashAttackInfo;
    [SerializeField]
    AttackStateInfo airNeutralAttackInfo;
    [SerializeField]
    Transform groundCheck;
    Animator playerAnimator;
    Vector2 movement;
    Rigidbody2D rb;
    bool isGrounded;
    bool performedAirAttack;
    float jumpTracker;
    float attackTracker;
    float timeSinceLastAttack;
    float noAttackingTime;
    
    public static PlayerController Instance { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        isGrounded = false;
        timeSinceLastAttack = 0;
        noAttackingTime = 0;
        performedAirAttack = false;
        Instance = this;
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
        bool checkGrounded = Physics2D.CircleCast(groundCheck.position, 0.1f, Vector2.zero, Mathf.Infinity, groundLayer.value);
        if (checkGrounded && !isGrounded) performedAirAttack = false;
        if (checkGrounded && !isGrounded) noAttackingTime = 0;
        playerAnimator.SetBool("InAir", !checkGrounded);
        isGrounded = checkGrounded;

        if (isGrounded) rb.linearVelocityX = movement.x * Time.fixedDeltaTime * speed;
        playerAnimator.SetBool("IsMoving", Vector2.zero != rb.linearVelocity);

        if (movement.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (movement.x < 0) transform.localScale = new Vector3(-1, 1, 1);

        if (jumpTracker > 0f && isGrounded)
        {
            jumpTracker = 0f;
            rb.AddForce(Vector2.up * jumpForce);
        }
        if (attackTracker > 0f && noAttackingTime <= 0f)
        {

            if (isGrounded)
            {
                foreach (AttackStateInfo stateInfo in attackInfo)
                {
                    if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateInfo.currentAnimationName))
                    {
                        noAttackingTime = stateInfo.timeUntilCancelable;
                        playerAnimator.Play(stateInfo.nextAnimationName);
                    }
                    continue;
                }
            }
            else if (!performedAirAttack)
            {
                if (movement != Vector2.zero)
                {
                    Vector2 move = movement.normalized * dashForce;
                    move.y *= 0.125f;
                    if (movement.x == 0) move.y += dashForce * 0.125f;
                    rb.AddForce(move);
                    noAttackingTime = airDashAttackInfo.timeUntilCancelable;
                    playerAnimator.Play(airDashAttackInfo.nextAnimationName);
                    performedAirAttack = true;
                }
                else
                {
                    rb.AddForce(Vector2.up * dashForce * 0.125f);
                    noAttackingTime = airNeutralAttackInfo.timeUntilCancelable;
                    playerAnimator.Play(airNeutralAttackInfo.nextAnimationName);
                    performedAirAttack = true;
                }
            }

        }

        attackTracker = Mathf.Clamp(attackTracker - Time.fixedDeltaTime, 0f, Mathf.Infinity);
        jumpTracker = Mathf.Clamp(jumpTracker - Time.fixedDeltaTime, 0f, Mathf.Infinity);
        timeSinceLastAttack = Mathf.Clamp(timeSinceLastAttack - Time.fixedDeltaTime, 0f, Mathf.Infinity);
        noAttackingTime = Mathf.Clamp(noAttackingTime - Time.fixedDeltaTime, 0f, Mathf.Infinity);

        rb.gravityScale = rb.linearVelocityY > 0 ? upGrav : downGrav;

    }

}

[System.Serializable]
class AttackStateInfo
{
    public string currentAnimationName;
    public string nextAnimationName;
    public float timeUntilCancelable;
}
