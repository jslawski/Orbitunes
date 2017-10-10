using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

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

	// Use this for initialization
	public void SetupAsteroidCreator(GameObject asteroidToEdit) 
	{
		this.asteroidToEdit = asteroidToEdit;

		Note originalNote = asteroidToEdit.GetComponentInChildren<Note>();
		this.asteroidColor = asteroidToEdit.GetComponentInChildren<ParticlePulse>().particleColor;
		this.phraseNumber = originalNote.phraseNumber;
		this.beatsPerPhrase = originalNote.beatsPerPhrase;
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
	}

	#region Preview
	public void StartPreview()
	{
		this.previewPhrase = new Phrase(this.phraseNumber, this.beatsPerPhrase);

		Metronome.OnStep += this.PlayStep;
	}

	public void EndPreview()
	{
		Metronome.OnStep -= this.PlayStep;	
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
		Note oldNote = this.asteroidToEdit.GetComponentInChildren<Note>();
		oldNote.phraseNumber = this.phraseNumber;
		oldNote.beatsPerPhrase = this.beatsPerPhrase;
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

		string savePath = "Assets/Resources/Asteroids/" + this.asteroidToEdit.name + ".prefab";
		GameObject prefabToEdit = Instantiate(this.asteroidToEdit) as GameObject;
		Object prefab = PrefabUtility.CreateEmptyPrefab(savePath);
		PrefabUtility.ReplacePrefab(prefabToEdit, prefab, ReplacePrefabOptions.Default);
		Destroy(prefabToEdit);
		this.activeGrid.SetActive(false);
		this.gameObject.SetActive(false);
	}
	#endregion

	#region Cancel
	public void CancelAsteroidCreation()
	{
		this.activeGrid.SetActive(false);
		this.gameObject.SetActive(false);
	}
	#endregion
}
