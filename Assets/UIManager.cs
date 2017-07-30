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
	[SerializeField]
	protected Text PowerText;
	[SerializeField]
	protected Text CiviliansSavedText;
	[SerializeField]
	protected Text CiviliansLostText;
	protected int saved = 0;
	protected int lost = 0;

	// Use this for initialization
	void Start () {

		sharedInstance = this;
		StartCoroutine(this.FindPlayer());
	}
		
	public void AbilityDisabled(int abilityNum, bool enabled) {

		abilities[abilityNum].interactable = enabled;
	}

	public void CivilianSaved() {

		this.saved++;
		this.CiviliansSavedText.text = this.saved.ToString();
	}

	public void CivilianLost() {

		this.lost++;
		this.CiviliansLostText.text = this.lost.ToString();
	}

	public void DisplayGameOverScreen() {


	}

	protected void SliderValueUpdated(float value) {

		this.powerSlider.value = value;
		this.PowerText.text = string.Format("Power {0:F1} / 100", value);
	}

	protected IEnumerator FindPlayer() {

		while(this.player == null) {

			this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
			yield return null;
		}

		this.player.playerPowerUpdated += this.SliderValueUpdated;
	}
}
