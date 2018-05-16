    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarStats : MonoBehaviour {

	public int maxHealth = 100;
	public int currentHealth { get; private set; }
	public float speed;
    public GameObject explosion;

    public event System.Action<int, int> OnHealthChanged;
    void Awake() {
		currentHealth = maxHealth;
	}
	
	public void TakeDamage(int damage) {
		currentHealth -= damage;
		Debug.LogWarning(transform.name + "takes damage");

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
        GameObject.Destroy(exp, 5);
    }
}
