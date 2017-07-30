using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

	public Vector2 minMaxMiniMap;
	public Vector2 minMaxRealWorld;
	public PlayerManager player;
	public RectTransform playerMinimap;

	void Start () {
		
	}
	
	void Update () {

		float x = (player.transform.position.x * 300) / 95;
		float y = 48;
		playerMinimap.localPosition = new Vector2(x, y);
	}
}
