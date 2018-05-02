using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStats : MonoBehaviour {
    public int maxHealth = 5;
    public int currentHealth { get; private set; }

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.LogWarning("takes damage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {	
		Destroy(gameObject.transform.parent.gameObject);
    }
}
