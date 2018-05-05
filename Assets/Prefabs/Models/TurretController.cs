using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
    public GameObject turretHead;
    public GameObject enemyCollection;
    public float scanSpeed;


    private string turretState;

	// Use this for initialization
	void Start () {
        turretState = "idle";
	}
	
    private void Idle() {
        turretHead.transform.Rotate(new Vector3(0, 0, scanSpeed) * Time.deltaTime);
    }

	// Update is called once per frame
	void Update () {
        print(turretState);
        switch(turretState) {
            case "idle":  Idle(); break;
            default: Idle(); break;
        }
        
	}
}
