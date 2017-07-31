using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager sharedInstance;

	[SerializeField]
	protected AudioSource punch;
	[SerializeField]
	protected AudioSource blink;
	[SerializeField]
	protected AudioSource blinkBomb;
	[SerializeField]
	protected AudioSource laser;
	[SerializeField]
	protected AudioSource pound;
	[SerializeField]
	protected AudioSource thankYou;

	void Start () {

		sharedInstance = this;
	}


	public void PlayPunch() {

		this.punch.Play();
	}

	public void PlayBlink() {

		this.blink.Play();
	}

	public void PlayBlinkBomb() {

		this.blinkBomb.Play();
	}

	public void PlayLaser() {

		this.laser.Play();
	}

	public void playPound() {

		this.pound.Play();
	}

	public void PlayThankYou() {

		this.thankYou.Play();
	}
}

