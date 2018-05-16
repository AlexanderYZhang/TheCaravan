using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {
    public int maxHealth = 100;
    public int currentHealth { get; private set; }
    public Stat damage;
    public Stat gathering;
    public float playerKillRange;
    public float carKillRange;
    public float turretKillRange;
    public GameObject explosion;

    public event System.Action<int, int> OnHealthChanged;
    void Awake() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;

        if (OnHealthChanged != null) {
            OnHealthChanged(maxHealth, currentHealth);
        }

        if (currentHealth <= 0) {
            Die();
        }
    }

    public virtual void Die() {
        GameObject.Destroy(gameObject);
        GameObject exp = GameObject.Instantiate(explosion, transform.position, Quaternion.identity);
        currentHealth = 0;
        if (OnHealthChanged != null) {
            OnHealthChanged(maxHealth, currentHealth);
        }
        GameObject.Destroy(exp, 5);
    }
}
