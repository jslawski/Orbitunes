using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class AsteroidCreator : MonoBehaviour {

	public static AsteroidCreator instance;

	public GameObject[] beatGrids;
	private GameObject activeGrid;

	public GameObject asteroidToEdit;

	public Color asteroidColor;

	public ushort phraseNumber;

	[SerializeField]
	private AudioSource previewAudio;
	private Phrase previewPhrase;

	[SerializeField]
	private PreviewButton previewButton;

	private int _beatsPerPhrase;
	public int beatsPerPhrase
	{
		get { return this._beatsPerPhrase; }
		set
		{ 
			this._beatsPerPhrase = value;
			this.SetActiveGrid();
			this.EndPreview();
			this.previewButton.ToggleToPlayButton();
		}
	}

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
	public void SetupAsteroidCreator(GameObject asteroidToEdit) 
	{
		this.asteroidToEdit = asteroidToEdit;

		PhraseMetadata originalPhrase = asteroidToEdit.GetComponentInChildren<PhraseMetadata>();
        Note originalNote = asteroidToEdit.GetComponentInChildren<Note>();

		this.asteroidColor = asteroidToEdit.GetComponentInChildren<ParticlePulse>().particleColor;
		this.phraseNumber = originalPhrase.phraseNumber;
		this.beatsPerPhrase = originalPhrase.beatsPerPhrase;
		this.previewAudio.clip = originalNote.noteAudio.clip;
	}

	//Activate the grid that corresponds to the original asteroid's beatsPerPhrase
	private void SetActiveGrid()
	{
		for (int i = 0; i < this.beatGrids.Length; i++)
		{
			if (i == (this.beatsPerPhrase - 1))
			{
				if ((this.beatGrids[i].gameObject.activeInHierarchy == false))
				{
					this.beatGrids[i].SetActive(true);
					this.beatGrids[i].GetComponent<BeatGrid>().SetupBeatGrid(this.beatsPerPhrase);
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
		if ((this.phraseNumber & stepNumber) > 0)
		{
			this.phraseNumber -= stepNumber;
		}
		else
		{
			this.phraseNumber += stepNumber;
		}
	}

	public void UpdateBeatsPerPhrase(int newBeatsPerPhrase)
	{
		this.beatsPerPhrase = newBeatsPerPhrase;

        AnalyticsEvent.Custom("Beat_Button_Clicked", new Dictionary<string, object> { { "Beats_Per_Phrase", newBeatsPerPhrase } });
    }

	#region Preview
	public void StartPreview()
	{
		this.previewPhrase = new Phrase(this.phraseNumber, this.beatsPerPhrase);

		Metronome.OnStep += this.PlayStep;

        AnalyticsEvent.Custom("Play_Preview_Button_Clicked", new Dictionary<string, object> { { "Phrase_Number", this.phraseNumber } });
    }

	public void EndPreview()
	{
		Metronome.OnStep -= this.PlayStep;
        AnalyticsEvent.Custom("Stop_Preview_Button_Clicked", new Dictionary<string, object> { { "Phrase_Number", this.phraseNumber } });
    }

	private void PlayStep()
	{
		//At every step, check for updates to the phrase.  Allows player to edit the phrase in real time.
		Phrase mostRecentPhrase = new Phrase(this.phraseNumber, this.beatsPerPhrase);
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
	private void UpdateNote()
	{
		PhraseMetadata oldPhrase = this.asteroidToEdit.GetComponentInChildren<PhraseMetadata>();
        oldPhrase.phraseNumber = this.phraseNumber;
        oldPhrase.beatsPerPhrase = this.beatsPerPhrase;
    }

	private void UpdateParticlePulse()
	{
		ParticlePulse oldParticlePulse = this.asteroidToEdit.GetComponentInChildren<ParticlePulse>();
		oldParticlePulse.particleColor = this.asteroidColor;
	}

	public void SavePrefab()
	{
		this.UpdateNote();
		this.UpdateParticlePulse();

		this.activeGrid.SetActive(false);
		this.gameObject.SetActive(false);

        AnalyticsEvent.Custom("Save_Button_Clicked", new Dictionary<string, object> { { "Phrase_Number", this.phraseNumber } });
    }
	#endregion

	#region Cancel
	public void CancelAsteroidCreation()
	{
		this.activeGrid.SetActive(false);
		this.gameObject.SetActive(false);

        AnalyticsEvent.Custom("Cancel_Button_Clicked", new Dictionary<string, object> { { "Phrase_Number", this.phraseNumber } });
    }
	#endregion
}
