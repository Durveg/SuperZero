using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class HelpNeeded : CharacterMovementController {

	[SerializeField]
	protected float health;

	[SerializeField]
	protected Transform target;

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

	public void Saved(Transform target) {

		this.target = target;
		StartCoroutine(this.UpdatePath());
	}

	public void TakeDamage(float damage) {

		health -= damage;
		if(health <= 0) {

			//TODO:Signal helpNeeded lost
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

		if(this.target == null) {

			//TODO: Path around.
		}

		//TODO: Add look at

		if(path == null)
			return;

		if(currentWaypoint >= path.vectorPath.Count) {
			if(pathIsEnded)
				return;

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

			if(this.target != null) {

				seeker.StartPath(this.transform.position, target.position, OnPathComplete);
			}

			yield return new WaitForSeconds(1 / updateRate);
		}
	}
}
