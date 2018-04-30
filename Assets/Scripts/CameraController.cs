using System.Collections;
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

    private bool dragging = false;
    private Vector3 prevMouseLocation;

	// Use this for initialization
	void Update() {
		currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
		currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

		currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
        currentPitch -= Input.GetAxis("Vertical") * yawSpeed * Time.deltaTime;

        if (!dragging && Input.GetMouseButtonDown(1)) {
            dragging = true;
            prevMouseLocation = Input.mousePosition;
        } else if (dragging && Input.GetMouseButton(1)) {
            Vector3 change = (Input.mousePosition - prevMouseLocation).normalized;

            currentYaw += change.x * yawSpeed * Time.deltaTime;
            currentPitch += change.y * yawSpeed * Time.deltaTime;

            prevMouseLocation = Input.mousePosition;
        } else if (dragging && Input.GetMouseButtonUp(1)) {
            dragging = false;
        }

		currentPitch = Mathf.Clamp(currentPitch, -30f, 30f);
	}
	void LateUpdate() {
		transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pitch);
        transform.RotateAround(target.position, Vector3.right, currentPitch);
        transform.RotateAround(target.position, Vector3.up, currentYaw);


    }
}
