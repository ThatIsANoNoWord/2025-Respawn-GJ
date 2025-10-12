using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float health;
    public float staggerLimit;
    float currHealth;
    float currStagger;

    private void Start()
    {
        currHealth = health;
        currStagger = 0;
    }

    public void TakeDamage(float damage, float stagger)
    {
        currHealth -= damage;
        currStagger += stagger;
    }
}
