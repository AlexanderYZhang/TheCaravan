using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarStats : MonoBehaviour {

	public int maxHealth = 100;
	public int currentHealth { get; private set; }
	public float speed;
	void Awake() {
		currentHealth = maxHealth;
	}
	
	public void TakeDamage(int damage) {
		currentHealth -= damage;
		Debug.LogWarning(transform.name + "takes damage");

		if (currentHealth <= 0) {
			Die();
		}
	}

	public virtual void Die() {
		Debug.Log(transform.name + " has been destroyed");
	}
}
