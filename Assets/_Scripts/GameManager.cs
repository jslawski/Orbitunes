using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

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
            AnalyticsEvent.Custom("Reset_Clicked", new Dictionary<string, object> {{ "time_elapsed", Time.timeSinceLevelLoad }});
            GameManager.OnRestartButtonClicked();
		}
	}

	public void OpenAsteroidCreator(string asteroidToEditName)
	{
		GameObject asteroidToEdit = AsteroidSelector.asteroidMasterDict[asteroidToEditName];

		this.asteroidCreator.gameObject.SetActive(true);
		this.asteroidCreator.SetupAsteroidCreator(asteroidToEdit);

        AnalyticsEvent.Custom("Asteroid_Creator_Opened", new Dictionary<string, object> { { "asteroid_name", asteroidToEditName }, { "time_elapsed", Time.timeSinceLevelLoad } });
    }
}

public struct LaunchValues
{
	public Vector2 startPoint;
	public Vector2 endPoint;
	public Vector2 curDirection;
	public float rawMagnitude;
}