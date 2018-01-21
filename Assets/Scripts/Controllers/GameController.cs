using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{

	public static GameController instance;

	public GameObject finishMenuPrefab;

	private string[] games = new string[]{ "Start", "MathGame" };
	private int currentGame = 1;

	void Awake ()
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


	public void Finish(Vector3 position) {
		GameObject finishMenu = Instantiate (finishMenuPrefab, position, Quaternion.identity);
		finishMenu.name = finishMenuPrefab.name;
	}


	public void StartMenu() {
		SceneManager.LoadScene (games [0]);
	}


	public void RunMathGame() {
		currentGame = 1;
		RunGame ();
	}
}
