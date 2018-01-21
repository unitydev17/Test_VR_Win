using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGPlayerController : CommonPlayerController
{
	private const float MOVEMENT_SPEED = 100f;
	private const float MENU_FRONT_DISTANCE = 4.5f;
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


	public override CommonCrosshairController GetCrosshair ()
	{
		return GetComponentInChildren<MGCrosshairController> ();
	}

	public override void Start ()
	{
		base.Start ();
		state = State.MoveNext;
		task = TaskController.instance.GetCurrentTask ();
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
				StartCoroutine (NextTask());
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
			GameController.instance.RunMathGame ();

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
		GameController.instance.Finish (GetMenuFrontPostion ());
	}


	public Vector3 GetMenuFrontPostion ()
	{
		return transform.position + Vector3.forward * MENU_FRONT_DISTANCE;
	}
}
