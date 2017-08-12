using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phrase {
	private ushort phraseNumber;	//16-bit number representing when notes should be played at each step. 1 = play note, 0 = rest
	private ushort phraseMask;		//Used to determine if a note should be played at a given beat in the phrase

	private ushort maxPhraseMask1 = 8;		//Decimal representation of 0000000000001000
	private ushort maxPhraseMask2 = 128;	//Decimal representation of 0000000010000000
	private ushort maxPhraseMask3 = 2048;	//Decimal representation of 0000100000000000
	private ushort maxPhraseMask4 = 32768; 	//Decimal representation of 1000000000000000

	private int beatsPerPhrase;
	public int currentStep = 0;	//Used to determine the current step in the phrase

	#region Constructors
	public Phrase(ushort phraseNumber, int beatsPerPhrase)
	{
		this.phraseNumber = phraseNumber;
		this.beatsPerPhrase = beatsPerPhrase;
		this.phraseMask = this.GetBasePhraseMask(this.beatsPerPhrase);

		if (this.phraseMask == 0)
		{
			Debug.LogError("Error: Number of beats per phrase invalid.  Unable to play note.");
			return;
		}
	}

	public Phrase(int beatsPerPhrase)
	{
		this.beatsPerPhrase = beatsPerPhrase;
		this.phraseMask = this.GetBasePhraseMask(this.beatsPerPhrase);
		this.phraseNumber = this.phraseMask;

		if (this.phraseMask == 0)
		{
			Debug.LogError("Error: Number of beats per phrase invalid.  Unable to play note.");
			return;
		}
	}
	#endregion

	private ushort GetBasePhraseMask(int beatsPerPhrase)
	{
		switch (beatsPerPhrase)
		{
		case 1:
			return this.maxPhraseMask1;
		case 2:
			return this.maxPhraseMask2;
		case 3:
			return this.maxPhraseMask3;
		case 4:
			return this.maxPhraseMask4;
		default:
			Debug.LogError("Error: Invalid number of beats per phrase");
			return 0;
		};
	}

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
		this.phraseMask = this.GetBasePhraseMask(this.beatsPerPhrase);
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
