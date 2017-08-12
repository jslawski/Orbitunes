using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	[SerializeField]
	public Camera mainCamera;

	[HideInInspector]
	public List<GameObject> allAsteroids;

	// Use this for initialization
	void Awake () {
		if (instance == null)
		{
			instance = this;
		}

		this.allAsteroids = new List<GameObject>();
		Restart.OnRestartButtonClicked += this.DestroyAllAsteroids;
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

	private void DestroyAllAsteroids()
	{
		AimManager.instance.buttonPressed = true;

		foreach (GameObject asteroid in this.allAsteroids) 
		{
			Destroy(asteroid);
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