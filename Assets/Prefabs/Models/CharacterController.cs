using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    public GameObject mainCam;
    public Vector3 mainCamOffset;
    public Vector3 mainCamRot;

    private Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        mainCam.transform.position = mainCamOffset;
        mainCam.transform.eulerAngles = mainCamRot;
        anim.SetTrigger("moving");
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
