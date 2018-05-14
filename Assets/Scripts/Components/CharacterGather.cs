using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterGather : MonoBehaviour {
	
	public float gatherSpeed = 1f;
	private float gatherCooldown = 0f;
	CharacterStats myStats;
	
	void Start() {
		myStats = GetComponent<CharacterStats>();
	}

	void Update() {
		gatherCooldown -= Time.deltaTime;
	}

	public void Gather (ResourceStats resourceStats) {
		if (gatherCooldown <= 0f) {
            resourceStats.TakeDamage(myStats.gathering.GetValue());
			gatherCooldown = 1f / gatherSpeed;
        }
    }
}
