using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public delegate void RestartButtonClicked();
	public static event RestartButtonClicked OnRestartButtonClicked;

	[SerializeField]
	public Camera mainCamera;

	[SerializeField]
	private AsteroidCreator asteroidCreator;

	// Use this for initialization
	void Awake () {
		if (instance == null)
		{
			instance = this;
		}
	}

	void Start()
	{
		AsteroidSelector.SetupAsteroidSelector();
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

	public void RestartGame()
	{
		if (GameManager.OnRestartButtonClicked != null)
		{
			GameManager.OnRestartButtonClicked();
		}
	}

	public void OpenAsteroidCreator(string asteroidToEditName)
	{
		GameObject asteroidToEdit = AsteroidSelector.asteroidMasterDict[asteroidToEditName];

		this.asteroidCreator.gameObject.SetActive(true);
		this.asteroidCreator.SetupAsteroidCreator(asteroidToEdit);
	}
}

public struct LaunchValues
{
	public Vector2 startPoint;
	public Vector2 endPoint;
	public Vector2 curDirection;
	public float rawMagnitude;
}