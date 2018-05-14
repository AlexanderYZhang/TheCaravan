using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	public TerrainGenerator generator;
	void OnTriggerEnter(Collider other) {
        if (other.tag == "Car") {
            generator.DestroyMap();
            generator.GenerateMap();
        }
	}
}
