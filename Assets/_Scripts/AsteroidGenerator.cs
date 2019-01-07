using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* * *
 * The AsteroidGenerator class should only handle the behavior of launching an asteroid
 * as well as its accompanying trailing asteroids.  Relies on AsteroidSelector in order to determine
 * which asteroid to launch
 * * */
public class AsteroidGenerator : MonoBehaviour 
{
	private LaunchValues launchValues;
	private GameObject launchAsteroid;
	private Phrase phrase;

	private bool initialAsteroidGenerated = false;

	public void SetupAsteroidGenerator(LaunchValues launchValues)
	{
		this.launchValues = launchValues;
		this.launchAsteroid = AsteroidSelector.selectedAsteroid;

		Note asteroidNote = this.launchAsteroid.GetComponentInChildren<Note>();
		this.phrase = new Phrase(asteroidNote.phraseNumber, asteroidNote.beatsPerPhrase);

		Metronome.OnStep += this.LaunchAsteroid;
		GameManager.OnRestartButtonClicked += this.DestroyAsteroidGenerator;
	}

	private void OnDestroy()
	{
		Metronome.OnStep -= this.LaunchAsteroid;
		GameManager.OnRestartButtonClicked -= this.DestroyAsteroidGenerator;
	}

	private void SetupAsteroid(GameObject asteroid)
	{
		Asteroid newAsteroid = asteroid.GetComponent<Asteroid>();
		newAsteroid.isStationary = false;
		newAsteroid.SetupAsteroid(this.launchValues);
	}

	private void SetupNote(GameObject asteroid)
	{
		Note newNote = asteroid.GetComponentInChildren<Note>();
		newNote.SetupNote(true);
	}

	private void SetupParticlePulse(GameObject asteroid)
	{
		ParticlePulse particlePulse = asteroid.GetComponentInChildren<ParticlePulse>();
		Note newNote = asteroid.GetComponentInChildren<Note>();
		particlePulse.SetupParticlePulse(newNote.beatsPerPhrase, this.phrase.currentStep);
	}

	private void DestroyAsteroidGenerator()
	{
		Destroy(this.gameObject);
	}

	public void LaunchAsteroid()
	{
		//Stop generating asteroids once the phrase is complete
		if (this.phrase.currentStep >= (this.phrase.GetMaxStepCount() - 1))
		{
			Metronome.OnStep -= LaunchAsteroid;
			Destroy(this.gameObject);
		}

		if (this.phrase.ShouldPlayAtStep() == true)
		{
			GameObject asteroidObject = GameObject.Instantiate(this.launchAsteroid, this.launchValues.startPoint, new Quaternion()) as GameObject;
			this.SetupAsteroid(asteroidObject);

			//Only setup a note for the initial asteroid
			if (this.initialAsteroidGenerated == false)
			{
				this.SetupNote(asteroidObject);
				this.initialAsteroidGenerated = true;
			}

			this.SetupParticlePulse(asteroidObject);
		}

		this.phrase.IncrementStep();
	}
}
