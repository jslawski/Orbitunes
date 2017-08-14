using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* * *
 * The backbone of the entire game.  The metronome's only job should be to reliably time.
 * Every other game object that keeps a constant beat must subscribe to OnStep in order to stay synced.
 * * */
public static class Metronome {

	public delegate void MetronomeStep();
	public static event MetronomeStep OnStep;

	public static float stepsPerBeat = 4f;
	private static float secondsBetweenSteps = 0f;

	private static float beatsPerMinute = 80f;
	public static float secondsBetweenBeats = 0f;

	public static double nextBeatTime = 0;

	public static bool metronomeStarted = false;

	public static IEnumerator StartMetronome()
	{
		Metronome.secondsBetweenBeats = 60.0f / Metronome.beatsPerMinute;
		Metronome.secondsBetweenSteps = Metronome.secondsBetweenBeats / Metronome.stepsPerBeat;

		Metronome.nextBeatTime = AudioSettings.dspTime;

		Metronome.metronomeStarted = true;

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
