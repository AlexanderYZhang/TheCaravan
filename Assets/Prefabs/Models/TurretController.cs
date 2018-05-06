using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
    public GameObject turretHead;
    public float scanSpeed;
    public float rateOfFire;
    public GameObject bulletType;

    private string turretState;
    private HashSet<GameObject> enemiesInRange;

    private GameObject closest;
    private float timeLastShot;

    // Use this for initialization
    void Start () {
        turretState = "idle";
        enemiesInRange = new HashSet<GameObject>();
        closest = null;
        timeLastShot = 0;
	}

    void OnTriggerEnter(Collider other) {
        GameObject obj = other.gameObject;
        if (obj.tag == "Enemy" && other is CapsuleCollider) {
            enemiesInRange.Add(obj);
        }
    }

    void OnTriggerExit(Collider other) {
        GameObject obj = other.gameObject;
        if (enemiesInRange.Contains(obj)) {
            enemiesInRange.Remove(obj);
        }
    }

    // Update is called once per frame
    void Update () {
        closest = null;
        if (enemiesInRange.Count > 0) {
            float minDist = float.MaxValue;
            foreach (GameObject obj in enemiesInRange) {
                float dist = Vector3.Distance(obj.transform.position, transform.position);
                if (dist < minDist) {
                    minDist = dist;
                    closest = obj;
                }
            }
            turretState = "attack";
        } else {
            turretState = "idle";
        }
    }

    void LateUpdate() {
        switch (turretState) {
            case "idle": Idle(); break;
            case "attack": Attack(closest); break;
            default: Idle(); break;
        }
    }

    private void Idle() {
        turretHead.transform.Rotate(new Vector3(0, 0, scanSpeed) * Time.deltaTime);
    }

    private void Fire(float x, float z) {
        GameObject bullet = GameObject.Instantiate(bulletType);
        Vector3 dir = new Vector3(x, 0, z).normalized;
        bullet.transform.position = turretHead.transform.position + 4 * dir + new Vector3(0, 0.65f, 0);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.speed = 50;
        bulletController.direction = dir;
        bulletController.maxDist = GetComponent<SphereCollider>().radius * transform.localScale.x;
        bullet.transform.parent = transform;
    }

    private void Attack(GameObject obj) {
        float x = obj.transform.position.x - turretHead.transform.position.x;
        float z = obj.transform.position.z - turretHead.transform.position.z;
        float faceRot = Mathf.Atan2(x, z) * Mathf.Rad2Deg;
        turretHead.transform.eulerAngles = new Vector3(turretHead.transform.eulerAngles.x, 0, faceRot);
        if (Time.time - timeLastShot >= 1 / rateOfFire) {
            Fire(x, z);
            timeLastShot = Time.time;
        }   
    }
}
