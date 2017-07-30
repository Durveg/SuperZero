using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Spine.Unity;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : CharacterMovementController {

	public delegate void PlayerValueUpdatedDelegate(float value);
	public event PlayerValueUpdatedDelegate playerPowerUpdated;

	protected Rigidbody2D rBody;

	protected bool playerOnChargeStation;
	protected bool rooted = false;

	[SerializeField]
	protected float baseMovementSpeed;
	[SerializeField]
	protected float baseAcceleration;
	[SerializeField]
	protected float sprintMovementSpeed;
	[SerializeField]
	protected float sprintAcceleration;
	[SerializeField] 
	protected float powerDownBaseRate;
	protected float powerDownRate;

	[SerializeField]
	protected float powerUpBaseRate;
	protected float powerUpRate;

	protected float movementSpeed;
	protected float acceleration;
	protected float playerPower = 100;

	[SerializeField]
	protected List<Ability> ablities;

	protected float facingDirection = 1;

	[SerializeField]
	protected AnimationController controller;

	#region Unity Methods
	void Start () {

		if(this.ablities == null) {

			Debug.Log("Abilities not set up.");
		}

		this.rBody = this.GetComponent<Rigidbody2D>();

		this.playerOnChargeStation = true;
		this.powerDownRate = this.powerDownBaseRate;
		this.powerUpRate = this.powerUpBaseRate;

		StartCoroutine(this.PowerDown());
		StartCoroutine(this.ScalePowerDownRate());
	}

	void FixedUpdate() {

		if(this.rooted == false) {

			this.controller.Walk();
			this.rBody.velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal") * this.movementSpeed, this.acceleration), Mathf.Lerp(0, Input.GetAxis("Vertical") * this.movementSpeed, this.acceleration));
			if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {

				this.controller.Idle();
			}
		} 
		else {

			this.rBody.velocity = Vector2.zero;
		}
	}

	void Update () {
		if(this.ablities != null && this.ablities.Count == 4) {
		
			if(Input.GetAxisRaw("Ability 1") > 0) {


				this.controller.punch();
				this.playerPower -= this.ablities[0].CastAbility();
			}

			if(Input.GetAxisRaw("Ability 2") > 0) {

				this.controller.Rooted(this.ablities[1].rootTimer, "Idle");
				this.playerPower -= this.ablities[1].CastAbility();
			}

			if(Input.GetAxisRaw("Ability 3") > 0) {

				this.controller.Rooted(this.ablities[2].rootTimer, "Idle");
				this.playerPower -= this.ablities[2].CastAbility();
			}

			if(Input.GetAxisRaw("Ability 4") > 0) {

				this.playerPower -= this.ablities[3].CastAbility();
			}
		}

		int dir = (int)Input.GetAxisRaw("Horizontal");
		if(dir > 0 || dir < 0) {
		
			if(this.facingDirection != dir) {

				this.facingDirection = dir;
				this.FlipDamageArea();
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

	#region Public Methods
	public void AbilityRoot(float rootTimer) {

		StartCoroutine(this.RootPlayer(rootTimer));
	}

	public void BlinkPlayer(float distance) {

		Vector2 newPos = new Vector2();
		newPos.y = this.transform.position.y;
		newPos.x = this.transform.position.x + (distance * this.facingDirection);
		if(newPos.x < -95.32143) {

			newPos.x = -95.32143f;
		} 
		else if(newPos.x > 95.32143) {

			newPos.x = 95.32143f;
		}


		this.transform.position = newPos;
	}

	public void PlayerEnterChargingStation(bool onStation) {

		this.playerOnChargeStation = onStation;
	}
	#endregion

	protected void FlipDamageArea() {

		Vector3 newScale = this.transform.localScale;
		newScale.x = newScale.x * -1f;
		this.transform.localScale = newScale;
	}

	#region CoRoutines
	protected IEnumerator RootPlayer(float rootTimer) {

		this.rooted = true;
		yield return new WaitForSeconds(rootTimer);
		this.rooted = false;
	}

	protected IEnumerator PowerDown() {

		while(true) {

			if(this.playerOnChargeStation == false) {
			
				this.playerPower -= this.powerDownRate * Time.deltaTime;
				if(this.playerPower < 0) {

					StartCoroutine(this.delayGameOver());
					this.playerPower = 0;
				}
			} 
			else {

				if((this.playerPower + this.powerUpRate * Time.deltaTime) <= 100) {
				
					this.playerPower += this.powerUpRate * Time.deltaTime;
				} 
				else {

					this.playerPower = 100;
				}
			}

			if(this.playerPowerUpdated != null) {

				this.playerPowerUpdated(this.playerPower);
			}
			yield return null;
		}
	}

	protected IEnumerator delayGameOver() {

		this.controller.Die();
		yield return new WaitForSeconds(.5f);
		GameManager.sharedInstance.GameOver();
	}

	protected IEnumerator ScalePowerDownRate() {

		int increseCounter = 0;
		int waitSeconds = 30;
		while(true) {
			
			yield return new WaitForSeconds(waitSeconds);

			switch(increseCounter) {
				
			case(0):
				this.powerDownRate *= 2;
				break;

			case(1):
				this.powerDownRate *= 2;
				waitSeconds = 60;
				break;

			case(2):
				this.powerDownRate *= 2;
				waitSeconds = 90;
				break;

			case(3):
				this.powerDownRate *= 1.5f;
				waitSeconds = 120;
				break;

			case(4):
				break;

			default:
				this.powerDownRate *= 1.5f;
				break;

			}

			increseCounter++;
		}
	}
	#endregion
}