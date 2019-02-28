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
	private ParticleSystem particlePulse;
    private ParticleSystem particleTrail;
	public Color particleColor;

	public ParticlePulse(Color particleColor)
	{
		this.particleColor = particleColor;
	}

	public void SetupParticlePulse(ushort phraseNumber, int beatsPerPhrase, ParticleSystem trailParticles)
	{
		this.phrase = new Phrase(phraseNumber, beatsPerPhrase);

		ParticleSystem.MainModule mainSettings = this.particlePulse.main;
		mainSettings.startColor = new ParticleSystem.MinMaxGradient(this.particleColor);

        this.SetupTrail(trailParticles);

		Metronome.OnStep += this.PlayParticles;

		this.PlayParticles();
	}

    private void SetupTrail(ParticleSystem trailParticles)
    {
        ParticleSystem.MainModule trailMainSettings = trailParticles.main;
        trailMainSettings.startColor = new ParticleSystem.MinMaxGradient(this.particleColor);
    }

	private void PlayParticles()
	{
		if (this.phrase.ShouldPlayAtStep() == true)
		{
			this.particlePulse.Play();
		}

		this.phrase.IncrementStep();
	}

	private void OnDestroy()
	{
		Metronome.OnStep -= this.PlayParticles;
	}
}
