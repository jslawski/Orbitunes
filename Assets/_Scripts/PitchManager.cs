using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* * *
 * The PitchManager handles everything related to managing the pitch and volumes of every audio source in the game
 * * */
public class PitchManager : MonoBehaviour {

	public static PitchManager instance;

	public static int notesInScale = 0;

	private float pitchFactor = Mathf.Pow(2.0f, (1.0f / 12.0f));

	public List<float> cMajorScale;

	public List<AudioSource> audioSourceMasterList;

	//Volume fields
	private float focusVolumePercent = 0.15f;		//Percent of "total volume" the focused notes will take up
	private int numFocusedNotes = 1;				//Number of most recent notes that will be played at a higher volume

	[SerializeField]
	private Note bassNote;

	// Use this for initialization
	void Awake () {
		PitchManager.instance = this;
		this.cMajorScale = new List<float>();

		this.audioSourceMasterList = new List<AudioSource>();

		this.CreateCMajorScale();

		bassNote.SetupNote(false);
	}

	private void CreateCMajorScale()
	{
		//C
		cMajorScale.Add(Mathf.Pow(pitchFactor, 12.0f));
		//B
		//cMajorScale.Add(Mathf.Pow(pitchFactor, 11.0f));
		//A
		cMajorScale.Add(Mathf.Pow(pitchFactor, 9.0f));
		//G
		cMajorScale.Add(Mathf.Pow(pitchFactor, 7.0f));
		//F
		//cMajorScale.Add(Mathf.Pow(pitchFactor, 5.0f));
		//E
		cMajorScale.Add(Mathf.Pow(pitchFactor, 4.0f));
		//D
		cMajorScale.Add(Mathf.Pow(pitchFactor, 2.0f));
		//C
		cMajorScale.Add(1.0f);

		PitchManager.notesInScale = cMajorScale.Count;
	}

	public void AddAudioSource(AudioSource source)
	{
		this.audioSourceMasterList.Add(source);

		this.NormalizeVolumes();
	}

	public void RemoveAudioSource(AudioSource source)
	{
		this.audioSourceMasterList.Remove(source);

		this.NormalizeVolumes();
	}

	private void NormalizeVolumes()
	{
		float baseNormalizedVolume = 1.0f / (float)this.audioSourceMasterList.Count;
		float normalizedVolumeBumpPerFocusNote = (baseNormalizedVolume * this.focusVolumePercent) / (float)this.numFocusedNotes;
		float focusVolume = baseNormalizedVolume + normalizedVolumeBumpPerFocusNote;
		float unfocusedVolume = baseNormalizedVolume - normalizedVolumeBumpPerFocusNote;

		foreach (AudioSource source in this.audioSourceMasterList) 
		{
			//Use baseNormalizedVolume if notes don't need to be focused yet
			if (this.audioSourceMasterList.Count < this.numFocusedNotes)
			{
				source.volume = baseNormalizedVolume;
			}
			else if (this.audioSourceMasterList.IndexOf(source) >= (this.audioSourceMasterList.Count - this.numFocusedNotes - 1))
			{
				source.volume = focusVolume;
			}
			else
			{
				source.volume = unfocusedVolume;
			}
		}
	}
}
