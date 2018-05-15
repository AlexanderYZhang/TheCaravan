using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {
    public int maxHealth = 100;
    public int currentHealth { get; private set; }
    public Stat damage;
    public Stat gathering;

    public event System.Action<int, int> OnHealthChanged;
    void Awake() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        Debug.LogWarning("takes damage");

        if (OnHealthChanged != null) {
            OnHealthChanged(maxHealth, currentHealth);
        }

        if (currentHealth <= 0) {
            Die();
        }
    }

    public virtual void Die() {
        Debug.Log(transform.name + "died");
    }
}
