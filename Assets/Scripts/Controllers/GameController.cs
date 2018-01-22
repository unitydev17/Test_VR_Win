using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
	private const string PLAYER_NAME = "Player";
	private const string START_SCENE = "Start";
	private const string MATH_GAME_SCENE = "MathGame";

	public static GameController instance;
	public static bool isVRSimulated = false;

	public GameObject finishMenuPrefab;
	public GameObject inGameMenuPrefab;

	private string[] games = new string[]{ START_SCENE, MATH_GAME_SCENE };

	private GameObject inGameMenu;
	private GameObject finishMenu;
	private GameObject player;

	private int currentGame = 1;



	private void Awake ()
	{
		SceneManager.sceneLoaded += PreProcessScenes;

		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this);
			return;
		}
		Destroy (this);
	}
		

	/// <summary>
	/// Preprocess scenes. Used for overload menu item`s text values and other initial stuff.
	/// </summary>
	/// <param name="scene">Scene.</param>
	/// <param name="mode">Mode.</param>
	public void PreProcessScenes(Scene scene, LoadSceneMode mode) {
		if (START_SCENE.Equals (scene.name)) {
			MenuBundle.instance.CustomizeMainMenu (isVRSimulated, PlayerData.instance.isGameActive);
		
		} else if (MATH_GAME_SCENE.Equals (scene.name)) {
			player = GameObject.Find (PLAYER_NAME);
		}
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
		MenuBundle.instance.SetPlayerStatistics (finishMenu);
	}


	public void BackToMainMenuFromFinishMenu() {
		PlayerData.instance.Reset ();
		StartMenu ();
	}


	public void BackToMainMenuFromInGameMenu() {
		SavePlayerState ();
		StartMenu ();
	}


	public void StartMenu() {
		SceneManager.LoadScene (games [0]);
	}


	private void SavePlayerState() {
		PlayerData.instance.currentTaskNumber = TaskController.instance.currentTaskNumber;
		TaskController.instance.GetTaskUserAnswers (out PlayerData.instance.taskAnswers);
		PlayerData.instance.position = player.transform.position;
		PlayerData.instance.rotation = player.transform.rotation;
		PlayerData.instance.Save ();
	}


	public void RunMathGame() {
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
		MenuBundle.instance.SetPlayerStatistics (inGameMenu);
	}


	public void CloseInGameMenu() {
		Destroy (inGameMenu);
	}


	public bool IsAnyMenuOpened() {
		return finishMenu != null || inGameMenu != null;
	}


	public void ChangeControlMode() {
		isVRSimulated = !isVRSimulated;
	}

}
