using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour {

	public GameObject[] Trees;
	public GameObject[] Terrain;
	public GameObject[] Rocks;
	public GameObject[] Misc;
	public GameObject[] Protrusions;
	public GameObject[] Cliffs;

	// Use this for initialization
	void Start () {
		// placeAssets(0, Trees);
		// placeAssets(3, Terrain);
		// placeAssets(6, Rocks);
		// placeAssets(9, Misc);
		// placeAssets(12, Protrusions);
		// placeAssets(15, Cliffs);
	}

	private void placeAssets(int startPos, GameObject[] arr) {
		Vector3 pos = new Vector3(startPos, 0, 0);
		foreach (GameObject obj in arr) {
			obj.transform.position = pos;
			pos.z += 3;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
