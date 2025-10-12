using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    public float damage;
    public float stagger;
    List<EnemyBase> hitEnemies;

    private void Start()
    {
        hitEnemies = new List<EnemyBase>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var hitEnemy = collision.GetComponent<EnemyBase>();
        if (hitEnemy != null && !hitEnemies.Contains(hitEnemy))
        {
            hitEnemies.Add(hitEnemy);
            hitEnemy.TakeDamage(damage, stagger);
        }
    }

    private void OnDisable()
    {
        hitEnemies.Clear();
    }
}