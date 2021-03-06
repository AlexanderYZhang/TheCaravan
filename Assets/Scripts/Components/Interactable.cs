﻿using UnityEngine;

public class Interactable : MonoBehaviour {
	//distance that player needs to get to object to interact
	public float radius = 3f;
	public Transform interactionTransform;

	bool isFocus = false;
	Transform player;

    private bool interacting;

	public virtual void Interact() {}

    public bool IsInteracting() {
        return interacting;
    }

	void Update() {
        if (isFocus) {
            float distance = Vector3.Distance(player.position, interactionTransform.position);

            if (distance <= radius) {
                Interact();
                interacting = true;
            }
		} else {
            interacting = false;
        }
	}

	public virtual void OnFocused (Transform playerTransform) {
		isFocus = true;
		player = playerTransform;
	}

	public virtual void OnDefocused() {
		isFocus = false;
		player = null;
	}

	void OnDrawGizmosSelected() {
		if (interactionTransform == null) {
			interactionTransform = transform;
		}

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(interactionTransform.position, radius);
	}
}
