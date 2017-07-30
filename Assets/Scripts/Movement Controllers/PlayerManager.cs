using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : CharacterMovementController {

	public delegate void PlayerValueUpdatedDelegate(float value);
	public event PlayerValueUpdatedDelegate playerPowerUpdated;

	protected Rigidbody2D rBody;

	[SerializeField]
	protected float baseMovementSpeed;
	[SerializeField]
	protected float baseAcceleration;
	[SerializeField]
	protected float sprintMovementSpeed;
	[SerializeField]
	protected float sprintAcceleration;

	protected float movementSpeed;
	protected float acceleration;

	[SerializeField]
	protected List<Ability> ablities;

	protected float facingDirection = 1;

	#region Unity Methods
	void Start () {

		if(this.ablities == null) {

			Debug.Log("Abilities not set up.");
		}

		this.rBody = this.GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {

		this.rBody.velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal") * this.movementSpeed, this.acceleration), Mathf.Lerp(0, Input.GetAxis("Vertical") * this.movementSpeed, this.acceleration));
	}

	void Update () {
		if(this.ablities != null && this.ablities.Count == 4) {
		
			if(Input.GetAxisRaw("Ability 1") > 0) {

				this.ablities[0].CastAbility();
			}

			if(Input.GetAxisRaw("Ability 2") > 0) {

				this.ablities[1].CastAbility();
			}

			if(Input.GetAxisRaw("Ability 3") > 0) {

				this.ablities[2].CastAbility();
			}

			if(Input.GetAxisRaw("Ability 4") > 0) {

				this.ablities[3].CastAbility();
			}
		}

		int dir = (int)Input.GetAxisRaw("Horizontal");
		if(dir > 0 || dir < 0) {
		
			if(this.facingDirection != dir) {

				this.facingDirection = dir;
				foreach(Ability ab in this.ablities) {

					ab.FlipDamageArea();
				}
			}
		}

		if(Input.GetAxisRaw("Sprint") > 0) {

			this.acceleration = this.sprintAcceleration;
			this.movementSpeed = this.sprintMovementSpeed;

		} else {

			this.acceleration = this.baseAcceleration;
			this.movementSpeed = this.baseMovementSpeed;
		}

		//Clamp Player to max position on screen.
		float y = Mathf.Clamp(this.transform.position.y, this.minMaxY.x, this.minMaxY.y);
		this.transform.position = new Vector2(this.transform.position.x, y);

		//Adjust player scale to depth on screen.
		this.AdjustSpriteScale(this.transform.position);
	}
	#endregion

	public void BlinkPlayer(float distance) {

		Vector2 newPos = new Vector2();
		newPos.y = this.transform.position.y;
		newPos.x = this.transform.position.x + (distance * this.facingDirection);

		this.transform.position = newPos;
	}
}