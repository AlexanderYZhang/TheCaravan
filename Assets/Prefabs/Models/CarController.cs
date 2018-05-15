using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CarController : MonoBehaviour {
    public GameObject player;
    public GameObject canvas;
    public GameObject marker;
    public NavMeshAgent agent;
    public float maxInteractionDist;

    private CharController playerController;
    private CameraController camController;

    private Text[] texts;
    private GameObject message;
    private Vector3 playerOffset;
    new Camera camera;

    private int groundLayer = 1 << 8;

    // Use this for initialization
    void Start () {
        playerController = player.GetComponentInChildren<CharController>();
        texts = canvas.GetComponentsInChildren<Text>();
        message = GameObject.Find("MessagePanel");
        camera = Camera.main;
    }

    public void setOffset(Vector3 playerPos) {
        playerOffset = playerPos - transform.position;
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Tree") || other.CompareTag("Rock")) {
            Destroy(other.gameObject);
        }
    }  

    // Update is called once per frame
    void Update () {
		if (playerController.InsideVehicle()) {
            if (Input.GetKeyDown(KeyCode.E)) {
                playerController.ExitVehicle(transform.position + playerOffset);
            } else {
                if (Input.GetMouseButton(0)) {
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
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
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= maxInteractionDist) {
                playerController.closeToVehicle = true;
            } else {
                playerController.closeToVehicle = false;
            }
        }
    }
}
