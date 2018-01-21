using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommonPlayerController : MonoBehaviour
{
	private const string CROSSHAIR_NAME = "Crosshair";

	private CommonCrosshairController crosshairController;

	private const float MAX_ANGLE_Y = 90f;
	private const float MIN_ANGLE_Y = -90f;
	private const float SHOW_IN_GAME_MENU_ANGLE_Y = 60f;
	private const float HIDE_IN_GAME_MENU_ANGLE_Y = 35f;

	protected float positionY = 4f;
	private float rotationY = 0f;

	private MouseState mouseState;
	private bool inGameMenuActive = false;

	public abstract CommonCrosshairController SetCrosshair ();


	public void OnSelectObject (GameObject selectedObject) {
		if (MenuBundle.instance.IsMenuItem (selectedObject)) {
			ValidateMenuSelection (selectedObject);
		} else {
			ValidateSelection (selectedObject);
		}
	}


	/// <summary>
	/// Validates the menu selection. Override this method to validate menu item selection.
	/// </summary>
	/// <param name="selectedItem">Selected item.</param>
	protected virtual void ValidateMenuSelection (GameObject selectedItem)
	{
	}


	/// <summary>
	/// Validates the selection. Override this method to validate gameObject selection.
	/// </summary>
	/// <param name="selectedObject">Selected object.</param>
	protected virtual void ValidateSelection (GameObject selectedObject)
	{
	}


	protected virtual void HandleMouseLeftButtonDown ()
	{
		mouseState.leftButtonDown = true;
	}


	protected virtual void HandleMouseLeftButtonUp ()
	{
		mouseState.leftButtonDown = false;
	}


	protected virtual void HandleMouseRightButtonDown ()
	{
		mouseState.rightButtonDown = true;
	}


	protected virtual void HandleMouseRightButtonUp ()
	{
		mouseState.rightButtonDown = false;
	}


	void Awake ()
	{
		crosshairController = SetCrosshair ();
		mouseState = new MouseState ();
	}

	protected virtual void Start ()
	{
		Cursor.visible = false;
	}


	protected virtual void Update ()
	{
		HandleInput ();
		UpdateCrosshair ();
	}


	private void UpdateCrosshair ()
	{
		crosshairController.UpdateCrosshair (transform, OnSelectObject, mouseState);
	}


	private void HandleInput ()
	{
		HandleMouseDirection ();

		if (Input.GetMouseButtonDown (0)) {
			HandleMouseLeftButtonDown ();

		} else if (Input.GetMouseButtonUp (0)) {
			HandleMouseLeftButtonUp ();

		} else if (Input.GetMouseButtonDown (1)) {
			HandleMouseRightButtonDown ();

		} else if (Input.GetMouseButtonUp (1)) {
			HandleMouseRightButtonUp ();
		}
	}


	private void HandleMouseDirection ()
	{
		float dx = Input.GetAxis ("Mouse X");
		float dy = Input.GetAxis ("Mouse Y");
		float rotationX = transform.localEulerAngles.y + dx;
		rotationY += dy;

		HandleInGameMenu ();
	
		rotationY = Mathf.Clamp (rotationY, MIN_ANGLE_Y, MAX_ANGLE_Y);
		transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0);
	}


	void HandleInGameMenu ()
	{
		if (!inGameMenuActive && rotationY >= SHOW_IN_GAME_MENU_ANGLE_Y) {
			HandleInGameMenuOpen ();
			inGameMenuActive = true;
		}
		if (inGameMenuActive && rotationY <= HIDE_IN_GAME_MENU_ANGLE_Y) {
			HandleInGameMenuClose ();
			inGameMenuActive = false;
		}
	}

	/// <summary>
	/// Handles the in game menu. Override this method to add menu.
	/// </summary>
	protected virtual void HandleInGameMenuOpen() {
	}

	/// <summary>
	/// Handles the in game menu close action.
	/// </summary>
	protected virtual void HandleInGameMenuClose() {
	}

}
