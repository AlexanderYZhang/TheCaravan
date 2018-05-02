﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

	public GameObject uiPrefab;
	public Transform target;
	float visibleTime = 5;

	float lastMadeVisibleTime;
	Transform ui;
	Image healthSlider;
	Transform cam;

	void Start () {
		cam = Camera.main.transform;

		foreach (Canvas c in FindObjectsOfType<Canvas>()) {
			if (c.renderMode == RenderMode.WorldSpace) {
				ui = Instantiate(uiPrefab, c.transform).transform;
				healthSlider = ui.GetChild(0).GetComponent<Image>();
                ui.gameObject.SetActive(false);

                break;
			}
		}
		Debug.Log(ui);
		GetComponent<ResourceStats>().OnHealthChanged += OnHealthChanged;
	}

	void OnHealthChanged(int maxHealth, int currentHealth) {
		Debug.Log(ui);
		if (ui != null) {
            ui.gameObject.SetActive(true);
			lastMadeVisibleTime = Time.time;
            float healthPercent = currentHealth / (float)maxHealth;
            healthSlider.fillAmount = healthPercent;
            if (currentHealth <= 0)
            {
                Destroy(ui.gameObject);
            }
		}
	}
	
	void LateUpdate () {
		if (ui != null) {
            ui.position = target.position;
            ui.forward = -cam.forward;

			if (Time.time - lastMadeVisibleTime > visibleTime) {
				ui.gameObject.SetActive(false);
			}
		}
	}
}
