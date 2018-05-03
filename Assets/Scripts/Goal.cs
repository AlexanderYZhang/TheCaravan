using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	public TerrainGenerator generator;
	void OnTriggerEnter(Collider other) {
		generator.DestroyMap();
		generator.GenerateMap();
	}
}
