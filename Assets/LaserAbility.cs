using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAbility : Ability {

	// Use this for initialization
	void Start () {
	
		this.InitValues();
	}

	public override float CastAbility() {

		float abilityCost = 0;
		if(this.disabled == false && this.onCooldown == false) {

			SoundManager.sharedInstance.PlayLaser();
			abilityCost = this.energyCost;

			if(this.abilitySpriteManager == null) {

				this.abilitySpriteManager = this.GetComponentInChildren<AbilitySprite>();
			}

			if(this.abilitySpriteManager != null) {

				this.abilitySpriteManager.ShowSprite(this.rootTimer);
			}

			if(this.rootTimer > 0 && this.player != null) {

				this.player.AbilityRoot(this.rootTimer);
			}

			this.DamageEnemies();

			StartCoroutine(this.CoolDown());
		}

		return abilityCost;
	}
}
