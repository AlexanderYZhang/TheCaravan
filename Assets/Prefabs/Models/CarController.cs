using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CarController : MonoBehaviour {
    public Camera cam;
    public GameObject player;
    public GameObject canvas;
    public GameObject marker;
    public NavMeshAgent agent;
    public NavMeshObstacle obstacle;

    private CharController playerController;
    private CameraController camController;

    private Text[] texts;
    private GameObject message;
    private Vector3 playerOffset;

    private int groundLayer = 1 << 8;

    // Use this for initialization
    void Start () {
        playerController = player.GetComponent<CharController>();
        camController = cam.GetComponent<CameraController>();
        texts = canvas.GetComponentsInChildren<Text>();
        message = GameObject.Find("MessagePanel");
    }

    public void setOffset(Vector3 playerPos) {
        playerOffset = playerPos - transform.position;
    }

    // Update is called once per frame
    void Update () {
		if (playerController.InsideVehicle()) {
            if (Input.GetKeyDown(KeyCode.E)) {
                playerController.ExitVehicle(transform.position + playerOffset);
            } else {
                if (!obstacle.enabled) {
                    agent.enabled = true;
                }

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
            }
        } else if (!playerController.InsideVehicle()) {
            if (!agent.enabled) {
                obstacle.enabled = true;
            }
        }
    }
}
