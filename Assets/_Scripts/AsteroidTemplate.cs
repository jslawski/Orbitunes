using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAsteroidTemplate", menuName = "ScriptableObject/Asteroid Template")]
public class AsteroidTemplate : ScriptableObject
{
	public string asteroidName;
    public AudioClip asteroidAudio;
    public Color asteroidColor;

    public ushort phraseNumber;
    public int beatsPerPhrase;
    public bool isDynamic;
}
