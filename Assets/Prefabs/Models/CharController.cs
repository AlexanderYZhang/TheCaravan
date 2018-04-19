using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {
    // Camera Setup
    public GameObject mainCam;
    public Vector3 mainCamOffset = new Vector3(15, 20, 15);
    public Vector3 mainCamRot = new Vector3(45, -135, 0);

    // Character Parameters
    public float walkSpeed = 5;
    public float runSpeed = 15;
    public float turnSpeed = 100;

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
        Vector2 forward = (new Vector2(Input.GetAxisRaw("Vertical"), 0.0f)).normalized;
        Vector2 turn = (new Vector2(Input.GetAxisRaw("Horizontal"), 0.0f)).normalized;

        bool running = Input.GetKey(KeyCode.LeftShift);
        float moveS = (running ? runSpeed : walkSpeed) * forward.x;
        float turnS = turnSpeed * turn.x;

        float animSpeedPct = (running ? 0.5f : 0.25f) * forward.x;
        anim.SetFloat("speedPct", animSpeedPct);

        transform.Translate(transform.forward * moveS * Time.deltaTime, Space.World);
        transform.Rotate(transform.up * turnS * Time.deltaTime);

        mainCam.transform.position = transform.position + mainCamOffset;
	}
}
