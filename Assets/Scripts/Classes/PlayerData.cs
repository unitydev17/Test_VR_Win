using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {

	public static PlayerData instance = new PlayerData();

	private PlayerData previous;

	public bool isGameActive;
	public int tasksSolved;
	public int attemptsMade;
	public int currentTaskNumber;
	public Dictionary<int, List<int>> taskAnswers = new Dictionary<int, List<int>>();
	public Vector3 position;
	public Quaternion rotation;


	public string Creativity() {
		return attemptsMade == 0 ? "not known yet" :  ((int)(100 * (float)tasksSolved / (float)attemptsMade)).ToString() + "%";
	}


	public void Save() {
		if (previous == null) {
			previous = new PlayerData ();
		}
		previous.CopyFrom(this);
		isGameActive = true;
	}


	public void Reset() {
		previous = null;
		tasksSolved = 0;
		attemptsMade = 0;
		currentTaskNumber = 0;
		taskAnswers = new Dictionary<int, List<int>> ();
		position = Vector3.zero;
		rotation = Quaternion.identity;
		isGameActive = false;
	}


	public void Restore() {
		if (previous != null) {
			CopyFrom (previous);
		}
		isGameActive = false;
	}


	private void CopyFrom(PlayerData data) {
		this.tasksSolved = data.tasksSolved;
		this.attemptsMade = data.attemptsMade;
		this.currentTaskNumber = data.currentTaskNumber;
		this.taskAnswers = new Dictionary<int, List<int>> (data.taskAnswers);
		this.position = data.position;
		this.rotation = data.rotation;
	}

}
