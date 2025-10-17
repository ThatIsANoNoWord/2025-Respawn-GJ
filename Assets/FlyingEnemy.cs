using UnityEngine;

public class FlyingEnemy : EnemyBase
{
    public float speed = 2f;       
    public float leftX = -10f;   
    public float rightX = 10f;   
    private bool movingRight = true;

    public Transform player;       
    public float chaseDistance = 5f;

    public float amplitude = 0.5f; 
    public float frequency = 2f;

    private Vector3 startPosition;

    public ParticleSystem takeDamageFx;

    public override void Start()
    {
        base.Start(); 
        startPosition = transform.position;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector2 moveDirection = Vector2.zero;

        if (player != null && Vector2.Distance(transform.position, player.position) < chaseDistance)
        {
            moveDirection = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            float targetX = movingRight ? rightX : leftX;
            moveDirection = new Vector2(targetX - transform.position.x, 0).normalized;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetX, transform.position.y), speed * Time.deltaTime);

            Vector3 pos = transform.position;
            pos.y += Mathf.Sin(Time.time * frequency) * amplitude * Time.deltaTime;
            transform.position = pos;

            if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
            {
                movingRight = !movingRight;
            }
        }

        if (moveDirection.x != 0)
        {       
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x) * (moveDirection.x > 0 ? -1 : 1);
            transform.localScale = localScale;
        }
    }

    public override void TakeDamage(float damage, float stagger)
    {
        base.TakeDamage(damage, stagger);
        takeDamageFx.Play();
        if (currHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
