using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PowerCharger : MonoBehaviour {

	[SerializeField]
	protected ParticleSystem chargingParticles;
	protected PlayerManager player;

	void Start() {

		this.chargingParticles = this.GetComponent<ParticleSystem>();
		this.chargingParticles.Play();
	}

	protected void StartCharging(Collision2D coll, bool exiting) {

		if(coll.gameObject.tag == "Player") {

			if(this.player == null) {

				this.player = coll.gameObject.GetComponent<PlayerManager>();
			}

			if(this.player.transform.position.x > -0.55f && this.player.transform.position.x < 0.55f && exiting == false) {

				if(this.chargingParticles.isPlaying == false) {
				
					this.player.PlayerEnterChargingStation(true);
					this.chargingParticles.Play();
				}
			}
			else {

				if(this.chargingParticles.isPlaying == true) {

					this.player.PlayerEnterChargingStation(false);
					this.chargingParticles.Pause();
					this.chargingParticles.Clear();
				}
			}
		} 
	}

	void OnCollisionEnter2D(Collision2D coll) {

		this.StartCharging(coll, false);
	}

	void OnCollisionStay2D(Collision2D coll) {

		this.StartCharging(coll, false);
	}

	void OnCollisionExit2D(Collision2D coll) {

		this.StartCharging(coll, true);
	}
}
