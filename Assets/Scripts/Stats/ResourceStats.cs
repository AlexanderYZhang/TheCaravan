using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStats : MonoBehaviour {
    public int maxHealth = 5;
    public int currentHealth { get; private set; }

	public event System.Action<int,int> OnHealthChanged;
    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.LogWarning("takes damage");

		if (OnHealthChanged != null) {
			OnHealthChanged(maxHealth, currentHealth);
		}

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
