using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkBombSprite : ShieldBoomSprite {

	[SerializeField]
	protected float rotSpeed;

	// Use this for initialization
	void Start () {
		this.spriteRenderer = this.GetComponent<SpriteRenderer>();
		this.spriteRenderer.enabled = false;
	}

	public override void ShowSprite(float timer) {

		StartCoroutine(ShowSpriteSeconds(timer));
		StartCoroutine(RotateCoRoutine(timer));
	}

	private IEnumerator RotateCoRoutine(float maxTimer) {

		float timer = 0;
		while(timer < maxTimer) {

			timer += Time.deltaTime;
			this.transform.Rotate(Vector3.forward * (rotSpeed * Time.deltaTime));
			yield return null;
		}
	}

}
