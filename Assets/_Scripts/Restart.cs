using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour {

	public delegate void RestartButtonClicked();
	public static event RestartButtonClicked OnRestartButtonClicked;

	void OnMouseDown()
	{
		AimManager.instance.buttonPressed = true;
		Restart.OnRestartButtonClicked();
	}
}
