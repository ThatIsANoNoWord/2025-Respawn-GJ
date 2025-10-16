using UnityEngine;

public class EnemyHurtbox : MonoBehaviour
{
    [SerializeField]
    Vector2 attackForce;

    private void OnTriggerStay2D(Collider2D collision)
    {
        var hitPlayer = collision.GetComponent<PlayerController>();
        if (hitPlayer != null)
        {
            hitPlayer.TakeDamage(transform.parent.localScale.x > 0 ? attackForce : new Vector2(-attackForce.x, attackForce.y));
        }
    }
}
