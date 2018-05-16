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
        StartCoroutine("followPlayer");
    }

    void OnTriggerEnter(Collider other) {
        // If it is a wall ro turret, or player, attack it
    }

    void OnTriggerExit(Collider other) {
        // if player moves out of range, stop following the player
    }

    // Update is called once per frame
    void Update () {


    }
    
    void OnDrawGizmos() {
        if (agent != null) {
            NavMeshPath path = agent.path;
            if (path != null) {
                for (int i = 0; i < path.corners.Length - 1; i++) {
                    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
                }

            }
        }
    }
    IEnumerator followPlayer()
    {
        while (true)
        {   
            if (target != null)
            {
                float animSpeedPct = 1;
                float distToTarget = Vector3.Distance(transform.position, target.transform.position);

                anim.SetFloat("speedPct", animSpeedPct);
                if (Vector3.Distance(agent.pathEndPosition, target.transform.position) >= 1f)
                {
                    agent.SetDestination(target.transform.position);
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

}

