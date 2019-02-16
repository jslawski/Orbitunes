using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class AsteroidCreator : MonoBehaviour {

	public static AsteroidCreator instance;

	public GameObject[] beatGrids;
	private GameObject activeGrid;

	private AsteroidTemplate originalAsteroidTemplate;
    public AsteroidTemplate newAsteroidTemplate;

	[SerializeField]
	private AudioSource previewAudio;
	private Phrase previewPhrase;

	[SerializeField]
	private PreviewButton previewButton;

	private void Awake()
	{
		//Always create a new one on awake
		AsteroidCreator.instance = this;
	}

	private void Start()
	{
		AimManager.OnLaunchAreaClicked += this.CancelAsteroidCreation;
	}

    // Use this for initialization
    public void SetupAsteroidCreator()
    {
        this.originalAsteroidTemplate = AsteroidSelector.selectedAsteroid;
        this.SetupNewAsteroidTemplate();
    }

    private void SetupNewAsteroidTemplate()
    {
        this.newAsteroidTemplate.asteroidAudio = this.originalAsteroidTemplate.asteroidAudio;
        this.newAsteroidTemplate.asteroidColor = this.originalAsteroidTemplate.asteroidColor;
        this.newAsteroidTemplate.phraseNumber = this.originalAsteroidTemplate.phraseNumber;
        this.newAsteroidTemplate.beatsPerPhrase = this.originalAsteroidTemplate.beatsPerPhrase;
        this.newAsteroidTemplate.isDynamic = this.originalAsteroidTemplate.isDynamic;

        this.UpdateBeatsPerPhrase(this.newAsteroidTemplate.beatsPerPhrase);
        this.previewAudio.clip = this.newAsteroidTemplate.asteroidAudio;
    }

    private void SaveNewAsteroidTemplate()
    {
        this.originalAsteroidTemplate.asteroidAudio = this.newAsteroidTemplate.asteroidAudio;
        this.originalAsteroidTemplate.asteroidColor = this.newAsteroidTemplate.asteroidColor;
        this.originalAsteroidTemplate.phraseNumber = this.newAsteroidTemplate.phraseNumber;
        this.originalAsteroidTemplate.beatsPerPhrase = this.newAsteroidTemplate.beatsPerPhrase;
        this.originalAsteroidTemplate.isDynamic = this.newAsteroidTemplate.isDynamic;
    }

    //Activate the grid that corresponds to the original asteroid's beatsPerPhrase
    private void SetActiveGrid()
	{
		for (int i = 0; i < this.beatGrids.Length; i++)
		{
			if (i == (this.newAsteroidTemplate.beatsPerPhrase - 1))
			{
				if ((this.beatGrids[i].gameObject.activeInHierarchy == false))
				{
					this.beatGrids[i].SetActive(true);
					this.beatGrids[i].GetComponent<BeatGrid>().SetupBeatGrid(this.newAsteroidTemplate.beatsPerPhrase);
					this.activeGrid = this.beatGrids[i];
				}
			}
			else
			{
				beatGrids[i].SetActive(false);
			}
		}
	}

	public void UpdatePhraseNumber(ushort stepNumber)
	{
		if ((this.newAsteroidTemplate.phraseNumber & stepNumber) > 0)
		{
			this.newAsteroidTemplate.phraseNumber -= stepNumber;
		}
		else
		{
			this.newAsteroidTemplate.phraseNumber += stepNumber;
		}
	}

	public void UpdateBeatsPerPhrase(int newBeatsPerPhrase)
	{
		this.newAsteroidTemplate.beatsPerPhrase = newBeatsPerPhrase;

        this.SetActiveGrid();
        this.EndPreview();
        this.previewButton.ToggleToPlayButton();

        AnalyticsEvent.Custom("Beat_Button_Clicked", new Dictionary<string, object> { { "Beats_Per_Phrase", newBeatsPerPhrase } });
    }

	#region Preview
	public void StartPreview()
	{
		this.previewPhrase = new Phrase(this.newAsteroidTemplate.phraseNumber, this.newAsteroidTemplate.beatsPerPhrase);

		Metronome.OnStep += this.PlayStep;

        AnalyticsEvent.Custom("Play_Preview_Button_Clicked", new Dictionary<string, object> { { "Phrase_Number", this.newAsteroidTemplate.phraseNumber } });
    }

	public void EndPreview()
	{
		Metronome.OnStep -= this.PlayStep;
        AnalyticsEvent.Custom("Stop_Preview_Button_Clicked", new Dictionary<string, object> { { "Phrase_Number", this.newAsteroidTemplate.phraseNumber } });
    }

	private void PlayStep()
	{
		//At every step, check for updates to the phrase.  Allows player to edit the phrase in real time.
		Phrase mostRecentPhrase = new Phrase(this.newAsteroidTemplate.phraseNumber, this.newAsteroidTemplate.beatsPerPhrase);
		mostRecentPhrase.SyncToStep(this.previewPhrase.currentStep);
		this.previewPhrase = mostRecentPhrase;

		if (this.previewPhrase.ShouldPlayAtStep() == true)
		{
			this.previewAudio.PlayScheduled(Metronome.nextBeatTime);
		}

		this.previewPhrase.IncrementStep();
	}
	#endregion

	#region Saving
	/*private void UpdateNote()
	{
		PhraseMetadata oldPhrase = this.asteroidToEdit.GetComponentInChildren<PhraseMetadata>();
        oldPhrase.phraseNumber = this.newAsteroidTemplate.phraseNumber;
        oldPhrase.beatsPerPhrase = this.newAsteroidTemplate.beatsPerPhrase;
    }

	private void UpdateParticlePulse()
	{
		ParticlePulse oldParticlePulse = this.asteroidToEdit.GetComponentInChildren<ParticlePulse>();
		oldParticlePulse.particleColor = this.newAsteroidTemplate.asteroidColor;
	}*/

	public void SaveTemplate()
	{
        this.SaveNewAsteroidTemplate();

        //this.UpdateNote();
		//this.UpdateParticlePulse();

		this.activeGrid.SetActive(false);
		this.gameObject.SetActive(false);

        AnalyticsEvent.Custom("Save_Button_Clicked", new Dictionary<string, object> { { "Phrase_Number", this.newAsteroidTemplate.phraseNumber } });
    }
	#endregion

	#region Cancel
	public void CancelAsteroidCreation()
	{
		this.activeGrid.SetActive(false);
		this.gameObject.SetActive(false);

        AnalyticsEvent.Custom("Cancel_Button_Clicked", new Dictionary<string, object> { { "Phrase_Number", this.newAsteroidTemplate.phraseNumber } });
    }
	#endregion
}
