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

	protected bool disabled = false;

	protected List<Enemy> enemiesInRange;

	void Start() {

		this.InitValues();
	}

	public virtual void FlipDamageArea() {

		Vector3 newScale = this.transform.localScale;
		newScale.x = newScale.x * -1f;
		this.transform.localScale = newScale;
	}

	public virtual void CastAbility() {

		if(this.disabled == false) {
			
			this.DamageEnemies();
		}
	}

	protected virtual void DamageEnemies() {

		for(int i = 0; i < this.enemiesInRange.Count; i++) {

			this.enemiesInRange[i].TakeDamage(this.damageDone);
		}
	}

	protected void PowerUpdated(float power) {

		if(power < this.powerDisableValue && this.disabled == false) {

			this.disabled = true;
			UIManager.sharedInstance.AbilityDisabled(this.abilityNumber, false);
		} 
		else if(this.disabled == true) {

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

		PlayerManager player = null;
		while(player == null) {

			player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
			yield return null;
		}

		player.playerPowerUpdated += this.PowerUpdated;
	}
}
