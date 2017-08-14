using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	[SerializeField]
	public Camera mainCamera;

	// Use this for initialization
	void Awake () {
		if (instance == null)
		{
			instance = this;
		}
	}

	void Update() 
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			Debug.Break();
		}

		if (Input.GetKey(KeyCode.Escape)) 
		{
			Application.Quit();
		}
	}
}

public struct LaunchValues
{
	public Vector2 startPoint;
	public Vector2 endPoint;
	public Vector2 curDirection;
	public float rawMagnitude;
}