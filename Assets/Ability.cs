using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Ability : MonoBehaviour {

	[SerializeField]
	protected int damageDone;
	[SerializeField]
	protected int energyCost;

	protected List<Enemy> enemiesInRange;

	void Start() {

		this.InitValues();
	}

	public void CastAbility() {

		foreach(Enemy enemy in enemiesInRange) {

			enemy.TakeDamage(this.damageDone);
		}
	}

	protected void CheckArray() {

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
}
