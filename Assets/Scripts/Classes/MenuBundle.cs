using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBundle
{
	public static MenuBundle instance = new MenuBundle();

	public const string MAIN_MENU_NAME = "MainMenu";

	public const string MENU_INFO_TAG = "Info";
	private const string MENU_ITEM_TAG = "MenuItem";

	private const string FINISH_MENU_ITEM1 = "FinishMenu.Item1";
	private const string FINISH_MENU_ITEM2 = "FinishMenu.Item2";
	private const string MAIN_MENU_ITEM1 = "MainMenu.Item1";
	private const string MAIN_MENU_ITEM2 = "MainMenu.Item2";
	private const string MAIN_MENU_ITEM3 = "MainMenu.Item3";
	private const string IN_GAME_MENU_ITEM1 = "InGameMenu.Item1";

	private const string VR_SIMULATION_MODE_TEXT = "VR simulation mode";
	private const string WIN_MODE_TEXT = "Win mode";
	private const string START_MATH_GAME_TEXT = "Start math game";
	private const string CONTINUE_MATH_GAME_TEXT = "Continue math game";
	private const string ATTEMPTS_MADE_TEXT = "attempts made: ";
	private const string TASK_SOLVED_TEXT = "task solved: ";
	private const string TOTAL_CREATIVITY_TEXT = "total creativity: ";

	// FINISH MENU

	public bool IsRestart(GameObject selectedItem) {
		return FINISH_MENU_ITEM1.Equals(GetItem(selectedItem));
	}


	public bool IsBackToMainMenuFromFinishMenu(GameObject selectedItem) {
		return FINISH_MENU_ITEM2.Equals(GetItem(selectedItem));
	}


	// IN GAME MENU

	public bool IsBackToMainMenuFromInGameMenu(GameObject selectedItem) {
		return IN_GAME_MENU_ITEM1.Equals(GetItem(selectedItem));
	}


	// START MENU

	public bool IsRunMathGame(GameObject selectedItem) {
		return MAIN_MENU_ITEM1.Equals(GetItem(selectedItem));
	}

	public bool IsChangeControlMode(GameObject selectedItem) {
		return MAIN_MENU_ITEM2.Equals(GetItem(selectedItem));
	}

	public bool IsQuit(GameObject selectedItem) {
		return MAIN_MENU_ITEM3.Equals(GetItem(selectedItem));
	}


	// Helpers

	public void CustomizeMainMenu (bool isVRMode, bool isGameActive)
	{
		GameObject menu = GameObject.Find (MenuBundle.MAIN_MENU_NAME);
		if (menu == null) {
			return;
		}
		Transform[] tfs = menu.GetComponentsInChildren<Transform> ();
		foreach (Transform tf in tfs) {
			if (IsMenuItem(tf.gameObject)) {

				if (IsChangeControlMode (tf.gameObject)) {
					var textItem = tf.GetComponentInChildren<Text> ();
					textItem.text = isVRMode ? WIN_MODE_TEXT : VR_SIMULATION_MODE_TEXT;


				} else if (IsRunMathGame (tf.gameObject)) {
					var textItem = tf.GetComponentInChildren<Text> ();
					textItem.text = isGameActive ? CONTINUE_MATH_GAME_TEXT : START_MATH_GAME_TEXT;
				}

			}
		}
	}


	public void SetPlayerStatistics (GameObject menu)
	{
		List<Text> infoList = new List<Text> ();
		Text[] objs = menu.GetComponentsInChildren<Text> ();
		foreach (Text obj in objs) {
			if (MENU_INFO_TAG == obj.tag) {
				infoList.Add (obj);
			}
		}
		Text[] infos = infoList.ToArray ();
		infos [0].text = TASK_SOLVED_TEXT + PlayerData.instance.tasksSolved.ToString ();
		infos [1].text = ATTEMPTS_MADE_TEXT + PlayerData.instance.attemptsMade.ToString ();
		infos [2].text = TOTAL_CREATIVITY_TEXT + PlayerData.instance.Creativity ();
	}


	public bool IsMenuItem (GameObject gameObject)
	{
		return MENU_ITEM_TAG == gameObject.tag;
	}

	private String GetItem(GameObject selectedItem) {
		return selectedItem.transform.parent.name + "." + selectedItem.name;
	}
}