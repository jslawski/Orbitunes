using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchManager : MonoBehaviour {

	public static PitchManager instance;

	public static int notesInScale = 0;

	private float pitchFactor = Mathf.Pow(2.0f, (1.0f / 12.0f));

	public List<float> cMajorScale;

	public int dictionaryIndex = 0;
	public Dictionary<int, AudioSource> audioSourceMasterDict;

	public AudioSource bassSound;

	// Use this for initialization
	void Awake () {
		PitchManager.instance = this;
		this.cMajorScale = new List<float>();
		this.audioSourceMasterDict = new Dictionary<int, AudioSource>();

		this.AddAudioSource(this.bassSound);

		this.CreateCMajorScale();

		Restart.OnRestartButtonClicked += this.ResetBass;
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

	private void ResetBass()
	{
		this.bassSound.volume = 1f;
	}

	public IEnumerator StartBass()
	{
		double nextBeatTime = AudioSettings.dspTime;

		while (true)
		{
			double curTime = AudioSettings.dspTime;
			if (curTime > nextBeatTime)
			{
				this.bassSound.PlayScheduled(nextBeatTime);
				nextBeatTime += Metronome.secondsBetweenBeats;
			}

			yield return null;
		}
	}

	public void AddAudioSource(AudioSource source)
	{
		this.audioSourceMasterDict.Add(this.dictionaryIndex, source);
		this.dictionaryIndex++;
		this.NormalizeVolumes();
	}

	private void NormalizeVolumes()
	{
		float normalizedVolume = 1.0f / (float)this.audioSourceMasterDict.Count;

		foreach (KeyValuePair<int, AudioSource> entry in this.audioSourceMasterDict) 
		{
			entry.Value.volume = normalizedVolume;
		}
	}
}
