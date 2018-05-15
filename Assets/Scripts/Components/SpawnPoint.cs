using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	public int maxSpawn;
	public int spawnFrequency;
	public GameObject enemy;
	int totalSpawn;
	void Start () {
		InvokeRepeating("Spawn", 2.0f, 2.0f);	
		totalSpawn = 0;
	}
	
	void Update () {
		if (totalSpawn >= maxSpawn) {
			CancelInvoke();
			Destroy(gameObject);
		}
	}

	void Spawn() {
		Instantiate(enemy, transform.position, Quaternion.identity);
        totalSpawn++;
    }
}
