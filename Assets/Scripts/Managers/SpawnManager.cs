using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public static SpawnManager instance = null;
	public GameObject spawnPoint;
	public int spawnPointCount;
	public int totalSpawns;
	DayNightController dayNight;
	
	void Awake() {
		if (instance == null) {
			instance = this;
		}
	}

	void Start () {
		dayNight = DayNightController.instance;
	}
	
	void Update () {
		if (!dayNight.isDayTime) {
			// start making spawn points
		} else {
			// destroy spawn points
		}
	}
}
