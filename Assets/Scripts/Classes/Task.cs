using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task {

	public const string ANSWER_TAG = "Answer";
	public const string UNTAGGED_TAG = "Untagged";

	private const float FRONT_DISTANCE = 5f;

	public int number;
	public Vector3 position;
	public string question;
	public string[] answers;
	public int correctNumber;


	public Task (int number, Vector3 position, string question, int correctNumber, params string[] answers) {
		this.number = number;
		this.position = position;
		this.question = question;
		this.answers = answers;
		this.correctNumber = correctNumber;
	}

	public int GetNumPieces() {
		return answers.Length;
	}

	public Vector3 GetFrontPostion() {
		return position - Vector3.forward * FRONT_DISTANCE;
	}

}
