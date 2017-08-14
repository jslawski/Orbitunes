using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* * *
 * ParticlePulse handles the pulse particle effect for each individual asteroid.
 * Each asteroid requires its on ParticlePulse class in order to ensure that the pulse
 * only happens on the specific step in the beat that the asteroid was created on.
 * * */
public class ParticlePulse : MonoBehaviour {

	Phrase phrase;

	[SerializeField]
	private ParticleSystem particles;
	public Color particleColor;

	public void SetupParticlePulse(int beatsPerPhrase, int startStep)
	{
		//The pulse phrase consists of ONE step (bit) that the particle effect is supposed to play on for the phrase
		//So just use the phrase mask from that step as the phrase for the class
		ushort phraseNumber = Phrase.GetPhraseMaskAtStep(startStep, beatsPerPhrase);
		this.phrase = new Phrase(phraseNumber, beatsPerPhrase);
		this.phrase.SyncToStep(startStep);

		ParticleSystem.MainModule mainSettings = this.particles.main;
		mainSettings.startColor = new ParticleSystem.MinMaxGradient(this.particleColor);

		Metronome.OnStep += this.PlayParticles;

		this.PlayParticles();
	}

	private void PlayParticles()
	{
		if (this.phrase.ShouldPlayAtStep() == true)
		{
			this.particles.Play();
		}

		this.phrase.IncrementStep();
	}

	private void OnDestroy()
	{
		Metronome.OnStep -= this.PlayParticles;
	}
}
