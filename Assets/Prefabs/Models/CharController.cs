﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CharController : MonoBehaviour {
    public Camera cam;
    public GameObject car;
    public GameObject canvas;
    public GameObject marker;
    public NavMeshAgent agent;
    public GameObject turret1;

    private CarController carController;
    private CameraController cameraController;

    // Character Parameter
    private Animator anim;
    private bool placed;
    private Text[] texts;
    private GameObject message;

    // Vehicle Check
    private bool closeToVehicle;
    private bool inVehicle;

    private int groundLayer = 1 << 8;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        anim.SetTrigger("moving");
        texts = canvas.GetComponentsInChildren<Text>();
        message = GameObject.Find("MessagePanel");

        carController = car.GetComponent<CarController>();
        cameraController = cam.GetComponent<CameraController>();

        placed = false;
        texts[0].text = "";
        message.SetActive(false);

        closeToVehicle = false;
        inVehicle = false;

        marker.SetActive(false);
    }

    void OnTriggerEnter(Collider other) {
        if (other.name == car.name) {
            texts[0].text = "Press E to Enter";
            message.SetActive(true);
            closeToVehicle = true;
        }
        if (other.name == marker.name) {
            marker.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.name == car.name) {
            texts[0].text = "";
            message.SetActive(false);
            closeToVehicle = false;
        }
    }

    public void EnterVehicle() {
        texts[0].text = "Press E to Exit";
        inVehicle = true;
        cameraController.target = car.transform;
        marker.transform.position = new Vector3(car.transform.position.x, 0, car.transform.position.z);
        carController.obstacle.enabled = false;
        carController.setOffset(transform.position);
        gameObject.SetActive(false);
    }

    public void ExitVehicle(Vector3 newPos) {
        inVehicle = false;
        transform.position = newPos;
        cameraController.target = transform;
        marker.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        carController.agent.enabled = false;
        gameObject.SetActive(true);
    }

    public bool InsideVehicle() {
        return inVehicle;
    }

    // Update is called once per frame
    void Update () {
        if (!agent.isActiveAndEnabled && GetComponent<Rigidbody>().velocity.y == 0) {
            agent.enabled = true;
        } else if (agent.isActiveAndEnabled) {
            if (closeToVehicle && Input.GetKeyDown(KeyCode.E)) {
                EnterVehicle();
            }
            if (!inVehicle) {
                bool running = Input.GetKey(KeyCode.LeftShift);

                if (Input.GetMouseButton(0)) {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    int layerMask = groundLayer;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
                        agent.SetDestination(hit.point);
                        marker.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
                        marker.SetActive(true);
                    }
                }

                agent.speed = running ? 15f : 5f;
                float animSpeedPct = (running ? 1.0f : 0.5f) * (agent.hasPath ? 1 : 0);
                anim.SetFloat("speedPct", animSpeedPct);
            }
        }
    }

    public bool IsPlaced() {
        return placed;
    }

    public void SetPlaced() {
        if (!placed) {
            placed = true;
        }
    }
}
