﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomAbility : Ability {

	// Use this for initialization
	void Start () {

		this.InitValues();
	}
	
	public override float CastAbility() {

		float abilityCost = 0;
		if(this.disabled == false && this.onCooldown == false) {

			this.onCooldown = true;
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
				
			StartCoroutine(this.WaitForDamage(this.rootTimer));
		}

		return abilityCost;
	}

	private IEnumerator WaitForDamage(float timer) {

		yield return new WaitForSeconds(timer);

		SoundManager.sharedInstance.playPound();
		this.DamageEnemies();
		StartCoroutine(this.CoolDown());
	}
}
