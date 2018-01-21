using UnityEngine;

public class MGCrosshairController : CommonCrosshairController {

	protected override bool IsSelectableGameObject (GameObject gameObject)
	{

		if (GameController.instance.IsAnyMenuOpened ()) {
			return MenuBundle.instance.IsMenuItem(gameObject);
		}

		return  TaskController.instance.IsCurrentTaskAnswerSelected (gameObject);
	}

}

