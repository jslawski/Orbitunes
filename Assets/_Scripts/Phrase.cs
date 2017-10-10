using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* * *
 * The Phrase class contains all of the data and logic necessary to create custom beats
 * A phrase consists of a 4 to 16-bit set of binary, with 1s denoting a note, and 0s denoting a rest
 * Looping through the binary set replicates a looping beat.
 * * */
public class Phrase {
	private ushort phraseMask;		//Used to determine if a note should be played at a given beat in the phrase
	public static ushort maxPhraseMask = 32768; 	//Decimal representation of 1000000000000000

	public ushort phraseNumber;	//16-bit number representing when notes should be played at each step. 1 = play note, 0 = rest
	public int beatsPerPhrase;
	public int currentStep = 0;	//Used to determine the current step in the phrase

	#region Constructors
	public Phrase(ushort phraseNumber, int beatsPerPhrase)
	{
		this.phraseNumber = phraseNumber;
		this.beatsPerPhrase = beatsPerPhrase;
		this.phraseMask = Phrase.maxPhraseMask;
	}

	public Phrase(int beatsPerPhrase)
	{
		this.beatsPerPhrase = beatsPerPhrase;
		this.phraseMask = Phrase.maxPhraseMask;
		this.phraseNumber = this.phraseMask;
	}
	#endregion

	public static ushort GetPhraseMaskAtStep(int step, int beatsPerPhrase)
	{
		Phrase dummyPhrase = new Phrase(1, beatsPerPhrase);
		for (int i = 0; i < step; i++)
		{
			dummyPhrase.IncrementStep();
		}

		return dummyPhrase.phraseMask;
	}

	private void ResetPhrase()
	{
		this.phraseMask = Phrase.maxPhraseMask;
		this.currentStep = 0;
	}

	//Moves phrase step up to the indicated step
	public void SyncToStep(int step)
	{	
		this.ResetPhrase();

		for (int i = 0; i < step; i++)
		{
			this.IncrementStep();
		}
	}

	public int GetMaxStepCount()
	{
		return (int)Metronome.stepsPerBeat * this.beatsPerPhrase;
	}

	public void IncrementStep()
	{
		//Shift to next beat in the phrase, or restart the phrase
		if (this.currentStep < this.GetMaxStepCount() - 1)
		{
			this.phraseMask = (ushort)(this.phraseMask >> 1);
			this.currentStep++;
		}
		else
		{
			this.ResetPhrase();
		}
	}

	public bool ShouldPlayAtStep()
	{
		return ((this.phraseNumber & this.phraseMask) != 0);
	}
}
