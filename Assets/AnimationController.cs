using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Spine.Unity;

public class AnimationController : MonoBehaviour {

	[SerializeField] 
	SkeletonAnimation controller;
	Spine.AnimationState state;

	private bool playing = false;
	private bool dead = false;

	private bool walking = false;
	public void punch(){
		this.CheckState();

		if(this.playing == false && dead == false) {

			walking = false;
			StartCoroutine(this.AnimationTimer(.25f, "Punch"));
		}
	}

	public void Rooted(float timer, string kind) {
		this.CheckState();

		if(this.playing == false && dead == false) {

			walking = false;
			StartCoroutine(this.AnimationTimer(timer, kind));
		}
	}

	public void Die() {

		if(this.playing == false && dead == false) {

			walking = false;
			this.CheckState();
			if(this.state != null) {

				this.state.SetAnimation(0, "Die", true);
			}
		}
	}

	public void Walk() {

		if(this.playing == false && dead == false && walking == false) {

			this.walking = true;
			this.CheckState();
			if(this.state != null) {

				this.state.SetAnimation(0, "Walk", true);
			}
		}
	}

	public void Idle() {

		if(this.playing == false && dead == false) {

			walking = false;
			this.CheckState();
			if(this.state != null) {

				this.state.SetAnimation(0, "Idle", true);
			}
		}
	}

	private void CheckState() {

		if(this.state == null && this.controller != null) {

			this.state = this.controller.AnimationState;
		}
	}

	private IEnumerator AnimationTimer(float timer, string animation) {

		playing = true;
		if(this.state != null) {

			this.state.SetAnimation(0, animation, false);
		}

		yield return new WaitForSeconds(timer);

		playing = false;
	}
}
