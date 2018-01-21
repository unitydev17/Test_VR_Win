using System;

public class PlayerData {

	public static PlayerData instance = new PlayerData();

	private PlayerData previous;

	public int tasksSolved;
	public int attemptsMade;


	public string Creativity() {
		return attemptsMade == 0 ? "not known yet" :  ((int)(100 * (float)tasksSolved / (float)attemptsMade)).ToString() + "%";
	}


	public void Save() {
		if (previous == null) {
			previous = new PlayerData ();
		}
		previous.CopyFrom(this);
	}


	public void Reset() {
		if (previous != null) {
			CopyFrom (previous);
		}
	}


	private void CopyFrom(PlayerData data) {
		this.tasksSolved = data.tasksSolved;
		this.attemptsMade = data.attemptsMade;
	}

}
