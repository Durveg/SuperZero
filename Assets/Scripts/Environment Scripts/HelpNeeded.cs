using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;
using Spine.Unity;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class HelpNeeded : CharacterMovementController {

	[SerializeField]
	protected float health;

	[SerializeField]
	protected Vector2 target;

	[SerializeField]
	protected float updateRate = 2f;

	private Seeker seeker;
	private Rigidbody2D rBody;

	public Path path;
	private int currentWaypoint = 0;

	//AI speed
	public float speed = 300f;
	public ForceMode2D fMode;

	[HideInInspector]
	public bool pathIsEnded = false;

	public float nextWaypointDistance = 3;

	public void Saved(Vector2 target) {

		SkeletonAnimation skel = this.GetComponent<SkeletonAnimation>();
		skel.AnimationState.SetAnimation(0, "Walk Normal", true);

		UIManager.sharedInstance.CivilianSaved();
		this.target  = target;
		StartCoroutine(this.UpdatePath());
	}

	public void TakeDamage(float damage) {

		health -= damage;
		if(health <= 0) {

			UIManager.sharedInstance.CivilianLost();
			GameObject.Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {

		this.minMaxY = new Vector2(-4.4f, 0.75f);
		this.minMaxScale = new Vector2(1.2f, 0.9f);

		this.seeker = this.GetComponent<Seeker>();
		this.rBody = this.GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {


		if(path == null)
			return;

		if(currentWaypoint >= path.vectorPath.Count) {
			if(pathIsEnded)
				return;

			GameObject.Destroy(this.gameObject);
			pathIsEnded = true;
			return;
		}
		pathIsEnded = false;


		Vector2 dir = (path.vectorPath[currentWaypoint] - this.transform.position).normalized;
		dir *= this.speed * Time.fixedDeltaTime;

		this.rBody.AddForce(dir, this.fMode);

		float dist = Vector3.Distance(this.transform.position, path.vectorPath[currentWaypoint]);
		if(dist < nextWaypointDistance) {

			currentWaypoint++;
		}
	}

	// Update is called once per frame
	void Update () {

		if(this.target != Vector2.zero) {

			float xDir = (target.x - this.transform.position.x);
			float xScale = this.transform.localScale.x;
			if(xDir > 0 && xScale < 0) {

				Vector3 scale = this.transform.localScale;
				scale.x *= -1;

				this.transform.localScale = scale;
			} 
			else if(xDir < 0 && xScale > 0) {

				Vector3 scale = this.transform.localScale;
				scale.x = scale.x * -1;

				this.transform.localScale = scale;
			}
		}

		float y = Mathf.Clamp(this.transform.position.y, this.minMaxY.x, this.minMaxY.y);
		this.transform.position = new Vector2(this.transform.position.x, y);

		this.AdjustSpriteScale(this.transform.position);
	}

	protected void OnPathComplete(Path p) {

		if(p.error == false) {

			this.path = p;
			this.currentWaypoint = 0;
		}
	}

	protected IEnumerator UpdatePath() {

		while(true) {

			seeker.StartPath(this.transform.position, target, OnPathComplete);

			yield return new WaitForSeconds(1 / updateRate);
		}
	}
}
