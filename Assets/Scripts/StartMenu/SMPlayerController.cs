using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMPlayerController : CommonPlayerController
{
	public override CommonCrosshairController GetCrosshair ()
	{
		return GetComponentInChildren<SMCrosshairController> ();
	}


	protected override void ValidateMenuSelection (GameObject selectedItem)
	{
		if (MenuBundle.instance.IsQuit (selectedItem)) {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else
			Application.Quit ();
			#endif
		} else if (MenuBundle.instance.IsRunMathGame (selectedItem)) {
			GameController.instance.RunMathGame ();
		}
			
	}

}
