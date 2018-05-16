using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public static UIManager instance = null;
	static GameObject[] menuObjects;

	void Awake() {
		if (instance == null) {
			instance = this;
		}
	}

	void Start () {
		menuObjects = GameObject.FindGameObjectsWithTag("Menu");
		hideLoseScreen();
	}
	
	void Update () {

	}

	public static void hideLoseScreen() {
		foreach(GameObject g in menuObjects) {
			g.SetActive(false);
		}
	}

	public static void showLoseScreen() {
		foreach(GameObject g in menuObjects) {
			g.SetActive(true);
		}
	}

}
