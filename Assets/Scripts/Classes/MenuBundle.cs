using System;
using UnityEngine;

public class MenuBundle
{
	public static MenuBundle instance = new MenuBundle();

	private const string MENU_ITEM_TAG = "MenuItem";

	private const string FINISH_MENU_ITEM1 = "FinishMenu.Item1";
	private const string FINISH_MENU_ITEM2 = "FinishMenu.Item2";

	private const string MAIN_MENU_ITEM1 = "MainMenu.Item1";
	private const string MAIN_MENU_ITEM4 = "MainMenu.Item4";


	// FINISH MENU

	public bool IsRestart(GameObject selectedItem) {
		return FINISH_MENU_ITEM1.Equals(GetItem(selectedItem));
	}

	public bool IsBackToMainMenu(GameObject selectedItem) {
		return FINISH_MENU_ITEM2.Equals(GetItem(selectedItem));
	}


	// START MENU

	public bool IsRunMathGame(GameObject selectedItem) {
		return MAIN_MENU_ITEM1.Equals(GetItem(selectedItem));
	}

	public bool IsQuit(GameObject selectedItem) {
		return MAIN_MENU_ITEM4.Equals(GetItem(selectedItem));
	}


	// Helpers

	public bool IsMenuSelected (GameObject gameObject)
	{
		return MENU_ITEM_TAG == gameObject.tag;
	}

	private String GetItem(GameObject selectedItem) {
		return selectedItem.transform.parent.name + "." + selectedItem.name;
	}
}