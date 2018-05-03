using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PlayerController))]
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
            agent.updateRotation = false;

			target = newFocus.interactionTransform;
        }
        else
        {
            agent.stoppingDistance = 0f;
            agent.updateRotation = true;
            target = null;
        }
    }

    void Update()
    {
        if (target != null)
        {
            MoveToPoint(target.position);
            FaceTarget();
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

}