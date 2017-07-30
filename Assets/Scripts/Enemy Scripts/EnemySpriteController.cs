using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteController : CharacterMovementController {


	void Start() {


	}

	void Update () {

		this.AdjustSpriteScale(this.transform.position);
	}
}
