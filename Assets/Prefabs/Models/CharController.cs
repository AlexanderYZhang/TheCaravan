using System.Collections;
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

    // Distance Params
    public float carActivationDist;

    // Character Parameter
    private Animator anim;
    private bool placed;
    private Text[] texts;
    private GameObject message;
    private GameObject itemBar;

    private bool closeToVehicle;
    private bool inVehicle;

    private int groundLayer = 1 << 8;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        anim.SetTrigger("moving");
        texts = canvas.GetComponentsInChildren<Text>();
        message = GameObject.Find("MessagePanel");
        itemBar = GameObject.Find("ItemBarPanel");

        placed = false;
        message.SetActive(false);
        texts[0].text = "";
        inVehicle = false;
        closeToVehicle = false;
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

    // Update is called once per frame
    void Update () {
        if (!agent.isActiveAndEnabled && GetComponent<Rigidbody>().velocity.y == 0) {
            agent.enabled = true;
        } else if (agent.isActiveAndEnabled) {
            if (closeToVehicle && !inVehicle && Input.GetKeyDown(KeyCode.E)) {
                gameObject.SetActive(false);
                texts[0].text = "Press E to Exit";
                message.SetActive(true);
                inVehicle = true;
            } else if (inVehicle && Input.GetKeyDown(KeyCode.E)) {
                gameObject.SetActive(true);
                inVehicle = false;
            }
            if (!inVehicle) {
                bool running = Input.GetKey(KeyCode.LeftShift);

                if (Input.GetMouseButtonDown(0)) {
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
            } else {

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
