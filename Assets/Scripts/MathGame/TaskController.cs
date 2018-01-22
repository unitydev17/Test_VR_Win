using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskController : MonoBehaviour
{
	public static TaskController instance;

	private const string ANSWER_PREFIX = "Answer_";
	public const string TASK_PREFIX = "Task_";
	public const float RAD = 1.2f;
	public const float ANSWER_DISTANCE = 2f;

	public GameObject answerPrefab;
	public GameObject questionPrefab;
	public Material incorrectMaterial;
	public Material correctMaterial;


	public int currentTaskNumber = 0;
	private Task[] tasks = new Task[4];
	private GameObject root;


	private void Awake ()
	{
		if (instance == null) {
			instance = this;
		}

		tasks [0] = new Task (0, new Vector3 (20f, 3f, -30f), "1 + 2 = ?", 1, "1", "3", "4");
		tasks [1] = new Task (1, new Vector3 (-25f, 3f, -30f), "3 * (4 - 12) = ?", 2, "64", "-33", "-24", "12");
		tasks [2] = new Task (2, new Vector3 (-25f, 3f, 25f), "44 * 11 = ?", 1, "554", "484", "441", "444", "447");
		tasks [3] = new Task (3, new Vector3 (25f, 3f, 25f), "12 * 13 = ?", 2, "165", "136", "156", "123", "126", "221");

		root = new GameObject ("Root");
		foreach (Task task in tasks) {
			PlaceTask (task);
		}
	}


	public Task GetCurrentTask ()
	{
		return tasks [currentTaskNumber];
	}

	public bool IsCurrentTask (int value)
	{
		return currentTaskNumber == value;
	}


	public Task NextTask ()
	{
		if (currentTaskNumber == (tasks.Length - 1)) {
			return null;
		} else {
			currentTaskNumber++;
		}
		return tasks [currentTaskNumber];
	}


	public bool IsCurrentTaskAnswerSelected (GameObject gameObject)
	{
		if (Task.ANSWER_TAG == gameObject.tag) {
			int taskNumber = GetTaskNumber (gameObject.transform.parent.gameObject);
			return IsCurrentTask (taskNumber);
		}
		return false;
	}


	private int GetTaskNumber (GameObject task)
	{
		return Helper.GetValueAfterPrefix (task.name, TaskController.TASK_PREFIX);
	}


	public bool CheckAnswer (GameObject answer)
	{
		return CheckAnswer (answer, currentTaskNumber);
	}


	private bool CheckAnswer (GameObject answer, int taskNumber)
	{
		Task task = tasks[taskNumber];
		int answerNum = GetAnswerNumber (answer);

		bool correct = (task.correctNumber == answerNum);
		answer.tag = Task.UNTAGGED_TAG;
		answer.GetComponent<MeshRenderer> ().material = correct ? correctMaterial : incorrectMaterial;
		task.userAnswers.Add(answerNum);

		return correct;
	}

	private int GetAnswerNumber (GameObject answer)
	{
		return int.Parse (answer.name.Substring (ANSWER_PREFIX.Length));
	}


	public void GetTaskUserAnswers(out Dictionary<int, List<int>> taskAnswers) {
		taskAnswers = new Dictionary<int, List<int>> ();
		foreach (Task task in tasks) {
			taskAnswers.Add (task.number, task.userAnswers);
		}
	}


	/**
	 *  Place task
	 */
	private void PlaceTask (Task task)
	{
		GameObject taskArea = new GameObject ();
		taskArea.name = TASK_PREFIX + task.number.ToString ();
		taskArea.transform.parent = root.transform;

		GameObject center = CreateTaskCenter (task.position);

		PlaceQuestion (taskArea.transform, center.transform.position, task.question);
		PlaceAnswers (taskArea.transform, center.transform, task.number);

		Destroy (center);
	}


	/**
	 *  Place question block
	 */
	private void PlaceQuestion (Transform parent, Vector3 position, string text)
	{
		position.y--;
		GameObject question = Instantiate (questionPrefab, position, Quaternion.identity);
		question.transform.parent = parent;
		question.GetComponentInChildren<Text> ().text = text;
	}


	private GameObject CreateTaskCenter (Vector3 position)
	{
		GameObject answerCenter = Instantiate (answerPrefab, position, Quaternion.identity);
		answerCenter.transform.rotation = answerCenter.transform.rotation * answerPrefab.transform.rotation;
		return answerCenter;
	}


	/**
	 * Draw answer`s blocks around a circle
	 */
	private void PlaceAnswers (Transform parent, Transform transform, int taskNumber)
	{
		Task task = tasks [taskNumber];

		Vector3 centerPosition = transform.position;
		centerPosition.y += 1;

		float angle0 = 0;
		float deltaAngle = 360 / task.GetNumPieces ();
		AdjustStartAngle (ref angle0, deltaAngle);

		Vector3 baseVector;
		float angle = angle0;
		int num = 0;

		List<int> playerAnswers;
		PlayerData.instance.taskAnswers.TryGetValue (taskNumber, out playerAnswers);

		while (angle < (360 + angle0)) {
			angle += deltaAngle;
			baseVector = Quaternion.AngleAxis (angle, Vector3.forward) * Vector3.right.normalized * RAD;
			Vector3 pos = centerPosition + baseVector;

			GameObject answer = Instantiate (answerPrefab, pos, transform.rotation);
			answer.name = ANSWER_PREFIX + num.ToString ();
			answer.transform.parent = parent;
			answer.GetComponentInChildren<Text> ().text = task.answers [num];

			if (playerAnswers != null && playerAnswers.Contains (num)) {
				CheckAnswer (answer, taskNumber);
			}
			num++;
		}
	}


	private void AdjustStartAngle (ref float angle0, float deltaAngle)
	{
		if (deltaAngle == 90) {
			angle0 = 45;
		} else if (deltaAngle == 120) {
			angle0 = 90;
		} else if (deltaAngle == 72) {
			angle0 = 90;
		}
	}
}


