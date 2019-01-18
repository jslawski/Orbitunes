using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PhraseMetadata is responsible for holding all of an asteroid group's overall note data (set in the Inspector and via the Asteroid creation tool),
/// including phrase numbers and beats per phrase.  Used during asteroid generation, but has no functional implementation.
/// </summary>
public class PhraseMetadata : MonoBehaviour {
    //A phrase number is a 16-bit number where each bit represents a "beat" in the phrase.
    //1 represents a note played, 0 represents a rest.
    public ushort phraseNumber = 0;

    //How many measures are in a single phrase of the note.
    public int beatsPerPhrase = 0;

    //Whether or not the note's pitch dependent on its position
    public bool isDynamic = true;
}
