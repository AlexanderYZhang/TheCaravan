using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {
    // Camera Setup
    public GameObject mainCam;
    private Vector3 mainCamOffset = new Vector3(0, 20, -20);
    private Vector3 mainCamRot = new Vector3(45, 0, 0);

    // Character Parameters
    private float walkSpeed = 5;
    private float runSpeed = 15;

    private Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        mainCam.transform.position = transform.position + mainCamOffset;
        mainCam.transform.eulerAngles = mainCamRot;
        anim.SetTrigger("moving");
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero) {
            transform.eulerAngles = Vector3.up * Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
        }

        bool running = Input.GetKey(KeyCode.LeftShift);
        float moveS = (running ? runSpeed : walkSpeed) * inputDir.magnitude;

        float animSpeedPct = (running ? 1.0f : 0.5f) * inputDir.magnitude;

        transform.Translate(transform.forward * moveS * Time.deltaTime, Space.World);
        anim.SetFloat("speedPct", animSpeedPct);

        mainCam.transform.position = transform.position + mainCamOffset;
    }
}
