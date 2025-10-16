using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float health;
    public float staggerLimit;
    protected float currHealth;
    protected float currStagger;

    virtual public void Start()
    {
        currHealth = health;
        currStagger = 0;
    }

    virtual public void TakeDamage(float damage, float stagger)
    {
        currHealth -= damage;
        currStagger += stagger;
    }
}
