using UnityEngine;

public class GroundEnemy : EnemyBase
{
    public float maxWander;
    public float visionDistance;
    public float combatDistance;
    public LayerMask visionObstacleLayer;
    public float walkSpeed;
    public float runSpeed;
    float xWanderTarget;
    Rigidbody2D rb;
    Animator animator;

    public override void Start()
    {
        base.Start();
        xWanderTarget = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
    }

    public void FixedUpdate()
    {

        if (Vector2.Distance(PlayerController.Instance.transform.position, transform.position) < visionDistance &&
            Physics2D.Raycast(transform.position, PlayerController.Instance.transform.position - transform.position,
            Vector2.Distance(PlayerController.Instance.transform.position, transform.position), visionObstacleLayer.value))
        {
            animator.SetBool("SeesPlayer", true);
            if (Mathf.Abs(transform.position.x - PlayerController.Instance.transform.position.x) < 0.1f)
            {
                rb.linearVelocityX = 0f;
            }
            else
            {
                rb.linearVelocityX = transform.position.x < PlayerController.Instance.transform.position.x ? runSpeed : -runSpeed;
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
        }
    }

}
