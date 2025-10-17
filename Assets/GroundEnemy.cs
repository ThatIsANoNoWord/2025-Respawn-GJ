using UnityEngine;

public class GroundEnemy : EnemyBase
{
    public float maxWander;
    public float visionDistance;
    public float combatDistance;
    public LayerMask visionObstacleLayer;
    public float walkSpeed;
    public float runSpeed;
    public float staggerTime;
    public float attackDelay;
    public float attackVariance;
    public float attackDuration;
    public string attackAnimName;
    float xWanderTarget;
    Rigidbody2D rb;
    Animator animator;
    float staggerTimer;
    float noAttackTimer;
    float attackWait;

    public override void Start()
    {
        base.Start();
        xWanderTarget = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        staggerTime = 0;
    }

    public void FixedUpdate()
    {
        if (currHealth < 0) return;

        staggerTime = Mathf.Clamp(staggerTime - Time.fixedDeltaTime, 0f, Mathf.Infinity);
        noAttackTimer = Mathf.Clamp(noAttackTimer - Time.fixedDeltaTime, 0f, Mathf.Infinity);
        attackWait = Mathf.Clamp(attackWait - Time.fixedDeltaTime, 0f, Mathf.Infinity);

        animator.SetFloat("Speed", rb.linearVelocity.sqrMagnitude);
        animator.SetBool("IsStagger", staggerTime > 0f);

        if (staggerTime > 0f) return;

        if (attackWait > 0f) return;

        if (Vector2.Distance(PlayerController.Instance.transform.position, transform.position) < visionDistance &&
            !Physics2D.Raycast(transform.position, PlayerController.Instance.transform.position - transform.position,
            Vector2.Distance(PlayerController.Instance.transform.position, transform.position), visionObstacleLayer.value))
        {
            animator.SetBool("SeesPlayer", true);
            if (Mathf.Abs(transform.position.x - PlayerController.Instance.transform.position.x) < combatDistance)
            {
                if (noAttackTimer <= 0f)
                {
                    attackWait = attackDuration;
                    noAttackTimer = attackDelay + Random.Range(-attackVariance, attackVariance);
                    animator.Play(attackAnimName);
                }

                rb.linearVelocityX = 0f;
            }
            else
            {
                rb.linearVelocityX = transform.position.x < PlayerController.Instance.transform.position.x ? runSpeed : -runSpeed;
                transform.localScale = rb.linearVelocityX >= 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
            }

            return;
        }

        animator.SetBool("SeesPlayer", false);
        if (Mathf.Abs(transform.position.x - xWanderTarget) < 0.1f)
        {
            rb.linearVelocityX = 0f;
        }
        else
        {
            rb.linearVelocityX = transform.position.x < xWanderTarget ? walkSpeed : -walkSpeed;
            transform.localScale = rb.linearVelocityX >= 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        }
    }
    public override void TakeDamage(float damage, float stagger)
    {
        currHealth -= damage;
        currStagger += stagger;
        if (currStagger > staggerLimit)
        {
            staggerTimer = staggerTime;
            animator.Play("GroundStagger");
        }

        if (currHealth < 0)
        {
            animator.Play("GroundDeath");
        }
    }

}
