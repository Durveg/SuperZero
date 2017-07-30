using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkBomb : Ability {

	[SerializeField]
	protected float blinkDistance;

	[SerializeField]
	protected float bombTimer;

	[SerializeField]
	protected PlayerMovementController player;

	void Start () {
	
		this.InitValues();
	}

	public override void CastAbility() {

		this.transform.parent = null;
		player.BlinkPlayer(this.blinkDistance);

		StartCoroutine(this.CountDownBomb());
	}

	protected IEnumerator CountDownBomb() {

		yield return new WaitForSeconds(this.bombTimer);

		this.DamageEnemies();

		this.transform.parent = player.transform;
		this.transform.localPosition = Vector3.zero;
	}
}
