using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {
	public Interactable focus;
	public LayerMask movementMask;
	public LayerMask interactionMask;
	PlayerMotor motor;
	Camera camera;

	void Start() {
		motor = GetComponent<PlayerMotor>();
		camera = Camera.main;
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, movementMask)) {
				motor.MoveToPoint(hit.point);

				SetFocus(null);
			}
		}

		if (Input.GetMouseButtonDown(1)) {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
				Interactable interactable = hit.collider.GetComponent<Interactable>();
				Debug.Log(hit.collider);
				if (interactable != null)
				{
					SetFocus(interactable);			
				}
            }

        }
	}

	public void SetFocus(Interactable newFocus) {
		Debug.Log(newFocus);
		if (focus != newFocus && focus != null) {
			focus.OnDefocused();
		}

		focus = newFocus;
	
		if (focus != null) {
			focus.OnFocused(transform);
		}
		
		motor.FollowTarget(newFocus);
	}	
}
