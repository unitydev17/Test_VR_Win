using System;
using UnityEngine;

public class Helper
{

	public static void Db (object obj) {
		Debug.Log(Time.time + ": " + obj);
	}

	public static int GetValueAfterPrefix(string name, string prefix) {
		return int.Parse(name.Substring (prefix.Length));
	}
}


