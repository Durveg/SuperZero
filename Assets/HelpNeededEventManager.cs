using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HelpNeededEventManager : MonoBehaviour {

	public delegate void PlayerInZoneDelegate(Transform playerTarget);
	public event PlayerInZoneDelegate PlayerInZone;

	[SerializeField]
	protected Transform defaultTarget;

	protected virtual void OnTriggerEnter2D(Collider2D col){ 

		if(col.tag == "Player") {

			if(PlayerInZone != null) {

				this.PlayerInZone(col.transform);
			}
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D col){ 

		if(col.tag == "Player") {

			if(PlayerInZone != null) {

				this.PlayerInZone(defaultTarget);
			}
		}
	}
}
