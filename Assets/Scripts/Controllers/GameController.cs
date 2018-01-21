using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{

	public static GameController instance;

	public GameObject finishMenuPrefab;
	public GameObject inGameMenuPrefab;

	private string[] games = new string[]{ "Start", "MathGame" };
	private GameObject inGameMenu;
	private GameObject finishMenu;
	private int currentGame = 1;

	private void Awake ()
	{
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this);
			return;
		}
		Destroy (this);
	}
		

	public void RunGame ()
	{
		if (IsTestRange (1, games.Length - 1, currentGame)) {
			SceneManager.LoadScene (games [currentGame]);
		}
	}


	private bool IsTestRange (int a, int b, int value)
	{
		return value >= a && value <= b;
	}


	public void OpenFinishMenu(Vector3 position) {
		finishMenu = Instantiate (finishMenuPrefab, position, Quaternion.identity);
		finishMenu.name = finishMenuPrefab.name;
		SetPlayerStatistics (finishMenu);
	}


	public void StartMenu() {
		SceneManager.LoadScene (games [0]);
	}


	public void RunMathGame() {
		PlayerData.instance.Save ();
		currentGame = 1;
		RunGame ();
	}

	public void RestartMathGame() {
		PlayerData.instance.Reset ();
		RunMathGame ();
	}

	public void OpenInGameMenu(Vector3 position, Quaternion rotation) {
		if (IsAnyMenuOpened ()) {
			return;
		}

		inGameMenu = Instantiate (inGameMenuPrefab, position, rotation);
		inGameMenu.name = inGameMenuPrefab.name;
		SetPlayerStatistics (inGameMenu);
	}


	private void SetPlayerStatistics (GameObject menu)
	{
		List<Text> infoList = new List<Text> ();
		Text[] objs = menu.GetComponentsInChildren<Text> ();
		foreach (Text obj in objs) {
			if (MenuBundle.MENU_INFO_TAG == obj.tag) {
				infoList.Add (obj);
			}
		}
		Text[] infos = infoList.ToArray ();
		infos [0].text = "task solved: " + PlayerData.instance.tasksSolved.ToString ();
		infos [1].text = "attempts made: " + PlayerData.instance.attemptsMade.ToString ();
		infos [2].text = "total creativity: " + PlayerData.instance.Creativity ();
	}


	public void CloseInGameMenu() {
		Destroy (inGameMenu);
	}

	public bool IsAnyMenuOpened() {
		return finishMenu != null || inGameMenu != null;
	}
}
