using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBoomSprite : AbilitySprite {

	[SerializeField]
	protected float scaleSize;

	// Use this for initialization
	void Start () {

		this.spriteRenderer = this.GetComponent<SpriteRenderer>();
		this.spriteRenderer.enabled = false;
	}
	
	public override void ShowSprite(float timer) {

		StartCoroutine(ShowSpriteSeconds(timer));
	}

	protected virtual IEnumerator ShowSpriteSeconds(float timer) {

		float waitTime = timer * 0.9f;
		float explosionTime = timer * 0.1f;

		this.spriteRenderer.enabled = true;

		yield return new WaitForSeconds(waitTime);

		float boomTimer = 0;
		Vector3 originalScale = this.transform.localScale;
		Vector3 newScale = new Vector3(this.scaleSize, this.scaleSize, originalScale.z);
		while(boomTimer < explosionTime) {

			this.transform.localScale = Vector3.Lerp(originalScale, newScale, boomTimer / explosionTime);
			boomTimer += Time.deltaTime;
			yield return null;
		}

		this.spriteRenderer.enabled = false;
		this.transform.localScale = originalScale;
	}
}
