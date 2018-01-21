using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interception {

	public Vector3 position;
	public GameObject gameObject;

	public Interception(Vector3 position, GameObject gameObject) {
		this.position = position;
		this.gameObject = gameObject;
	}

	public bool Collides() {
		return gameObject != null;
	}

}
