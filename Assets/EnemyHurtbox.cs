using UnityEngine;

public class EnemyHurtbox : MonoBehaviour
{
    [SerializeField]
    Vector2 attackForce;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var hitPlayer))
        {
            hitPlayer.TakeDamage(transform.parent.localScale.x > 0 ? attackForce : new Vector2(-attackForce.x, attackForce.y));
        }
    }
}
