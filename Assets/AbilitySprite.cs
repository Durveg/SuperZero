using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AbilitySprite : MonoBehaviour {

	protected SpriteRenderer spriteRenderer;

	void Start() {

		this.spriteRenderer = this.GetComponent<SpriteRenderer>();
		this.spriteRenderer.enabled = false;
	}

	public virtual void ShowSprite(float timer) {

		StartCoroutine(ShowSpriteSeconds(timer));
	}

	private IEnumerator ShowSpriteSeconds(float timer) {

		this.spriteRenderer.enabled = true;

		yield return new WaitForSeconds(timer);

		this.spriteRenderer.enabled = false;
	}
}
