using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	public GameObject Tutorial;
	public GameObject MainMenu;

	void Start() {

		MainMenu.gameObject.SetActive(true);
		Tutorial.gameObject.SetActive(false);
	}

	public void ShowTutorial() {

		Tutorial.SetActive(true);
		MainMenu.SetActive(false);
	}

	public void hideTutorial() {

		MainMenu.SetActive(true);
		Tutorial.SetActive(false);
	}

	public void StartGame() {
		
		Time.timeScale = 1;
		SceneManager.LoadScene(1);
	}

	public void ExitGame() {

		Application.Quit();
	}
}
