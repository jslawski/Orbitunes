﻿using System.Collections;
using UnityEngine;
/* * *
 * The backbone of the entire game.  The metronome's only job should be to reliably time.
 * Every other game object that keeps a constant beat must subscribe to OnStep in order to stay synced.
 * * */
public static class Metronome {

	public delegate void MetronomeStep();
	public static event MetronomeStep OnStep;

	public static float stepsPerBeat = 4f;
	public static float secondsBetweenSteps = 0f;

	public static float beatsPerMinute = 80f;
	public static float secondsBetweenBeats = 0f;

	public static double nextBeatTime = 0;

	public static bool metronomeStarted = false;

    public static bool metronomePaused = false;

	public static IEnumerator StartMetronome()
	{
		Metronome.secondsBetweenBeats = 60.0f / Metronome.beatsPerMinute;
		Metronome.secondsBetweenSteps = Metronome.secondsBetweenBeats / Metronome.stepsPerBeat;

		Metronome.nextBeatTime = AudioSettings.dspTime;

		Metronome.metronomeStarted = true;

		while (true) 
		{
            if (Metronome.metronomePaused == false)
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
            }
            else
            {
                Metronome.nextBeatTime = AudioSettings.dspTime;
            }

			yield return null;
		}
	}

    public static void UpdateMetronomeTempo(float newBeatsPerMinute)
    {
        Metronome.beatsPerMinute = newBeatsPerMinute;
        Metronome.secondsBetweenBeats = 60.0f / Metronome.beatsPerMinute;
        Metronome.secondsBetweenSteps = Metronome.secondsBetweenBeats / Metronome.stepsPerBeat;
    }
}
