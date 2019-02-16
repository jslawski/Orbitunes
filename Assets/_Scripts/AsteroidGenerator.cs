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
    private NoteParent currentNoteParent;

    private bool initialAsteroidGenerated = false;

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

    private void SetupNoteParent(GameObject asteroid)
    {
        GameObject newNoteParentObject = GameObject.Instantiate(this.noteParentPrefab, this.transform.position, new Quaternion()) as GameObject;

        this.currentNoteParent = newNoteParentObject.GetComponent<NoteParent>();
        Note asteroidNote = asteroid.GetComponentInChildren<Note>();
        asteroidNote.noteAudio.clip = this.selectedAsteroidTemplate.asteroidAudio;

        this.currentNoteParent.SetupNoteParent(asteroidNote, this.selectedAsteroidTemplate.phraseNumber, this.selectedAsteroidTemplate.beatsPerPhrase, this.selectedAsteroidTemplate.isDynamic);
    }

    private void AddNoteToNoteParent(GameObject asteroid)
    {
        Note asteroidNote = asteroid.GetComponentInChildren<Note>();
        asteroidNote.noteAudio.clip = this.selectedAsteroidTemplate.asteroidAudio;

        this.currentNoteParent.AddNoteToList(asteroidNote);
        asteroid.transform.parent = this.currentNoteParent.transform;
    }

	private void SetupParticlePulse(GameObject asteroid)
	{
		ParticlePulse particlePulse = asteroid.GetComponentInChildren<ParticlePulse>();
        particlePulse.particleColor = this.selectedAsteroidTemplate.asteroidColor;
		particlePulse.SetupParticlePulse(this.selectedAsteroidTemplate.beatsPerPhrase, this.phrase.currentStep);
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
			GameObject asteroidObject = GameObject.Instantiate(this.baseAsteroidPrefab, this.launchValues.startPoint, new Quaternion()) as GameObject;
			this.SetupAsteroid(asteroidObject);

            //Only setup a note for the initial asteroid
            if (this.initialAsteroidGenerated == false)
            {
                this.SetupNoteParent(asteroidObject);
                asteroidObject.transform.parent = this.currentNoteParent.transform;

                this.initialAsteroidGenerated = true;

                AnalyticsEvent.Custom("Asteroid_Launched", new Dictionary<string, object> { { "Asteroid_Name", this.selectedAsteroidTemplate.asteroidName }, { "Phrase_Number", this.phrase.phraseNumber },
                    { "Launch_Start_Point", this.launchValues.startPoint }, { "Launch_End_Point", this.launchValues.endPoint }, { "Launch_Direction", this.launchValues.curDirection },
                    { "Launch_Magnitude", this.launchValues.rawMagnitude } });
            }
            else
            {
                this.AddNoteToNoteParent(asteroidObject);
            }
            
			this.SetupParticlePulse(asteroidObject);
		}

		this.phrase.IncrementStep();

        
    }
}
