using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	protected PlayerMovementController player;

	[SerializeField]
	protected Slider powerSlider;

	// Use this for initialization
	void Start () {

		StartCoroutine(this.FindPlayer());
	}
		
	protected void SliderValueUpdated() {


	}

	protected IEnumerator FindPlayer() {

		while(this.player == null) {

			this.player = GameObject.FindGameObjectWithTag("Player");
			yield return null;
		}
	}
}
