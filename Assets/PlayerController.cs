using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
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
    List<AttackStateInfo> attackInfo;
    [SerializeField]
    Transform groundCheck;
    Animator playerAnimator;
    Vector2 movement;
    Rigidbody2D rb;
    bool isGrounded;
    float jumpTracker;
    float attackTracker;
    ComboState playerComboState;
    float timeSinceLastAttack;
    float noAttackingTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        isGrounded = false;
        playerComboState = ComboState.Idle;
        timeSinceLastAttack = 0;
        noAttackingTime = 0;
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
        playerAnimator.SetBool("IsMoving", Vector2.zero != rb.linearVelocity);

        if (jumpTracker > 0f && isGrounded)
        {
            jumpTracker = 0f;
            rb.AddForce(Vector2.up * jumpForce);
        }
        if (attackTracker > 0f && noAttackingTime <= 0f)
        {
            print("GOO GOO GAA GAA");
            switch (playerComboState)
            {

                case ComboState.Idle:
                    AttackStateInfo obtainedInfo = attackInfo.Find(x => x.comboState == ComboState.GroundAtt1);
                    if (obtainedInfo == null)
                    {
                        noAttackingTime = 0.25f;
                        timeSinceLastAttack = 0.333f;
                        playerAnimator.Play("PlayerAttack1");
                        playerComboState = ComboState.GroundAtt1;
                        break;
                    }
                    noAttackingTime = obtainedInfo.timeUntilCancelable;
                    timeSinceLastAttack = obtainedInfo.attackDuration;
                    playerAnimator.Play(obtainedInfo.animationName);
                    playerComboState = ComboState.GroundAtt1;
                    break;
                case ComboState.GroundAtt1:
                    AttackStateInfo obtainedInfo1 = attackInfo.Find(x => x.comboState == ComboState.GroundAtt2);
                    if (obtainedInfo1 == null)
                    {
                        noAttackingTime = 0.25f;
                        timeSinceLastAttack = 0.333f;
                        playerAnimator.Play("PlayerAttack1");
                        playerComboState = ComboState.GroundAtt2;
                        break;
                    }
                    noAttackingTime = obtainedInfo1.timeUntilCancelable;
                    timeSinceLastAttack = obtainedInfo1.attackDuration;
                    playerAnimator.Play(obtainedInfo1.animationName);
                    playerComboState = ComboState.GroundAtt2;
                    break;
                case ComboState.GroundAtt2:
                    AttackStateInfo obtainedInfo2 = attackInfo.Find(x => x.comboState == ComboState.GroundAtt3);
                    if (obtainedInfo2 == null)
                    {
                        noAttackingTime = 0.25f;
                        timeSinceLastAttack = 0.333f;
                        playerAnimator.Play("PlayerAttack1");
                        playerComboState = ComboState.GroundAtt3;
                        break;
                    }
                    noAttackingTime = obtainedInfo2.timeUntilCancelable;
                    timeSinceLastAttack = obtainedInfo2.attackDuration;
                    playerAnimator.Play(obtainedInfo2.animationName);
                    playerComboState = ComboState.GroundAtt3;
                    break;
                case ComboState.GroundAtt3:
                    AttackStateInfo obtainedInfo3 = attackInfo.Find(x => x.comboState == ComboState.GroundAtt3);
                    if (obtainedInfo3 == null)
                    {
                        noAttackingTime = 0.25f;
                        timeSinceLastAttack = 0.333f;
                        playerAnimator.Play("PlayerAttack1");
                        playerComboState = ComboState.Idle;
                        break;
                    }
                    noAttackingTime = obtainedInfo3.timeUntilCancelable;
                    timeSinceLastAttack = obtainedInfo3.attackDuration;
                    playerAnimator.Play(obtainedInfo3.animationName);
                    playerComboState = ComboState.Idle;
                    break;
                default:
                    break;
            }
        }

        attackTracker = Mathf.Clamp(attackTracker - Time.fixedDeltaTime, 0f, Mathf.Infinity);
        jumpTracker = Mathf.Clamp(jumpTracker - Time.fixedDeltaTime, 0f, Mathf.Infinity);
        timeSinceLastAttack = Mathf.Clamp(timeSinceLastAttack - Time.fixedDeltaTime, 0f, Mathf.Infinity);
        noAttackingTime = Mathf.Clamp(noAttackingTime - Time.fixedDeltaTime, 0f, Mathf.Infinity);
        if (timeSinceLastAttack == 0f) playerComboState = ComboState.Idle;

        rb.gravityScale = rb.linearVelocityY > 0 ? upGrav : downGrav;

    }

}

[System.Serializable]
class AttackStateInfo
{
    public ComboState comboState;
    public float attackDuration;
    public string animationName;
    public float timeUntilCancelable;
}
