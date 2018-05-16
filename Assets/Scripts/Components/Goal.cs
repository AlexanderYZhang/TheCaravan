using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	TerrainGenerator generator;
	private int level;

	void Start() {
		generator = TerrainGenerator.instance;
		level = 1;
	}
	void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Car")) {
            generator.DestroyMap();
            generator.GenerateMap();
			level ++;
			UIManager.instance.screenTimeout("Level " + level);
        }
	}
}
