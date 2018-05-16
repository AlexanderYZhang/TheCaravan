using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager instance = null;
	GameObject[] menuObjects;
	public Text displayText;

	void Awake() {
		if (instance == null) {
			instance = this;
		}
	}

	void Start () {
		menuObjects = GameObject.FindGameObjectsWithTag("Menu");
		hideScreen();
	}
	
	void Update () {

	}

	public void hideScreen() {
		foreach(GameObject g in menuObjects) {
			g.SetActive(false);
		}
	}

	public void showScreen(string text) {
		displayText.text = text;
		foreach(GameObject g in menuObjects) {
			g.SetActive(true);
		}
	}
}
