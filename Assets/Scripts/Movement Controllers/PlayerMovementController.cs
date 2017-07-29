using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : CharacterMovementController {

	protected Rigidbody2D rBody;

	[SerializeField]
	protected float movementSpeed;
	[SerializeField]
	protected float acceleration;

	// Use this for initialization
	void Start () {

		this.rBody = this.GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {

		this.rBody.velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal") * this.movementSpeed, this.acceleration), Mathf.Lerp(0, Input.GetAxis("Vertical") * this.movementSpeed, this.acceleration));
	}

	// Update is called once per frame
	void Update () {

		float y = Mathf.Clamp(this.transform.position.y, this.minMaxY.x, this.minMaxY.y);
		this.transform.position = new Vector2(this.transform.position.x, y);

		this.AdjustSpriteScale(this.transform.position);
	}
}