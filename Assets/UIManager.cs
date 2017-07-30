using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager sharedInstance;

	protected PlayerManager player;

	[SerializeField]
	protected Slider powerSlider;
	[SerializeField]
	protected Button[] abilities;

	// Use this for initialization
	void Start () {

		sharedInstance = this;
		StartCoroutine(this.FindPlayer());
	}
		
	public void AbilityDisabled(int abilityNum, bool enabled) {

		abilities[abilityNum].interactable = enabled;
	}

	protected void SliderValueUpdated(float value) {

		this.powerSlider.value = value;
	}

	protected IEnumerator FindPlayer() {

		while(this.player == null) {

			this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
			yield return null;
		}

		this.player.playerPowerUpdated += this.SliderValueUpdated;
	}
}
