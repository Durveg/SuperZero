using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager sharedInstance;

	public delegate void GamePaused(bool paused);
	public event GamePaused gamePausedEvent;

	private bool gameCanBePaused = true;
	public bool gamePaused = false;

	public void GameOver() {

		Time.timeScale = 0;
		UIManager.sharedInstance.DisplayGameOverScreen();
	}

	public void ResumeGame() {

		this.gamePaused = false;
		Time.timeScale = 1;
		StartCoroutine(this.DelayRePause());

		if(this.gamePausedEvent != null) {

			this.gamePausedEvent(this.gamePaused);
		}
	}

	public void BackToMainMenu() {

		Time.timeScale = 1;
		SceneManager.LoadScene(0);
	}

	public void StartOver() {

		Time.timeScale = 1;
		SceneManager.LoadScene(1);
	}

	public void ExitGame() {

		Application.Quit();
	}

	void Start() {

		sharedInstance = this;
	}

	void Update() {

		if(Input.GetAxisRaw("Pause") > 0 && this.gameCanBePaused == true) {

			if(this.gamePaused == false) {

				this.gamePaused = true;
				Time.timeScale = 0;
				StartCoroutine(this.DelayRePause());
			}
			else if(this.gamePaused == true){

				this.gamePaused = false;
				Time.timeScale = 1;
				StartCoroutine(this.DelayRePause());
			}

			if(this.gamePausedEvent != null) {

				this.gamePausedEvent(this.gamePaused);
			}
		}
	}

	private IEnumerator DelayRePause() {

		this.gameCanBePaused = false;

		float startTime = Time.realtimeSinceStartup;
		while(Time.realtimeSinceStartup - startTime  < 0.5f) {

			yield return null;
		}

		this.gameCanBePaused = true;
	}
}
