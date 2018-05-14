using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    public GameObject target;
    public float health;

    private Animator anim;
    private bool placed;
    private bool tracking;
    NavMeshAgent agent;


    private HashSet<GameObject> enemiesInRange;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        anim.SetTrigger("moving");
        target = PlayerManager.instance.car;
        agent = gameObject.GetComponent<NavMeshAgent>();
        if (target) {
            agent.SetDestination(target.transform.position);
        }
    }

    void OnTriggerEnter(Collider other) {
        // If it is a wall ro turret, or player, attack it
    }

    void OnTriggerExit(Collider other) {
        // if player moves out of range, stop following the player
    }

    // Update is called once per frame
    void Update () {

        if (target != null) {
            float animSpeedPct = 1;
            float distToTarget = Vector3.Distance(transform.position, target.transform.position);

            anim.SetFloat("speedPct", animSpeedPct);
            agent.SetDestination(target.transform.position);
            Debug.Log(target.transform.position);
        }
    }
}
