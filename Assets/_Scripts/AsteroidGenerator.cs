using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

/* * *
 * The AsteroidGenerator class should only handle the behavior of launching an asteroid
 * as well as its accompanying trailing asteroids.  Relies on AsteroidSelector in order to determine
 * which asteroid to launch
 * * */
public class AsteroidGenerator : MonoBehaviour
{
    public GameObject noteParentPrefab;
    public GameObject baseAsteroidPrefab;

    private AsteroidTemplate selectedAsteroidTemplate;
    private LaunchValues launchValues;
    private Phrase phrase;

    public void SetupAsteroidGenerator(LaunchValues launchValues)
    {
        this.selectedAsteroidTemplate = AsteroidSelector.selectedAsteroid;

        this.launchValues = launchValues;

        this.phrase = new Phrase(this.selectedAsteroidTemplate.phraseNumber, this.selectedAsteroidTemplate.beatsPerPhrase);

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
        asteroid.GetComponent<MeshRenderer>().material.color = this.selectedAsteroidTemplate.asteroidColor;

        Asteroid newAsteroid = asteroid.GetComponent<Asteroid>();
        newAsteroid.isStationary = false;
        newAsteroid.SetupAsteroid(this.launchValues);
    }

    private void SetupNote(GameObject asteroid)
    {
        Note newNote = asteroid.GetComponentInChildren<Note>();

        newNote.AssignAudioClip(AsteroidSelector.selectedAsteroid.asteroidAudio);

        newNote.SetupNote(this.selectedAsteroidTemplate.phraseNumber, this.selectedAsteroidTemplate.beatsPerPhrase, this.selectedAsteroidTemplate.isDynamic);
    }

	private void SetupParticlePulse(GameObject asteroid)
	{
		ParticlePulse particlePulse = asteroid.GetComponentInChildren<ParticlePulse>();
        particlePulse.particleColor = this.selectedAsteroidTemplate.asteroidColor;
        GameObject trailObject = Instantiate(selectedAsteroidTemplate.asteroidTrail, asteroid.transform) as GameObject;

		particlePulse.SetupParticlePulse(this.selectedAsteroidTemplate.phraseNumber, this.selectedAsteroidTemplate.beatsPerPhrase, trailObject.GetComponent<ParticleSystem>());
	}

	private void DestroyAsteroidGenerator()
	{
		Destroy(this.gameObject);
	}

	public void LaunchAsteroid()
	{
		GameObject asteroidObject = GameObject.Instantiate(this.baseAsteroidPrefab, this.launchValues.startPoint, new Quaternion()) as GameObject;
	    this.SetupAsteroid(asteroidObject);
        this.SetupNote(asteroidObject);
        this.SetupParticlePulse(asteroidObject);

        AnalyticsEvent.Custom("Asteroid_Launched", new Dictionary<string, object> { { "Asteroid_Name", this.selectedAsteroidTemplate.asteroidName }, { "Phrase_Number", this.phrase.phraseNumber },
                { "Launch_Start_Point", this.launchValues.startPoint }, { "Launch_End_Point", this.launchValues.endPoint }, { "Launch_Direction", this.launchValues.curDirection },
                { "Launch_Magnitude", this.launchValues.rawMagnitude } });

        Metronome.OnStep -= this.LaunchAsteroid;

        DestroyAsteroidGenerator();
    }
}
