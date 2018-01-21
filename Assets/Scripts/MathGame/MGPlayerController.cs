using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGPlayerController : CommonPlayerController
{
	private const float MOVEMENT_SPEED = 100f;
	private const float MENU_FRONT_DISTANCE = 4.5f;
	private const float MENU_UP_DISTANCE = 5.5f;
	private const float REGULAR_DELAY = 1f;

	private enum State
	{
		NextTask,
		StayAndSolve,
		MoveNext,
		Finished
	}

	private State state;
	private Task task;
	private float startTime;
	private CommonCrosshairController crosshair;


	public override CommonCrosshairController SetCrosshair ()
	{
		crosshair = GetComponentInChildren<MGCrosshairController> ();
		return crosshair;
	}

	protected override void Start ()
	{
		base.Start ();
		state = State.MoveNext;
		task = TaskController.instance.GetCurrentTask ();
		if (GameController.isVRSimulated) {
			crosshair.SetVRControlMode();
		}
	}


	protected override void Update ()
	{
		base.Update ();
		ProcessStates ();
	}


	protected override void ValidateSelection (GameObject selectedObject)
	{
		if (State.Finished != state) {
			if (TaskController.instance.CheckAnswer (selectedObject)) {
				PlayerData.instance.attemptsMade++;
				PlayerData.instance.tasksSolved++;
				StartCoroutine (NextTask ());
			} else {
				PlayerData.instance.attemptsMade++;
			}
		}
	}

	private IEnumerator NextTask() {
		yield return new WaitForSeconds(REGULAR_DELAY);
		state = State.NextTask;
	}


	protected override void ValidateMenuSelection (GameObject selectedItem)
	{
		if (MenuBundle.instance.IsRestart (selectedItem)) {
			GameController.instance.RestartMathGame ();

		} else if (MenuBundle.instance.IsBackToMainMenu (selectedItem)) {
			GameController.instance.StartMenu ();
		}
	}


	private void ProcessStates ()
	{
		if (State.NextTask == state) {
			task = TaskController.instance.NextTask ();
			if (task == null) {
				StartCoroutine (FinishAction ());
				state = State.Finished;
				return;
			}
			state = State.MoveNext;
			startTime = Time.time;


		} else if (State.MoveNext == state) {
			MoveToNextPoint ();
		}
	}


	private void MoveToNextPoint ()
	{
		float deltaTime = (Time.time - startTime) / MOVEMENT_SPEED;
		transform.position = Vector3.Slerp (transform.position, task.GetFrontPostion (), deltaTime);
		Vector3 positionLerped = transform.position;
		positionLerped.y = positionY;
		transform.position = positionLerped;
		if (Vector3.Distance (transform.position, task.GetFrontPostion ()) <= 0.1) {
			state = State.StayAndSolve;
		}
	}


	private IEnumerator FinishAction ()
	{
		yield return new WaitForSeconds (REGULAR_DELAY);
		GameController.instance.OpenFinishMenu (GetMenuFrontPosition ());
	}


	public Vector3 GetMenuFrontPosition ()
	{
		return transform.position + Vector3.forward * MENU_FRONT_DISTANCE;
	}


	public Vector3 GetMenuUpPosition ()
	{
		return transform.position + transform.forward * MENU_UP_DISTANCE;
	}


	protected override void HandleInGameMenuOpen ()
	{
		GameController.instance.OpenInGameMenu (GetMenuUpPosition(), transform.rotation);
	}

	protected override void HandleInGameMenuClose ()
	{
		GameController.instance.CloseInGameMenu ();
	}


}
