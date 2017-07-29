using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Ability : MonoBehaviour {

	protected List<Enemy> enemiesInRange;
	protected float abilityDepthRange;

	void Start() {

		this.InitValues();
	}

	public void CastAbility() {

		foreach(Enemy enemy in enemiesInRange) {

			enemy.TakeDamage();
		}
	}

	protected virtual void InitValues() {

		enemiesInRange = new List<Enemy>();
	}

	protected virtual void OnTriggerEnter2D(Collider2D col){ 

		Enemy enemy = col.GetComponent<Enemy>();
		if(enemy != null) {

			enemiesInRange.Add(enemy);
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D col){ 

		Enemy enemy = col.GetComponent<Enemy>();
		if(enemy != null) {

			enemiesInRange.Remove(enemy);
		}
	}
}
