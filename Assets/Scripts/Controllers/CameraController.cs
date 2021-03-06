﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform target;
	public Vector3 offset;
	public float pitch = 2;
	public float zoomSpeed = 4f;
	public float minZoom = 5f;
	public float maxZoom = 15f;
	public float yawSpeed = 100f;
	private float currentZoom = 10;
	private float currentYaw = 0f;
	private float currentPitch = 0f;

    // private bool dragging = false;
    // private Vector3 prevMouseLocation;

	// Use this for initialization
	void Update() {
		currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
		currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

		currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
        currentPitch += Input.GetAxis("Vertical") * yawSpeed * Time.deltaTime;

		currentPitch = Mathf.Clamp(currentPitch, -30f, 30f);
	}
	void LateUpdate() {
		if (target != null) {
            transform.position = new Vector3(target.position.x, 0, target.position.z) - offset * currentZoom;
            transform.RotateAround(target.position, Vector3.right, currentPitch);
            transform.LookAt(new Vector3(target.position.x, 0, target.position.z) + Vector3.up * pitch);
            transform.RotateAround(target.position, Vector3.up, currentYaw);
		}	
    }
}
