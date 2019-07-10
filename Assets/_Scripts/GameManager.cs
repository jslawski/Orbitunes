using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public delegate void RestartButtonClicked();
	public static event RestartButtonClicked OnRestartButtonClicked;

	[SerializeField]
	public Camera mainCamera;

	[SerializeField]
	private AsteroidCreator asteroidCreator;

    [SerializeField]
    private Slider tempoSlider;

    public List<Star> stars;

    public AudioMixer audioMixer;
    public Queue<AudioMixerGroup> availableAudioMixerGroups;

    [HideInInspector]
    public int audioMixerIndex = 1;

	// Use this for initialization
	void Awake () {
		if (instance == null)
		{
			instance = this;
            AnalyticsEvent.Custom("Game_Loaded", new Dictionary<string, object> {});
        }

        //this.SetupAudioMixer();
    }

    private void SetupAudioMixer()
    {
        this.audioMixer = Resources.Load<AudioMixer>("StarEffects");
        AudioMixerGroup[] groups = this.audioMixer.FindMatchingGroups("Master");
        this.availableAudioMixerGroups = new Queue<AudioMixerGroup>();

        for (int i = 1; i < groups.Length; i++)
        {
            this.availableAudioMixerGroups.Enqueue(groups[i]);
        }
    }

    private IEnumerator LoadAsteroidCreator()
    {
        SceneManager.LoadScene("AsteroidCreator", LoadSceneMode.Additive);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("AsteroidCreator"));
    }

    private IEnumerator LoadTutorialScene()
    {
        SceneManager.LoadScene("TutorialScene", LoadSceneMode.Additive);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("TutorialScene"));
    }

    void Start()
	{
		AsteroidSelector.SetupAsteroidSelector();
	}

	void Update() 
	{
        Metronome.UpdateMetronomeTempo(this.tempoSlider.value);

        if (Input.GetKeyDown(KeyCode.P))
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

    public bool IsClickingAStar()
    {
        for (int i = 0; i < this.stars.Count; i++)
        {
            Vector3 mousePositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float xDiff = Mathf.Abs(mousePositionWorld.x - this.stars[i].starTransform.position.x);
            float yDiff = Mathf.Abs(mousePositionWorld.y - this.stars[i].starTransform.position.y);

            if (xDiff <= this.stars[i].starCollider.radius && yDiff <= this.stars[i].starCollider.radius)
            {
                return true;
            }
        }

        return false;
    }

    public void TogglePause()
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

	public void OpenAsteroidCreator()
	{
        StartCoroutine(this.LoadAsteroidCreator());

        AnalyticsEvent.Custom("Asteroid_Creator_Opened", new Dictionary<string, object> { { "asteroid_name", AsteroidSelector.selectedAsteroid.asteroidName }, { "time_elapsed", Time.timeSinceLevelLoad } });
    }

    public void OpenTutorialScene()
    {
        this.TogglePause();

        StartCoroutine(this.LoadTutorialScene());
    }

    public void CloseTutorialScene()
    {
        SceneManager.UnloadSceneAsync("TutorialScene");
        GameManager.instance.TogglePause();
    }

    public void Load1StarLevel()
    {
        this.RestartGame();
        SceneManager.LoadScene("OneStar");
    }

    public void Load2StarLevel()
    {
        this.RestartGame();
        SceneManager.LoadScene("TwoStars");
    }

    public void Load3StarLevel()
    {
        this.RestartGame();
        SceneManager.LoadScene("ThreeStars");
    }
}

public struct LaunchValues
{
	public Vector2 startPoint;
	public Vector2 endPoint;
	public Vector2 curDirection;
	public float rawMagnitude;
}