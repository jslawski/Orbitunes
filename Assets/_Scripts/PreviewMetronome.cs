using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PreviewMetronome
{
    public delegate void PreviewMetronomeStep();
    public static event PreviewMetronomeStep OnPreviewStep;

    public static double nextBeatTime = 0;

    public static bool continuePreview = true;

    public static IEnumerator StartPreviewMetronome()
    {
        PreviewMetronome.continuePreview = true;

        PreviewMetronome.nextBeatTime = AudioSettings.dspTime;

        while (PreviewMetronome.continuePreview == true)
        {
            double curTime = AudioSettings.dspTime;

            if (curTime >= nextBeatTime)
            {
                if (PreviewMetronome.OnPreviewStep != null)
                {
                    PreviewMetronome.OnPreviewStep();
                }

                PreviewMetronome.nextBeatTime += Metronome.secondsBetweenSteps;
            }

            yield return null;
        }
    }

    public static void StopPreviewMetronome()
    {
        PreviewMetronome.continuePreview = false;
    }
}
