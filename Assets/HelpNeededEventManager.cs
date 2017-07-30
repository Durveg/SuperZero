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
	protected Transform savedTarget;

	void Start() {

		eventEnemies = new List<Enemy>();
		Enemy[] enemies = this.GetComponentsInChildren<Enemy>();
		foreach(Enemy e in enemies) {

			eventEnemies.Add(e);
			e.OnEnemyDestoryed += EventEnemyDestoryed;
			e.UpdateTarget(this.helpNeededTarget.transform);
		}
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
			this.helpNeededTarget.transform.parent = null;
			this.helpNeededTarget.Saved(savedTarget);

			GameObject.Destroy(this.gameObject);
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D col){ 

		if(col.tag == "Player") {

			if(PlayerInZone != null) {

				this.PlayerInZone(this.helpNeededTarget.transform);
			}
		}
	}
}
