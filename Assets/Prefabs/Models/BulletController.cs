using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public Vector3 direction;
    public float speed;
    public float maxDist;

    // Use this for initialization
    void Start () {
		
	}

    void OnTriggerEnter(Collider other) {
        GameObject obj = other.gameObject;
        if (obj.tag == "Enemy") {
            
        }
    }

    // Update is called once per frame
    void Update () {
	}

    void LateUpdate() {
        transform.Translate(direction * Time.deltaTime * speed);
        if (Vector3.Distance(transform.position, transform.parent.position) > maxDist) {
            GameObject.Destroy(gameObject);
        }
    }
}
