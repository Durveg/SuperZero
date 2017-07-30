using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkBomb : Ability {

	[SerializeField]
	protected float blinkDistance;

	[SerializeField]
	protected float bombTimer;

	void Start () {
	
		this.InitValues();
	}

	public override float CastAbility() {

		float abilityCost = 0;
		if(this.disabled == false && this.onCooldown == false) {
			
			abilityCost = this.energyCost;

			this.transform.parent = null;
			player.BlinkPlayer(this.blinkDistance);

			if(this.abilitySpriteManager == null) {

				this.abilitySpriteManager = this.GetComponentInChildren<AbilitySprite>();
			}

			if(this.abilitySpriteManager != null) {

				this.abilitySpriteManager.ShowSprite(this.bombTimer - 0.05f);
			}

			StartCoroutine(this.CoolDown());
			StartCoroutine(this.CountDownBomb());
		}

		return abilityCost;
	}

	protected IEnumerator CountDownBomb() {

		yield return new WaitForSeconds(this.bombTimer);

		this.DamageEnemies();

		this.transform.parent = player.transform;
		this.transform.localPosition = Vector3.zero;
	}
}
