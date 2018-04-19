using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    // Camera Setup
    public GameObject mainCam;
    public Vector3 mainCamOffset = new Vector3(15, 20, 15);
    public Vector3 mainCamRot = new Vector3(45, -135, 0);

    // Character Parameters
    public float walkSpeed = 10;
    public float runSpeed = 20;

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
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero) {
            transform.eulerAngles = Vector3.up * Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
        }

        bool running = Input.GetKey(KeyCode.LeftShift);
        float speed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;

        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

        mainCam.transform.position = transform.position + mainCamOffset;
	}
}
