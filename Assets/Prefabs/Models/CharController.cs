using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CharController : MonoBehaviour {
    public Camera cam;
    public GameObject car;
    public GameObject canvas;
    public NavMeshAgent agent;

    // Character Parameter
    private Animator anim;
    private bool placed;
    private Text[] texts;
    private GameObject message;
    private GameObject itemBar;

    public bool IsPlaced() {
        return placed;
    }

    public void SetPlaced() {
        if (!placed) {
            placed = true;
        }
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        anim.SetTrigger("moving");
        placed = false;
        texts = canvas.GetComponentsInChildren<Text>();
        message = GameObject.Find("MessagePanel");
        itemBar = GameObject.Find("ItemBarPanel");
        message.SetActive(false);
        texts[0].text = "BLANK";
    }

	// Update is called once per frame
	void Update () {
        if (!agent.isActiveAndEnabled && GetComponent<Rigidbody>().velocity.y == 0) {
            agent.enabled = true;
        } else if (agent.isActiveAndEnabled) {
            bool running = Input.GetKey(KeyCode.LeftShift);

            if (Input.GetMouseButton(0)) {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) {
                    agent.SetDestination(hit.point);
                }
            }

            agent.speed = running ? 15f : 5f;
            float animSpeedPct = (running ? 1.0f : 0.5f) * (agent.hasPath ? 1 : 0);
            anim.SetFloat("speedPct", animSpeedPct);
        }
    }
}
