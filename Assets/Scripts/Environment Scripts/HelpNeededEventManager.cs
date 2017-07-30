using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HelpNeededEventManager : MonoBehaviour {

	public delegate void PlayerInZoneDelegate(Transform playerTarget);
	public event PlayerInZoneDelegate PlayerInZone;

	[SerializeField]
	protected HelpNeeded helpNeededTarget;
	protected List<Enemy> eventEnemies;
	protected Vector2 savedTarget;
	public GameObject marker;

	void Start() {

		eventEnemies = new List<Enemy>();
		Enemy[] enemies = this.GetComponentsInChildren<Enemy>();
		foreach(Enemy e in enemies) {

			eventEnemies.Add(e);
			e.OnEnemyDestoryed += EventEnemyDestoryed;
			e.UpdateTarget(this.helpNeededTarget.transform);
		}
	}

	void Update() {

		if(this.helpNeededTarget == null) {

			MiniMap.sharedInstance.EventEnded(this.marker);
		}
	}

	public void SetExitLocation(Vector2 loc) {

		StartCoroutine(SetMarkerCoRoutine());
		this.savedTarget = loc;
	}

	protected virtual void OnTriggerEnter2D(Collider2D col){ 

		if(col.tag == "Player") {

			if(PlayerInZone != null) {

				this.PlayerInZone(col.transform);
			}
		}
	}

	protected void EventEnemyDestoryed(Enemy enemy) {

		this.eventEnemies.Remove(enemy);
		if(this.eventEnemies.Count == 0) {

			//TODO:Signal Thank you sound effect here.
			//TODO:Signal Point gain.
			if(this.helpNeededTarget != null) { 
			
				this.helpNeededTarget.transform.parent = null;
				this.helpNeededTarget.Saved(savedTarget);
			}
			MiniMap.sharedInstance.EventEnded(this.marker);

			GameObject.Destroy(this.gameObject);
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D col){ 

		if(col.tag == "Player") {

			if(PlayerInZone != null) {

				if(this.helpNeededTarget != null) {
				
					this.PlayerInZone(this.helpNeededTarget.transform);
				}
			}
		}
	}

	protected IEnumerator SetMarkerCoRoutine() {

		while(MiniMap.sharedInstance == null) {

			yield return null;
		}

		this.marker = MiniMap.sharedInstance.EventSpawned(this.transform.position.x);
	}
}
