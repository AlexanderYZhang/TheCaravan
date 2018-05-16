using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	public int maxSpawn;
	public float spawnFrequency;
	public GameObject enemy;
	int totalSpawn;
	void Start () {
		float randomSpawnStart = Random.Range(0f, 10f);
		float randSpawnFrequency = Random.Range(0f, 4f);
		InvokeRepeating("Spawn", randomSpawnStart, spawnFrequency + randSpawnFrequency);	
		totalSpawn = 0;
	}
	
	void Update () {
		if (totalSpawn >= maxSpawn) {
			CancelInvoke();
			//Destroy(gameObject);
		} else {
            if (!IsInvoking("Spawn")) {
                float randomSpawnStart = Random.Range(0f, 10f);
                float randSpawnFrequency = Random.Range(0f, 4f);
                InvokeRepeating("Spawn", randomSpawnStart, spawnFrequency + randSpawnFrequency);
            }
        }
        if (!DayNightController.instance.isDayTime && totalSpawn >= maxSpawn) {
            totalSpawn = 0; 
        }
    }

	void Spawn() {
		Instantiate(enemy, transform.position, Quaternion.identity, transform);
        totalSpawn++;
    }
}
