using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMotor : MonoBehaviour
{

    Transform target;
    NavMeshAgent agent;     // Reference to our NavMeshAgent

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveToPoint(Vector3 point)
    {
        NavMeshHit hit;
        NavMeshQueryFilter filter = new NavMeshQueryFilter();
        // walkable
        filter.areaMask = (1 << NavMesh.GetAreaFromName("Walkable"));
        filter.agentTypeID = agent.agentTypeID;
        if (!NavMesh.SamplePosition(point, out hit, 1.0f, filter)) {
            //Debug.Log("invalid sample" + hit);
            for (float i = 5f; i <= 12f; i++) {
                if (NavMesh.SamplePosition(point, out hit, i, filter))
                {
                    //Debug.Log("found sample" + hit.position);
                    agent.SetDestination(hit.position);
                    //Debug.Log("query filter" + filter.agentTypeID);
                    if (Vector3.Distance(agent.pathEndPosition, hit.position) > 1f) {
                        break;
                    }        
                }
            }

        } else {
            agent.SetDestination(point);
        }
    }

    public void StopMoveToPoint() {
        agent.ResetPath();
    }

    public void FollowTarget(Interactable newFocus)
    {
        if (newFocus != null)
        {
            //agent.stoppingDistance = newFocus.radius * .8f;
            //agent.updateRotation = false;

			target = newFocus.interactionTransform;
        }
        else
        {
            agent.stoppingDistance = 0f;
            //agent.updateRotation = true;
            target = null;
        }
    }

    public bool IsMoving() {
        return agent.hasPath;
    }

    void Update()
    {
        if (target != null)
        {
            //Debug.Log(target.position);
            MoveToPoint(target.position);
            //FaceTarget();
        }
    }

    public void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

}