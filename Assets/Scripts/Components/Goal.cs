using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	TerrainGenerator generator;
	
	void Start() {
		generator = TerrainGenerator.instance;
	}
	void OnTriggerEnter(Collider other) {
		Debug.Log(generator);
 		generator.DestroyMap();
		generator.GenerateMap();
	}
}
