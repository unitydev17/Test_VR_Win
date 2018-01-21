using UnityEngine;

public class MGCrosshairController : CommonCrosshairController {

	protected override bool IsSelectableGameObject (GameObject gameObject)
	{
		return TaskController.instance.IsCurrentTaskAnswerSelected (gameObject) || MenuBundle.instance.IsMenuSelected(gameObject);
	}

}

