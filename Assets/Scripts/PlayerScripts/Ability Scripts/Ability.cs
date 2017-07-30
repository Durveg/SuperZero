using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Ability : MonoBehaviour {

	[SerializeField]
	protected float powerDisableValue;
	[SerializeField]
	protected int damageDone;
	[SerializeField]
	protected int energyCost;
	[SerializeField]
	protected int abilityNumber;
	[SerializeField]
	protected float cooldownLength;
	[SerializeField]
	protected float rootTimer;
	[SerializeField]
	protected float knockBackForce;

	protected bool onCooldown = false;
	protected float power;

	protected bool disabled = false;

	protected List<Enemy> enemiesInRange;
	protected PlayerManager player;
	protected AbilitySprite abilitySpriteManager;

	void Start() {

		this.InitValues();
	}

	public virtual void FlipDamageArea() {

		Vector3 newScale = this.transform.localScale;
		newScale.x = newScale.x * -1f;
		this.transform.localScale = newScale;
	}

	public virtual float CastAbility() {

		float abilityCost = 0;
		if(this.disabled == false && this.onCooldown == false) {

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

	protected virtual void DamageEnemies() {

		for(int i = 0; i < this.enemiesInRange.Count; i++) {

			this.enemiesInRange[i].TakeDamage(this.damageDone, this.transform.position, this.knockBackForce);
		}
	}
		
	protected void PowerUpdated(float power) {

		this.power = power;
		if(power < this.powerDisableValue && this.disabled == false) {

			this.disabled = true;
			UIManager.sharedInstance.AbilityDisabled(this.abilityNumber, false);
		} 
		else if(power >= this.powerDisableValue && this.disabled == true) {

			UIManager.sharedInstance.AbilityDisabled(this.abilityNumber, true);
			this.disabled = false;
		}
	}

	protected virtual void CheckArray() {

		if(this.enemiesInRange == null) {

			this.enemiesInRange = new List<Enemy>();
		}
	}

	protected virtual void InitValues() {

		this.CheckArray();
		StartCoroutine(this.FindPlayer());
	}

	protected virtual void OnTriggerEnter2D(Collider2D col){ 

		this.CheckArray();

		Enemy enemy = col.GetComponent<Enemy>();
		if(enemy != null && enemiesInRange.Contains(enemy) == false) {

			enemiesInRange.Add(enemy);
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D col){ 

		this.CheckArray();

		Enemy enemy = col.GetComponent<Enemy>();
		if(enemy != null && enemiesInRange.Contains(enemy) == true) {

			enemiesInRange.Remove(enemy);
		}
	}

	protected IEnumerator FindPlayer() {

		while(this.player == null) {

			this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
			yield return null;
		}

		this.player.playerPowerUpdated += this.PowerUpdated;
	}

	protected IEnumerator CoolDown() {

		this.onCooldown = true;
		UIManager.sharedInstance.AbilityDisabled(this.abilityNumber, false);

		yield return new WaitForSeconds(this.cooldownLength);

		this.onCooldown = false;

		if(this.power >= this.powerDisableValue) {
			UIManager.sharedInstance.AbilityDisabled(this.abilityNumber, true);
		}
	}
}
