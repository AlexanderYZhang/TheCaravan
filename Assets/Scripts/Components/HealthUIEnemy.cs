using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIEnemy : MonoBehaviour {

    public GameObject uiPrefab;
    public Transform target;
    public Vector2 barSize;
    float visibleTime = 5;

    public Transform ui;
    Image healthSlider;
    Transform cam;

    void Start() {
        cam = Camera.main.transform;

        foreach (Canvas c in FindObjectsOfType<Canvas>()) {
            if (c.renderMode == RenderMode.WorldSpace) {
                ui = Instantiate(uiPrefab, c.transform).transform;
                healthSlider = ui.GetChild(0).GetComponent<Image>();
                RectTransform rt = ui.GetComponent<RectTransform>();
                rt.sizeDelta = barSize;
                RectTransform hsRt = healthSlider.GetComponent<RectTransform>();
                hsRt.sizeDelta = barSize;
                ui.gameObject.SetActive(true);
                break;
            }
        }
        GetComponent<EnemyStats>().OnHealthChanged += OnHealthChanged;
    }

    void OnHealthChanged(int maxHealth, int currentHealth) {
        if (ui != null) {
            ui.gameObject.SetActive(true);
            float healthPercent = currentHealth / (float)maxHealth;
            healthSlider.fillAmount = healthPercent;
            if (currentHealth <= 0) {
                Destroy(ui.gameObject);
            }
        }
    }

    void LateUpdate() {
        if (ui != null) {
            ui.position = target.position;
            ui.forward = -cam.forward;
        }
    }
}

