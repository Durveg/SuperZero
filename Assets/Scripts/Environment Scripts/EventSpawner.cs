using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpawner : MonoBehaviour {

	public float baseSpawnEventTimer;
	private float spawnEventTimer;

	public Vector2 minMaxYvalue;

	public float[] eventPositions;
	private List<HelpNeededEventManager> eventManagers;

	public Vector2[] exitPositions;
	// Use this for initialization
	void Start () {

		spawnEventTimer = baseSpawnEventTimer;
		StartCoroutine(SpawnEventsOnTimer());
		StartCoroutine(updateEventsTimer());
	}

	private HelpNeededEventManager spawnEvent() {

		HelpNeededEventManager helpEvent = null;
		helpEvent = GameObject.Instantiate(Resources.Load("Event") as GameObject).GetComponent<HelpNeededEventManager>();
		return helpEvent;
	}

	protected IEnumerator updateEventsTimer() {

		spawnEventTimer = 20;
		int increseCounter = 0;
		int waitSeconds = 30;
		while(true) {

			yield return new WaitForSeconds(waitSeconds);

			switch(increseCounter) {

			case(0):
				this.spawnEventTimer = 20;
				break;

			case(1):
				this.spawnEventTimer = 18;
				break;

			case(2):
				this.spawnEventTimer = 16;
				break;

			case(3):
				this.spawnEventTimer = 15f;
				break;

			case(4):
				break;

			default:
				this.spawnEventTimer = 7f;
				break;

			}

			increseCounter++;
		}
	}

	protected IEnumerator SpawnEventsOnTimer() {

		while(true) {

			HelpNeededEventManager helpEvent = this.spawnEvent();
			int randomLoc = Random.Range(0, 6);
			int negValue = Random.Range(0, 2);
			if(negValue == 0) {

				negValue = -1;
			}

			float x = eventPositions[randomLoc] *  negValue;
			float y = Random.Range(minMaxYvalue.x, minMaxYvalue.y);
			helpEvent.transform.position = new Vector2(x, y);
			helpEvent.SetExitLocation(exitPositions[Random.Range(0, 6)]);

			yield return new WaitForSeconds(spawnEventTimer);
		}
	}
}
