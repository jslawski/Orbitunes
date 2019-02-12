using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

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
            AnalyticsEvent.Custom("Game_Loaded", new Dictionary<string, object> {});
        }

        //StartCoroutine(this.LoadLevel());

    }

    private IEnumerator LoadLevel()
    {
        SceneManager.LoadScene("AsteroidCreator", LoadSceneMode.Additive);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("AsteroidCreator"));
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.TogglePause();
        }

        if (Input.GetKey(KeyCode.Escape)) 
		{
			Application.Quit();
		}
	}

    private void TogglePause()
    {
        Metronome.metronomePaused = !Metronome.metronomePaused;

        if (Metronome.metronomePaused == true)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Metronome.OnStep += this.UnPauseInTime;
        }
    }

    /// <summary>
    /// This function is needed to ensure that physics continue at the exact same time that the audio continues, so they don't fall out of sync
    /// </summary>
    private void UnPauseInTime()
    {
        Metronome.OnStep -= this.UnPauseInTime;

        Time.timeScale = 1f;
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