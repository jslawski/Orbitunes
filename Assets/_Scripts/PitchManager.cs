using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* * *
 * The PitchManager handles everything related to managing the pitch and volumes of every audio source in the game
 * * */
public enum Scale { CMajor, EMajor, GMinor };

public class PitchManager : MonoBehaviour {

    public static PitchManager instance;

    public static int notesInScale = 6;

    private float pitchFactor = Mathf.Pow(2.0f, (1.0f / 12.0f));

    public List<float> currentScale;

    public Dictionary<Scale, List<float>> scalesDictionary;

    public List<Note> noteMasterList;

    //Volume fields
    private float attenuationRatio = 0.15f;
    private int numFocusedNotes = 5;				//Number of most recent notes that will be played at a higher volume

    [SerializeField]
    private Note bassNote;

    public delegate void ScaleSelected(Scale selectedScale);
    public static event ScaleSelected OnScaleSelected;

    // Use this for initialization
    void Start() {
        PitchManager.instance = this;

        this.scalesDictionary = new Dictionary<Scale, List<float>>();

        this.noteMasterList = new List<Note>();

        this.CreateCMajorScale();
        this.CreateEMajorScale();
        this.CreateGMinorScale();

        this.bassNote.SetupNote(32768, 1, false);
        this.SelectScale(Scale.CMajor);
    }

    private void CreateCMajorScale()
    {
        List<float> cMajorScale;
        cMajorScale = new List<float>();
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

        this.scalesDictionary.Add(Scale.CMajor, cMajorScale);
    }

    private void CreateEMajorScale()
    {
        List<float> eMajorScale;
        eMajorScale = new List<float>();

        //E
        eMajorScale.Add(Mathf.Pow(pitchFactor, 16.0f));
        //D#
        //eMajorScale.Add(Mathf.Pow(pitchFactor, 15.0f));
        //C#
        eMajorScale.Add(Mathf.Pow(pitchFactor, 13.0f));
        //B
        eMajorScale.Add(Mathf.Pow(pitchFactor, 11.0f));
        //A
        //eMajorScale.Add(Mathf.Pow(pitchFactor, 9.0f));
        //G#
        eMajorScale.Add(Mathf.Pow(pitchFactor, 8.0f));
        //F#
        eMajorScale.Add(Mathf.Pow(pitchFactor, 6.0f));
        //E
        eMajorScale.Add(Mathf.Pow(pitchFactor, 4.0f));

        this.scalesDictionary.Add(Scale.EMajor, eMajorScale);
    }

    private void CreateGMinorScale()
    {
        List<float> gMinorScale;
        gMinorScale = new List<float>();

        //G
        gMinorScale.Add(Mathf.Pow(pitchFactor, 7.0f));
        //F
        //gMinorScale.Add(Mathf.Pow(pitchFactor, 5.0f));
        //Eflat
        gMinorScale.Add(Mathf.Pow(pitchFactor, 3.0f));
        //D
        gMinorScale.Add(Mathf.Pow(pitchFactor, 2.0f));
        //C
        //gMinorScale.Add(1.0);
        //Bflat
        gMinorScale.Add(Mathf.Pow(pitchFactor, -2.0f));
        //A
        gMinorScale.Add(Mathf.Pow(pitchFactor, -3.0f));
        //G
        gMinorScale.Add(Mathf.Pow(pitchFactor, -5.0f));

        this.scalesDictionary.Add(Scale.GMinor, gMinorScale);
    }

    public void SelectScale(Scale scaleToSelect)
    {
        this.currentScale = this.scalesDictionary[scaleToSelect];

        if (PitchManager.OnScaleSelected != null)
        {
            PitchManager.OnScaleSelected(scaleToSelect);
        }
    }

    public void AddNote(Note newNote)
	{
		this.noteMasterList.Add(newNote);
	}

	public void RemoveNote(Note oldNote)
	{
		this.noteMasterList.Remove(oldNote);
	}

    public float GetVolume(Note checkNote)
    {
        int index = this.noteMasterList.IndexOf(checkNote);

        //Return full volume if it is one of the most recent notes
        if (index > this.noteMasterList.Count - this.numFocusedNotes - 1)
        {
            return 1.0f;
        }

        //Otherwise, determine volume based on how old the note is compared to the others
        //Add 1 to index to prevent 0 division
        float noteAge = (this.noteMasterList.Count - this.numFocusedNotes) / (index + 1);
        float volumeBasedOnAge = 1.0f - (noteAge * this.attenuationRatio);


        //Don't go below a certain volume
        if (volumeBasedOnAge <= 0.15f)
        {
            volumeBasedOnAge = 0.15f;
        }

        return volumeBasedOnAge;
    }

	/*private void NormalizeVolumes()
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
	}*/
}
