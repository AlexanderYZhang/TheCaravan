using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour {

	DayNightController sun;
	Light light;

	void Start () {
		sun = DayNightController.instance;
		light = gameObject.GetComponent<Light>();
	}
	
	void Update () {
		if (sun.isDayTime) {
			light.enabled = false;
		} else {
			light.enabled = true;
		}
	}
}
