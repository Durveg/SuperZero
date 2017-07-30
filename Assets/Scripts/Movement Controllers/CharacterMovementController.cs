using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour {

	[SerializeField]
	protected Transform playerSpriteTransform;
	[SerializeField]
	protected Vector2 minMaxY;
	[SerializeField]
	protected Vector2 minMaxScale;

	protected float valDist;

	protected void CalcValDist() {

		this.valDist = minMaxScale.y - minMaxScale.x;
	}

	protected float CalcPercentDistance(float currentDist) {

		float min = this.minMaxY.x;
		float max = this.minMaxY.y;

		return Mathf.Abs(min - currentDist) / Mathf.Abs(min - max);
	}

	protected void AdjustSpriteScale(Vector3 characterPos) {

		this.CalcValDist();

		float clampedPos = Mathf.Clamp(characterPos.y, this.minMaxY.x, this.minMaxY.y);
		float percentDist = this.CalcPercentDistance(clampedPos);

		float scale = minMaxScale.x + (percentDist * this.valDist);
		scale = Mathf.Clamp(scale, this.minMaxScale.y, this.minMaxScale.x);
		Vector3 newScale = new Vector3(scale, scale, 1);

		if(playerSpriteTransform.localScale.x < 0) {

			newScale.x *= -1;
		}

		if(playerSpriteTransform != null) {
		
			playerSpriteTransform.localScale = newScale;
		}
	}
}
 