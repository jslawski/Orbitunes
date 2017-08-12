using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Metronome {

	public delegate void MetronomeStep();
	public static event MetronomeStep OnStep;

	public static float stepsPerBeat = 4f;
	private static float secondsBetweenSteps = 0f;

	private static float beatsPerMinute = 80f;
	public static float secondsBetweenBeats = 0f;

	public static double nextBeatTime = 0;

	public static IEnumerator StartMetronome()
	{
		Metronome.secondsBetweenBeats = 60.0f / Metronome.beatsPerMinute;
		Metronome.secondsBetweenSteps = Metronome.secondsBetweenBeats / Metronome.stepsPerBeat;

		Metronome.nextBeatTime = AudioSettings.dspTime;

		while (true) 
		{
			double curTime = AudioSettings.dspTime;
			if (curTime >= nextBeatTime)
			{
				if (Metronome.OnStep != null) 
				{
					Metronome.OnStep();
				}

				Metronome.nextBeatTime += Metronome.secondsBetweenSteps;
			}

			yield return null;
		}
	}
}
