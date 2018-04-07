using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public float speed { get; set; }
    public float angularSpeed { get; set; }
    public float jumpForce { get; set; }

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();

		this.speed = 10.0f;
		this.angularSpeed = 2.0f;
        this.jumpForce = 20.0f;
    }

	// Update is called once per frame
	void Update ()
    {

	}

	// Physics stuff called before physics calculations
	void FixedUpdate() {
		float moveVertical = Input.GetAxis ("Vertical");
		float moveHorizontal = Input.GetAxis("Horizontal");

		Vector3 rotation = new Vector3 (0, moveHorizontal, 0);
        Vector3 upwardsVelocity = new Vector3(0, rb.velocity.y, 0);

        rb.velocity = (transform.forward * this.speed * moveVertical) + upwardsVelocity;
       
		transform.Rotate(rotation * this.angularSpeed);
	}
}
