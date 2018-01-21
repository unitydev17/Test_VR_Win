using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMPlayerController : CommonPlayerController
{
	private CommonCrosshairController crosshair;
	
	public override CommonCrosshairController SetCrosshair ()
	{
		crosshair = GetComponentInChildren<CommonCrosshairController> ();
		return crosshair;
	}


	protected override void Start() {
		base.Start ();
		if (GameController.isVRSimulated) {
			crosshair.SetVRControlMode();
		}
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

		} else if (MenuBundle.instance.IsChangeControlMode (selectedItem)) {
			GameController.instance.ChangeControlMode();
			GameController.instance.StartMenu ();
		}
			
	}

}
