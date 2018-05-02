using UnityEngine;

public class Interactable : MonoBehaviour {
	//distance that player needs to get to object to interact
	public float radius = 3f;
	public Transform interactionTransform;

	bool isFocus = false;
	Transform player;

	public virtual void Interact() {}

	void Update() {
		if (isFocus) {
			float distance = Vector3.Distance(player.position, interactionTransform.position);

			if (distance <= radius) {
				Interact();
			}
		}
	}

	public void OnFocused (Transform playerTransform) {
		isFocus = true;
		player = playerTransform;
	}

	public void OnDefocused() {
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
