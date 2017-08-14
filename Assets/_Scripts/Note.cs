using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* * * 
 * The Note class handles the behavior of any object that emits sound to the beat of the Metronome.  
 * If the note is static, it will always play the same pitch.
 * If the note is dynamic, it will change it's pitch depending on its distance from the center
 *  *  */
public class Note : MonoBehaviour {
	public string noteName;

	private float curDistanceFromStar = 0f;
	private float maxDistanceFromStar = 12f;

	public AudioSource noteAudio;

	[HideInInspector]
	public int curPitchIndex = 0;

	private Phrase notePhrase;

	//A phrase number is a 16-bit number where each bit represents a "beat" in the phrase.
	//1 represents a note played, 0 represents a rest.
	public ushort phraseNumber = 0;

	//How many measures are in a single phrase of the note.
	public int beatsPerPhrase = 0;

	public void SetupNote(bool isDynamic)
	{
		notePhrase = new Phrase(this.phraseNumber, this.beatsPerPhrase);

		if (isDynamic == true)
		{
			Metronome.OnStep += this.PlayDynamicNote;
			this.PlayDynamicNote();
		}
		else
		{
			Metronome.OnStep += this.PlayStaticNote;
			this.PlayStaticNote();
		}

		PitchManager.instance.AddAudioSource(this.noteAudio);
	}

	private void OnDestroy()
	{
		Metronome.OnStep -= this.PlayDynamicNote;
		Metronome.OnStep -= this.PlayStaticNote;

		PitchManager.instance.RemoveAudioSource(this.noteAudio);
	}

	public float GetDistance(Vector2 point1, Vector2 point2)
	{
		return (Mathf.Sqrt(Mathf.Pow((point2.x - point1.x), 2) + Mathf.Pow((point2.y - point1.y), 2)));
	}

	private void GetCurrentPitchIndex()
	{
		this.curDistanceFromStar = this.GetDistance(Vector2.zero, this.transform.position);

		this.curPitchIndex = Mathf.RoundToInt((this.curDistanceFromStar / this.maxDistanceFromStar) * (PitchManager.notesInScale - 1));
		if (this.curPitchIndex > PitchManager.notesInScale - 1) 
		{
			this.curPitchIndex = PitchManager.notesInScale - 1;
		}
	}

	//Pitch changes depending on distance to the star
	private void PlayDynamicNote()
	{
		if (Metronome.metronomeStarted == false)
		{
			return;
		}

		if (this.notePhrase.ShouldPlayAtStep() == true)
		{
			this.GetCurrentPitchIndex();
			this.noteAudio.pitch = PitchManager.instance.cMajorScale[this.curPitchIndex];
			this.noteAudio.PlayScheduled(Metronome.nextBeatTime);
		}

		this.notePhrase.IncrementStep();
	}

	//Pitch is static throughout its lifecycle
	private void PlayStaticNote()
	{
		if (Metronome.metronomeStarted == false)
		{
			return;
		}

		if (this.notePhrase.ShouldPlayAtStep() == true)
		{
			this.noteAudio.PlayScheduled(Metronome.nextBeatTime);
		}

		this.notePhrase.IncrementStep();
	}
}
