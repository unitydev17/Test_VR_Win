using System;
using UnityEngine;
using UnityEngine.UI;

public class CommonCrosshairController : MonoBehaviour {

	protected enum ControlMode {
		Windows,
		VR
	}

	private enum State {
		Default,
		Targeted,
		TargetedProgress
	}

	private const string IMG_FILLED = "image_filled";

	private const float MAX_CROSSHAIR_DISANCE = 10f;
	private const float CROSSHAIR_MOVEMENT_SPEED = 0.1f;
	private const float TIME_TO_START_SELECTION = 1f;
	private const float DURATION_SELECTION = 1f;

	private Canvas canvas;
	private Image baseImage;
	private Image filledImage;

	public Sprite defaultSprite;
	public Sprite targetedSprite;
	public Sprite targetedProgressSprite;

	private State state;
	private float startTime;
	private int currentId;
	private Interception currentInterception;
	private Action<GameObject> onSelectObjectCallback;
	private MouseState mouseState;

	protected ControlMode controlMode;

	/// <summary>
	/// Determines whether this instance is selectable game object.
	/// </summary>
	/// <returns><c>true</c> if this instance is selectable game object; otherwise, <c>false</c>.</returns>
	/// <param name="gameObject">Game object.</param>
	protected virtual bool IsSelectableGameObject (GameObject gameObject)
	{
		return true;
	}


	public void SetVRControlMode() {
		controlMode = ControlMode.VR;
	}

	public void SetWinControlMode() {
		controlMode = ControlMode.Windows;
	}

	void Awake() {
		SetWinControlMode ();
	}


	private bool VRMode() {
		return ControlMode.VR == controlMode;
	}


	private bool WindowsMode() {
		return ControlMode.Windows == controlMode;
	}


	void Start() {
		canvas = GetComponent<Canvas> ();

		Image[] images = canvas.GetComponentsInChildren<Image> ();
		foreach (Image img in images) {
			if (IMG_FILLED.Equals(img.name)) {
				filledImage = img;
			} else {
				baseImage = img;
			}
		}
		DefaultMode ();
	}


	private void DefaultMode() {
		baseImage.sprite = defaultSprite;
		state = State.Default;
		filledImage.enabled = false;
		currentId = 0;
	}

	private void TargetedMode() {
		baseImage.sprite = targetedSprite;
		state = State.Targeted;
	}

	private void TargetedProgressMode() {
		state = State.TargetedProgress;
		filledImage.enabled = true;
	}


	public void UpdateCrosshair(Transform playerTransform, Action<GameObject> onSelectObjectCallback, MouseState mouseState) {
		this.mouseState = mouseState;
		this.onSelectObjectCallback = onSelectObjectCallback;

		Interception interception = GetTargetInterception (playerTransform);
		canvas.transform.position = Vector3.Lerp (canvas.transform.position, interception.position, CROSSHAIR_MOVEMENT_SPEED);

		if (interception.Collides ()) {
			if (IsNewInterception (interception)) {
				ProcessNewInterception (interception);
			}
		} else {
			ProcessNoInterception ();
		}

		HandleStates ();
	}


	private	void ProcessNoInterception ()
	{
		if (State.Default != state) {
			DefaultMode ();
		}
	}


	private void ProcessNewInterception (Interception interception)
	{
		if (IsSelectableGameObject (interception.gameObject)) {
			TargetedMode ();
			currentInterception = interception;
			startTime = Time.time;
		} else {
			DefaultMode ();
		}
		currentId = interception.gameObject.GetInstanceID ();
	}


	void HandleStates ()
	{
		if (State.Targeted == state ) {

			if (WindowsMode() && mouseState.leftButtonDown) {
				UserSelectAction ();
			}
				
			else if (VRMode() && (Time.time - startTime) >= TIME_TO_START_SELECTION) {
				TargetedProgressMode ();
				startTime = Time.time;
			}
		}

		if (VRMode() && State.TargetedProgress == state) {
			float progress = (Time.time - startTime) / DURATION_SELECTION;
			filledImage.fillAmount = progress;
			if (progress >= 1) {
				UserSelectAction ();
			}
		}
	}


	void UserSelectAction ()
	{
		DefaultMode ();
		onSelectObjectCallback (currentInterception.gameObject);
	}


	private bool IsNewInterception (Interception interception)
	{
		return interception.gameObject.GetInstanceID () != currentId;
	}


	private Interception GetTargetInterception(Transform playerTransform) {
		Vector3 target = Vector3.zero;
		Ray ray = new Ray (playerTransform.position, playerTransform.forward);
		RaycastHit hit;
		float distance;

		if (Physics.Raycast (ray, out hit)) {
			distance = hit.distance > MAX_CROSSHAIR_DISANCE ? MAX_CROSSHAIR_DISANCE : hit.distance - 0.5f;
		} else {
			distance = MAX_CROSSHAIR_DISANCE;
		}
		Vector3 targetPosition = playerTransform.position + playerTransform.forward.normalized * distance;
		var collidedObject = hit.collider == null ? null : hit.collider.gameObject;

		return new Interception (targetPosition, collidedObject);
	}

}

