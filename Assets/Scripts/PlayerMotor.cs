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
        agent.SetDestination(point);
    }

    public void StopMoveToPoint() {
        agent.ResetPath();
    }

    public void FollowTarget(Interactable newFocus)
    {
        if (newFocus != null)
        {
            agent.stoppingDistance = newFocus.radius * .8f;
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