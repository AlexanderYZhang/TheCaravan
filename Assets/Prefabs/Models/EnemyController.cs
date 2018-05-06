using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    public NavMeshAgent agent;
    public GameObject player;
    public float health;

    private Animator anim;
    private bool placed;
    private bool tracking;

    private HashSet<GameObject> enemiesInRange;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        enemiesInRange = new HashSet<GameObject>();
        anim.SetTrigger("moving");
        tracking = false;
    }

    void OnTriggerEnter(Collider other) {
        GameObject obj = other.gameObject;
        if (obj.tag == "Player" || obj.tag == "Turret" || obj.tag == "Vehicle") {
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
        if (!agent.isActiveAndEnabled && GetComponent<Rigidbody>().velocity.y == 0) {
            agent.enabled = true;
        } else if (agent.isActiveAndEnabled && player != null) {
            float animSpeedPct = 0;
            float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
           
            anim.SetFloat("speedPct", animSpeedPct);
        }
    }
}
