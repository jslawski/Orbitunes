using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Fix particle pulses for asteroids.  Currently they don't play for the main asteroid, and only play once for each trailing asteroid.

/* * * 
 * The Note class handles all non-physics-based aspects of an asteroid
 * including audio and particle effects
 *  *  */
public class Note : MonoBehaviour {
	public string noteName;

	[SerializeField]
	private Asteroid asteroid;
	[SerializeField]
	private ParticleSystem particles;
	public Color particleColor;
	public AudioSource noteAudio;

	[HideInInspector]
	public int curPitchIndex = 0;

	private int audioSourceMasterDictKey = 1;

	public void Start()
	{
		ParticleSystem.MainModule mainSettings = this.particles.main;
		mainSettings.startColor = new ParticleSystem.MinMaxGradient(this.particleColor);


		StartCoroutine(this.Pulse());

		//Don't play a note if the asteroid is a trailing asteroid
		if (this.asteroid.isTrailingAsteroid == false)
		{
			StartCoroutine(PitchManager.instance.PlayToBeat(this, asteroid.incrementsPerBeat));

			this.audioSourceMasterDictKey = PitchManager.instance.dictionaryIndex;
			PitchManager.instance.AddAudioSource(this.noteAudio);
		}
	}

	void OnDestroy()
	{
		PitchManager.instance.audioSourceMasterDict.Remove(this.audioSourceMasterDictKey);
	}

	public void GetCurrentPitchIndex()
	{
		this.curPitchIndex = Mathf.RoundToInt((this.asteroid.curDistanceFromStar / this.asteroid.maxDistanceFromStar) * (PitchManager.notesInScale - 1));
		if (this.curPitchIndex > PitchManager.notesInScale - 1) 
		{
			this.curPitchIndex = PitchManager.notesInScale - 1;
		}
	}

	public IEnumerator Pulse()
	{
		while (true) {
			particles.Play();

			yield return new WaitForSeconds(PitchManager.instance.secondsBetweenBeats);
		}
	}
}
