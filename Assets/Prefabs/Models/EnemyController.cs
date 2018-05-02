using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    public NavMeshAgent agent;
    public GameObject player;
    public float startTracking;
    public float stopTracking;

    private Animator anim;
    private bool placed;
    private bool tracking;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        anim.SetTrigger("moving");
        tracking = false;
    }

    // Update is called once per frame
    void Update () {
        if (!agent.isActiveAndEnabled && GetComponent<Rigidbody>().velocity.y == 0) {
            agent.enabled = true;
        } else if (agent.isActiveAndEnabled && player != null) {
            float animSpeedPct = 0;
            float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (!tracking && distToPlayer < startTracking) {
                tracking = true;
            } else if (tracking && distToPlayer > stopTracking) {
                tracking = false;
                agent.SetDestination(transform.position);
            } else if (tracking) {
                agent.SetDestination(player.transform.position);
                animSpeedPct = 0.5f;
            }
            anim.SetFloat("speedPct", animSpeedPct);
        }
    }
}
