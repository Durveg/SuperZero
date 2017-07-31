using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;
using Spine.Unity;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class Enemy : EnemySpriteController {

	public delegate void OnEnemyDestroyedDelegate(Enemy enemy);
	public event OnEnemyDestroyedDelegate OnEnemyDestoryed;

	[SerializeField]
	protected float health;
	[SerializeField]
	protected float damageDealt;

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
	protected float facing = 1;

	protected HelpNeeded citizen;

	protected SkeletonDataAsset dataAsset;

	// Use this for initialization
	void Start () {

		this.minMaxY = new Vector2(-4.4f, 0.75f);
		this.minMaxScale = new Vector2(1.2f, 0.9f);

		this.seeker = this.GetComponent<Seeker>();
		this.rBody = this.GetComponent<Rigidbody2D>();


		HelpNeededEventManager manager = this.GetComponentInParent<HelpNeededEventManager>();
		if(manager != null) {

			manager.PlayerInZone += UpdateTarget;
		}

		StartCoroutine(this.UpdatePath());
		StartCoroutine(this.DealDamageCitizen());
	}

	public void UpdateTarget(Transform newTarget) {

		this.target = newTarget;
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

		if(this.target != null) {

			float xDir = (target.transform.position.x - this.transform.position.x);
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

	public void TakeDamage(int damageTaken, Vector3 incoming, float force) {

		this.health -= damageTaken;
		if(this.health <= 0) {

			this.EnemyDied();
		}


		Vector3 dir = (incoming - this.transform.position).normalized;
		this.rBody.AddForce(dir * force * -1);
	}

	protected void EnemyDied() {

		//TODO: Signal for game manager here maybe.
		//Add something else instead of destroying, maybe cache.
		if(this.OnEnemyDestoryed != null) {

			this.OnEnemyDestoryed(this);
		}

		GameObject.Destroy(this.gameObject);
	}

	protected void OnPathComplete(Path p) {

		if(p.error == false) {

			this.path = p;
			this.currentWaypoint = 0;
		}
	}

	protected virtual void OnTriggerEnter2D(Collider2D col){ 

		HelpNeeded citizen = col.GetComponent<HelpNeeded>();
		if(citizen != null) {

			this.citizen = citizen;
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D col){ 

		HelpNeeded citizen = col.GetComponent<HelpNeeded>();
		if(citizen != null) {

			this.citizen = null;
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

	protected IEnumerator DealDamageCitizen() {

		while(true) {

			if(this.citizen != null && this.target != null && this.target.tag != "Player") {

				this.citizen.TakeDamage(this.damageDealt);
			}

			yield return new WaitForSeconds(1);
		}
	}
}
