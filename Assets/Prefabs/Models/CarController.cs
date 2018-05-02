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

    private CharController playerController;

    private Text[] texts;
    private GameObject message;
    private GameObject itemBar;

    // Use this for initialization
    void Start () {
        playerController = GetComponent<CharController>();
        texts = canvas.GetComponentsInChildren<Text>();
        message = GameObject.Find("MessagePanel");
        itemBar = GameObject.Find("ItemBarPanel");
    }

    // Update is called once per frame
    void Update () {
        /*
		if (playerController.InsideVehicle()) {
            texts[0].text = "Press E to Exit";
            message.SetActive(true);
            marker.SetActive(false);
        }
        */
	}
}
