using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteController : CharacterMovementController {


	void Start() {

		this.minMaxY = new Vector2(-4.4f, 0.75f);
		this.minMaxScale = new Vector2(1.2f, 0.9f);
	}

	void Update () {

		this.AdjustSpriteScale(this.transform.position);
	}
}
