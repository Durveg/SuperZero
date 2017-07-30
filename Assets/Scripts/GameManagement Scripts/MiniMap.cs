using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {


	public static MiniMap sharedInstance;

	public Vector2 minMaxMiniMap;
	public Vector2 minMaxRealWorld;
	public PlayerManager player;
	public RectTransform playerMinimap;
	private List<GameObject> markers;

	void Start () {

		markers = new List<GameObject>();
		sharedInstance = this;
	}

	public GameObject EventSpawned(float xPos) {

		float x = (xPos * 300) / 95;
		float y = 48;

		GameObject marker = GameObject.Instantiate(Resources.Load("EventMarker") as GameObject);

		marker.GetComponent<RectTransform>().SetParent(this.transform);
		marker.GetComponent<RectTransform>().localPosition = new Vector2(x, y);
		markers.Add(marker);

		return marker;
	}

	public void EventEnded(GameObject marker) {

		markers.Remove(marker);
		GameObject.Destroy(marker.gameObject);
	}

	void Update () {

		float x = (player.transform.position.x * 300) / 95;
		float y = 48;
		playerMinimap.localPosition = new Vector2(x, y);
	}
}
